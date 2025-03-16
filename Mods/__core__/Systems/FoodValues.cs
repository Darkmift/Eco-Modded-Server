// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Core.Plugins.Interfaces;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Players.Food;
    using Eco.Shared.Time;

    //Defines values about food
    public class FoodValue : IModInit
    {
        public static void Initialize()
        {
            //Balanced diet bonus
            Nutrients.MinBalancedDietMultiplier = .5f;   //Fully unbalanced diet
            Nutrients.MaxBalancedDietMultiplier = 1.5f;  //Fully balanced diet.

            //Food vatiery settings.
            FoodVariety.Settings = new FoodVarietySettings() { MinCaloriesToBeIncludedInVariertyBonus = 2000 };   //Number of calories that must be eaten for a food to be included in variety bonus.
            FoodVariety.Settings.LimitValues.InputMin      = 1;     //At one food type, it outputs at output min (1, no bonus)
            FoodVariety.Settings.LimitValues.InputHalflife = 20;    //After 20 types of food, we get halfway closer to the max value.
            FoodVariety.Settings.LimitValues.OutputAtMin   = 1;     //Minimum multiplier is 1
            FoodVariety.Settings.LimitValues.OutputLimit   = 1.55f; //After each set of 20 types of food, we get halfway closer to this limit value.

            //Craving values.
            //Cravings appear as a food you have a sudden desire for, and eating it will satisfy that craving and give you a boost.
            Cravings.MinAgeBeforeCravings            = TimeUtil.HoursToSeconds(2);      //Dont start having cravings till 2 hours in. 
            Cravings.TimeBetweenCravings             = TimeUtil.HoursToSeconds(2f);     //A new craving will happen this many hours after the last one is satisfied.
            Cravings.MaxTimeToSatisfyCraving         = TimeUtil.HoursToSeconds(4f);     //A craving will expire after this long/
            Cravings.MaxCravingSatisfied             = 3;                               //When satisfying a craving, it lasts 24 hours. While you have this many satisfied, new ones wont appear.
            Cravings.CravingsBoost                   = .1f;                             //Each satisfied craving will boost tastiness multiplier by this much.
            Cravings.MinCaloriesForCravings          = 500;                             //Foods require this amount of calories to be eligible for cravings.
            Cravings.MinNutrientsForCravings         = 24;                              //Foods require this amount of total nutrients to be eligible for cravings.

            //Tastiness values.
            //You get a tastiness multiplier based on the food you eat, depending what you like.  These value determine
            //the odds of food being each taste preference, and the contribution to the tastiness multiplier that each 
            //category yields. The tastiness multiplier is the average of all eaten foods in the last 24 hours weighted
            //by calories-eaten.
                                                        //Worst,     Horrible,     Bad,    Ok,    Good,    Delicious,  Favorite }
            ItemTaste.TasteWeightedRandomness    = new[] { 0f,         1f,         2f,     4f,     2f,     1f,         0f };   //Determines how many foods are in each category, by weight. The ends are zero because there may be only one of each.
            ItemTaste.TastinessMultiplier        = new[] { .7f,        .8f,       .9f,     1f,     1.1f,   1.2f,       1.3f }; //Determines the contribution a food contributes to the 'tastiness' multiplier based on its taste level. 
            ItemTaste.MinCaloriesToBeFavOrWorst  = 500;                                                                        //Determines how many calories a food must provide in order to be a favorite or least favorite food
            ItemTaste.MinNutrientsToBeFavOrWorst = 45;                                                                         //Determines the minimum number of total nutrients a food must provide in order to be a favorite or least favorite food
        }
    }
}
