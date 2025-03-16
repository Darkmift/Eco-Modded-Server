// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.


namespace Eco.Mods.TechTree
{
    using Eco.Core.Plugins.Interfaces;
    using Eco.Core.Utils;
    using Eco.Gameplay.Culture;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Settlements.Culture;
    using Eco.Shared.Localization;
    using Eco.Shared.Utils;
    using System.Collections.Generic;
    using Eco.Gameplay.Economy.Reputation;
    using Eco.Gameplay.Systems;
    using Eco.Shared;
    using Eco.Gameplay.Settlements;

    public class CulturalValues : IModInit
    {
        public static void Initialize()
        {
            //Big penalty for same settlement reptuatiopn.
            CultureConfig.SameSettlementReputationMutliplier = .1f;

            //Calculate the cultural value of a given user picture.
            CultureConfig.CultureValueCalculation = (inputs) =>
            {
                var worldobj = inputs.WorldObject;
                var settlement = inputs.Settlement;
                var repTarget = inputs.ReputationTarget;

                //We look at both the user's reputation and the picture's reputation to determine its value. Get the rep of each
                //and convert it to culture using the limit-conversion defined above.
                //Reputation from citizens with the same settlement counts at a reduction
                var userRep = ReputationManager.Obj.CalculateWeightedPositiveRep(repTarget.UserSource, RepContributionTowardsCultureValue);
                var workRep = ReputationManager.Obj.CalculateWeightedPositiveRep(repTarget, RepContributionTowardsCultureValue);

                var repheaderUser = Header(Localizer.DoStr("artist's"), repTarget.UserSource, userRep);
                var repheaderWork = Header(inputs.TypeOfWorkPossessive, repTarget, workRep);

                LocString Header(LocString source, IHasReputation target, float rep)
                {
                    var sb = new LocStringBuilder();
                    if (FeatureConfig.Obj.UseSettlementSystem)
                    {
                        sb.AppendLineLoc($"The {source} {ReputationManager.Obj.GetRepMarkedupName(target, RepTitle)} determines the cultural value of the {inputs.TypeOfWork}. Only {TextLoc.PositiveLocStr("Positive")} reputation counts, {TextLoc.NegativeLocStr("negative")} reputation is ignored.");
                        if (settlement != null)
                        {
                            sb.AppendLine();
                            sb.AppendLineLoc($"Positive reputation from Citizens of foreign settlements counts more than from Citizens of {settlement.MarkedUpName}");
                        }
                        sb.AppendLine();
                    }

                    if (FeatureConfig.Obj.UseSettlementSystem)
                    {
                        sb.AppendLine(Localizer.Do($"Base Positive {ReputationManager.Obj.GetReputationWordWithMarkedupName(target)}: {Text.StyledNum(ReputationManager.Obj.GetPositiveReputation(target))}").Dash());
                        var percent = Text.StyledPercent(1 - CultureConfig.SameSettlementReputationMutliplier);
                        if (settlement != null) sb.AppendLine(Localizer.Do($"Reputation after reducing {settlement.MarkedUpName} Citizen contributions by {percent}: {Text.StyledNum(rep)}").Dash());
                        else sb.AppendLine(TextLoc.SubtextLoc($"Note: No settlement applied at the moment, keep in mind that this culture will be reduced when placed in a settlement, with reputation from Citizens of that settlement counting {percent} less. Until then, the cultural value is {Text.StyledInt(0)}"));
                    }
                    else
                        sb.AppendLine(Localizer.NL($"{ReputationManager.Obj.GetRepMarkedupName(target, RepTitle)}: {Text.StyledNum(ReputationManager.Obj.GetPositiveReputation(target))}"));
                    return sb.ToLocString();
                }

                //The rep of the work is capped at 5x the rep of the user.
                float ratioWorkRepToPlayerRep = 5f;

                //Calculate how reputation value transfers to culture
                var playerReputationToCultureConversion = SettlementsPlugin.Obj.Config.PlayerReputationToCultureFormula;
                var workReputationToCultureConversion = SettlementsPlugin.Obj.Config.ArtworkReputationToCultureFormula;
                var userComponent = LimitMapper.MapAndDescribe(userRep, playerReputationToCultureConversion, Localizer.DoStr("culture value"), CultureUtils.AsAdjustedReputation, CultureUtils.AsCulture, repheaderUser);
                var workComponent = LimitMapper.MapAndDescribe(workRep, workReputationToCultureConversion, Localizer.DoStr("culture value"), CultureUtils.AsAdjustedReputation, CultureUtils.AsCulture, repheaderWork);
                var finalVal = Mathf.Min(userComponent.Output * ratioWorkRepToPlayerRep, workComponent.Output);
                if (FeatureConfig.Obj.UseSettlementSystem && settlement == null)
                    finalVal = 0;

                //Reenable when fixed: https://app.clickup.com/t/862j4wfmq
                //var table = TableExtensions.MakeTableLoc(Table(picture, userRepCulture), $"Artwork Value");

                var sb = new LocStringBuilder();
                sb.AppendLineLoc($"A work's {CultureUtils.Culture} value is capped at {ratioWorkRepToPlayerRep.StyledNum()}x the artist's reputation. Negative reputation is ignored.");
                if (CultureConfig.SameSettlementReputationMutliplier != 1f)
                {
                    sb.AppendLine();
                    sb.AppendLineLoc($"Reputation from the settlement that the work is currently placed in is reduced by {Text.StyledPercent(1f - CultureConfig.SameSettlementReputationMutliplier)}.  Thus, foreign opinion matters more for culture value (some works, like paintings, can be traded and yield net positive culture to two settlements).");
                }

                sb.Append(Text.Table(Table()));
                IEnumerable<(LocString, LocString)> Table()
                {
                    var workName = inputs.TypeOfWork.ToString().Capitalize();
                    yield return (Localizer.Do($"Artist ({repTarget.UserSource?.MarkedUpName}):"), userComponent.Description);
                    yield return (Localizer.NL($"{workName}:"), workComponent.Description);
                    yield return (TextLoc.BoldLoc($"Final value ({workName} value, limited to {ratioWorkRepToPlayerRep.StyledNum()}x of artist's value ({(userComponent.Output * ratioWorkRepToPlayerRep).StyledNum()})):"), finalVal.AsCulture());
                }
                if (FeatureConfig.Obj.UseSettlementSystem && settlement == null)
                    sb.Append(TextLoc.ErrorLightLoc($"Note: Culture reduced to zero because not placed in a Settlement yet."));
                else if (userComponent.Output == 0)
                    sb.Append(TextLoc.ErrorLightLoc($"Note: Culture reduced to zero because the artist has no positive reputation."));

                //Now wrap it all in another foldout.
                var header = settlement != null ? Localizer.Do($"Cultural Value in {settlement.MarkedUpName}") : Localizer.Do($"Cultural Value Without Settlement");
                var valFoldout = TextLoc.Foldout(finalVal.AsCulture(), header, sb.ToLocString());

                return ValResult.Make(finalVal, valFoldout);

                //We discount reputation that is given from a citizen of the same settlement this world object is on.
                float RepContributionTowardsCultureValue(IGivesReputation repGiver, float repGiven)
                {
                    if (repGiver is User u && (settlement?.Citizenship.HasCitizen(u) ?? false)) return repGiven * CultureConfig.SameSettlementReputationMutliplier;
                    else return repGiven;
                }

            };

            /*IEnumerable<(LocString, LocString)> Table(UserTexture picture, float userRepCulture)
            {
                yield return (Localizer.Do($"Artist's reputation:"),  Localizer.NL($"{Text.StyledNum(picture.Creator?.Reputation ?? 0)} ({picture.Creator?.MarkedUpName ?? CommonLocs.Unknown}) -> {userRepCulture.AsCulture()}"));
                yield return (Localizer.Do($"Picture's reputation:"), TextLoc.StyledNum(0f)); //todo
                yield return (Localizer.Do($"Resulting value:"), userRepCulture.AsCulture());
            }*/
        }

        static LocString RepTitle { get; } = TextLoc.UnderlineLocStr("Reputation");

    }
}
