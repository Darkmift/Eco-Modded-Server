// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;

    public partial class GluttonTalent : Talent
    {
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
            user.Stomach.ChangedMaxCalories();
        }
    }

    public partial class DeeperPocketsTalent : Talent
    {
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
            user.ChangedCarryWeight();
        }
    }

    public partial class UrbanTravellerTalent : Talent
    {
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
            user.ChangedMovementSpeed();
        }
    }

    public partial class NatureAdventurerTalent : Talent
    {
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
            user.ChangedMovementSpeed();
        }
    }
}
