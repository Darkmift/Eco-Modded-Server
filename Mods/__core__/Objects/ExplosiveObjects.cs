// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.Explosions;
    using Eco.Gameplay.Objects;

    [RequireComponent(typeof(ExplosionComponent))]
    [RequireComponent(typeof(StatusComponent))]
    [RequireComponent(typeof(AuthComponent))]
    public partial class DynamiteObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<ExplosionComponent>().Initialize(new ExplosionConfig()
            {
                ExplosionRadius   = 5,
                FuseTime          = 4,
                CraterRadius      = 4,
                PollutionTons     =0.4f,
                CaloriesBurn      = 300f,
                BlockFallConfig   = new BlockFallConfig()
                {
                    Enabled       = true,
                    BlockFilter   = BlockFallProcessType.All,
                    IterationsMax = 50,
                    MovedDeltaMin = 4
                },
            });
        }
    }

    [RequireComponent(typeof(ExplosionComponent))]
    [RequireComponent(typeof(ExplosionLinkComponent))]
    [RequireComponent(typeof(StatusComponent))]
    [RequireComponent(typeof(AuthComponent))]
    public partial class MiningChargeObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<ExplosionComponent>().Initialize(new ExplosionConfig()
            {
                ExplosionRadius   = 6,
                FuseTime          = 2,
                CraterRadius      = 5,
                PollutionTons     = 0.2f,
                CaloriesBurn      = 500f,
                BlockFallConfig   = new BlockFallConfig()
                {
                    Enabled       = true,
                    BlockFilter   = BlockFallProcessType.All,
                    IterationsMax = 50,
                    MovedDeltaMin = 5
                },
            }, canUseManually: false);
            this.GetComponent<ExplosionLinkComponent>().Initialize(0);
        }
    }
}
