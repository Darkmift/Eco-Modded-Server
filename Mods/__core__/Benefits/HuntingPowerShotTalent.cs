// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Items;

    public partial class PowerShotTalent : Talent
    {
        //When the player learns this talent, we make the damage tooltip dirty to update its value.
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
            WeaponItem.WeaponDamageUpdateEvent?.Invoke(user);
        }
    }
}
