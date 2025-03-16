// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Shared.Items;
    using Eco.Shared.Serialization;
    using Eco.Simulation;
    using Eco.Simulation.Agents;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.Gameplay.DynamicValues;
    using Eco.Shared.Localization;
    using Eco.Gameplay.Rooms;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Systems.Messaging.Chat.Commands;
    using System.Collections.Generic;
    using System.Text;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Gameplay.UI;
    using Eco.Gameplay.Utils;
    using Eco.Shared.SharedTypes;
    using Eco.Mods.Organisms;
    using Eco.Shared.Networking;
    using Eco.Shared.Math;
    using Eco.Gameplay.Components.Storage;

    [Serialized]
    [LocDisplayName("Dev Tool")]
    [LocDescription("DOES CHEATER THINGS THROUGH CHEATING POWERS!")]
    [IgnoreAuth]
    [BuilderCheat]
    [Tier(10, false)]
    [Category("Hidden")]
    public class DevtoolItem : HammerItem
    {
        private static IDynamicValue skilledRepairCost  = new ConstantValue(1);
        public override IDynamicValue CaloriesBurn      => new ConstantValue(0);
        public override IDynamicValue SkilledRepairCost => skilledRepairCost;
        private static IDynamicValue tier               = new ConstantValue(4);
        public override IDynamicValue Tier              => tier;
        public override bool Decays                     => false; //Don't use durability for this tool.


        [RPC] public void Smite(Player player, InteractionTriggerInfo trigger, InteractionTarget target)
        {
            if (target.IsBlock)
            {
                var position = target.BlockPosition.Value;
                var block = World.GetBlock(position);
                if (block != null && block != Block.Empty && !block.Is<Impenetrable>())
                {
                    //If we are selecting a plant then just destroyplant else destroy block and check if we have plant on top to destroy it as well.
                    if (EcoSim.PlantSim.GetPlant(position) is { } plant)
                    {
                        EcoSim.PlantSim.DestroyPlant(plant, DeathType.DivineIntervention, killer: player.User);
                    }
                    else
                    {
                        World.DeleteBlock(position);
                        if (EcoSim.PlantSim.GetPlant(position + Vector3i.Up) is { } plantAbove)
                            EcoSim.PlantSim.DestroyPlant(plantAbove, DeathType.DivineIntervention, killer: player.User);
                    }

                    RoomData.QueueRoomTest(position);
                }
            }
            else if(target.NetObj != null)
            {
                if (target.NetObj is WorldObject obj) 
                {
                    //Cleans up any public storage if found within the world object to make sure inventories are emptied out before the world object is destroyed permanently.
                    //e.g Stockpiles will have their contents removed before being forcefully destroyed by the devtool
                    if (obj.TryGetComponent<PublicStorageComponent>(out var storage)) ((AuthorizationInventory)storage.Storage).Clear();
                    WorldObjectManager.DestroyPermanently(obj);
                }
                
                if      (target.NetObj is PickupableBlock) World.DeleteBlock(target.BlockPosition.Value);
                else if (target.NetObj is RubbleObject rubble) rubble.Destroy();
                else if (target.NetObj is TreeEntity     tree) tree.Destroy();
                else if (target.NetObj is Animal       animal) animal.Destroy();
            }
        }

        [RPC] public void Sample(Player player, InteractionTarget target, bool maxAmount = false)
        {
            Item item = null;
            if (target.IsBlock)                             
            { 
                var block = World.GetBlock(target.BlockPosition.Value);
                item = block != null ? BlockUtils.GetItem(block) : null;
            }
            else if (target.NetObj is WorldObject obj)      
                item = obj.CreatingItem as Item;

            if (item != null) { player.User.Inventory.ReplaceStack(player, player.User.Inventory.Carried.Stacks.First(), item.TypeID, maxAmount ? item.MaxStackSize : 1); player.InfoBoxLoc($"{item.MarkedUpName} added to carry slot."); }
            else              player.ErrorLoc($"Nothing found to copy."); 

        }

        [NewTooltip(CacheAs.Global, 200)]
        public static LocString ControllsTooltip()
        {
            var res = new StringBuilder();

            res.AppendLine(Localizer.DoStr("Can smite any block or world object."));
            res.AppendLine(Localizer.DoStr("Can be used as a hammer to place infinite amount of blocks."));
            res.AppendLine(Localizer.DoStr("Can ignore auth for different actions (storage, vehicles, blocks)."));
            res.AppendLine(Localizer.DoStr("Can copy world block to carried slot with Shift+E."));
            res.AppendLine(Localizer.DoStr("Can smite or copy water blocks if you hold Ctrl while performing the respective action."));

            return new TooltipSection(Localizer.DoStr("Special Actions"), res.ToStringLoc());
        }

        public override string OnUsed(Player player, ItemStack itemStack)
        {
            player.PopupTypePicker(Localizer.DoStr("Pick Material for DevTool"), typeof(BlockItem), material => this.SetMaterial(material, player));

            return base.OnUsed(player, itemStack);
        }

        private void SetMaterial(List<Type> result, Player player)
        {
            if (result.Count() == 0) return;

            //Put the first in the carry slot.
            player.User.Carrying.ReplaceStack(Item.Get(result[0]), 1, true);

            //Put the rest in the toolbar.
            int index = 1;
            foreach (var stack in player.User.Inventory.Toolbar.Stacks.Where(x => !(x.Item is ToolItem)))
            {
                if (index >= result.Count()) break;
                var entry = result[index++];
                stack.ReplaceStack(Activator.CreateInstance(entry) as Item, 1, true);
            }

            AdminCommands.CarryAll(player.User, true);

            //Select the dev tool
            player.User.Inventory.Toolbar.SelectType(player, this.GetType());
        }
    }
}
