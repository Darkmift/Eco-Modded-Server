// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Core.Plugins.Interfaces;
    using Eco.Gameplay.Settlements;
    using Eco.Shared.Localization;
    using Eco.Shared.Utils;
    using System.Collections.Generic;
    using Eco.Shared.IoC;
    using Range = Eco.Shared.Math.Range;
    using Eco.Shared;
    using System;
    using Eco.Shared.Items;
    using Eco.Gameplay.Property;
	using Eco.Core.Utils;
	using Eco.Gameplay.Settlements.Culture;
	using Eco.Gameplay.Items;
	using Eco.Shared.SharedTypes;

	//Definitions for properties of the settlements system that we want to only be able to change using mods and not through server settings.
	public class SettlementDefs : IModKitPlugin, IInitializablePlugin
    {
        //The function used to convert culture to influence.
        static ValResult<float> CultureToInfluenceRadius(Settlement settlement, Deed deed, float cultureOfDeed)
        {
            var influence = LimitMapper.MapAndDescribe(cultureOfDeed,
													   SettlementsPlugin.Obj.Config.CultureToInfluenceMappingPerSettlementType[(int)settlement.SettlementType],
													   CultureUtils.InfluenceRadius,
                                                       CultureUtils.AsCulture,
                                                       CultureUtils.AsInfluenceRadius,
													   Localizer.Do($"Culture to Influence ({settlement.SettlementType.DisplayName} settings)"));

            //Now if there's multiplier, apply those and describe them.
            LocStringBuilder sb = new();
            sb.AppendLine(influence.Description);
            var influenceMult = ServiceHolder<SettlementConfig>.Obj.SettlementInfluenceMultiplier[(int)settlement.SettlementType];
            if (influenceMult != 1)
            { 
                sb.AppendLineLoc($"Influence Multiplier ({settlement.SettlementType.DisplayName} settings): {Text.StyledPercent(influenceMult)} ({Text.SignedNum((influence.Output * influenceMult) - influence.Output)})");
                influence.Output *= influenceMult;
            }

            var sizeMult = SettlementConfigExtensions.WorldSizeMultiplier;
            if (sizeMult != 1)
            {
                sb.AppendLineLoc($"World Size Multiplier: {Text.StyledPercent(sizeMult)} ({Text.SignedNum((influence.Output * sizeMult) - influence.Output)})");
                influence.Output *= sizeMult;
            }

            if (influenceMult != 1 || sizeMult != 1)
                sb.AppendLineLoc($"Total Influence Radius: {influence.Output.AsInfluenceRadius()}");

            return ValResult.Make(influence.Output, sb.ToLocString());
        }

        public void Initialize(TimedTask timer)
        {
            var config = SettlementsPlugin.Obj.Config;

            config.InventorySourceForAnnexationCosts = typeof(EmbassyDeskObject);
            config.ClaimStakeItems                   = new List<Type> { typeof(TownClaimStakeItem), typeof(CountryClaimStakeItem), typeof(FederationClaimStakeItem) };
            config.ClaimStakeWorldObjects                 = new List<Type> { typeof(TownClaimStakeItem), typeof(CountryClaimStakeItem), typeof(FederationClaimStakeItem) };

            config.CultureToInfluenceRadius          = CultureToInfluenceRadius;

            SettlementConfig.LeaderNames = new[] { 
                //Town
                new[] { Localizer.DoStr("Mayor") },
                //Country
                new[] { Localizer.DoStr("President"),
                        Localizer.DoStr("Chief Executive"),
                        Localizer.DoStr("Czar"),
                        Localizer.DoStr("Sultan"),
                        Localizer.DoStr("Prime Minister"),
                        Localizer.DoStr("Head of State"),
                        Localizer.DoStr("Premier"),
                        Localizer.DoStr("Chancellor") },
                //Federation
                new[] { Localizer.DoStr("Secretary-General") } };

            SettlementConfig.DefaultSuffixes = new[] { 
                //Town
                new[] { Localizer.DoStr("ville"),
                        Localizer.DoStr(" Town"),
                        Localizer.DoStr("sdale"),
                        Localizer.DoStr("burg"),
                        Localizer.DoStr("shire"),
                        Localizer.DoStr("berg"),
                        Localizer.DoStr("borough"),
                        Localizer.DoStr("field"),
                        Localizer.DoStr("tropolis"),
                        Localizer.DoStr(" Square"),
                        Localizer.DoStr("rock"),
                        Localizer.DoStr("spire"),
                        Localizer.DoStr("bury"),
                        Localizer.DoStr("borough"),
                        Localizer.DoStr(" Garden"),
                        Localizer.DoStr("glen"),
                        Localizer.DoStr("stead"),
                        Localizer.DoStr("'s Landing"),},
                //Country
                new[] { Localizer.DoStr("land"),},
                //Federation
                new[] { Localizer.DoStr(" United Nations"),
                        Localizer.DoStr(" League of Nations"),
                        Localizer.DoStr(" Federation"),
                        Localizer.DoStr(" Union")}};

        }

        public string GetCategory() => Localizer.DoStr("Settlements");
        public string GetStatus()   => string.Empty;
    }
}
