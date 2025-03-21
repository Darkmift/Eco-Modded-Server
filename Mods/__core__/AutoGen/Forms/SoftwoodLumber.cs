﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// <auto-generated from FormsTemplate.tt/>

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
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.World;
    using Eco.World.Water;
    using Eco.World.Blocks;
    using Eco.Gameplay.Pipes;
    using Tag = Eco.Core.Items.TagAttribute;
    using Eco.World.Color;

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(CubeFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberCubeBlock :
        Block, IRepresentsItem, IColoredBlock
    {
        public Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(FloorFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberFloorBlock :
        Block, IRepresentsItem, IColoredBlock
    {
        public Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(RoofCubeFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofCubeBlock :
        Block, IRepresentsItem, IColoredBlock
    {
        public Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(WallFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberWallBlock :
        Block, IRepresentsItem, IWaterLoggedBlock, IColoredBlock
    {
        public Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(RoofFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofBlock :
        Block, IRepresentsItem, IWaterLoggedBlock, IColoredBlock
    {
        public Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(ColumnFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberColumnBlock :
        Block, IRepresentsItem, IWaterLoggedBlock, IColoredBlock
    {
        public Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(WindowFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberWindowBlock :
        Block, IRepresentsItem, IWaterLoggedBlock, IColoredBlock
    {
        public Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(FenceFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberFenceBlock :
        Block, IRepresentsItem, IWaterLoggedBlock, IColoredBlock
    {
        public Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(WindowGrillesFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberWindowGrillesBlock :
        Block, IRepresentsItem, IWaterLoggedBlock, IColoredBlock
    {
        public Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(RoofPeakSetFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofPeakSetBlock :
        Block, IRepresentsItem, IWaterLoggedBlock, IColoredBlock
    {
        public Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }





    [RotatedVariants(typeof(SoftwoodLumberStairsBlock), typeof(SoftwoodLumberStairs90Block), typeof(SoftwoodLumberStairs180Block), typeof(SoftwoodLumberStairs270Block))]
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(StairsFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberStairsBlock : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberStairs90Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberStairs180Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberStairs270Block : Block, IWaterLoggedBlock, IColoredBlock
    { }
    [RotatedVariants(typeof(SoftwoodLumberLadderBlock), typeof(SoftwoodLumberLadder90Block), typeof(SoftwoodLumberLadder180Block), typeof(SoftwoodLumberLadder270Block))]
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(LadderFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberLadderBlock : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberLadder90Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberLadder180Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberLadder270Block : Block, IWaterLoggedBlock, IColoredBlock
    { }
    [RotatedVariants(typeof(SoftwoodLumberRoofSideBlock), typeof(SoftwoodLumberRoofSide90Block), typeof(SoftwoodLumberRoofSide180Block), typeof(SoftwoodLumberRoofSide270Block))]
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(RoofSideFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofSideBlock : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofSide90Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofSide180Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofSide270Block : Block, IWaterLoggedBlock, IColoredBlock
    { }
    [RotatedVariants(typeof(SoftwoodLumberRoofTurnBlock), typeof(SoftwoodLumberRoofTurn90Block), typeof(SoftwoodLumberRoofTurn180Block), typeof(SoftwoodLumberRoofTurn270Block))]
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(RoofTurnFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofTurnBlock : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofTurn90Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofTurn180Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofTurn270Block : Block, IWaterLoggedBlock, IColoredBlock
    { }
    [RotatedVariants(typeof(SoftwoodLumberRoofCornerBlock), typeof(SoftwoodLumberRoofCorner90Block), typeof(SoftwoodLumberRoofCorner180Block), typeof(SoftwoodLumberRoofCorner270Block))]
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(RoofCornerFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofCornerBlock : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofCorner90Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofCorner180Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofCorner270Block : Block, IWaterLoggedBlock, IColoredBlock
    { }
    [RotatedVariants(typeof(SoftwoodLumberRoofPeakBlock), typeof(SoftwoodLumberRoofPeak90Block), typeof(SoftwoodLumberRoofPeak180Block), typeof(SoftwoodLumberRoofPeak270Block))]
    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [IsForm(typeof(RoofPeakFormType), typeof(SoftwoodLumberItem))]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofPeakBlock : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofPeak90Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofPeak180Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

    [Serialized]
    [Wall, Constructed, Solid, BuildRoomMaterialOption]
    [BlockTier(3)]
    [Tag("Constructable")]
    public partial class SoftwoodLumberRoofPeak270Block : Block, IWaterLoggedBlock, IColoredBlock
    { }

}