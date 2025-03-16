// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Objects;

    public partial class IndustrialBargeObject : PhysicsWorldObject
    {
        public override float Priority => base.Priority - 1;    // barges need higher priority to always spawn before other vehicles they carry
    }

    public partial class WoodenBargeObject : PhysicsWorldObject
    {
        public override float Priority => base.Priority - 1;    // barges need higher priority to always spawn before other vehicles they carry
    }
}
