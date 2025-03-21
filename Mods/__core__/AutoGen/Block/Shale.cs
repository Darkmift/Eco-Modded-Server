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
    [Solid, Wall, Cliff, Minable(1)]
        public partial class ShaleBlock :
        Block
        , IRepresentsItem
    {
        public virtual Type RepresentedItemType { get { return typeof(ShaleItem); } }
    }

    [Serialized]
    [LocDisplayName("Shale")]
    [LocDescription("A soft rock, with few potential uses. Shale is a sedimentary rock formed by thin layers of compacted clay or mud.")]
    [MaxStackSize(500)]
    [Weight(6000)]
    [ResourcePile]
    [Ecopedia("Natural Resources", "Stone", createAsSubPage: true)]
    [Tag("Rock")]
    [Tag("Excavatable")]
    public partial class ShaleItem :
 
    BlockItem<ShaleBlock>
    {
        public override LocString DisplayNamePlural { get { return Localizer.DoStr("Shale"); } }

        public override bool CanStickToWalls { get { return false; } }

        private static Type[] blockTypes = new Type[] {
            typeof(ShaleStacked1Block),
            typeof(ShaleStacked2Block),
            typeof(ShaleStacked3Block),
            typeof(ShaleStacked4Block)
        };
        
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Tag("Rock")]
    [Tag("Excavatable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class ShaleStacked1Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Rock")]
    [Tag("Excavatable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class ShaleStacked2Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Rock")]
    [Tag("Excavatable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class ShaleStacked3Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Rock")]
    [Tag("Excavatable")]
    [Tag(BlockTags.FullStack)]
    [Serialized, Solid,Wall] public class ShaleStacked4Block : PickupableBlock, IWaterLoggedBlock { } //Only a wall if it's all 4 Shale
}
