// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.Organisms.Behaviors
{
    using System;
    using System.Linq;
    using Eco.Gameplay.AI;
    using Eco.Gameplay.Players;
    using Eco.Mods.TechTree;
    using Eco.Shared.Math;
    using Eco.Shared.Networking;
    using Eco.Shared.States;
    using Eco.Shared.Utils;
    using Eco.Simulation.Agents;
    using Eco.Simulation.Agents.AI;
    using Eco.Simulation.RouteProbing;
    using Eco.Simulation.Time;
    using Eco.World.Blocks;
    using Vector3 = System.Numerics.Vector3;

    public static class MovementBehaviors
    {
        public const float WanderMinDist  = 5f;
        public const float WanderMaxDist  = 20f;
        public const float FleeMinDist    = 10f;
        public const float FleeMaxDist    = 50f;
        public const float FleeMinAngle   = 0f;
        public const float FleeMaxAngle   = 90f;
        public const float GoHomeDistance = 80f; //2 animal world layer cells

        const float NextTickWhenTrapped = 5f; //Number of seconds abetween ticks when animal is trapped in a small area
        const int   MaxUnStuckDistance  = 20; //How far animal can move to get unstuck

        #region syntactic sugar

        public static Behavior<Animal> Wander            = new BehaviorBasic<Animal>("Wander",            (beh, agent) => WanderMovement(agent));
        public static Behavior<Animal> AmphibiousWander  = new BehaviorBasic<Animal>("AmphibiousWander",  (beh, agent) => AmphibiousWanderMovement(agent));
        public static Behavior<Animal> SwimWander        = new BehaviorBasic<Animal>("SwimWander",        (beh, agent) => SwimWanderMovement(agent));
        public static Behavior<Animal> SwimWanderSurface = new BehaviorBasic<Animal>("SwimWanderSurface", (beh, agent) => Swim(agent, Vector2.zero, true, AnimalState.SurfaceSwimming, true));
        
        //public static Behavior<Animal> ContinueWander           = new BehaviorBasic<Animal>("Continue To Wander",            (beh, agent) => ContinueToWander(agent, WanderMovement));
        //public static Behavior<Animal> ContinueAmphibiousWander = new BehaviorBasic<Animal>("Continue To Amphibious Wander", (beh, agent) => ContinueToWander(agent, AmphibiousWanderMovement));
        //public static Behavior<Animal> ContinueSwimWander       = new BehaviorBasic<Animal>("Continue To Swim Wander",       (beh, agent) => ContinueToWander(agent, SwimWanderMovement));

        public static Behavior<Animal> WanderHome            = new BehaviorBasic<Animal>("WanderHome",            (beh, agent) => LandMovement(agent, agent.GetDirectionHome(), true, AnimalState.Wander, WanderMinDist, WanderMaxDist, minDirectionOffsetDegrees: FleeMinAngle, maxDirectionOffsetDegrees: FleeMaxAngle, requireFlat: true));
        public static Behavior<Animal> AmphibiousWanderHome  = new BehaviorBasic<Animal>("AmphibiousWanderHome",  (beh, agent) => AmphibiousMovement(agent, agent.GetDirectionHome(), true, AnimalState.WanderHome, requireFlat: true));
        public static Behavior<Animal> SwimWanderHome        = new BehaviorBasic<Animal>("SwimWanderHome",        (beh, agent) => Swim(agent, agent.GetDirectionHome(), true, AnimalState.WanderHome, false));
        public static Behavior<Animal> SwimWanderHomeSurface = new BehaviorBasic<Animal>("SwimWanderHomeSurface", (beh, agent) => Swim(agent, agent.GetDirectionHome(), true, AnimalState.WanderHome, true));

        public static Behavior<Animal> Flee            = new BehaviorBasic<Animal>("Flee",            (beh, agent) => agent.ShouldFlee(beh) ? FleeMovement(agent)           : BTResult.Failure());
        public static Behavior<Animal> AmphibiousFlee  = new BehaviorBasic<Animal>("AmphibiousFlee",  (beh, agent) => agent.ShouldFlee(beh) ? AmphibiousFleeMovement(agent) : BTResult.Failure());
        public static Behavior<Animal> SwimFlee        = new BehaviorBasic<Animal>("SwimFlee",        (beh, agent) => agent.ShouldFlee(beh) ? SwimFleeMovement(agent)       : BTResult.Failure());
        public static Behavior<Animal> SwimFleeSurface = new BehaviorBasic<Animal>("SwimFleeSurface", (beh, agent) => agent.ShouldFlee(beh) ? Swim(agent, agent.GetFleeDirectionWithChanceToStopFleeing().XZ(), false, AnimalState.Flee, true, 30) : BTResult.Failure());
        
        //public static Behavior<Animal> ContinueFlee           = new BehaviorBasic<Animal>("Continue To Flee",            (beh, agent) => ContinueToFlee(agent, FleeMovement));
        //public static Behavior<Animal> ContinueAmphibiousFlee = new BehaviorBasic<Animal>("Continue To Amphibious Flee", (beh, agent) => ContinueToFlee(agent, AmphibiousFleeMovement));
        //public static Behavior<Animal> ContinueSwimFlee       = new BehaviorBasic<Animal>("Continue To Swim Flee",       (beh, agent) => ContinueToFlee(agent, SwimFleeMovement));

        public static Behavior<Animal> MoveToWater = new BehaviorBasic<Animal>("Move To Water", (beh, agent) => RouteToWater(agent, true, AnimalState.Wander));
        #endregion

        public static (bool Test, string Result) ShouldReturnHome(Animal agent)
        {
            var distHome = Vector2.WrappedDistance(agent.Position.XZi(), agent.WorldHomePos);
            return (distHome > GoHomeDistance, $"distance from home ({distHome}) > {GoHomeDistance}");
        }

        public static Behavior<Animal> TryReturnHome => BT.If("Try Return Home", ShouldReturnHome, WanderHome);

        public static BTResult MoveTo(Animal agent, Vector3 target, bool wandering)
        {
            var routeProps = new RouteProperties { MaxTargetLocationHeightDelta = agent.Species.ClimbHeight };
            var route = AIUtil.GetRouteFacingTarget(agent.FacingDir, agent.Position, target, agent.Species.GetTraversalData(wandering), agent.Species.HeadDistance * 2, routeProps: routeProps, allowBasic: false);
            if (!route.IsValid) return BTResult.Failure("Can't build route");

            // Prevent setting route with 0 time length
            var timeToFinishRoute = agent.SetRoute(route, wandering ? AnimalState.Wander : AnimalState.Flee);
            if (timeToFinishRoute < float.Epsilon) return BTResult.Failure("route not set");
            return BTResult.RunningChanged("Moving to target");
        }

        /// <param name="delay">Start of the route is delayed by this number of seconds by adding them to initial rotation time.</param>
        static BTResult FleeMovement          (Animal agent, float delay = 0f, Vector3? startPosition = null) => LandMovement      (agent, agent.GetFleeDirection().XZ(), false, AnimalState.Flee, FleeMinDist, FleeMaxDist, FleeMinAngle, FleeMaxAngle, 20, delay: delay, startPosition: startPosition);
        static BTResult SwimFleeMovement      (Animal agent, float delay = 0f, Vector3? startPosition = null) => Swim              (agent, agent.GetFleeDirection().XZ(), false, AnimalState.Flee, false, 30, delay: delay, startPosition: startPosition);
        static BTResult AmphibiousFleeMovement(Animal agent, float delay = 0f, Vector3? startPosition = null) => AmphibiousMovement(agent, agent.GetFleeDirection().XZ(), false, AnimalState.Flee, delay: delay, startPosition: startPosition);

        /// <param name="delay">Start of the route is delayed by this number of seconds by adding them to initial rotation time.</param>
        static BTResult WanderMovement          (Animal agent, float delay = 0f, Vector3? startPosition = null) => LandMovement      (agent, Vector2.zero, true, AnimalState.Wander, WanderMinDist, WanderMaxDist, minDirectionOffsetDegrees: FleeMinAngle, maxDirectionOffsetDegrees: FleeMaxAngle, delay: delay, startPosition: startPosition, requireFlat: true);
        static BTResult SwimWanderMovement      (Animal agent, float delay = 0f, Vector3? startPosition = null) => Swim              (agent, Vector2.zero, true, AnimalState.Diving, false, delay: delay, startPosition: startPosition);
        static BTResult AmphibiousWanderMovement(Animal agent, float delay = 0f, Vector3? startPosition = null) => AmphibiousMovement(agent, Vector2.zero, true, AnimalState.Wander, delay: delay, startPosition: startPosition, requireFlat: true);

        /// <param name="delay">Start of the route is delayed by this number of seconds by adding them to initial rotation time.</param>
        /// <param name="requireFlat">When true only flat ground will be considered walkable.</param>
        public static BTResult LandMovement(Animal agent,
                                            Vector2 direction,
                                            bool wandering,
                                            AnimalState state,
                                            float minDistance = 1f,
                                            float maxDistance = 20f,
                                            float minDirectionOffsetDegrees = 0,
                                            float maxDirectionOffsetDegrees = 360,
                                            int tryCount = 10,
                                            bool skipRouteProperties = false,
                                            float delay = 0f,
                                            Vector3? startPosition = null,
                                            bool requireFlat = false)
        {
            if (agent == null || agent.AiTarget == null) return BTResult.Failure("Agent is null");
            if (!agent.CanInterruptMovement) return BTResult.Failure("Cannot interrupt current movement");

            const float stepMultiplier = 1.25f; //multiply by maxDistance from animal position to dertermine maxSteps we need to take before A* stops search
            var routeProps = skipRouteProperties ? null : new RouteProperties
            {
                MaxTargetLocationHeightDelta = agent.Species.ClimbHeight,
                MinDirectionOffsetDegrees = minDirectionOffsetDegrees,
                MaxDirectionOffsetDegrees = maxDirectionOffsetDegrees,
                RotationBasedPath = agent.GetType() == typeof(Elk) //TODO: move to tech tree
            };

            var groundPos = startPosition ?? agent.GroundPosition;
            var maxSteps  = (int)MathUtil.Clamp(maxDistance * stepMultiplier, 5, 50);
            var search    = RouteProbingUtils.FindRandomGroundRoute(agent.FacingDir, groundPos, minDistance, maxDistance, agent.FacingDir.XZ().Normalized, direction, routeProps, tryCount, maxSteps: maxSteps, requireFlat: requireFlat);
            
            if (search != null)
            {
                var smoothed = search.LineOfSightSmoothGroundPosition(groundPos);
                var route    = RouteFactory.Create(agent.Species.GetTraversalData(wandering), agent.FacingDir, smoothed, routeProps?.RotationBasedPath ?? false); //TODO: remove route props usage

                //Apply route if its need more than 0 time to finish
                if (route.TravelTime() > float.Epsilon)
                {
                    agent.SetRoute(route, state, delay: delay);
                    agent.RemoveMemory(Animal.TriesToUnStuckMemory);
                    return BTResult.RunningChanged($"Picked new point.");
                }
            }

            agent.SetMemory(Animal.ShouldFleeTillMemory, -1);
            return LandAnimalUnStuckOrDie(agent);
        }

        /// <summary>Finds target swim position taking into account species characteristics like if it can swim near coast and can float on surface.</summary>
        static BTResult FindTargetSwimPosition(Animal agent, Vector2 direction, bool surface, int tries, out Vector3i targetPosition, Vector3? startPosition = null)
        {
            var result = BTResult.Failure("failed to find target swim position.");
            var position = startPosition.TryGetValue(out var startPos) ? startPos + Vector3.UnitY /*convert ground pos to agent pos*/ : agent.Position;
            var originalPosition = position.XYZi();
            var options = AIUtil.FindTargetSwimPositions(position, 5.0f, 20.0f, direction, 90, 360, tries, surface);
            foreach (var pos in options)
            {
                targetPosition = pos.XYZi();
                if (targetPosition == originalPosition) continue;
                // Avoid fish swimming too close to coast line
                // TODO: Avoid building routes near coast, cache available points far away from coast
                if (!agent.Species.CanSwimNearCoast)
                {
                    var waterHeight = World.World.GetWaterHeight(targetPosition.XZ);
                    var isNearCoast = WorldPosition3i.TryCreate(targetPosition, out var packedTargetPosition) && packedTargetPosition.SpiralOutXZIter(3).Any(groundPos => World.World.GetBlock((WrappedWorldPosition3i)groundPos.X_Z(waterHeight)).Is<Solid>());
                    if (isNearCoast)
                    {
                        result = BTResult.Failure("target position is too close to coast line");
                        continue;
                    }
                }

                var targetBlock = World.World.GetBlock(targetPosition);
                // If an animal can't float on water surface - move it a block below highest water block TODO: make them move on underwater ground
                // TODO: Remove after pathfinder improvements
                //if (!agent.Species.FloatOnSurface && targetBlock is IWaterBlock && World.World.GetBlock(targetPosition + Vector3i.Up).Is<Empty>())
                //    targetPosition += Vector3i.Down;
                /*if (!targetBlock.Is<Empty>())
                {
                    //targetPosition += Vector3i.Down;
                    // Fail if target position is too shallow
                    if (World.World.GetBlock(targetPosition).Is<Solid>())
                    {
                        result = BTResult.Failure("target position is too thin");
                        continue;
                    }
                }*/

                // all checks passed, return succeed result and current target position
                return BTResult.Success("target position found");
            }

            targetPosition = default;
            return result;
        }

        public static BTResult Swim(Animal agent, Vector2 direction, bool wandering, AnimalState state, bool surface, int tries = 10, float delay = 0f, Vector3? startPosition = null)
        {
            var result = FindTargetSwimPosition(agent, direction, surface, tries, out var targetPos, startPosition);
            if (result.Status != BTStatus.Success)
                return result;

            //What for?
            // Clamp current position to ground or water, if can't float on water surface - stay below water height TODO: make them move on underwater ground
            //var pos = World.World.ClampToWaterHeight(agent.Position.XYZi());

            //For swimming  we need to compensate Y coordinate, since route is built from positions of the ground below + Y
            var pos = startPosition.TryGetValue(out var startPos) ? startPos : agent.Position.XYZi() + Vector3i.Down;

            targetPos += Vector3i.Down;

            var route = RouteFactory.CreateBasic(agent.Species.GetTraversalData(wandering), agent.FacingDir, pos, targetPos);
            var routeTime = route.IsValid ? agent.SetRoute(route, state, null, delay) : 0f;
            return routeTime < float.Epsilon ? BTResult.Failure("route not set") : BTResult.Success($"swimming path");
        }

        /// <param name="requireFlat">When true only flat ground will be considered walkable.</param>
        public static BTResult AmphibiousMovement(Animal agent, Vector2 generalDirection, bool wandering, AnimalState state, float minDistance = 2f, float maxDistance = 20f, float delay = 0f, Vector3? startPosition = null, bool requireFlat = false)
        {
            if (!WorldPosition3i.TryCreate(startPosition.TryGetValue(out var startPos) ? startPos : agent.Position - Vector3.UnitY, out var startPathPos))
                return LandMovement(agent, generalDirection, wandering, state, minDistance, maxDistance, delay: delay, startPosition: startPosition, requireFlat: requireFlat);

            generalDirection = generalDirection == Vector2.zero ? Vector2.right.Rotate(RandomUtil.Range(0f, 360)) : generalDirection.Normalized;

            // Take random ground position in the given direction
            var targetGround = ((Vector3)startPathPos + (generalDirection * RandomUtil.Range(minDistance, maxDistance)).X_Z()).WorldPosition3iOrInvalid();
            //if target position is in water make sure animal is on wate surface, otherwise look for nearest walkable block on ground and set it to new target
            if (World.World.GetBlock((WrappedWorldPosition3i)targetGround).IsWater())
                targetGround.Y = World.World.MaxWaterHeight[targetGround];
            else
            {
                targetGround = RouteManager.NearestWalkableXYZ(targetGround, 5);
                if (!agent.Species.FloatOnSurface && !targetGround.IsValid)
                {
                    agent.Floating = false;
                    return BTResult.Failure("Can't float, continue swimming");
                }
            }

            //TODO make sure that route to surface for amphis is checking Solid block on top of the water.

            // This is a low-effort search that includes water surface and should occasionally fail, just pick a semi-random node that was visited when it fails
            var routeProps = new RouteProperties() { MaxTargetLocationHeightDelta = agent.Species.ClimbHeight };
            var allowWaterSearch = new AStarSearch(RouteCacheData.NeighborsIncludeWater, agent.FacingDir, startPathPos, targetGround, 1000, 40, routeProps, false);
            if (allowWaterSearch.Status != SearchStatus.PathFound)
            {
                targetGround = allowWaterSearch.GroundNodes.Last().Key;
                allowWaterSearch.GetPathToWaterPos(targetGround);
            }

            // Apply land movement only for land positions
            if (!World.World.GetBlock((Vector3i)startPathPos).IsWater())
            {
                if (allowWaterSearch.GroundPath.Count < 2)
                    return LandMovement(agent, generalDirection, wandering, state, minDistance, maxDistance, skipRouteProperties: true, delay: delay, startPosition: startPosition, requireFlat: requireFlat);
                if (allowWaterSearch.Status == SearchStatus.Unpathable && allowWaterSearch.GroundNodes.Count < RouteRegions.MinimumRegionSize)
                {
                    // Search region was unexpectedly small and agent is on land, might be trapped by player construction.
                    // Try regular land movement so region checks can apply & the agent can get unstuck (or die)
                    return LandMovement(agent, generalDirection, wandering, state, minDistance, maxDistance, delay: delay, startPosition: startPosition, requireFlat: requireFlat);
                }
            }

            var smoothed = allowWaterSearch.LineOfSightSmoothWaterPosition((Vector3)startPathPos);
            var route = RouteFactory.Create(agent.Species.GetTraversalData(wandering), agent.FacingDir, smoothed);
            if (!route.IsValid) route = RouteFactory.CreateBasic(agent.Species.GetTraversalData(wandering), agent.FacingDir, startPosition.TryGetValue(out startPos) ? startPos : agent.GroundPosition, route.EndPosition);
            var routeTime = agent.SetRoute(route, state, null, delay);
            return routeTime < float.Epsilon ? BTResult.Failure("route not set") : BTResult.Success($"swimming path");
        }

        public static BTResult RouteToWater(Animal agent, bool wandering, AnimalState state)
        {
            if (WorldPosition3i.TryCreate(agent.Position, out var start))
            {
                // only works for surface start points
                if (World.World.GetBlock((WrappedWorldPosition3i)start).IsWater()) return BTResult.Failure("Already underwater.");
                if (start.Y != World.World.MaxYCache[start] + 1) return BTResult.Failure("Not on world surface.");
                if (RouteManager.RouteToSeaMap == null) return BTResult.Failure("Missing sea route map.");

                agent.ChangeState(state, 0, false);

                // get a destination a decent
                var current = start;
                for (int i = 0; i < 10; i++)
                {
                    var next = RouteManager.RouteToSeaMap[current];
                    if (next == current) break;
                    current = next;
                    if (WorldPosition3i.Distance(current, start) > 10)
                        break;
                }

                var target = ((Vector2)WorldPosition3i.GetDelta(current, start).XZ).Rotate(RandomUtil.Range(-20, 20));
                return AmphibiousMovement(agent, target, wandering, state, 10, 20);
            }

            return BTResult.Failure("Invalid start pos");
        }

        // If the animal is stuck somewhere it shouldn't be help it escape or die if its very far from walkable terrain
        public static BTResult LandAnimalUnStuckOrDie(Animal agent)
        {
            var worldPos     = (agent.Position - Vector3.UnitY).WorldPosition3iOrInvalid(); //Convert agent position to ground position
            var nearestValid = RouteManager.NearestWalkableXYZ(worldPos, MaxUnStuckDistance, RouteRegions.AnyRegion);
            
            if (nearestValid == worldPos)
            {
                agent.NextTick = WorldTime.Seconds + NextTickWhenTrapped;
                return BTResult.Failure("Unable to find any valid path.");
            }

            if (!nearestValid.IsValid)
            {
                // cheating failed? time to die!
                agent.Kill();
                return BTResult.Success();
            }
            // ignore terrain, path directly to a valid area, but only if noone is around *shy* or if he's really trying
            if (agent.TryGetMemory(Animal.TriesToUnStuckMemory, out int tries)) tries += 1;
            agent.SetMemory(Animal.TriesToUnStuckMemory, tries);
            if (!NetObjectManager.Default.GetObjectsWithin(agent.Position.XZ(), 20).OfType<Player>().Any() || tries >= 3)
            {
                var route = AIUtil.GetRoute(agent.FacingDir, agent.Position, (Vector3)nearestValid, agent.Species.GetTraversalData(true), null, null);
                if (!route.IsValid)
                {
                    agent.NextTick = WorldTime.Seconds + 10;
                    return BTResult.Failure("Failed to find a path to unstuck.");
                }

                // Proceed with route with time length more than 0
                var timeToFinishRoute = route.TravelTime();
                if (timeToFinishRoute > float.Epsilon)
                {
                    agent.SetRoute(route, AnimalState.Wander);
                    agent.RemoveMemory(Animal.TriesToUnStuckMemory);
                    return BTResult.Success();
                }
            }

            agent.NextTick = WorldTime.Seconds + 10;
            return BTResult.Failure("Failed to unstuck.");
        }

        public static BTResult ContinueToFlee(Animal agent, Func<Animal, float, Vector3?, BTResult> continuation, float continuationChance = Animal.DefaultChanceToStopFleeing)
        {
            const float minEarlyTickDistanceSq = (Animal.EarlyTickDistance * 1.2f) * (Animal.EarlyTickDistance * 1.2f); //Add 20% margin of error just in case

            if (agent.State != AnimalState.Flee)                                               return BTResult.Failure($"Not fleeing (current state: {agent.State}).");
            if (!agent.ShouldFlee())                                                           return BTResult.Failure($"Not fleeing ({nameof(Animal.ShouldFlee)} returned false).");
            if (!agent.RouteDestination.TryGetValue(out var destination))                      return BTResult.Failure("No route set.");
            if (Vector3.DistanceSquared(destination, agent.Position) > minEarlyTickDistanceSq) return BTResult.Failure("Too far from the end of the current route.");
            
            //If there is no need to keep fleeing do a random check to determine if we want to flee a bit longer anyway or if we should stop
            if (agent.FleeTarget == null && RandomUtil.Chance(1f - continuationChance) && (!agent.TryGetMemory(Animal.ShouldFleeTillMemory, out double fleeTill) || WorldTime.Seconds > fleeTill))
                EndFlee(agent, "Decided to stop fleeing, allowing current route to finish.");

            var result = continuation(agent, agent.RemainingPathTime, agent.RouteDestination + -Vector3.UnitY /*Convert agent position to ground position*/); //Try to continue fleeing

            return result.Status >= BTStatus.Success ? result : EndFlee(agent, $"Failed to flee, reason: \"{result.Msg}\". Allowing current route to finish."); //Continue to flee on success, otherwise give up

            static BTResult EndFlee(Animal agent, string message)
            {
                //Exit flee state, but finish current route
                agent.StopFlee();
                agent.NextTick = agent.DestinationReachTime; //Tick next time when end of current route is reached
                return BTResult.Success(message);            //Return success to not run any more logic this tick
            }
        }

        public static BTResult ContinueToWander(Animal agent, Func<Animal, float, Vector3?, BTResult> continuation, float continuationChance = 1f / 3f)
        {
            const float minEarlyTickDistanceSq = (Animal.EarlyTickDistance * 1.2f) * (Animal.EarlyTickDistance * 1.2f); //Add 20% margin of error just in case

            if (agent.State != AnimalState.Wander)                                                  return BTResult.Failure("Not wandering.");
            if (!agent.RouteDestination.TryGetValue(out var destination))                           return BTResult.Failure("No route set.");
            if (Vector3.DistanceSquared(destination, agent.Position) > minEarlyTickDistanceSq)      return BTResult.Failure("Too far from the end of the current route.");
            if (agent.TryGetMemory<bool>(Animal.DoNotContinueToWanderMemory, out var stop) && stop) return BTResult.Failure("Already decided to not continue.");
            if (agent.ShouldFlee())                                                                 return BTResult.Failure("Animal should flee.");
            if (agent.OnFlatGround() && RandomUtil.Chance(1f - continuationChance))                 return EndWander(agent, "Decided to stop wandering, allowing current route to finish."); //If we are on flat ground and that random check passes, we stop wandering

            var result = continuation(agent, agent.RemainingPathTime, agent.RouteDestination + -Vector3.UnitY /*Convert agent position to ground position*/); //Try to continue wandering

            return result.Status >= BTStatus.Success ? result : EndWander(agent, $"Failed to wander, reason: \"{result.Msg}\". Allowing current route to finish."); //Continue to wander on success, otherwise give up

            static BTResult EndWander(Animal agent, string message)
            {
                agent.SetMemory(Animal.DoNotContinueToWanderMemory, true);
                agent.NextTick = agent.DestinationReachTime; //Tick next time when end of current route is reached
                return BTResult.Success(message);            //Return success to not run any more logic this tick
            }
        }
    }
}
