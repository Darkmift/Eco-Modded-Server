// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.Items;
    using Eco.Shared.Items;
    using Eco.Shared.Serialization;

    [Serialized]
    [Category("Hidden")]
    [Tag("Harvester")]
    [Mower]
    public abstract partial class SickleItem : BlockHarvestItem
    {
    }
}
