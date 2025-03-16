// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Shared.Serialization;
    using Eco.Core.Controller;
    using System;
    using Eco.Shared.Items;

    public partial class RealEstateDeskItem : IPersistentData, IRepresentsItem
    {
        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
        public virtual Type RepresentedItemType { get { return typeof(RealEstateDeskItem); } }
    }
}
