// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.Organisms.Behaviors
{
    using Eco.Gameplay.AI;
    using Eco.Shared.States;
    using Eco.Shared.Utils;
    using Eco.Simulation.Agents;
    using Eco.Simulation.Agents.AI;
    using Eco.Simulation.Time;

    public static class RelaxBehaviors
    {
        public const float MinIdleTime = 5f;
        public const float MaxIdleTime = 8f;
        public static Behavior<Animal> Idle    => Brain.Anim(AnimalState.Idle, true, _ => RandomUtil.Range(MinIdleTime, MaxIdleTime));
        public static Behavior<Animal> TryIdle => BT.If("Try Idle", x => (x.OnFlatGround(),
                                                                          "flat ground"),
                                                                          Idle);

        public const float ChanceToStopLyingDown = 0.1f;
        public static Behavior<Animal> RandomWake = BT.If("Random Wake", x => (RandomUtil.Chance(ChanceToStopLyingDown), $"{ChanceToStopLyingDown.ToString("P0")} random getup chance"),
                                                                         Brain.Anim(AnimalState.Idle, true, x => x.Species.TimeLayToStand + RandomUtil.Range(MinIdleTime, MaxIdleTime)));

        public const float LyingTickDuration = 30f;
        public static Behavior<Animal> TryLay = BT.If("Try Lay", x => (x.Alertness < Animal.FleeThreshold &&
                                                                      x.OnFlatGround(),
                                                                      $"{x.Alertness} alert < {Animal.FleeThreshold} flee and flat ground"),
                                                                      Brain.Anim(AnimalState.LyingDown, true, LyingTickDuration));

        public static Behavior<Animal> TrySleep = BT.If("Continue Sleeping", x => x.ShouldSleep,
                                                                             Brain.Anim(AnimalState.Sleeping, true, LyingTickDuration));



        public static (bool Sleep, string Description) DaySleeper => (!WorldTime.IsNight(), "is day (day sleeper");

        //Random behavior that wanders, lies, or idles.
        public static Behavior<Animal> Relax = 
            BT.Selector("Relax or Wander",
                BT.If("Should End Wander", //Check if animal didn't decide that it wants to relax after finishing last wander path
                    agent => agent.CheckAndConsumeDoNotContinueToWanderFlag() ? (true, "decided to stop") : (false, "decided to stop"),
                    BT.Random("Relax",
                        (TryLay,  1f),
                        (TryIdle, 1f))),
                BT.Random("Relax or Wander", //Otherwise just randomize behavior
                    (TryLay,                   1f), 
                    (TryIdle,                  1f),
                    (MovementBehaviors.Wander, 1f)));
    }
}
