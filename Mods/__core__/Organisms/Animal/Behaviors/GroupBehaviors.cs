// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.Organisms.Behaviors
{
    using Eco.Shared.Utils;
    using Eco.Shared.States;
    using Eco.Simulation.Agents;
    using Eco.Simulation.Agents.AI;
    using System.Collections.Generic;
    using Eco.Shared.Networking;
    using System.Linq;
    using Eco.Shared.Math;
    using Eco.Shared.Voxel;
    using Eco.Simulation.RouteProbing;
    using Vector3 = System.Numerics.Vector3;
    using Eco.Shared.Time;

    // This is a pretty last-minute addition that could use some refinement, should be a good place for modders to start experimenting with animal behaviors
    public class GroupBehaviors
    {
        const string IsLeader             = "Leader";            // Leader memory contains list of followers
        const string IsFollower           = "Follow";            // Follower memory points to leader
        const string IsAlone              = "Alone";             // Next time to check for a pack
        const float TimeBetweenHerdChecks = 20f;

        public static Behavior<Animal> StayNearLeader(float maxDistance) { return new BehaviorBasic<Animal>("Stay Near Leader", (beh, agent) => StayNearLeader(agent, maxDistance)); }
        public static BTResult StayNearLeader(Animal agent, float maxDistance)
        {
            var leader = GetLeader(agent);
            if (leader == null) return BTResult.Failure("No leader");
            if (leader == agent) return BTResult.Failure("We are the leader");

            var leaderPosition = leader.Position;
            var dist = World.WrappedDistance(agent.Position, leaderPosition);
            if (dist > maxDistance)
            {
                // try to stay near alpha
                return MovementBehaviors.LandMovement(agent, Vector3Extensions.XZWrappedDirectionalVector(agent.Position, leaderPosition).XZ(),
                    leader.State != AnimalState.Flee,
                    AnimalState.Wander,
                    minDirectionOffsetDegrees: MovementBehaviors.FleeMinAngle,
                    maxDirectionOffsetDegrees: MovementBehaviors.FleeMaxAngle).Append($"Leader {dist:0.#}m away");
            }
            return BTResult.Failure($"Leader close enough: {dist}m");
        }

        public static Behavior<Animal> SleepNearLeader = new BehaviorBasic<Animal>("Sleep Near Leader", DoSleepNearLeader);
        static BTResult DoSleepNearLeader(Behavior<Animal> beh, Animal agent)
        {
            var leader = GetLeader(agent);
            if (leader == null) return BTResult.Failure("No leader");
            if (leader == agent) return BTResult.Failure("We are leader");

            //If the leader is sleeping...
            if (leader.State == AnimalState.Sleeping)
            {
                // and we're already close enough, sleep now.
                if (World.WrappedDistance(agent.Position, leader.Position) < agent.Species.HeadDistance * 3)
                    return agent.ChangeState(AnimalState.LyingDown, 60f, true);

                //Otherwise, move to them to sleep.
                // TODO: avoid overlapping with other herd members, make the leader pick a spot to sleep that can accommodate the herd
                var routeProps = new RouteProperties {MaxTargetLocationHeightDelta = agent.Species.ClimbHeight};
                var route      = AIUtil.GetRouteFacingTarget(agent.FacingDir, agent.Position, leader.Position, agent.Species.GetTraversalData(true), agent.Species.HeadDistance * 2, routeProps: routeProps);
                if (!route.IsValid) return BTResult.Failure("can't get route to leader");
                agent.SetRoute(route, AnimalState.Wander);
                return BTResult.Success("walking to sleep near leader");
            }
            return BTResult.Failure("leader not sleeping");
        }

        public static List<Animal> GetHerdList(Animal agent)
        {
            agent.TryGetMemory(IsFollower, out Animal leader);
            (leader ?? agent).TryGetMemory(IsLeader, out List<Animal> herdList);
            return herdList;
        }

        public static void SyncFleePosition(Animal agent, double fleeDuration = Animal.DefaultFleeDuration)
        {
            if (agent.FleePosition != Vector3.Zero && agent.Alertness > Animal.FleeThreshold)
            {
                var herdList = GetHerdList(agent);
                if (herdList != null)
                    foreach (var other in herdList)
                        if (other.FleePosition == Vector3.Zero && World.WrappedDistance(other.Position, agent.Position) < agent.DetectionRange)
                        {
                            // flee in same direction
                            if (other.Alertness < Animal.FleeThreshold)
                                other.Alertness = agent.Alertness;
                            other.SetFleePositionAndDuration(other.Position + Vector3Extensions.XZWrappedDirectionalVector(agent.Position, agent.FleePosition), fleeDuration);
                            other.ForceTick();
                        }
            }
        }

        public static bool HaveAdjacentHomePositions(Animal one, Animal two)
        {
            return World.WrappedDistance(one.WorldHomePos.X_Z(), two.WorldHomePos.X_Z()) < 2 * one.Species.VoxelsPerEntry;
        }

        // Groups form and dissolve along with animal visibility
        public static Animal GetLeader(Animal agent)
        {
            if (agent.TryGetMemory(IsAlone, out float checkTime))
            {
                if (TimeUtil.Seconds < checkTime)
                    return agent;

                agent.RemoveMemory(IsAlone);
            }

            if (agent.TryGetMemory(IsLeader, out List<Animal> followers))
            {
                followers.RemoveAll(x => x == null || x.Dead);

                // this animal is a leader as long as it has followers
                if (followers.Count > 0)
                    return agent;
                else
                    agent.RemoveMemory(IsLeader);
            }

            if (!agent.TryGetMemory(IsFollower, out Animal leader))
            {
                var agentRegion = RouteRegions.GetRegion(agent.GroundPosition);
                var nearAnimals = NetObjectManager.Default.GetObjectsWithin(agent.Position.XZ(), agent.Species.VoxelsPerEntry).OfType<Animal>()
                    .Where(x => x.Alive && x.Visible && x.Species == agent.Species && x != agent
                    && HaveAdjacentHomePositions(agent, x) // don't join a herd far from home
                    && RouteRegions.GetRegion(x.GroundPosition) == agentRegion);

                if (!nearAnimals.Any())
                {
                    agent.SetMemory(IsAlone, TimeUtil.Seconds + TimeBetweenHerdChecks);
                    return agent;
                }

                List<Animal> stragglers = new List<Animal>();
                foreach (var nearAnimal in nearAnimals)
                {
                    if (nearAnimal.TryGetMemory(IsFollower, out Animal tmpLeader))
                    {
                        if (leader == null && tmpLeader.Visible && tmpLeader.Alive && HaveAdjacentHomePositions(tmpLeader, agent))
                            leader = tmpLeader;
                    }
                    else if (nearAnimal.HasMemory(IsLeader))
                    {
                        if (leader == null)
                            leader = nearAnimal;
                    }
                    else
                        stragglers.Add(nearAnimal);
                }

                if (leader == null)
                {
                    // become the leader
                    leader = agent;
                    agent.SetMemory(IsLeader, stragglers);
                }
                else
                {
                    // join the pack along w/ any other stragglers
                    stragglers.Add(agent);

                    if (!leader.TryGetMemory(IsLeader, out followers))
                    {
                        followers = new List<Animal>();
                        leader.SetMemory(IsLeader, followers);
                    }

                    foreach (var newMember in stragglers)
                        followers.Add(newMember);
                }

                foreach (var newMember in stragglers)
                    newMember.SetMemory(IsFollower, leader);
            }
            else
            {
                if (leader == null || !leader.Visible || leader.Dead)
                {
                    agent.RemoveMemory(IsFollower);
                    return agent;
                }
            }
            return leader;
        }
    }
}
