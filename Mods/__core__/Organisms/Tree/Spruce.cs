﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.Organisms
{
    using System;
    using System.Collections.Generic;
    using Eco.Mods.TechTree;
    using Eco.Simulation.Types;
    using Eco.World.Blocks;
    using Range = Eco.Shared.Math.Range;
    
    public partial class Spruce : TreeEntity
    {
        public partial class SpruceSpecies : TreeSpecies
        {
            partial void SetDefaultProperties()
            {
                this.TreeHealth = 10f;
                this.LogHealth = 2f;
                // Resources
                this.ChanceToSpawnDebris = 0.3f;
                // Visuals
                this.BranchingDef = new List<TreeBranchDef>()
                {
                    new TreeBranchDef() { Name = "Branch0", Health = 3f, LeafPoints = 1, GrowthStartTime = new Range(0f, 0f), GrowthEndTime = new Range(1f, 1f) },
                    new TreeBranchDef() { Name = "Branch1", Health = 3f, LeafPoints = 1, GrowthStartTime = new Range(0f, 0f), GrowthEndTime = new Range(1f, 1f) },
                    new TreeBranchDef() { Name = "Branch2", Health = 3f, LeafPoints = 1, GrowthStartTime = new Range(0f, 0f), GrowthEndTime = new Range(1f, 1f) },
                };
                this.TopBranchLeafPoints = 0;
                this.TopBranchHealth = 3;
                this.SequentialBranchRotations = true;
                this.BranchRotations = new float[] { 0.0f, 120.0f, 240.0f };
                this.RandomYRotation = true;
                this.BranchCount = new Range(3f, 3f);
                this.BlockType = typeof(TreeBlock);
                this.DebrisType = typeof(SpruceTreeDebrisBlock);
                this.DebrisResources = new Dictionary<Type, Range>()
                {
                    { typeof(WoodPulpItem), new Range(4, 5) },
                    { typeof(SpruceSeedItem), new Range(0, 1) },
                };
                this.TrunkResources = new Dictionary<Type, Range>()
                {
                    { typeof(WoodPulpItem), new Range(8, 10) }
                };
                this.XZScaleRange = new Range(.8f, 1.2f);
                this.YScaleRange = new Range(.8f, 1.4f);
                this.Density = 450f;
            }
        }
    }
}
