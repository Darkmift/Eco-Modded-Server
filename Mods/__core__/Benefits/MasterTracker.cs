// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System.Collections.Generic;
    using Eco.Gameplay.Animals;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using System.Linq;
    using Eco.Simulation.Agents;
    using Priority_Queue;

    /// <summary>This talents give user ability to highlight last hit animal using <see cref="ConstantHighlightSetting"/>.</summary>
    public partial class MasterTrackerTalent : Talent
    {
        public MasterTrackerTalent() => AnimalEntity.OnAttackAnimal += this.OnPlayerHitsAnimal;

        public override void RegisterTalent(User user)
        {
            base.RegisterTalent(user);
            user.ConstantHighlightSetting.ObjectRemoved.Add(this.OnObjectRemoved);
        }

        public override void UnRegisterTalent(User user)
        {
            base.UnRegisterTalent(user);
            user.ConstantHighlightSetting.ObjectRemoved.Remove(this.OnObjectRemoved);
        }

        private void OnPlayerHitsAnimal(Player attacker, AnimalEntity animal)
        {
            if(!attacker.User.Talentset.HasTalent<HuntingMasterTrackerTalent>()) //if player has no talent, do nothing
                return;
            
            var highlighter = attacker.User.ConstantHighlightSetting;
            highlighter.TryAddOrUpdate(animal, HighlightPurpose.MasterTracker,30);    //adding last hit animal to highlight list
            highlighter.TryRemoveByFunc(this.RemoveFunc); //trying to remove "unwanted" animals

            animal.Destroyed.AddUnique(_ => this.OnAnimalDestroyed(animal, highlighter)); //handling animal destroyed, removing from highlight list
        }
        
        /// <summary> Method used as param to pass for farthest animal removal from highlight </summary>
        private IEnumerable<ConstantHighlightInfo> RemoveFunc(SimplePriorityQueue<ConstantHighlightInfo, double> queue, User user)
        {
            return queue
                .Where(info => info.HighlightPurpose == HighlightPurpose.MasterTracker) //finding animals in queue supposing none not Animals will be added with MasterTracker purpose
                .OrderByDescending(queue.GetPriority) //sorting by priority(time) to skip animals with highest priority(more time to exceed)
                .Skip((int) user.Talentset.GetTalent(typeof(HuntingMasterTrackerTalent)).Value); //skipping animals count allowed by talent value
        }

        /// <summary>Callback to remove destroyed animal from highlight list and unsub from destroyed animal </summary>
        private void OnAnimalDestroyed(Animal animal, ConstantHighlightSetting highlighter)
        {
            highlighter.TryRemove(animal, HighlightPurpose.MasterTracker);
            animal.Destroyed.Remove(_ => this.OnAnimalDestroyed(animal, highlighter));
        }

        /// <summary>Callback need to remove sub for animal destruction(after which it removed from highlight) because the animal now is not present in highlight list. </summary>
        private void OnObjectRemoved(ConstantHighlightSetting highlighter, ConstantHighlightInfo info)
        {
            if(info.HighlightPurpose != HighlightPurpose.MasterTracker)
                return;
            var animal = (AnimalEntity)info.ObjectToHighLight;
            animal.Destroyed.Remove(_ => this.OnAnimalDestroyed(animal, highlighter));
        }
    }
}
