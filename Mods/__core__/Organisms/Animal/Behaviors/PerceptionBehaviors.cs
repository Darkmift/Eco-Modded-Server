// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.Organisms.Behaviors
{
    using Eco.Gameplay.AI;
    using Eco.Shared.States;
    using Eco.Simulation.Agents;
    using Eco.Simulation.Agents.AI;

    public static class PerceptionBehaviors
    { 
        public const float AlertIdleTime       = 2.5f;
        public const float AlertThreshold      = 50;
        public const float FullyAlertThreshold = 200;

        public static Behavior<Animal> Alert = BT.If("Alert", x => (x.Alertness > AlertThreshold &&
                                                                         x.Alertness < FullyAlertThreshold &&
                                                                         x.State != AnimalState.Flee &&
                                                                         x.State != AnimalState.AlertIdle &&
                                                                         x.OnFlatGround(), $"alert level ({x.Alertness}) is {AlertThreshold} - {FullyAlertThreshold} and not fleeing or alert idling"),
                                                                         Brain.Anim(AnimalState.AlertIdle, true, AlertIdleTime));
    }
}
