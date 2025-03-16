// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Simulation.WorldLayers;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.Gameplay.Components;
    using System;
    using System.Collections.Generic;
    using Eco.Simulation.WorldLayers.Layers;
    using Eco.Gameplay.Components.Storage;
    using Eco.Mods.Organisms.SpeciesCatchers;
    using Eco.Gameplay.Animals.Catchers;

    [RequireComponent(typeof(AnimalTrapComponent))]
    public partial class FishTrapObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<PublicStorageComponent>().Initialize(4, 15000);
            this.GetComponent<PublicStorageComponent>().Inventory.AddInvRestriction(new SpecificItemTypesRestriction(new System.Type[] { typeof(TroutItem), typeof(SalmonItem) }));
            this.GetComponent<AnimalTrapComponent>().Initialize(new List<string>() { "Trout", "Salmon" });
            this.GetComponent<AnimalTrapComponent>().FailStatusMessage = Localizer.DoStr("Wooden fish traps must be placed underwater in fresh water to function.");
            this.GetComponent<AnimalTrapComponent>().EnabledTest = this.WaterTest;
            this.GetComponent<AnimalTrapComponent>().UpdateEnabled();

            SpeciesLayeredCatchPlugin.Obj.AddLayeredCatcher(this, new FishCatcher(null, this));
        }

        public bool WaterTest(Vector3i pos)
        {
            var blockAbove = World.GetBlock(pos + Vector3i.Up);                                     // Get the block above the FishTrap.

            if (blockAbove is IWaterBlock waterBlock && !waterBlock.PipeSupplied)                   // If the above block IS water and NOT from a pipe, take the effort to check if it is in freshwater.
            {
                LayerPosition layerPos = LayerPosition.FromWorldPosition(pos.XZ, 1);                // Determine the position within the layer that the FishTrap is in.
                WorldLayer saltLayer = WorldLayerManager.Obj.GetLayer(LayerNames.SaltWater);        // Get the SaltWater layer to compare.

                return saltLayer.GetValue(layerPos) <= 0.5f;                                        // If the amount of salt water at this position is less than 50%, call it freshwater and return true, otherwise return false.
            }
            return false;
        }
    }

    public partial class FishTrapItem : WorldObjectItem<FishTrapObject>
    {
        public override Type[] Blockers { get { return AllowWaterPlacement; } }
    }
}
