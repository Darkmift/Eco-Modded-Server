// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
	using Eco.Core.Plugins.Interfaces;
	using Eco.Gameplay.Players;
	using Eco.Gameplay.Skills;
	using System;

	/// <summary>Provides hooks for mods to affect skills related features.</summary>
	public class SkillMods : IModInit
    {
        public static void Initialize()
        {
            Skill.CalculateStarsNeededForSpecialty = (user, skillType) => 1;
        }
    }
}
