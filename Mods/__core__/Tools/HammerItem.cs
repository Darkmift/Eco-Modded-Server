// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
namespace Eco.Mods.TechTree
{
	using System.ComponentModel;
    using Eco.Core.Controller;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.World.Blocks;

	[Serialized]
    [LocDisplayName("Hammer")]
    [LocDescription("Used to construct buildings and pickup manmade objects.")]
    [Category("Hidden")]
    public abstract class HammerItem : BuildingToolItem
    {
        static IDynamicValue tier = new ConstantValue(0);
        static IDynamicValue caloriesBurn = new ConstantValue(1);
        private static IDynamicValue skilledRepairCost = new ConstantValue(1);
        
        public override IDynamicValue SkilledRepairCost             { get { return skilledRepairCost; } }
        [SyncToView] public override IDynamicValue Tier             => base.Tier; // tool tier, overriden to have SyncToView only for hammer
        public override IDynamicValue CaloriesBurn                  { get { return caloriesBurn; } }
        
        public override bool IsValidForInteraction(Item item)
        {
            var blockItem = item as BlockItem;
            return !(item is LogItem) && blockItem != null && Block.Is<Constructed>(blockItem.OriginType);
        }
    }
}
