// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.Organisms.SpeciesCatchers
{
    using System;
    using System.Numerics;
    using Eco.Core.Utils;
    using Eco.Gameplay.Players;
    using Eco.Mods.TechTree;
    using Eco.Shared.Utils;
    using Eco.Simulation;
    using Eco.Simulation.Types;
    using Eco.Gameplay.Animals.Catchers.Internal;

    /// <summary> Catcher to use with player fishing pole </summary>
    /// Unpon succsefull catch will send fish to client
    public class FishingPoleCatcher : UserLayeredCatchEntry
    {
        public override ThreadSafeList<string> DefaultTargetSpecies => new(CatchTarget.SmallFishTarget);

        public override bool OnValidationCheck()
        {
            //Check user and pole for active states, anything can be null at this point as its a delayed validity check
            var isFishingPoleActive = this.pole?.Lure?.Active ?? false;
            if (!isFishingPoleActive) return false;
            
            var isUserActive = this.User?.Player?.Active ?? false;
            if (!isUserActive) return false;

            return true;
        }

        public override TimeSpan NextCatchDelay => TimeSpan.FromSeconds(RandomUtil.Range(60, 90));
        protected override Range CatchRange => 0..1;    // can catch 0 to 1 chance each catch try

        FishingPoleItem pole;

        protected override bool ApplyCatch(Species species, int qt)
        {
            // fishing pole uses client side logic for catch, fish is spawned and reeled in
            var fishPos = (Vector3)this.pole.Lure.WorldPos() - Vector3.One; // spawn fish at pole position, but underwater (- Vector3.One correction)

            // send catch fish animation to player
            EcoSim.AnimalSim.SpawnAnimal((AnimalSpecies)species, fishPos, (animal) =>
            {
                this.pole.Lure.FishCaught(animal.ID);   // send fish to player
            });

            return true;
        }

        public FishingPoleCatcher(User user, FishingPoleItem pole) : base(user) { this.pole = pole;}
        public FishingPoleCatcher() { }
    }
}
