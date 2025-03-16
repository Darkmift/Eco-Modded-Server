// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.Components.VehicleModules
{
    using Eco.Core.Controller;
    using Eco.Gameplay.Animals;
    using Eco.Gameplay.Animals.Catchers;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Storage;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Utils;
    using Eco.Mods.Organisms.SpeciesCatchers;
    using Eco.Mods.TechTree;
    using Eco.Shared.Networking;
    using Eco.Shared.Serialization;
    using System;

    [Serialized]
    [RequireComponent(typeof(PublicStorageComponent), "FishingStorage")]
    [RequireComponent(typeof(ModularVehicleComponent))]
    [ForceCreateView, NoIcon]
    public class FishingComponent : WorldObjectComponent
    {
        [Serialized, SyncToView] public PublicStorageComponent Storage  { get; private set; }   // fishing storage
        [SyncToView]             public bool Fishing                    { get; private set; }   // fishing state                                                      

        public bool CanFish => (!this.VehicleMod.VehicleToolItem?.Broken ?? false) && this.VehicleMod.State == Shared.States.ModularVehicleToolState.Operating 
                                && this.Fishing && !this.Storage.Inventory.IsFull;

        static readonly Type[] SegmentTypeList = Array.Empty<Type>();                                                       // these will be moved to autogen, please ignore
        static readonly Type[] AttachmentTypeList = new Type[] { typeof(NylonTrawlerNetItem), typeof(FlaxTrawlerNetItem)};  // same

        public ModularVehicleComponent VehicleMod;

        public void Initialize(int numSlots, int maxWeight)
        {
            this.VehicleMod = this.Parent.GetComponent<ModularVehicleComponent>();
            this.VehicleMod.Initialize(0, 1, SegmentTypeList, AttachmentTypeList);  // will be moved to autogen, please ignore
            this.VehicleMod.UpdateVehicleToolItem();                                    

            this.Storage = this.Parent.GetComponent<PublicStorageComponent>("FishingStorage");
            this.Storage.Initialize(numSlots, maxWeight, new TagRestriction("Fish"));               // only allows fish tag
        }

        [RPC] public void SetFishing(Player player, bool state)
        {
            this.VehicleMod.SetState(state ? Shared.States.ModularVehicleToolState.Operating : Shared.States.ModularVehicleToolState.Disabled);
            this.Fishing = this.VehicleMod.State == Shared.States.ModularVehicleToolState.Operating;
            if (this.CanFish) SpeciesLayeredCatchPlugin.Obj.AddLayeredCatcher(this, new BoatCatcher(player.User, this.Parent, this.Parent.GetComponent<VehicleComponent>() , this));
        }

        [RPC] public void Catch(Player player, INetObject target)
        {
            if (target is not AnimalEntity animal) return;

            var resourceType = animal.Species.ResourceItemType;
            if (resourceType == null) return;

            if (!this.Storage.Inventory.TryAddItemNonUnique(resourceType, player.User).Notify(player)) return; // add to boat inventory
            animal.Destroy();
        }
    }
}
