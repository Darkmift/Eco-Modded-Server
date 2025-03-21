﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.World.Blocks;
    using Eco.Shared.Serialization;
    using Eco.Shared.SharedTypes;
    using Eco.Shared.Localization;
    using Eco.Core.Items;

    [Serialized, Weight(18000)]
    [LocDisplayName("Tailings")]
    [LocDescription("Waste product from concentrating ore. When stored improperly the run-off will create pollution; killing nearby plants and seeping into the water supply. Bury deep underground to help neutralize the effect.")]
    [MaxStackSize(500)]
    [RequiresTool(typeof(ShovelItem))]
    [Tag(BlockTags.Diggable)]
    [Tag(BlockTags.Excavatable)]
    [Tag("Waste Product")]
    [Ecopedia("Blocks", "Byproducts", true)]
    public class TailingsItem : BlockItem<TailingsBlock>
    {
        public override LocString DisplayNamePlural { get { return Localizer.DoStr("Tailings"); } }
        public override bool CanStickToWalls      { get { return false; } }
    }
}
