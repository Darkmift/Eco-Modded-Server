﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.WorldLayers
{
    using Eco.Simulation.WorldLayers.Layers;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Utils;

    public class AnimalLayerSettingsCrab : AnimalLayerSettings
    {
        public AnimalLayerSettingsCrab() : base()
        {
            this.Name = "Crab";
            this.MinimapName = Localizer.Do($"{Localizer.DoStr("Crab")} Population");
            this.InitMultiplier = 1f;
            this.SyncToClient = false;
            this.Range = new Range(0f, 2.1f);
            this.OverrideRenderRange = null;
            this.MinColor = new Color(1f, 1f, 1f);
            this.MaxColor = new Color(0f, 1f, 0f);
            this.SumRelevant = true;
            this.Unit = "Crab";
            this.VoxelsPerEntry = 40;
            this.Category = WorldLayerCategory.Animal;
            this.ValueType = WorldLayerValueType.Amount;
            this.AreaDescription = string.Empty;
            this.HabitabilityLayer = "CrabCapacity";
            this.SpreadRate = 0.5f;
            this.SpeciesChangeRate = 0.05f;
        }
    }
}
