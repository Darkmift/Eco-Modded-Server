// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
#nullable enable
namespace Eco.Mods.TechTree
{
    using System;
    using System.ComponentModel;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Gameplay.Items;
    using Eco.Shared.Serialization;
    using Eco.World.Blocks;
    using Eco.Shared.Localization;
    using Eco.Gameplay.GameActions;
    using Eco.Core.Items;
    using Eco.Gameplay.Players;
    using Eco.Shared.Items;
    using Eco.Shared.SharedTypes;
	using static Eco.Gameplay.Players.User;

    [Serialized]
    [LocDisplayName("Hoe")]
    [LocDescription("Used to till soil and prepare it for planting.")]
    [Category("Hidden")]
    [Hoer, Tag("Plow")]
    public abstract class HoeItem : ToolItem, IInteractor
    {
        private static IDynamicValue skilledRepairCost = new ConstantValue(1);
        private static IDynamicValue caloriesBurn      = new ConstantValue(1);
        private static IDynamicValue tier              = new ConstantValue(0);
    
        public override GameActionDescription      DescribeBlockAction   => GameActionDescription.DoStr("plow a block", "plowing a block");
        public override int                        FullRepairAmount      => 1;
        public override Item                       RepairItem            => Item.Get<StoneItem>(); 
        public override IDynamicValue              SkilledRepairCost     => skilledRepairCost; 
        public override IDynamicValue              CaloriesBurn          => caloriesBurn; 
        public override IDynamicValue              Tier                  => tier; 
        public override Type                       ExperienceSkill       => typeof(FarmingSkill);
  
        [Interaction(InteractionTrigger.LeftClick, tags: BlockTags.Tillable, animationDriven: true)]
        public bool Hoe(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            if (!this.TryCreateMultiblockContext(out var context, target, player, tagsTargetable: BlockTags.Tillable, gameActionConstructor: () => new PlowField())) return false;

            player.User.OnInteract.Invoke(new InteractionEvent() { Name = nameof(SeedItem.Plant), ItemInInteraction = this, Target = target });
            return AtomicActions.ChangeBlockNow(context, typeof(TilledDirtBlock)).Success;
        }
    }
}
