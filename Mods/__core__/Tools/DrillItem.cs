// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System;
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Shared.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Serialization;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.Shared.Networking;

    [Serialized]
    [LocDisplayName("Drill")]
    [LocDescription("A useful tool for prospecting blocks.")]
    [Weight(0)]
    [Ecopedia("Items", "Tools")]
    [Category("Hidden")]
    public abstract partial class DrillItem : ToolItem
    {
        public override ItemHandOrigin HandOrigin          => ItemHandOrigin.Middle;
        public override bool           CanBeUsedWithEmotes => false;

        // Some const values for drilling logic
        public const float BlockHardnessModifier = 0.5f;

        public virtual float ProspectSpeed => 1f;
        public virtual int DrillDepth => 3;


        private static SkillModifiedValue caloriesBurn = CreateCalorieValue(20, typeof(SelfImprovementSkill), typeof(DrillItem));
        public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

        static IDynamicValue tier = new ConstantValue(0);
        public override IDynamicValue Tier { get { return tier; } }

        private static IDynamicValue skilledRepairCost = new ConstantValue(1);
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        public override int FullRepairAmount { get { return 1; } }

        public override int MaxTake { get { return 1; } }

        [RPC]
        public ProspectData GetProspectData(Player player, Vector3i rawDirection, Vector3i startingPos)
        {
            // On errors, or plant entities we have zero normal, that will broke dig results,
            // we need to replace dir with offset of 1 to start prospect from next block downside
            var direction = rawDirection == Vector3i.Zero ? Vector3i.Up : rawDirection;
            
            // Apply offset for starting pos by 1 in case we have unusual normal values
            if (rawDirection == Vector3i.Zero) startingPos -= direction;
            
            var result = new ProspectData(this, direction, this.DrillDepth);

            var neededCaloriesPerProspect = this.NeededCalories(player);
            // We calculate the max amount of blocks to be prospected as the ceiling of the amount of calories divided by the amount of calories a single prospect,
            // and if its more than the drillDepth then use it instead. We make sure the needed calories are not zero so we don't divide by it
            var maxBlocks =  neededCaloriesPerProspect > 0 ? (int)MathF.Min(MathF.Ceiling(player.User.Stomach.Calories / neededCaloriesPerProspect), this.DrillDepth) : this.DrillDepth;

            for (var i = 0; i < maxBlocks; i++)
            {
                var data = this.ProspectBlock(startingPos - (direction * i));
                result.Items.Add(data);
                result.MaxBlocksCanProspect = i + 1;

                // if we hit core, no need to drill further, exit
                if (data.ItemTypeId == -2) break;
            }

            if (result.MaxBlocksCanProspect < 0) result.MaxBlocksCanProspect = this.DrillDepth;
            
            return result;
        }

        // Parses all required block data to send to client
        ProspectItemData ProspectBlock(Vector3i position)
        {
            // Unpack block from position, block position won't be null
            var block = World.GetBlock(position);

            // Get resulting item from block
            var typeId = -1;
            if (block is IRepresentsItem item) typeId = Item.Get(item)?.TypeID ?? -1;
            if (typeId == -1)                  typeId = BlockItem.CreatingItem(block.GetType())?.TypeID ?? -1;
            if (World.GetBlock(position).Is<Impenetrable>() || position.y <= 0) typeId = -2;

            // Calculate prospect speed, based on skills, and hardness (gives 0.5s by default)
            var blockProspectSpeed = (block.Get<Minable>()?.Hardness ?? 1 * BlockHardnessModifier) / this.ProspectSpeed;
            
            var res = new ProspectItemData()
            {
                ItemTypeId = typeId,
                Position = position,
                ProspectSeconds = blockProspectSpeed,
            };

            return res;
        }
    }
}
