// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

#nullable enable
namespace Eco.Mods.TechTree
{
    using Eco.Core.Items;
    using Eco.Gameplay.GameActions;
    using Eco.Gameplay.Items;
    using Eco.Shared.Localization;
    using System.ComponentModel;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Shared.SharedTypes;
    using Eco.Gameplay.Players;
    using Eco.Shared.Items;

    [Tag("Harvester")]
    [Category("Hidden")]
    [Mower]
    public abstract partial class MacheteItem : ToolItem, IInteractor
    {
        static readonly LocString ClearString                 = Localizer.DoStr("Clear");

        public override bool CustomHighlight                  => true;

        [Interaction(InteractionTrigger.LeftClick, tags: BlockTags.Clearable, AnimationDriven = true)]
        public bool ClearVegetation(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            if (!this.TryCreateMultiblockContext(out var context, target, player, tagsTargetable: BlockTags.Clearable)) return false;

            return AtomicActions.DestroyPlantNow(context, notify: false).Success;
        }
    }
}
