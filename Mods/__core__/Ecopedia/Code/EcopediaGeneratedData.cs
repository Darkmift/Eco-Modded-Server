// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.EcopediaRoot;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Utils;
    using Eco.Shared.Utils;
    using System.Collections.Generic;
    using System.Linq;
    using Eco.Gameplay.Civics;
    using Eco.Shared.Localization;
    using Eco.Gameplay.GameActions;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Gameplay.Civics.Misc;
    using Eco.Shared.Localization.ConstLocs;
    using Eco.Gameplay.Housing.PropertyValues;
    using Eco.Gameplay.Economy.Reputation.Internal;
    using Eco.Gameplay.Systems;

    //Various classes that append dynamic data into Ecopedia pages.
    public class EcopediaConstitutionSettings : IEcopediaGeneratedData
    {
        public IEnumerable<EcopediaPageReference> PagesWeSupplyDataFor() => new EcopediaPageReference("CivicObjectComponent", "World Index", "Current Government", Localizer.DoStr("Current Government")).SingleItemAsEnumerable();

        public LocString GetEcopediaData(Player player, EcopediaPage page)
        {
            var sb = new LocStringBuilder();
            sb.AppendLine(TextLoc.HeaderLocStr("World Settings"));
            var valToOverthrow = Text.StyledNum(CivicsPlugin.Obj.Config.ValueAdditionToOverthrow);
            sb.AppendLineLoc($"Constitutions can be overthrown when their value is exceeded by {valToOverthrow}%.");
            sb.AppendLine();
            sb.AppendLine(CivicsUtils.AllActiveAndValid<Constitution>(null).MakeListLoc($"Current Constitutions"));
            return sb.ToLocString();
        }
    }

    //Various classes that append dynamic data into Ecopedia pages.
    public class EcopediaHousingData : IEcopediaGeneratedData
    {
        public IEnumerable<EcopediaPageReference> PagesWeSupplyDataFor() => new EcopediaPageReference(null, null, "Residency", Localizer.DoStr("Residency")).SingleItemAsEnumerable();

        public LocString GetEcopediaData(Player player, EcopediaPage page)
        {
            var multipliers = Enumerable.Range(0, 10).Select(x => HousingConfig.OccupancyMultiplierGenerator.Invoke(x)).ToArray(); //fills first 10 values for ecopedia
            if (multipliers == null) return LocString.Empty;
            List<string> list = new List<string>();
            for (int i = 1; i < multipliers.Length; i++) list.Add($"{Localizer.Plural("Occupant", i - 1)}: {Text.StyledPercent(multipliers[i])}");
            return list.MakeListLoc($"Occupancy Multipliers:", false);
        }
    }

    
    //Various classes that append dynamic data into Ecopedia pages.
    public class EcopediaSkillData : IEcopediaGeneratedData
    {
        static EcopediaPageReference[] pages = new[] { new EcopediaPageReference(null, null, "Skills Overview", Localizer.DoStr("Skills Overview")),
                                                        new EcopediaPageReference(null, null, "Experience", Localizer.DoStr("Experience"))};
        public IEnumerable<EcopediaPageReference> PagesWeSupplyDataFor() => pages;

        public LocString GetEcopediaData(Player player, EcopediaPage page)
        {
            var sb = new LocStringBuilder();

            var maxProfs = DifficultySettingsConfig.Obj.GameSettings.AdvancedGameSettings.MaxProfessionsPerCitizen;
            var maxSpecs = DifficultySettingsConfig.Obj.GameSettings.AdvancedGameSettings.MaxSpecialtiesPerCitizen;

            sb.AppendLine(TextLoc.HeaderLocStr($"World Skill Settings"));
            sb.AppendLineLoc($"Max Professions per Citizen: {(maxProfs < SkillTree.TotalProfessions ? Text.StyledInt(SkillTree.TotalProfessions) : CommonLocs.None)}");
            sb.AppendLineLoc($"Max Specialties per Citizen: {(maxSpecs < SkillTree.TotalProfessions ? Text.StyledInt(SkillTree.TotalSpecialties) : CommonLocs.None)}");

            sb.AppendLine(TextLoc.HeaderLocStr("XP Needed For Each Star"));
            SkillManager.Settings.LevelUps.ForEachIndex((val,index) => sb.AppendLineLoc($"{Localizer.Ordinal(index + 1)} star: {Text.StyledInt(val)}"));
            sb.AppendLineLocStr($"Each additional star after these costs {Text.StyledPercent(SkillManager.Settings.LevelUpPostPercent)} more than the previous.");

            return sb.ToLocString();
        }
    }


    //Various classes that append dynamic data into Ecopedia pages.
    public class EcopediaLawData : IEcopediaGeneratedData
    {
        public IEnumerable<EcopediaPageReference> PagesWeSupplyDataFor() => new EcopediaPageReference(null, "Government", "Laws", Localizer.DoStr("Laws")).SingleItemAsEnumerable();

        public LocString GetEcopediaData(Player player, EcopediaPage page)
        {
            var sb = new LocStringBuilder();
            sb.AppendLine(typeof(GameAction).DerivedTypes().Where(x=>!x.IsAbstract).OrderBy(x=>x.GetLocDisplayName()).Select(x=>x.UILink()).MakeListLoc($"Available Triggers"));
            sb.AppendLine(typeof(LegalAction).DerivedTypes().Where(x=>!x.IsAbstract).OrderBy(x => x.GetLocDisplayName()).Select(x => x.UILink()).MakeListLoc($"Available Legal Actions"));
            return sb.ToLocString();
        }
    }

    public class EcopediaReputationData : IEcopediaGeneratedData
    {
        public IEnumerable<EcopediaPageReference> PagesWeSupplyDataFor() => new EcopediaPageReference(null, null, "Reputation", Localizer.DoStr("Reputation")).SingleItemAsEnumerable();

        public LocString GetEcopediaData(Player player, EcopediaPage page)
        {
            var sb = new LocStringBuilder();
            sb.AppendLineLoc($"A citizen may grant reputation to others with a limit of {Text.StyledNum(ReputationConfig.Obj.MaxGivableRepPerDay)} points (positive or negative) per day, up to total of {Text.StyledNum(ReputationConfig.Obj.MaxRepFromOnePerson)}.");
            sb.AppendLineLoc($"A citizen may only grant a single target up to {ReputationConfig.Obj.MaxGivableRepPerDayPerTarget.StyledNum()} reputation per day.");
            sb.AppendLine();
            //We show all the titles specific for users.
            var reps = ReputationConfig.Obj.Reputations.Select(x=>x.ToString(true));
            sb.AppendListLoc($"Reputation titles for users", reps, false, false);
            //And then we show all the titles used for any other object.
            reps = ReputationConfig.Obj.Reputations.Select(x => x.ToString(false));
            sb.AppendListLoc($"Reputation titles for other objects", reps, false, false);

            return sb.ToLocString();
        }
    }
}
