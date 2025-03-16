// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.WorldLayers
{
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Systems;
    using Eco.Mods.TechTree;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Utils;
    using Eco.Simulation.WorldLayers;
    using Eco.Simulation.WorldLayers.Layers;

    public class WorldLayerSettingsOilfield : WorldLayerSettings
    {
        public WorldLayerSettingsOilfield() : base()
        {
            this.Name = "Oilfield";
            this.MinimapName = Localizer.DoStr("Oilfield");
            this.InitMultiplier = 1f;
            this.SyncToClient = true;
            this.Range = new Range(0f, 1f);
            this.OverrideRenderRange = null;
            this.MinColor = new Color(1f, 1f, 1f);
            this.MaxColor = new Color(0f, 0f, 0f);
            this.SumRelevant = false;
            this.Unit = string.Empty;
            this.VoxelsPerEntry = 5;
            this.Category = WorldLayerCategory.World;
            this.ValueType = WorldLayerValueType.Percent;
            this.AreaDescription = string.Empty;
            this.Visible = BalanceConfig.Obj.ShowOilLayer || this.IsDiscovered();
            DiscoveryManager.NewItemsDiscoveredEvent.Add(this.ChangeVisibility);  // We have no way to diffrentiate layer outside of settings, so we adding tiny bit of logic here, so its still customizable if needed
        }
        
        public void ChangeVisibility()
        {
            if (this.IsDiscovered())
                WorldLayerManager.Obj.UpdateLayerVisibility(this, true);
        }
        
        public bool IsDiscovered()
        {
            return DiscoveryManager.Obj.Discovered(typeof(OilDrillingSkill));
        }
    }
}
