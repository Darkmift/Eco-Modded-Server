// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
	using Eco.Core.Items;
	using Eco.Core.Utils;
    using Eco.Gameplay.Auth;
    using Eco.Gameplay.Components.Storage;
    using Eco.Gameplay.GameActions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Placement;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Settlements.ClaimStakes;
    using Eco.Gameplay.Settlements.ClaimStakes.Internal;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.UI.WorldMarker;
    using Eco.Shared.IoC;
    using Eco.Shared.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.Voxel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Vector3 = System.Numerics.Vector3;

    [Serialized]
    [LocDisplayName("Starter Camp")]
    [LocDescription("A combination of a small tent and a tiny stockpile.")]
    [Tag("PlaceableOnUnownedLand")]
    public class StarterCampItem : WorldObjectItem<StarterCampObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

        private static WorldRange virtualOccupancy = new WorldRange(new Vector2i(-2, -3), new Vector2i(3, 2)); // A WorldRange that uses the whole plot.
        private static WorldRange GetRotatedOccupancy(Quaternion rotation) => virtualOccupancy.RotatedByInc(rotation);

        ///<summary>Iterates the plots that this world object covers.</summary>
        IEnumerable<PlotPos> OverlappingPlots(Vector3i worldPos, Quaternion rotation) => GetRotatedOccupancy(rotation).Translate(worldPos).IntersectingPlotsInc();

        public override async Task PlacingObject(GameActionPack pack, Player player, ItemStack containingStack, Vector3 pos, Quaternion rotation)
        {
            if (!pack.EarlyResult) return;

            //Drop the claimstake/homestead and associate it with deed.
            WorldObjectItem claimItem = FeatureConfig.Obj.UseSettlementSystem ? new HomesteadClaimStakeItem() { User = player.User } : new OutpostClaimStakeItem();
            var claimPos = pos + rotation.RotateVector(new Vector3i(1, 0, -2));
            var claim = await WorldObjectPlacementUtils.TryPlaceWorldObject(pack, player, claimItem, null, claimPos.Round(), rotation, null);
            if (!pack.EarlyResult) return;

            //Make a local that gets assigned by the post effect closure, then used by the other ptg6ost effect setup in Claim Property. 
            WorldObject camp = null;
            WorldObject stockpile = null;
            pack.AddPostEffect(() =>
            {
                camp = WorldObjectManager.ForceAdd(typeof(CampsiteObject), player.User, pos, rotation, false);
                stockpile = WorldObjectManager.ForceAdd(typeof(TinyStockpileObject), player.User, pos + rotation.RotateVector(Vector3i.Right * 3), rotation, false);
                player.User.OnWorldObjectPlaced.Invoke(camp);
                player.User.Markers.Add(player.User, camp.Position3i + Vector3i.Up, camp.UILinkContent(), false, folderStructure: MarkerFolderName.WorldObjects);
                var storage = camp.GetComponent<PublicStorageComponent>();
                using var changeSet = InventoryChangeSet.New(storage.Inventory);
                var itemsToAdd = PlayerDefaults.GetDefaultCampsiteInventory();


                itemsToAdd.ForEach(x => changeSet.AddItemsNonUnique(x.Key, x.Value, storage.Inventory));
                changeSet.Apply();
            });

            //Do the claiming in a separate step, after the homestead deed has been established. This will run another action pack.
            pack.AddPostEffect(() =>
            {
                //Now create the deed and claim everything that needs to be claim.
                var deed = claim().GetDeed();
                var plots = this.OverlappingPlots(pos.XYZi(), rotation).Where(x => !PropertyManager.IsClaimed(x));
                AtomicActions.ClaimOrUnclaimPropertiesNow(deed, player.User, plots, null, false, true, true, false, player.User, null, PropertyType.Residence);
            });

            pack.AddPostEffect(() =>
            {
                WorldObjectPlacementUtils.FinishPlacement(player.User, camp);
                WorldObjectPlacementUtils.FinishPlacement(player.User, stockpile);
                WorldObjectPlacementUtils.FinishPlacement(player.User, claim());
            });
        }

        public override bool ShouldCreate => false;

        public override Task<bool> CanPlaceObject(Player player, Vector3 worldPos, Quaternion rotation)
        {
            var canPlace = this.OccupancyContext?.CanPlaceObject(player, this, worldPos, rotation) ?? true;
            if (!canPlace) return Task.FromResult(false);

            var claimStake = (ClaimStakeItemBase)(FeatureConfig.Obj.UseSettlementSystem ? Item.Get(typeof(HomesteadClaimStakeItem)) : Item.Get(typeof(OutpostClaimStakeItem)));
            if (!claimStake.CanClaim(player, worldPos.XYZi())) return Task.FromResult(false);

            //Check if any of the plots we would cover are owned by someone else.
            foreach (var plotPos in this.OverlappingPlots(worldPos.XYZi(), rotation))
            {
                var plot = PropertyManager.GetPlotFromPlotPos(plotPos);
                if (plot == null || plot.Deed == null) continue;   // Unowned plot.

                player.ErrorLoc($"Plot already owned by {plot.Deed.UILink()}.");
                return Task.FromResult(false);
            }

            return base.CanPlaceObject(player, worldPos, rotation);
        }

        LazyResult IsPlotAuthorized(PlotPos plotPos, User user, out bool canClaim)
        {
            var plot = PropertyManager.GetPlotFromPlotPos(plotPos);
            if (plot == null || plot.Deed == null)
            {
                canClaim = true;
                return LazyResult.Succeeded;
            }

            canClaim = false;
            return plot.Owners == user ? LazyResult.Succeeded : ServiceHolder<IAuthManager>.Obj.IsAuthorized(plot.PlotPos, user, AccessType.ConsumerAccess, null);
        }

        public override void OnSelected(Player player)
        {
            base.OnSelected(player);

            //We check whether we're going to use the homestead or the legacy claim style for the starter camp.
            var claimStyle = FeatureConfig.Obj.UseSettlementSystem ? PropertyClaimStyle.PlacingHomestead : PropertyClaimStyle.LegacyClaimStake;
            // Add camp's virtual occupancy to player's property selector so it could highlight four plots at once.
            player?.SetPropertyClaimingMode(claimStyle, virtualOccupancy, overrideTitle: Localizer.Do($"Place Starter Camp"), overrideSubtitle: Localizer.Do($"This will also claim some of the surrounding plots if available.")); 
        }

        public override void OnDeselected(Player player)
        {
            base.OnDeselected(player);
            player?.StopPropertyClaimingMode();
        }
    }

    [Serialized]
    public partial class StarterCampObject : WorldObject
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Starting Camp"); } }
    }
}
