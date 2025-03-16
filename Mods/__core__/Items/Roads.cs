// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Eco.Core.Items;
using Eco.Gameplay.GameActions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Mods.TechTree;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.SharedTypes;
using Eco.World.Blocks;

namespace Eco.Mods.TechTree
{
    [Road(1.1f), ConstructWithoutTool(false)] //Stone Road, unlike dirt road, DOES need a hammer to place, so undo the [ConstructWithoutTool] value set by parents.
    public partial class StoneRoadItem { }

    [Road(1.2f), ConstructWithoutTool(false)] //Asphalt Concrete Road, unlike dirt road, DOES need a hammer to place, so undo the [ConstructWithoutTool] value set by parents.
    public partial class AsphaltConcreteItem { } 
}

[Serialized]
[Tag("Usable", Unset = true)]
public abstract class BaseRampObject : WorldObject { }

[Serialized]
public class DirtRampObject : BaseRampObject
{
    public override LocString DisplayName { get { return Localizer.DoStr("Dirt Ramp"); } }

    private DirtRampObject() { }
}

[Serialized]
[LocDisplayName("Dirt Ramp")]
[LocDescription("4 x 1 Dirt Ramp.")]
[ItemGroup("Road Items")]
[Tag("Road")]
[Ecopedia("Blocks", "Roads", createAsSubPage: true)]
[Weight(60000)]
public class DirtRampItem : RampItem<DirtRampObject>
{
    public override Dictionary<Vector3i, Type[]> BlockTypes { get { return new Dictionary<Vector3i, Type[]>
    {
        {Vector3i.Left,    new[] { typeof(DirtRampABlock), typeof(DirtRampBBlock), typeof(DirtRampCBlock), typeof(DirtRampDBlock) }},
        {Vector3i.Forward, new[] { typeof(DirtRampA90Block), typeof(DirtRampB90Block), typeof(DirtRampC90Block), typeof(DirtRampD90Block) }},
        {Vector3i.Right,   new[] { typeof(DirtRampA180Block), typeof(DirtRampB180Block), typeof(DirtRampC180Block), typeof(DirtRampD180Block) }},
        {Vector3i.Back,    new[] { typeof(DirtRampA270Block), typeof(DirtRampB270Block), typeof(DirtRampC270Block), typeof(DirtRampD270Block) }},
    };}}
}

[Serialized]
public class AsphaltConcreteRampObject : BaseRampObject
{
    public override LocString DisplayName => Localizer.DoStr("Asphalt Concrete Ramp");

    private AsphaltConcreteRampObject() { }
}

[Serialized]
public class StoneRampObject : BaseRampObject
{
    public override LocString DisplayName => Localizer.DoStr("Stone Ramp");
    private StoneRampObject() { }
}
#region DirtRampBlocks
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampABlock : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampBBlock : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampCBlock : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampDBlock : DirtRampBlock { }

[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampA90Block : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampB90Block : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampC90Block : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampD90Block : DirtRampBlock { }

[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampA180Block : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampB180Block : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampC180Block : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampD180Block : DirtRampBlock { }

[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampA270Block : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampB270Block : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampC270Block : DirtRampBlock { }
[Road(typeof(DirtRoadBlock)), Ramp(typeof(DirtRampItem))]
[Serialized, Solid, Constructed, Tag(BlockTags.CanBeRoad, Unset = true)] public partial class DirtRampD270Block : DirtRampBlock { }
#endregion
