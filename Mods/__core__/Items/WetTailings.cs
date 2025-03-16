// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.World.Blocks;
    using Eco.Shared.Serialization;
    using Eco.Shared.Localization;
    using Eco.Core.Items;
    using Eco.Shared.SharedTypes;

    [Serialized, Weight(28000)]
    [LocDisplayName("Wet Tailings")]
    [LocDescription("Waste product from concentrating ore. The run-off creates ground pollution; killing nearby plants and seeping into the water supply.")]
    [MaxStackSize(500)]
    [RequiresTool(typeof(ShovelItem))]
    [Tag(BlockTags.Diggable)]
    [Tag("Waste Product")]
    [Ecopedia("Blocks", "Byproducts", true)]
    public class WetTailingsItem : BlockItem<WetTailingsBlock>
    {
        public override LocString DisplayNamePlural         { get { return Localizer.DoStr("Wet Tailings"); } }
        public override bool CanStickToWalls                { get { return false; } }
    }
}
