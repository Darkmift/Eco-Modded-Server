// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Core.Items;
    using Eco.Gameplay.Interactions;
    using Eco.Shared.Items;
    using Eco.Shared.Math;
    using Vector3 = System.Numerics.Vector3;
    using Eco.Gameplay.Blocks;

    [Tag("Construction")]
    public partial class CraneItem : WorldObjectItem<CraneObject>	{}

    [ObjectCanMakeBlockForm(new[] { 
                                    //Basic Forms
                                    typeof(AqueductFormType),typeof(CubeFormType),
                                    typeof(DocksBarrelPlatformFormType),typeof(DocksPlatformFormType),
                                    typeof(DocksPlatformFillFormType),typeof(FlatRoofFormType),
                                    typeof(FloorFormType),typeof(FullWallFormType),
                                    typeof(SimpleFloorFormType),typeof(TwoWhiteEdgeRotateFormType),
                                    typeof(WhiteCubeFormType),typeof(WhiteDashLineFormType),
                                    typeof(WhiteEdgeFormType),typeof(WhiteEdgeRotateFormType),
                                    typeof(WhiteLineFormType),
                                    //Climbing Forms
                                    typeof(FloatStairsFormType),typeof(FloatStairsCornerFormType),
                                    typeof(FloatStairsTurnFormType),typeof(LadderFormType),
                                    typeof(StairsFormType),typeof(StairsCornerFormType),
                                    typeof(StairsTurnFormType),typeof(UnderStairsFormType),
                                    //Ramps Forms
                                    typeof(DocksRampAFormType),typeof(DocksRampBFormType),
                                    typeof(DocksRampCFormType),typeof(DocksRampDFormType),
                                    typeof(DocksRampsFormType),typeof(DocksRampsCornerFormType),
                                    typeof(DocksRampsCornerInvertedFormType),typeof(RampAFormType),
                                    typeof(RampBFormType),typeof(RampCFormType),
                                    typeof(RampDFormType),typeof(WhiteRampDashLineAFormType),
                                    typeof(WhiteRampDashLineBFormType),typeof(WhiteRampDashLineCFormType),
                                    typeof(WhiteRampDashLineDFormType),typeof(WhiteRampEdgeAFormType),
                                    typeof(WhiteRampEdgeBFormType),typeof(WhiteRampEdgeCFormType),
                                    typeof(WhiteRampEdgeDFormType),typeof(WhiteRampLineAFormType),
                                    typeof(WhiteRampLineBFormType),typeof(WhiteRampLineCFormType),
                                    typeof(WhiteRampLineDFormType),
                                    //Roofs Forms
                                    typeof(RoofFormType),typeof(RoofCornerFormType),
                                    typeof(RoofCubeFormType),typeof(RoofPeakFormType),
                                    typeof(RoofPeakSetFormType),typeof(RoofSideFormType),
                                    typeof(RoofTurnFormType),
                                    //Slopes Forms
                                    typeof(BasicSlopeCornerFormType),typeof(BasicSlopePointFormType),
                                    typeof(BasicSlopeSideFormType),typeof(BasicSlopeTurnFormType),
                                    typeof(HalfSlopeAFormType),typeof(HalfSlopeBFormType),
                                    typeof(PeakSetFormType),typeof(SlopeCornerFormType),
                                    typeof(SlopeFlatFormType),typeof(SlopePointFormType),
                                    typeof(SlopeSideFormType),typeof(SlopeTurnFormType),
                                    typeof(UnderInnerPeakFormType),typeof(UnderPeakSetFormType),
                                    typeof(UnderSlopeCornerFormType),typeof(UnderSlopePeakFormType),
                                    typeof(UnderSlopeSideFormType),typeof(UnderSlopeTurnFormType),
                                    //Supports Forms
                                    typeof(BraceFormType),typeof(BraceCornerFormType),
                                    typeof(BraceTurnFormType),typeof(ChimneyFormType),
                                    typeof(ColumnFormType),typeof(DocksColumnFormType),
                                    typeof(DocksPillarFormType),typeof(DocksPillarBeamFormType),
                                    typeof(DocksPillarBeamCornerFormType),typeof(DocksPillarBeamEndFormType),
                                    typeof(DocksPillarBeamEndAltFormType),typeof(DocksPillarBeamJunctionFormType),
                                    typeof(DocksPillarBeamTFormType),typeof(DocksPillarBeamXFormType),
                                    typeof(SideBraceFormType),typeof(SmallCornerBraceFormType),
                                    typeof(ThinColumnFormType),typeof(UnderBraceFormType),
                                    typeof(UnderBraceCornerFormType),typeof(UnderBraceTurnFormType),
                                    //Thin Forms
                                    typeof(CanopyWindowFormType),typeof(CladWallFormType),
                                    typeof(DocksFenceCornerFormType),typeof(DocksFenceEndCapFormType),
                                    typeof(DocksFenceEndCapDoubleFormType),typeof(DocksFenceXFormType),
                                    typeof(DocksFenceMidFormType),typeof(DocksFenceSoloFormType),
                                    typeof(DocksFenceTFormType),typeof(DoubleWindowFormType),
                                    typeof(EdgeWallFormType),typeof(EdgeWallTurnFormType),
                                    typeof(FenceFormType),typeof(RoadBarrierFormType),
                                    typeof(SideFenceFormType),typeof(ThinFloorBottomFormType),
                                    typeof(ThinFloorTopFormType),typeof(ThinWallCornerFormType),
                                    typeof(ThinWallEdgeFormType),typeof(ThinWallStraightFormType),
                                    typeof(WallFormType),typeof(WallTrimFormType),
                                    typeof(WindowFormType),typeof(WindowCornersFormType),
                                    typeof(WindowEdgeFormType),typeof(WindowGrillesFormType),
                                    typeof(WindowGrillesEdgeFormType),typeof(WindowT2FormType),
                                    typeof(WindowWallFormType),typeof(FenceSoloFormType),typeof(FenceMidFormType),typeof(FenceEndFormType),typeof(FenceCornerFormType),typeof(FenceXFormType),typeof(FenceTFormType),
                                     })]
    public partial class CraneObject : PhysicsWorldObject    {}
}
