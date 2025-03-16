// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Core.Plugins.Interfaces;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Gameplay.Players;

    /// <summary> Registers recipe variants for different difficulty settings. </summary>
    public class DifficultyBasedRecipeVariants : IModInit
    {
        public static void PostInitialize()
        {
            // Normal recipe for lower collaboration settings. Uses defaults found in Tech Tree
			// Endgame Goal world object recipes
            RecipeVariant.RegisterDefault<ComputerLabRecipe>(DifficultySettingsConfig.EndgameRecipesNormal);
            RecipeVariant.RegisterDefault<LaserRecipe>(DifficultySettingsConfig.EndgameRecipesNormal);
            // Techtree skillbook recipes
            RecipeVariant.RegisterDefault<AdvancedBakingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<AdvancedCookingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<AdvancedMasonrySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<AdvancedSmeltingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<BakingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<BasicEngineeringSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<BlacksmithSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<ButcherySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<CarpentrySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<CompositesSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<CookingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<CuttingEdgeCookingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<ElectronicsSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<FarmingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<FertilizersSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<GlassworkingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<IndustrySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<MasonrySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<MechanicsSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<MillingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<OilDrillingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<PaperMillingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
			RecipeVariant.RegisterDefault<PaintingSkillBookRecipe>(DifficultySettingsConfig.EndgameRecipesNormal);
            RecipeVariant.RegisterDefault<PotterySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<SmeltingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);
            RecipeVariant.RegisterDefault<TailoringSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesNormal);



            // Expensive recipes for higher collaboration settings. All costs are static
            RecipeVariant.Register<ComputerLabRecipe>(DifficultySettingsConfig.EndgameRecipesExpensive, new[]
            {
                new IngredientElement(typeof(AdvancedMasonryUpgradeItem), 1, true),
                new IngredientElement(typeof(CompositesUpgradeItem), 1, true),
                new IngredientElement(typeof(ElectronicsUpgradeItem), 1, true),
                new IngredientElement(typeof(IndustryUpgradeItem), 1, true),
                new IngredientElement(typeof(OilDrillingUpgradeItem), 1, true),
                new IngredientElement(typeof(AdvancedSmeltingUpgradeItem), 1, true),
                new IngredientElement(typeof(AdvancedCircuitItem), 100, true),
                new IngredientElement(typeof(PlasticItem), 100, true),
                new IngredientElement(typeof(ReinforcedConcreteItem), 200, true),
                 new IngredientElement("CompositeLumber", 200, true)

            });
            RecipeVariant.Register<LaserRecipe>(DifficultySettingsConfig.EndgameRecipesExpensive, new[]
            {
                new IngredientElement(typeof(GoldBarItem), 80, true),
                new IngredientElement(typeof(SteelBarItem), 80, true),
                new IngredientElement(typeof(FramedGlassItem), 80, true),
                new IngredientElement(typeof(AdvancedCircuitItem), 40, true),
                new IngredientElement(typeof(ElectricMotorItem), 2, true),
                new IngredientElement(typeof(RadiatorItem), 10, true)
            });
			// Expensive skill book recipes for higher collaboration settings. All costs are static
			RecipeVariant.Register<AdvancedBakingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(CulinaryResearchPaperAdvancedItem), 30, true),
                new IngredientElement(typeof(DendrologyResearchPaperModernItem), 15, true),
                new IngredientElement(typeof(GeologyResearchPaperModernItem), 15, true),
                new IngredientElement("Basic Research", 45, true),
                new IngredientElement("Advanced Research", 20, true),
                new IngredientElement("Modern Research", 20, true),
            });
			RecipeVariant.Register<AdvancedCookingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(CulinaryResearchPaperAdvancedItem), 30, true),
                new IngredientElement(typeof(DendrologyResearchPaperModernItem), 15, true),
                new IngredientElement(typeof(GeologyResearchPaperModernItem), 15, true),
                new IngredientElement("Basic Research", 45, true),
                new IngredientElement("Advanced Research", 15, true),
                new IngredientElement("Modern Research", 15, true),
            });
            RecipeVariant.Register<AdvancedMasonrySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(GeologyResearchPaperAdvancedItem), 30, true),
                new IngredientElement(typeof(GeologyResearchPaperModernItem), 15, true),
                new IngredientElement(typeof(MetallurgyResearchPaperModernItem), 15, true),
                new IngredientElement(typeof(EngineeringResearchPaperModernItem), 15, true),
                new IngredientElement("Basic Research", 45, true),
                new IngredientElement("Advanced Research", 15, true),
            });
			RecipeVariant.Register<AdvancedSmeltingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(MetallurgyResearchPaperBasicItem), 30, true),
                new IngredientElement(typeof(MetallurgyResearchPaperAdvancedItem), 30, true),
                new IngredientElement("Basic Research", 15, true),
            });
			RecipeVariant.Register<BakingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(CulinaryResearchPaperBasicItem), 15, true),
                new IngredientElement(typeof(MetallurgyResearchPaperBasicItem), 10, true),
                new IngredientElement("Basic Research", 15, true),
            });
			RecipeVariant.Register<BasicEngineeringSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(DendrologyResearchPaperAdvancedItem), 6, true),
                new IngredientElement(typeof(GeologyResearchPaperAdvancedItem), 6, true),
                new IngredientElement("Basic Research", 10, true),
            });
            RecipeVariant.Register<BlacksmithSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
{
                new IngredientElement(typeof(MetallurgyResearchPaperBasicItem), 15, true),
                new IngredientElement(typeof(DendrologyResearchPaperAdvancedItem), 10, true),
                new IngredientElement(typeof(GeologyResearchPaperAdvancedItem), 10, true),
                new IngredientElement("Basic Research", 10, true),
            });
            RecipeVariant.Register<ButcherySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(CulinaryResearchPaperBasicItem), 6, true),
            });
			RecipeVariant.Register<CarpentrySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(DendrologyResearchPaperBasicItem), 6, true),
                new IngredientElement(typeof(GatheringResearchPaperBasicItem), 6, true),
            });
			RecipeVariant.Register<CompositesSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(DendrologyResearchPaperAdvancedItem), 30, true),
                new IngredientElement(typeof(DendrologyResearchPaperModernItem), 15, true),
                new IngredientElement(typeof(MetallurgyResearchPaperModernItem), 15, true),
                new IngredientElement(typeof(EngineeringResearchPaperModernItem), 15, true),
                new IngredientElement("Basic Research", 45, true),
                new IngredientElement("Advanced Research", 15, true),
            });
			RecipeVariant.Register<CookingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(CulinaryResearchPaperBasicItem), 15, true),
                new IngredientElement(typeof(MetallurgyResearchPaperBasicItem), 10, true),
                new IngredientElement("Basic Research", 15, true),
            });
			RecipeVariant.Register<CuttingEdgeCookingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(CulinaryResearchPaperAdvancedItem), 30, true),
                new IngredientElement(typeof(CulinaryResearchPaperModernItem), 30, true),
                new IngredientElement(typeof(MetallurgyResearchPaperModernItem), 15, true),
                new IngredientElement(typeof(AgricultureResearchPaperModernItem), 15, true),
                new IngredientElement("Basic Research", 45, true),
                new IngredientElement("Advanced Research", 15, true),
            });
			RecipeVariant.Register<ElectronicsSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(MetallurgyResearchPaperAdvancedItem), 15, true),
                new IngredientElement(typeof(EngineeringResearchPaperModernItem), 15, true),
                new IngredientElement(typeof(MetallurgyResearchPaperModernItem), 30, true),
                new IngredientElement("Basic Research", 45, true),
                new IngredientElement("Advanced Research", 30, true),
                new IngredientElement("Modern Research", 30, true),
            });
			RecipeVariant.Register<FarmingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(GatheringResearchPaperBasicItem), 4, true),
                new IngredientElement(typeof(GeologyResearchPaperBasicItem), 2, true),
            });
			RecipeVariant.Register<FertilizersSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(AgricultureResearchPaperAdvancedItem), 5, true),
                new IngredientElement(typeof(GeologyResearchPaperBasicItem), 5, true),
                new IngredientElement("Basic Research", 10, true),
            });
			RecipeVariant.Register<GlassworkingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(GeologyResearchPaperBasicItem), 20, true),
                new IngredientElement(typeof(GeologyResearchPaperAdvancedItem), 10, true),
                new IngredientElement(typeof(EngineeringResearchPaperAdvancedItem), 10, true),
                new IngredientElement("Basic Research", 15, true),
            });
			RecipeVariant.Register<IndustrySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(EngineeringResearchPaperAdvancedItem), 10, true),
                new IngredientElement(typeof(EngineeringResearchPaperModernItem), 20, true),
                new IngredientElement(typeof(MetallurgyResearchPaperModernItem), 20, true),
                new IngredientElement("Basic Research", 30, true),
                new IngredientElement("Advanced Research", 20, true),
                new IngredientElement("Modern Research", 10, true),
            });
			RecipeVariant.Register<MasonrySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(GeologyResearchPaperBasicItem), 6, true),
                new IngredientElement(typeof(GatheringResearchPaperBasicItem), 6, true),
            });
			RecipeVariant.Register<MechanicsSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(EngineeringResearchPaperAdvancedItem), 15, true),
                new IngredientElement(typeof(MetallurgyResearchPaperAdvancedItem), 15, true),
                new IngredientElement("Basic Research", 30, true),
                new IngredientElement("Advanced Research", 10, true),
            });
			RecipeVariant.Register<MillingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(DendrologyResearchPaperBasicItem), 10, true),
                new IngredientElement(typeof(GeologyResearchPaperBasicItem), 10, true),
                new IngredientElement(typeof(CulinaryResearchPaperBasicItem), 10, true),
                new IngredientElement(typeof(GatheringResearchPaperBasicItem), 5, true),
                new IngredientElement("Basic Research", 15, true),
            });
			RecipeVariant.Register<OilDrillingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(AgricultureResearchPaperAdvancedItem), 20, true),
                new IngredientElement(typeof(GeologyResearchPaperModernItem), 20, true),
                new IngredientElement(typeof(DendrologyResearchPaperModernItem), 20, true),
                new IngredientElement(typeof(EngineeringResearchPaperModernItem), 20, true),
                new IngredientElement("Basic Research", 45, true),
                new IngredientElement("Advanced Research", 30, true),
            });
			RecipeVariant.Register<PaperMillingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(DendrologyResearchPaperAdvancedItem), 10, true),
                new IngredientElement(typeof(GatheringResearchPaperBasicItem), 10, true),
                new IngredientElement("Basic Research", 10, true),
            });
			RecipeVariant.Register<PaintingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
            new IngredientElement(typeof(EngineeringResearchPaperAdvancedItem), 15, typeof(BasicEngineeringSkill)),
            new IngredientElement(typeof(GatheringResearchPaperBasicItem), 15, typeof(BasicEngineeringSkill)),
            new IngredientElement("Basic Research", 30, typeof(BasicEngineeringSkill)), //noloc
            });
			RecipeVariant.Register<PotterySkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(GeologyResearchPaperBasicItem), 15, true),
                new IngredientElement(typeof(GeologyResearchPaperAdvancedItem), 10, true),
                new IngredientElement(typeof(EngineeringResearchPaperAdvancedItem), 10, true),
                new IngredientElement("Basic Research", 15, true),
            });
			RecipeVariant.Register<SmeltingSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(MetallurgyResearchPaperBasicItem), 15, true),
                new IngredientElement(typeof(DendrologyResearchPaperAdvancedItem), 10, true),
                new IngredientElement(typeof(GeologyResearchPaperAdvancedItem), 10, true),
                new IngredientElement("Basic Research", 10, true),
            });
            RecipeVariant.Register<ShipwrightSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(DendrologyResearchPaperBasicItem), 10, true),
                new IngredientElement(typeof(GatheringResearchPaperBasicItem), 10, true),
                new IngredientElement("Basic Research", 15, true), //noloc
            });
            RecipeVariant.Register<TailoringSkillBookRecipe>(DifficultySettingsConfig.SkillbookRecipesExpensive, new[]
            {
                new IngredientElement(typeof(GatheringResearchPaperBasicItem), 10, true),
                new IngredientElement("Basic Research", 10, true),
            });
        }
    }
}
