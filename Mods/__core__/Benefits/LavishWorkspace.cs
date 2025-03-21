﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using System.Linq;

    public partial class LavishWorkspaceTalent
    {
        public override void OnLearned(User user)
        {
            base.OnLearned(user);
            ResetRoomRequirements(user);
        }
        public override void OnUnLearned(User user)
        {
            base.OnUnLearned(user);
            ResetRoomRequirements(user);
        }
        private static void ResetRoomRequirements(User user)
        {
            foreach (var obj in WorldObjectManager.GetOwnedBy(user))
            {
                var requirements = obj.GetComponent<RoomRequirementsComponent>();
                requirements?.MarkDirty();
            }
        }

        public override bool HasActiveRequirements { get { return true; } }
        public override bool Active(object obj, User user = null)
        {
            var cc = obj as CraftingComponent;
            if (cc != null && (cc.Parent.Owners?.UserSet.Any() ?? false))
                return true;
            return false;
        }
    }
    
}
