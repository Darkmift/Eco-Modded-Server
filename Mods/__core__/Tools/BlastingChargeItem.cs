// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Eco.Core.Controller;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.GameActions;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.SharedTypes;
using Eco.Shared.Utils;
using Eco.World;
using Eco.World.Blocks;

[Serialized]
[LocDisplayName("Blasting Charge")]
[LocDescription("Blasts appart rock.")]
[Category("Hidden")]
[NoIcon]
public class BlastingChargeItem : ToolItem, IInteractor
{
    static IDynamicValue tier = new ConstantValue(0);
    static IDynamicValue caloriesBurn = new ConstantValue(1);
    private static IDynamicValue skilledRepairCost = new ConstantValue(1);
    
    public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }
    public override IDynamicValue Tier { get { return tier; } }
    public override IDynamicValue CaloriesBurn { get { return caloriesBurn; } }

    //Blasts all mineable objects inside an area, converting them to rumble.
    [Interaction(InteractionTrigger.RightClick, tags: BlockTags.Minable)]
    public bool Blast(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
    {
        //TO DO: convert to AOE mode
        if (!target.IsBlock || target.Block().Is<Solid>()) return false;

        //Get the necessary values to get the affected world range.
        var targetPos = target.BlockPosition.Value;
        var minTop = targetPos + new Vector3i(-2, -1, -2);
        var maxTop = targetPos + new Vector3i(2, -1, 2);
        var minBottom = minTop + new Vector3i(0, -5, 0);

        //Iterate through all blocks inside the affected range, and destroy the ones that are mineable.
        var range = new WorldRange(minBottom, maxTop);
        foreach (var blockPos in range.XYZIterInc())
        {
            var blockType = World.GetBlock(blockPos).GetType();
            if (RubbleObject.BecomesRubble(blockType))
            {
                AtomicActions.DeleteBlockNow(this.CreateMultiblockContext(player, true, blockPos.SingleItemAsEnumerable()));
                RubbleObject.TrySpawnFromBlock(player, blockType, blockPos, RubbleObject.MaxAmountPerBlock);
            }
        }

        return true;
    }
}
