﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// <auto-generated from BlockTemplate.tt/>

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Core.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.SharedTypes;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.World.Water;
    using Eco.Gameplay.Pipes;
    using Eco.Core.Controller;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Shared.Graphics;
    using Eco.World.Color;

    /// <summary>Auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization.</summary>

    /// <summary>
    /// <para>Server side recipe definition for "CottonCurtains".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [RequiresSkill(typeof(TailoringSkill), 3)]
    [Ecopedia("Blocks", "Building Materials", subPageName: "Cotton Curtains Item")]
    public partial class CottonCurtainsRecipe : RecipeFamily
    {
        public CottonCurtainsRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "CottonCurtains",  //noloc
                displayName: Localizer.DoStr("Cotton Curtains"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(CottonFabricItem), 5, typeof(TailoringSkill), typeof(TailoringLavishResourcesTalent)),
                    new IngredientElement(typeof(CottonThreadItem), 2, typeof(TailoringSkill), typeof(TailoringLavishResourcesTalent)),
                    new IngredientElement("HewnLog", 2, typeof(TailoringSkill), typeof(TailoringLavishResourcesTalent)), //noloc
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<CottonCurtainsItem>(4)
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 1.5f; // Defines how much experience is gained when crafted.
            
            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(120, typeof(TailoringSkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(CottonCurtainsRecipe), start: 2, skillType: typeof(TailoringSkill), typeof(TailoringFocusedSpeedTalent), typeof(TailoringParallelSpeedTalent));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Cotton Curtains"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Cotton Curtains"), recipeType: typeof(CottonCurtainsRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(LoomObject), recipeFamily: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [Solid, Wall, Constructed,BuildRoomMaterialOption]
    [RequiresSkill(typeof(TailoringSkill), 3)]
        public partial class CottonCurtainsBlock :
        Block
        , IRepresentsItem
    {
        public virtual Type RepresentedItemType { get { return typeof(CottonCurtainsItem); } }
    }

    [Serialized]
    [LocDisplayName("Cotton Curtains")]
    [LocDescription("Curtains woven using the finest cotton. \n\n (Only cosmetic does not impact room value.)")]
    [MaxStackSize(500)]
    [Weight(5000)]
    [Ecopedia("Blocks", "Building Materials", createAsSubPage: true)]
    [Tag("Constructable")]
    public partial class CottonCurtainsItem :
 
    BlockItem<CottonCurtainsBlock>
    {
        public override LocString DisplayNamePlural { get { return Localizer.DoStr("Cotton Curtains"); } }

        public override bool IgnoreRooms     { get { return true;  } }

        private static Type[] blockTypes = new Type[] {
            typeof(CottonCurtainsStacked1Block),
            typeof(CottonCurtainsStacked2Block),
            typeof(CottonCurtainsStacked3Block),
            typeof(CottonCurtainsStacked4Block)
        };
        
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Tag("Constructable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class CottonCurtainsStacked1Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Constructable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class CottonCurtainsStacked2Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Constructable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class CottonCurtainsStacked3Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Constructable")]
    [Tag(BlockTags.FullStack)]
    [Serialized, Solid,Wall] public class CottonCurtainsStacked4Block : PickupableBlock, IWaterLoggedBlock { } //Only a wall if it's all 4 CottonCurtains
}
