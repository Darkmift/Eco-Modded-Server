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
    [LocDisplayName("Small Wooden Boat")]
    [LocDescription("A personal transport vessel capable of holding a suprising amount of goods.")]
    [IconGroup("World Object Minimap")]
    [Weight(10000)]
    [WaterPlaceable]
    [Ecopedia("Crafted Objects", "Vehicles", createAsSubPage: true)]
    public partial class SmallWoodenBoatItem : WorldObjectItem<SmallWoodenBoatObject>, IPersistentData
    {
        public float InteractDistance => DefaultInteractDistance.WaterPlacement;
        public bool ShouldHighlight(Type block) => false;
        [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }
    }

    /// <summary>
    /// <para>Server side recipe definition for "SmallWoodenBoat".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [RequiresSkill(typeof(ShipwrightSkill), 3)]
    [Ecopedia("Crafted Objects", "Vehicles", subPageName: "Small Wooden Boat Item")]
    public partial class SmallWoodenBoatRecipe : RecipeFamily
    {
        public SmallWoodenBoatRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "SmallWoodenBoat",  //noloc
                displayName: Localizer.DoStr("Small Wooden Boat"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(WoodenHullPlanksItem), 8, typeof(ShipwrightSkill)),
                    new IngredientElement(typeof(WoodenWheelItem), 2, true),
                    new IngredientElement(typeof(GearboxItem), 2, true),
                    new IngredientElement(typeof(MediumWoodenShipFrameItem), 1, true),
                    new IngredientElement(typeof(LubricantItem), 1, true),
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<SmallWoodenBoatItem>()
                });
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 16; // Defines how much experience is gained when crafted.
            
            // Defines the amount of labor required and the required skill to add labor
            this.LaborInCalories = CreateLaborInCaloriesValue(680, typeof(ShipwrightSkill));

            // Defines our crafting time for the recipe
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(SmallWoodenBoatRecipe), start: 10, skillType: typeof(ShipwrightSkill));

            // Perform pre/post initialization for user mods and initialize our recipe instance with the display name "Small Wooden Boat"
            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Small Wooden Boat"), recipeType: typeof(SmallWoodenBoatRecipe));
            this.ModsPostInitialize();

            // Register our RecipeFamily instance with the crafting system so it can be crafted.
            CraftingComponent.AddRecipe(tableType: typeof(SmallShipyardObject), recipeFamily: this);
        }

        /// <summary>Hook for mods to customize RecipeFamily before initialization. You can change recipes, xp, labor, time here.</summary>
        partial void ModsPreInitialize();

        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [RequireComponent(typeof(StandaloneAuthComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    [RequireComponent(typeof(TailingsReportComponent))]
    [RequireComponent(typeof(MovableLinkComponent))]
    [RequireComponent(typeof(VehicleComponent))]
    [RequireComponent(typeof(BoatComponent))]
    [RequireComponent(typeof(ModularStockpileComponent))]
    [RequireComponent(typeof(MinimapComponent))]           
    [RequireComponent(typeof(PartsComponent))]
    [RepairRequiresSkill(typeof(ShipwrightSkill), 3)]
    [Ecopedia("Crafted Objects", "Vehicles", subPageName: "SmallWoodenBoat Item")]
    public partial class SmallWoodenBoatObject : PhysicsWorldObject, IRepresentsItem
    {
        static SmallWoodenBoatObject()
        {
            WorldObject.AddOccupancy<SmallWoodenBoatObject>(new List<BlockOccupancy>(0));
        }
        public override float InteractDistance => DefaultInteractDistance.WaterPlacement;
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        public override bool PlacesBlocks            => false;
        public override LocString DisplayName { get { return Localizer.DoStr("Small Wooden Boat"); } }
        public Type RepresentedItemType { get { return typeof(SmallWoodenBoatItem); } }

        private SmallWoodenBoatObject() { }
        protected override void Initialize()
        {
            this.ModsPreInitialize();
            base.Initialize();         
            this.GetComponent<VehicleComponent>().HumanPowered(1.5f);
            this.GetComponent<StockpileComponent>().Initialize(new Vector3i(2,2,2));
            this.GetComponent<PublicStorageComponent>().Initialize(18, 3500000);
            this.GetComponent<MinimapComponent>().InitAsMovable();
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Vehicles"));
            this.GetComponent<VehicleComponent>().Initialize(12, 1,1, null, true);
            this.GetComponent<BoatComponent>().Size = BoatComponent.BoatSize.Small;
            this.GetComponent<VehicleComponent>().FailDriveMsg = Localizer.Do($"You are too hungry to drive {this.DisplayName}!");
            this.ModsPostInitialize();
                        {
                this.GetComponent<PartsComponent>().Config(() => LocString.Empty, new PartInfo[]
                {
                                        new() { TypeName = nameof(WoodenHullPlanksItem), Quantity = 2},
                                        new() { TypeName = nameof(GearboxItem), Quantity = 1},
                                        new() { TypeName = nameof(LubricantItem), Quantity = 1},
                                    });
            }
        }

        /// <summary>Hook for mods to customize before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize after initialization.</summary>
        partial void ModsPostInitialize();
    }
}
