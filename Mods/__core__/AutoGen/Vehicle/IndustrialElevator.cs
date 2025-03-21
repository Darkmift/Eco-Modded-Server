﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// <auto-generated from VehicleTemplate.tt />

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Core.Controller;
    using Eco.Core.Items;
    using Eco.Core.Utils;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
	using Eco.Gameplay.Components.Storage;
    using Eco.Gameplay.Components.VehicleModules;
    using Eco.Gameplay.GameActions;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Items.Recipes;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Systems.Exhaustion;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Math;
    using Eco.Shared.Networking;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.Items;
    using static Eco.Gameplay.Components.PartsComponent;

    [Serialized]
    [LocDisplayName("Industrial Elevator")]
    [LocDescription("An industrial elevator for transporting extra large loads vertically. It requires a 6x10 vertical shaft to function.")]
    [IconGroup("World Object Minimap")]
    [Weight(25000)]
    [Ecopedia("Crafted Objects", "Specialty", createAsSubPage: true)]
    public partial class IndustrialElevatorItem : WorldObjectItem<IndustrialElevatorObject>, IPersistentData
    {
        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "IndustrialElevator".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [RequiresSkill(typeof(IndustrySkill), 2)]
    [Ecopedia("Crafted Objects", "Specialty", subPageName: "Industrial Elevator Item")]
    public partial class IndustrialElevatorRecipe : RecipeFamily
    {
        public IndustrialElevatorRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "IndustrialElevator",  //noloc
                displayName: Localizer.DoStr("Industrial Elevator"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(FlatSteelItem), 30, typeof(IndustrySkill)),
                    new IngredientElement(typeof(SteelGearboxItem), 4, typeof(IndustrySkill)),
                    new IngredientElement(typeof(CopperWiringItem), 20, typeof(IndustrySkill)),
                    new IngredientElement(typeof(ElectricMotorItem), 2, true),
                    new IngredientElement(typeof(LubricantItem), 2, true),
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<IndustrialElevatorItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 20; // Defines how much experience is gained when crafted.
            
            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(2500, typeof(IndustrySkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(IndustrialElevatorRecipe), start: 10, skillType: typeof(IndustrySkill));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Industrial Elevator"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Industrial Elevator"), recipeType: typeof(IndustrialElevatorRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(RoboticAssemblyLineObject), recipeFamily: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(ElevatorComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(MinimapComponent))]           
    [RequireComponent(typeof(PartsComponent))]
    [RepairRequiresSkill(typeof(IndustrySkill), 2)]
    [Ecopedia("Crafted Objects", "Specialty", subPageName: "IndustrialElevator Item")]
    public partial class IndustrialElevatorObject : PhysicsWorldObject, IRepresentsItem
    {
        static IndustrialElevatorObject()
        {
            WorldObject.AddOccupancy<IndustrialElevatorObject>(new List<BlockOccupancy>(0));
        }
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        public override bool PlacesBlocks            => false;
        public override LocString DisplayName { get { return Localizer.DoStr("Industrial Elevator"); } }
        public Type RepresentedItemType { get { return typeof(IndustrialElevatorItem); } }

        private IndustrialElevatorObject() { }
        protected override void Initialize()
        {
            this.ModsPreInitialize();
            base.Initialize();         
            var elevatorComponent = this.GetComponent<ElevatorComponent>();
            elevatorComponent.Initialize();
            this.GetComponent<MinimapComponent>().InitAsMovable();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Vehicles"));
            this.ModsPostInitialize();
                        {
                this.GetComponent<PartsComponent>().Config(() => LocString.Empty, new PartInfo[]
                {
                                        new() { TypeName = nameof(ElectricMotorItem), Quantity = 1},
                                        new() { TypeName = nameof(LubricantItem), Quantity = 2},
                                    });
            }
        }

        /// <summary>Hook for mods to customize before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize after initialization.</summary>
        partial void ModsPostInitialize();
    }
}
