﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.Organisms
{
    using System;
    using System.Collections.Generic;
    using Eco.Core.Items;
    using Eco.Gameplay.AI;
    using Eco.Gameplay.Animals;
    using Eco.Gameplay.GameActions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Mods.TechTree;
    using Eco.Shared.Localization;
    using Eco.Shared.Networking;
    using Eco.Shared.SharedTypes;
    using Eco.Shared.States;
    using Eco.Shared.Utils;
    using Eco.Simulation.Agents;
    using Eco.Simulation.Agents.AI;
    using Eco.Simulation.Types;
    using Range = Eco.Shared.Math.Range;
    using Vector3 = System.Numerics.Vector3;

    public class Tortoise : AnimalEntity
    {
        public Tortoise(Animal parent, Vector3 pos, bool corpse = false) : base(parent, pos, species, corpse) { }
        static AnimalSpecies species;

        [Ecopedia("Animals", "Reptiles", true)]
        public class TortoiseSpecies : AnimalSpecies
        {
            public TortoiseSpecies() : base()
            {
                species = this;
                this.InstanceType = typeof(Tortoise);

                // Info
                this.Name = "Tortoise";
                this.DisplayName = Localizer.DoStr("Tortoise");
                this.DisplayDescription = Localizer.DoStr("A slow-moving land-dwelling reptile with a hard, domed shell that provides protection from predators. Tortoises are herbivorous and have a long lifespan, with some individuals living up to 150 years or more.");
                // Lifetime
                this.MaturityAgeDays = 1f;
                // Food
                this.FoodSources = new List<System.Type>() { typeof(CreosoteBush), typeof(PricklyPear), typeof(Agave) };
                this.CalorieValue = 50f;
                // Movement
                this.Flying = false;
                this.Swimming = false;
                // Resources
                this.ResourceList = new List<SpeciesResource>()
                {
                    new SpeciesResource(typeof(RawMeatItem), new Range(1f, 2f)),
                };
                this.ResourceBonusAtGrowth = 0.9f;
                // Behavior
                this.BrainType = typeof(Brain<Tortoise>);
                this.WanderingSpeed = 0.1f;
                this.Speed = 0.1f;
                this.ClimbHeight = 0;
                this.Health = 1f;
                this.Damage = 1f;
                this.DelayBetweenAttacksRangeSec = new Range(0.8f, 2.5f);
                this.FearFactor = 1f;
                this.FleePlayers = true;
                this.AttackRange = 1f;
                // Climate
                this.ReleasesCO2TonsPerDay = 0.02f;

            }
        }

        // tortoise behavior
        public static readonly Behavior<Animal> TortoiseTreeRoot;
        static Tortoise()
        {
            const float MinHidingTime = 5f;
            const float MaxHidingTime = 10f;

            TortoiseTreeRoot =
                BT.Selector("Tortoise Brain",
                    // Tortoise hide instead of fleeing, fixed time to block pointless flee interruptions
                    BT.If("Try Hide", x => (x.Alertness > Animal.FleeThreshold, $"{x.Alertness} > {Animal.FleeThreshold}"),
                        Brain.Anim(AnimalState.Hiding, true, _ => RandomUtil.Range(MinHidingTime, MaxHidingTime))),
                    LandAnimalBrain.LandAnimalTreeRoot);

            Brain<Tortoise>.SharedBehavior = TortoiseTreeRoot;
        }
        
        public static float BlockDamage(INetObject damager, float damage, AnimalEntity entity )
        {
            // turtle power! (or uhh tortoise power!)
            if (entity.State == AnimalState.Hiding)
            {
                damage /= 4;
                // TODO: sound effect of arrow bouncing off
                if (damager is Player damagerPlayer) damagerPlayer.MsgLoc($"{entity.Species.Name}: Arrow damage mitigated by tortoise shell!");
            }

            return damage;
        }

        public override GameActionPack TryApplyDamage(GameActionPack pack, INetObject damager, float damage, InteractionTarget target, Item tool, out float damageReceived, Type damageDealer = null, float experienceMultiplier = 1f)
        {
            damage = BlockDamage(damager, damage, this);
            return base.TryApplyDamage(pack, damager, damage, target, tool, out damageReceived, damageDealer, experienceMultiplier);
        }
    }
}
