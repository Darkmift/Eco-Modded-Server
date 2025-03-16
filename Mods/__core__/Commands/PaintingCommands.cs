// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods
{
    using System;
    using System.Collections.Generic;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Systems.Messaging.Chat.Commands;
    using Eco.Mods.TechTree;
    using Eco.Shared.Graphics;
    using Eco.Shared.Math;
    using Eco.Shared.Networking;
    using Eco.Shared.Utils;
    using Eco.World;
    using Eco.World.Color;

    [ChatCommandHandler]
    public class PaintingCommands
    {
        [ChatSubCommand("Inventory", "Give yourself mixed paint for predefined named colors", "paint", ChatAuthorizationLevel.Admin)]
        public static void GivePaint(User user, int namedColorIndex, int number = 1)
        {
            var color = ((NamedColors)namedColorIndex).GetNamedColor();
            GivePaintInternal(user, color, number);
        }
        
        [ChatSubCommand("Land", "Clears paint in area with radius specified around the player. Max = 20", ChatAuthorizationLevel.Admin)]
        public static void ClearPaint(User user, int radius = 4)
        {
            radius = Math.Clamp(radius, 1, 20); // prevent too big volume calls
            var center = user.Position.XYZi();
            var offset = new Vector3i(radius, radius, radius);
            var range = new WorldRange(center - offset, center + offset);

            // Clear color in batch and then trigger dirty struct update
            BlockColorManager.Obj.ClearColors(range.XYZIterInc(), true);

            // Client prediction as we do not want to force all chunk rebuild in case range is big,
            // just color changes locally and then lazy chunk updater will do its work
            user.Player?.RPC("ClearBlockPaint", center, radius);
        }
        
        [ChatSubCommand("Inventory", "Give yourself mixed paint with any rgb color", "paintrgb", ChatAuthorizationLevel.Admin)]
        public static void GivePaintRGB(User user, int r, int g, int b, int amount = 1)
        {
            GivePaintInternal(user, new ByteColor((byte)r, (byte)g, (byte)b, 255), amount);
        }
        
        [ChatSubCommand("Preset", "Lots of different paint buckets for test", ChatAuthorizationLevel.DevTier)]
        public static void Paint(User user, int stackCount = 5)
        {
            // Build random unique buckets list
            var paintBuckets = new List<PaintBucketItem>();
            for (var i = 0; i < stackCount; i++)
                paintBuckets.Add(GetPaint(NamedColorUtils.GetRandomNamedColorAll().Color));
            
            // Reuse presets core to spawn previously built list
            ItemPresetsCommands.SpawnStorage(user, "Paint buckets", 0, paintBuckets, 100);
        }
        
        [ChatSubCommand("Land", "Spawns giant walls and paints them with random colors", "painttest", ChatAuthorizationLevel.Admin)]
        public static void PaintTest(User user, int size = 50, int walls = 5)
        {
            var startPos = user.Position.XYZi();
            for (var w = 0; w < walls; w++)
            {
                var zOffset = 1 + (w * 3); // offset for walls on z axis
                
                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        var namedColor = NamedColorUtils.GetRandomNamedColorAll();
                        var targetPos = startPos + new Vector3i(1 + j, 1 + i, zOffset);
                        World.SetBlock(typeof(HewnLogFloorBlock), targetPos);
                        BlockColorManager.Obj.SetColor(targetPos, namedColor.Color);
                    }
                }
            }
        }

        static PaintBucketItem GetPaint(ByteColor color)
        {
            // Creating unique paint item.
            var mixedPaint = Item.Create<PaintBucketItem>();
            mixedPaint.SetColor(color);
            return mixedPaint;
        }

        static void GivePaintInternal(User user, ByteColor color, int amount = 1)
        {
            using var changeSet = InventoryChangeSet.New(user.Inventory.ToolbarBackpack, user);
            
            // Creating unique paint item.
            var mixedPaint = GetPaint(color);
            
            // Todo works badly with item comparison for uniques, need to fix
            changeSet.AddItem(mixedPaint, amount);
            var res = changeSet.TryApply();
            if (!res.Success) user.MsgLocStr(res.Message);
        }
    }
}
