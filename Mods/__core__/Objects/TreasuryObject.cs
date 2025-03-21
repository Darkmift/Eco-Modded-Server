﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.Objects;

    [RequireComponent(typeof(TreasuryComponent))]
    [RequireComponent(typeof(AuthDataTrackerComponent))]
    public partial class TreasuryObject : WorldObject
    {
        protected override void OnCreatePostInitialize()
        {
            base.OnCreatePostInitialize();
            this.GetComponent<PropertyAuthComponent>().SetPublic();
        }
    }
}
