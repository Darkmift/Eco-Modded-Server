// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Objects;

    [RequireComponent(typeof(BoatMooragePostComponent))]
    [RequireComponent(typeof(MustBeOwnedComponent))]
    public partial class WoodenMooragePostObject : WorldObject
    {
        protected override void PostInitialize()
        {
            this.GetComponent<BoatMooragePostComponent>().MaxAttachedBoats = 2;
            this.GetComponent<BoatMooragePostComponent>().MaxBoatSize = BoatComponent.BoatSize.Large;
        }
    }

    [RequireComponent(typeof(BoatMooragePostComponent))]
    [RequireComponent(typeof(MustBeOwnedComponent))]
    public partial class SteelMooragePostObject : WorldObject
    {
        protected override void PostInitialize()
        {
            this.GetComponent<BoatMooragePostComponent>().MaxAttachedBoats = 5;
            this.GetComponent<BoatMooragePostComponent>().MaxBoatSize = BoatComponent.BoatSize.Large;
        }
    }
}
