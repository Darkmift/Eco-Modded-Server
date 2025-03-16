// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
using Eco.Shared.SharedTypes;
using System.ComponentModel;
using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

[Serialized]
[LocDisplayName("Garbage")]
[LocDescription("A disgusting pile of garbage. Unpleasant to the eye and a source of ground pollution. Bury underground to help mitigate the effect.")]
[Weight(30000)]
[MaxStackSize(500)]
[RequiresTool(typeof(ShovelItem))]
[Tag(BlockTags.Diggable)]
[Tag(BlockTags.Excavatable)]
[Ecopedia("Blocks", "Byproducts", true)]
public partial class GarbageItem : BlockItem<GarbageBlock>
{
    public override LocString DisplayNamePlural { get { return Localizer.DoStr("Garbage"); } }
    public override bool CanStickToWalls { get { return false; } }
}

[Serialized]
[Category("Hidden")]
[MaxStackSize(1)]
public partial class TrashItem : Item { }

[Serialized]
[Category("Hidden")]
[MaxStackSize(1)]
public partial class CompostablesItem : Item { }
