// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Gameplay.Players;
    using Eco.Shared.Items;
    using Eco.Shared.SharedTypes;

    public partial class TorchItem : ToolItem, IInteractor
    {
        private static IDynamicValue skilledRepairCost = new ConstantValue(1);
        public override IDynamicValue SkilledRepairCost { get { return skilledRepairCost; } }

        [Interaction(InteractionTrigger.LeftClick, canHoldToTrigger: TriBool.True, animationDriven: true, Flags = InteractionFlags.NoTargetRequired)]
        public void TorchInteraction(Player player, InteractionTriggerInfo trigger, InteractionTarget target) { }
    }
}
