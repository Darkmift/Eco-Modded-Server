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
    [Solid, Wall, Cliff, Minable(3)]
        public partial class GraniteBlock :
        Block
        , IRepresentsItem
    {
        public virtual Type RepresentedItemType { get { return typeof(GraniteItem); } }
    }

    [Serialized]
    [LocDisplayName("Granite")]
    [LocDescription("A hard rock useful for construction, and host to various ores. Granite forms from cooling magma deep underground. It is the most common continental rock type!")]
    [MaxStackSize(500)]
    [Weight(6500)]
    [ResourcePile]
    [Ecopedia("Natural Resources", "Stone", createAsSubPage: true)]
    [Tag("Rock")]
    [Tag("Excavatable")]
    public partial class GraniteItem :
 
    BlockItem<GraniteBlock>
    {
        public override LocString DisplayNamePlural { get { return Localizer.DoStr("Granite"); } }

        public override bool CanStickToWalls { get { return false; } }

        private static Type[] blockTypes = new Type[] {
            typeof(GraniteStacked1Block),
            typeof(GraniteStacked2Block),
            typeof(GraniteStacked3Block),
            typeof(GraniteStacked4Block)
        };
        
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Tag("Rock")]
    [Tag("Excavatable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class GraniteStacked1Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Rock")]
    [Tag("Excavatable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class GraniteStacked2Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Rock")]
    [Tag("Excavatable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class GraniteStacked3Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Rock")]
    [Tag("Excavatable")]
    [Tag(BlockTags.FullStack)]
    [Serialized, Solid,Wall] public class GraniteStacked4Block : PickupableBlock, IWaterLoggedBlock { } //Only a wall if it's all 4 Granite
}
