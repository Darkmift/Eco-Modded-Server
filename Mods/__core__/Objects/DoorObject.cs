// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Wires;
    using Eco.Shared.Networking;
    using Eco.Shared.Serialization;
    using System.Collections.Generic;
    using Eco.Shared.Utils;
    using Eco.Gameplay.Components;

    [Serialized, RequireComponent(typeof(DoorComponent))]
    public abstract class DoorObject : WorldObject, IWireContainer
    {
        WireInput     input;
        DoorComponent doorComp;

        IEnumerable<WireConnection> IWireContainer.Wires => this.input.SingleItemAsEnumerable();

        protected override void Initialize()     => this.doorComp = this.GetComponent<DoorComponent>();
        protected override void PostInitialize() => this.input = WireInput.CreateSignalInput(this, "Open Door", v => this.doorComp.ForceSetOpen(v == 0f ? false : true));

        public override void SendInitialState(BSONObject bsonObj, INetObjectViewer viewer)
        {
            base.SendInitialState(bsonObj, viewer);
            bsonObj["open"]     = this.doorComp.IsOpen;
            bsonObj["opensOut"] = this.doorComp.OpensOutwards;
        }
    }
}
