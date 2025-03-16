// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.Organisms.SpeciesCatchers
{
    using System;
    using Eco.Core.Utils;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Mods.Components.VehicleModules;
    using Eco.Mods.TechTree;
    using Eco.Shared.Utils;
    using Eco.Gameplay.Animals.Catchers.Internal;
    using System.Collections.Generic;
    using System.Text;
    using Eco.Gameplay.Animals.Catchers;
    using Eco.Shared.Localization;
    using Eco.Simulation.Types;
    using Eco.Gameplay.Components;

    /// <summary> Catcher to use with  trawler vehicles with fishing nets</summary>
    public class BoatCatcher : WorldObjectLayeredCatchEntry
    {
        public override ThreadSafeList<string> DefaultTargetSpecies => new(CatchTarget.SmallFishTarget);
        public override Inventory TargetInventory   => this.fishing.Storage.Inventory;
        public override bool OnValidationCheck()    => this.fishing.CanFish && base.OnValidationCheck();
        public override TimeSpan NextCatchDelay     => TimeSpan.FromSeconds(RandomUtil.Range(5, 15));
        protected override Range CatchRange         => 1..6;

        FishingComponent fishing;
        VehicleComponent vehicle;

        public BoatCatcher(User user, WorldObject obj, VehicleComponent vehicle, FishingComponent fishing) : base(user, obj) 
        {
            this.fishing = fishing;
            this.vehicle = vehicle;
        }
        public BoatCatcher() { }

        public override void Initialize(List<string> layers)
        {
            base.Initialize(layers);

            // give player frequent updates about fishing area
            this.NoLayerData      += () => this.User?.MsgLoc($"Nothing to catch around {this.targetObject.DisplayName}.");
            this.SuccessfullCatch += (species, amount) => this.User?.MsgLoc($"{this.targetObject.DisplayName} caught {amount} of {species.DisplayName}.");
            this.TryCatch         += (data) =>
            {
                if(SpeciesLayeredCatchPlugin.Obj.Config.DisplayLayeredCatchInfo)
                {
                    var displayData = new LocStringBuilder();
                    displayData.AppendLineLocStr($"{this.targetObject.DisplayName} trying to catch at {this.targetObject.WorldPosXZi()}:");
                    foreach (var d in data)
                        displayData.AppendNL($" - {d.Layer.DisplayName}: {d.LayerValue}");
                    this.User?.Msg(displayData.ToLocString());
                }
            };
        }

        protected override bool ApplyCatch(Species species, int amount)
        {
            var result = this.vehicle.IsMoving && base.ApplyCatch(species, amount);                     // apply normal catch
            if (result) this.fishing.VehicleMod.VehicleToolItem.UseDurability(1f, this.User.Player, true);    // handle durability
            if (this.fishing.VehicleMod.VehicleToolItem.Broken) this.fishing.SetFishing(null, false);   // stop fishing when item breaks
            return result;
        }
    }
}
