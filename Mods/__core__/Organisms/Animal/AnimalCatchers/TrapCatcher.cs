// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.Organisms.SpeciesCatchers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Eco.Core.Utils;
    using Eco.Gameplay.Animals;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Mods.Components.VehicleModules;
    using Eco.Mods.TechTree;
    using Eco.Shared.Utils;
    using Eco.Simulation.WorldLayers.Layers;
    using Eco.Gameplay.Animals.Catchers.Internal;

    /// <summary>Shared readonly target lists for <see cref="LayeredCatchEntry.DefaultTargetSpecies"/></summary>
    public static class CatchTarget
    {
        // easy acess for small fish
        public static readonly List<string> SmallFishTarget = new()
        {
            nameof(Salmon),
            nameof(Trout),
            nameof(Tuna),
            nameof(PacificSardine),
            nameof(Cod),
            nameof(Bass),
        };

        // only one type of crab
        public static readonly List<string> CrabTarget = new() { nameof(Crab) };
    }

    /// <summary> Catcher to use with traps </summary>
    public abstract class TrapCatcher : WorldObjectLayeredCatchEntry
    {
        public override Inventory TargetInventory => this.trap.Storage.Inventory;
        public override bool OnValidationCheck() => base.OnValidationCheck() && this.trap.Enabled && !this.trap.Storage.Inventory.IsFull;
        protected override Range CatchRange => 0..1;

        readonly AnimalTrapComponent trap;

        public TrapCatcher(User user, WorldObject obj) : base(user, obj)
        {
            this.trap = obj.GetComponent<AnimalTrapComponent>();
        }
        public TrapCatcher() { }
    }

    public class CrabCatcher : TrapCatcher
    {
        public override ThreadSafeList<string> DefaultTargetSpecies => new (CatchTarget.CrabTarget);
        public override TimeSpan NextCatchDelay                     => TimeSpan.FromMinutes(RandomUtil.Range(5, 15));
        public CrabCatcher(User user, WorldObject obj) : base(user, obj) { }
        public CrabCatcher() { }
    }

    public class FishCatcher : TrapCatcher
    {
        public override ThreadSafeList<string> DefaultTargetSpecies => new(CatchTarget.SmallFishTarget);
        public override TimeSpan NextCatchDelay                     => TimeSpan.FromMinutes(RandomUtil.Range(5, 10));

        public FishCatcher(User user, WorldObject obj) : base(user, obj) { }
        public FishCatcher() { }
    }
}
