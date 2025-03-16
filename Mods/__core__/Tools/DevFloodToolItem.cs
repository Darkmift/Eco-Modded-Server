// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Eco.Gameplay.Items;
using Eco.Shared.Serialization;
using Eco.World;
using Eco.World.Blocks;
using Eco.World.Utils;
using Eco.Gameplay.DynamicValues;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Utils;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Shared.SharedTypes;
using Eco.Gameplay.Players;

[Serialized]
[LocDisplayName("Dev Flood Tool")]
[LocDescription("Flood tool! Left click on the water to remove the top layer, or Right-click on a block to add a water layer.")]
[Category("Hidden")]
public class DevFloodToolItem : ToolItem, IInteractor
{
    private static IDynamicValue skilledRepairCost = new ConstantValue(1);
    public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

    [Interaction(InteractionTrigger.LeftClick, tags: BlockTags.Liquid)]
    public void RemoveTopLayerOfWater(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target) => Flooding.DeleteTopWaterLayer(target.BlockPosition.Value); //Remove the water layer on the target position.

    [Interaction(InteractionTrigger.RightClick)]
    public void AddTopLayerOfWater(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)    => Flooding.AddWaterLayer(target.BlockPosition.Value + target.Normal.ToVec()); //Add a water layer on the target + normal position.
    
    [Interaction(InteractionTrigger.LeftClick, modifier: InteractionModifier.Shift, tags: BlockTags.Liquid)]
    public async void RemoveAllConnectedWater(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
    {
        var result = Flooding.GetAllConectedWater(target.BlockPosition.Value, out var waterBlocks); //Remove the water layer on the target position.
        if (!result) player.Error(result.Message);
        else if(await player.ConfirmBoxLoc($"Found {waterBlocks.Count} water blocks to delete. Are you sure?"))
        {
            foreach (var waterPos in waterBlocks)
                World.DeleteBlock((WrappedWorldPosition3i)waterPos, false);
        }
    }
}
