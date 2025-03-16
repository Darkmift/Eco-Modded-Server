// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.Shared.Serialization;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Interactions.Interactors;

    public partial class EckoTheDolphinItem : ToolItem, IInteractor
    {
        [Serialized]
        private string wantedItem = string.Empty;
        private static readonly IDynamicValue SkilledRepairCostValue = new ConstantValue(1);
        public override IDynamicValue SkilledRepairCost { get { return SkilledRepairCostValue; } }

        /*
        [Interaction(InteractionTrigger.RightClick), RPC] 
        public bool DoInteraction(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            if (string.IsNullOrEmpty(this.wantedItem))
                this.wantedItem = DiscoveryManager.Obj.GetRandomDiscoveredNotCarriedItem().DisplayName;

            var itemStack = player.User.Inventory.NonEmptyStacks.Where(stack => stack.Item.DisplayName == this.wantedItem).FirstOrDefault();
            if (itemStack != null)
            {
                var gift = AllItems.Where(x => x is not Skill && x.Group != "Actionbar Items").Shuffle().First();
                var result = player.User.Inventory.TryModify(changeSet =>
                {
                    changeSet.RemoveItem(itemStack.Item.Type);
                    changeSet.AddItem(gift.Type);
                }, player.User);

                if (result.Success)
                {                    
                    player.Msg(Localizer.Format("Ecko accepts your tribute of {0:wanted} and grants you {1:given} for your devotion.", this.wantedItem, gift.DisplayName));
                    this.wantedItem = DiscoveryManager.Obj.GetRandomDiscoveredNotCarriedItem().DisplayName;
                }
            }
            else
                player.Msg(Localizer.Format("Ecko demands {0}!", this.wantedItem));

            return true;
        }*/ //Disabling because its unbalancing
    }
}
