// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Core.Items;
    using Eco.Gameplay.Items;
    using Eco.Shared.Serialization;
    using Eco.Shared.Localization;
    using Eco.Shared.SharedTypes;

    [Serialized]
    [LocDisplayName("Compost")]
    [LocDescription("Delicious decomposed organic matter that can be used to fertilze crops. Compost is created overtime when organic material is left outdoors to decompose. This is accomplished in Eco by dropping an organic item on the ground and overtime it will become Compost.")]
    [Weight(30000)]
    [MaxStackSize(500)]
    [RequiresTool(typeof(ShovelItem))]
    [Tag(BlockTags.Diggable)]
    [Tag(BlockTags.Excavatable)]
    [Ecopedia("Blocks", "Byproducts", true)]
    public class CompostItem : BlockItem<CompostBlock>
    {
        public override LocString DisplayNamePlural  { get { return Localizer.DoStr("Compost"); } }

        public override bool CanStickToWalls => false;
    }
}
