// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System;
    using System.ComponentModel;
    using Eco.Core.Controller;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Shared.Serialization;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Gameplay.Occupancy;

    [Serialized]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(LiquidConverterComponent))]
    [Category("Hidden")]
    public partial class LiquidCrafterObject :
        WorldObject,
        IRepresentsItem
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Liquid Crafter"); } }
        public virtual Type RepresentedItemType { get { return typeof(LiquidCrafterItem); } }

        protected override void Initialize()
        {
            this.GetComponent<LiquidConverterComponent>().Setup(Item.Get("WaterItem").Type, Item.Get("SewageItem").Type, BlockOccupancyType.InputPort, BlockOccupancyType.OutputPort, 1f, 0f);
        }
    }

    [Serialized]
    [LocDisplayName("Liquid Converter")]
    [LocDescription("Example crafting table that uses liquids.")]
    [Category("Hidden"), NoIcon]
    public partial class LiquidCrafterItem :
        WorldObjectItem<LiquidCrafterObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));
    }
}
