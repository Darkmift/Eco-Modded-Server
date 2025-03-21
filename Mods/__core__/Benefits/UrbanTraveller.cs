﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Players;
    using Eco.Shared.Math;
    using Eco.Simulation.WorldLayers;


    public partial class UrbanTravellerTalent : Talent
    {
        public override bool HasActiveRequirements { get { return true; } }
        public override bool Active(object obj, User user = null)
        {
            var playerActivity = WorldLayerManager.Obj.GetLayer(LayerNames.PlayerActivity);
            if (playerActivity.EntryWorldPos(user.Position.XZi()) > 0.8)
                return true;
            return false;
        }
    }
}
