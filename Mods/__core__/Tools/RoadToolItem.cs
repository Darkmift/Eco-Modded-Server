// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
namespace Eco.Mods.TechTree
{
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.GameActions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Shared.SharedTypes;
    using Eco.Gameplay.Players;
    using Eco.Shared.Items;

    [Category("Hidden"), Tag("Tamper")]
    public abstract partial class RoadToolItem : ToolItem, IInteractor
    {
        [Interaction(InteractionTrigger.LeftClick, tags: BlockTags.CanBeRoad, AnimationDriven = true)]
        public bool BuildRoad(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            if (!this.TryCreateMultiblockContext(out var context, target, player, tagsTargetable: BlockTags.CanBeRoad, applyXPSkill: true, gameActionConstructor: () => new TampRoad())) return false;

            return AtomicActions.ChangeBlockNow(context, typeof(DirtRoadBlock)).Success;
        }
    }
}
