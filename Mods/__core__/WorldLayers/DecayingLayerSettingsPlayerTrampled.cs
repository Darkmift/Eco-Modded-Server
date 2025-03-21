﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.WorldLayers
{
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Utils;
    using Eco.Simulation.WorldLayers.Layers;

    public class DecayingLayerSettingsPlayerTrampled : DecayingLayerSettings
    {
        public DecayingLayerSettingsPlayerTrampled() : base()
        {
            this.Name = "PlayerTrampled";
            this.MinimapName = Localizer.DoStr("Player Trampled");
            this.InitMultiplier = 1f;
            this.SyncToClient = false;
            this.Range = new Range(0f, 1f);
            this.OverrideRenderRange = null;
            this.MinColor = new Color(1f, 1f, 1f);
            this.MaxColor = new Color(1f, 0.5f, 0.5f);
            this.SumRelevant = false;
            this.Unit = string.Empty;
            this.VoxelsPerEntry = 5;
            this.Category = WorldLayerCategory.Pollution;
            this.ValueType = WorldLayerValueType.Percent;
            this.AreaDescription = string.Empty;
            this.DecayRateFlat = 0f;
            this.DecayRatePercent = 0.001f;

        }
    }
}
