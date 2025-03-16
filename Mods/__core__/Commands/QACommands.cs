// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods
{
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Systems.Messaging.Chat.Commands;
    using System.Linq;
    using Eco.Shared.Math;
    using Eco.Shared.IoC;
    using Eco.Gameplay.Objects;
    using Eco.Mods.TechTree;
    using Eco.Shared.Utils;
    using Eco.Gameplay.Components;
    using Eco.Core.Utils;
    using System;
    using Eco.World;
    using System.Collections.Generic;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Utils;
    using Eco.Gameplay.Occupancy;

    [ChatCommandHandler]
    public static class QACommands
    {
        [ChatSubCommand("Build", "Spawn all doors.", ChatAuthorizationLevel.DevTier)]
        public static void Doors(User user) => SpawnObjects(user, objectType => typeof(DoorObject).IsAssignableFrom(objectType));                                       // all doors inherit from DoorObject.

        [ChatSubCommand("Build", "Spawn all signs and add a random text to it.", ChatAuthorizationLevel.DevTier)]
        public static void Signs(User user, int textLengthMin = 3, int textLengthMax = 40) =>
            SpawnObjects(user, 
                CommandsUtil.ItemsWithComponent(typeof(CustomTextComponent)),    // all signs have CustomTextComponent.
                obj        => obj.GetComponent<CustomTextComponent>()?.SetText(user.Player, StringUtils.RandomString(RandomUtil.Range(textLengthMin, textLengthMax)))); // after a sign is spawned, assign a random text to it.

        [ChatSubCommand("Preset", "Spawn storage with all type of doors", ChatAuthorizationLevel.DevTier)]
        public static void Doors(User user, int sType = 0) => ItemPresetsCommands.SpawnStorage(user, "Doors", ItemPresetsCommands.StoragesTypes[sType], Item.AllItemsIncludingHidden.Where(i => typeof(DoorObject).IsAssignableFrom((i as WorldObjectItem)?.WorldObjectType)).ToList());
        
        [ChatSubCommand("Preset", "Spawns storage with sign containing items", ChatAuthorizationLevel.DevTier)]
        public static void Signs(User user, int sType = 0) => ItemPresetsCommands.SpawnStorage(user, "Signs", ItemPresetsCommands.StoragesTypes[sType], CommandsUtil.ItemsWithComponent(typeof(CustomTextComponent)).NonNull().ToList());

        [ChatSubCommand("Preset", "Spawns storage with bed items", ChatAuthorizationLevel.DevTier)]
        public static void Beds(User user, int sType = 0) => ItemPresetsCommands.SpawnStorage(user, "Beds", ItemPresetsCommands.StoragesTypes[sType], CommandsUtil.ItemsWithComponent(typeof(BedComponent)).NonNull().ToList());
        
        /// <summary> Spawn multiple objects by given WorldObjectItems. There is an optional onSpawned action to perform for every spawned object. </summary>
        private static void SpawnObjects(User user, List<WorldObjectItem> items, Action<WorldObject> onSpawned = null)
        {
            const int offset      = 3;                                                                                                                // each object is placed next to each others but in an offset distance.
            var       position    = (Vector3i)user.Player.WorldPosInt() + (Vector3i.Forward * offset);                                                // set start position a little aside from player.

            foreach (var item in items.NonNull())
            {
                // force spawning each of them
                var obj = WorldObjectManager.ForceAdd(item.WorldObjectType, user, position, Quaternion.Identity, false);
                onSpawned?.Invoke(obj);
                var occupancyInfo = WorldObject.GetOccupancyInfo(item.WorldObjectType);
                position.X += (occupancyInfo?.Dimensions.X ?? 0) + offset;

                if (occupancyInfo != null) // checking this for destroying blocks that overlaps with current object occupancy. Some objects like vehicle dont have occupancy, so skip them.
                {
                    var ground = OccupancyUtils.GroundBelow(obj);
                    var destroyBlocks = new List<BlockChange>(ground.Count() * occupancyInfo.Range.HeightInc); // list where need to beed added position of blocks to destroy with BlockChange.
                    foreach (var g in ground)
                    {
                        for (int i = 1; i <= occupancyInfo.Range.HeightInc; i++)
                        {
                            var destroyBlock = g;
                            destroyBlock.y += i;
                            destroyBlocks.Add(new BlockChange() { Position = destroyBlock });
                        }
                    }
                    World.BatchApply(destroyBlocks);
                }
            }
        }

        /// <summary> Spawn multiple objects filtered by a predicate. There is an optional onSpawned action to perform for every spawned object. </summary>
        private static void SpawnObjects(User user, Func<Type, bool> predicate, Action<WorldObject> onSpawned = null)
        {                                                             
            var objectTypes = ServiceHolder<IWorldObjectManager>.Obj.AllWorldObjectTypes.Values
                .Where(t => predicate?.Invoke(t) ?? true)
                .Select(x => WorldObjectItem.GetCreatingItemTemplateFromType(x)).ToList();              // do a filter to find objects.
            SpawnObjects(user, objectTypes, onSpawned);
        }

        [ChatSubCommand("Land", "Spawns a corridor-like tunnel that gives entrance to the mines.", "spawnMines", ChatAuthorizationLevel.DevTier)]
        public static void SpawnMines(User user, int corridorWidth = 1, int corridorHeight = 3, int corridorDepth = 1, int depth = 100, int levelDifferenceHeight = 1)
        {
            var pos = user.Position.XYZi(); //Get the position of the player, will be used to spawn the mines.
            pos += Vector3i.Forward;        //Move dig point 1 step further from where the player is standing.
            depth = Math.Min(depth, pos.y); //Limit the depth to not go past the planet's core.

            //Starting from player's current position, start digging a tunnel downwards, with given width and height.
            for (int i = 0; i < depth / levelDifferenceHeight; i++)
            {
                for (int x = 0; x < corridorWidth; x++)                             //For every unit    in the X axis (based on width)
                    for (int y = 0; y < corridorHeight + levelDifferenceHeight; y++)//..as well as      in the Y axis (based on height)
						for (int z = 0; z < corridorDepth; z++)                     //..delete N blocks in the Z axis (based on depth)
                            World.DeleteBlock(pos + (Vector3i.Up * y) + (Vector3i.Right * x) + (Vector3i.Forward * z) + (Vector3i.Up * levelDifferenceHeight));

                //Move the next dig point for the next iteration.
                pos += (Vector3i.Forward * corridorDepth) + (Vector3i.Down * levelDifferenceHeight);
            }
        }
    }
}
