﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// <auto-generated from BlockTemplate.tt/>

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Core.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.SharedTypes;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.World.Water;
    using Eco.Gameplay.Pipes;
    using Eco.Core.Controller;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Shared.Graphics;
    using Eco.World.Color;

    /// <summary>Auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization.</summary>

    [Serialized]
    [Solid, Wall, Constructed]
        public partial class LogBlock :
        Block
        , IRepresentsItem
    {
        public virtual Type RepresentedItemType { get { return typeof(LogItem); } }
    }

    [Serialized]
    [LocDisplayName("Log")]
    [LocDescription("A stack of logs.")]
    [MaxStackSize(500)]
    [Weight(10000)]
    [Fuel(4000)][Tag("Fuel")]
    [Category("Hidden")]
    [ResourcePile]
    public partial class LogItem :
 
    BlockItem<LogBlock>
    {


        private static Type[] blockTypes = new Type[] {
            typeof(LogStacked1Block),
            typeof(LogStacked2Block),
            typeof(LogStacked3Block),
            typeof(LogStacked4Block)
        };
        
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class LogStacked1Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class LogStacked2Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class LogStacked3Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag(BlockTags.FullStack)]
    [Serialized, Solid,Wall] public class LogStacked4Block : PickupableBlock, IWaterLoggedBlock { } //Only a wall if it's all 4 Log
}
