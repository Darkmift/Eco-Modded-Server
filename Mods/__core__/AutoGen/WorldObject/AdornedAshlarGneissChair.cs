﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// <auto-generated from WorldObjectTemplate.tt />

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Modules;
    using Eco.Gameplay.Minimap;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Pipes.Gases;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Shared.Networking;
    using Eco.Gameplay.Pipes;
    using Eco.World.Blocks;
    using Eco.Gameplay.Housing.PropertyValues;
    using Eco.Gameplay.Civics.Objects;
    using Eco.Gameplay.Settlements;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Core.Controller;
    using Eco.Core.Utils;
	using Eco.Gameplay.Components.Storage;
    using static Eco.Gameplay.Housing.PropertyValues.HomeFurnishingValue;
    using Eco.Gameplay.Items.Recipes;

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(HousingComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [RequireComponent(typeof(MountComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireRoomContainment]
    [RequireRoomVolume(4)]
    [Tag("Usable")]
    [Ecopedia("Housing Objects", "Seating", subPageName: "Adorned Ashlar Gneiss Chair Item")]
            public partial class AdornedAshlarGneissChairObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(AdornedAshlarGneissChairItem);
        public override LocString DisplayName => Localizer.DoStr("Adorned Ashlar Gneiss Chair");
        public override TableTextureMode TableTexture => TableTextureMode.Stone;

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<HousingComponent>().HomeValue = AdornedAshlarGneissChairItem.homeValue;
            this.GetComponent<MountComponent>().Initialize(1);
            this.ModsPostInitialize();
        }
        protected override void OnCreatePostInitialize()
        {
            base.OnCreatePostInitialize();
            this.GetComponent<PropertyAuthComponent>().SetPublic();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Adorned Ashlar Gneiss Chair")]
    [LocDescription("A fancy ashlar stone chair that has been adorned with gold. A throne fit for a king.")]
    [Ecopedia("Housing Objects", "Seating", createAsSubPage: true)]
    [Tag("Housing")]
    [Tag("Mountable")]
    [Weight(1000)] // Defines how heavy AdornedAshlarGneissChair is.
    [Tag(nameof(SurfaceTags.CanBeOnRug))] 
        public partial class AdornedAshlarGneissChairItem : WorldObjectItem<AdornedAshlarGneissChairObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext( 0  | DirectionAxisFlags.Down , WorldObject.GetOccupancyInfo(this.WorldObjectType));
        public override HomeFurnishingValue HomeValue => homeValue;
        public static readonly HomeFurnishingValue homeValue = new HomeFurnishingValue()
        {
            ObjectName                              = typeof(AdornedAshlarGneissChairObject).UILink(),
            Category                                = HousingConfig.GetRoomCategory("Seating"),
            BaseValue                               = 1.7f,
            TypeForRoomLimit                        = Localizer.DoStr("Chair"),
            DiminishingReturnMultiplier             = 0.6f
            
        };

    }

    /// <summary>
    /// <para>Server side recipe definition for "AdornedAshlarGneissChair".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [RequiresSkill(typeof(AdvancedMasonrySkill), 5)]
    [ForceCreateView]
    [Ecopedia("Housing Objects", "Seating", subPageName: "Adorned Ashlar Gneiss Chair Item")]
    public partial class AdornedAshlarGneissChairRecipe : Recipe
    {
        public AdornedAshlarGneissChairRecipe()
        {
            this.Init(
                name: "AdornedAshlarGneissChair",  //noloc
                displayName: Localizer.DoStr("Adorned Ashlar Gneiss Chair"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(AshlarGneissItem), 10, typeof(AdvancedMasonrySkill), typeof(AdvancedMasonryLavishResourcesTalent)),
                    new IngredientElement(typeof(GoldBarItem), 4, typeof(AdvancedMasonrySkill), typeof(AdvancedMasonryLavishResourcesTalent)),
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<AdornedAshlarGneissChairItem>()
                });
            // Perform post initialization steps for user mods and initialize our recipe instance as a tag product with the crafting system
            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(AdvancedMasonryTableObject), typeof(AdornedAshlarStoneChairRecipe), this);
        }


        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }
}
