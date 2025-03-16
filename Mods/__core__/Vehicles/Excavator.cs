// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Eco.Core.Controller;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Mods.Organisms;
    using Eco.Shared.Networking;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.World.Blocks;

    [RequireComponent(typeof(VehicleScoopHandlerComponent))]
    public partial class ExcavatorObject : PhysicsWorldObject
    {
        public static Dictionary<Type, int> GetBlockStackSizeMap(int diggableSize, int minableSize)
        {
            var blockMap = new Dictionary<Type, int>();
            var blockItems = Item.AllItemsIncludingHidden.Where(x => x is BlockItem).Cast<BlockItem>();

            foreach (var item in blockItems.Where(x => x.OriginType.HasAttribute<Diggable>())) blockMap.Add(item.Type, diggableSize);
            foreach (var item in blockItems.Where(x => x.OriginType.HasAttribute<Minable>())) blockMap.Add(item.Type, minableSize);

            return blockMap;
        }

        protected override void PostInitialize() => this.GetComponent<VehicleToolComponent>().ToolControlOnMount = false; //We want the excavator to start driving, not using tool.
    }

    [RequireComponent(typeof(VehicleScoopHandlerComponent))]
    public partial class SkidSteerObject { }

    /// <summary>Handles shared vehicle scoop/bucket tool logic</summary>
    [Category("Hidden"), Serialized, NoIcon]
    public class VehicleScoopHandlerComponent : WorldObjectComponent
    {
        VehicleComponent vehicle;

        public override void PostInitialize()
        {
            this.vehicle = this.Parent.GetComponent<VehicleComponent>();
            this.Parent.GetComponent<VehicleToolComponent>().ScoopEvent += this.TryDestroyStump; // try destroy stump picked up by scoop
        }

        void TryDestroyStump(INetObject obj)
        {
            if (obj is TreeEntity tree && tree.IsStump && tree.TryDestroyStump(this.vehicle.Driver))
                this.Parent.GetComponent<ModularVehicleComponent>()?.VehicleToolItem?.UseDurability(2f, this.vehicle.Driver);
        }
    }
}
