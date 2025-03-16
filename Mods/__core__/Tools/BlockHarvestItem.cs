// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.GameActions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Gameplay.Players;
    using Eco.Shared.SharedTypes;

    /// <summary>Wrapper for tools like sickle and scythe, contains same code.</summary>
	public abstract partial class BlockHarvestItem : ToolItem, IInteractor
    {
        [Interaction(InteractionTrigger.LeftClick, tags:BlockTags.Reapable, animationDriven: true)]
        public bool Reap(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            //Check if there are any plants we can harvest
            if (!this.TryCreateMultiblockContext(out var context, target, player, tagsTargetable: BlockTags.Reapable)) return false;

            return AtomicActions.HarvestPlantNow(context, player?.User.Inventory).Success; //Try to harvest plants
        }
    }
}
