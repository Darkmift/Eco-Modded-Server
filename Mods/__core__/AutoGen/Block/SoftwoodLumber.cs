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
    /// <para>Server side recipe definition for "SoftwoodLumber".</para>
    /// <para>More information about RecipeFamily objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Items.RecipeFamily.html</para>
    /// </summary>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    [RequiresSkill(typeof(CarpentrySkill), 1)]
    [ForceCreateView]
    [Ecopedia("Blocks", "Building Materials", subPageName: "Softwood Lumber Item")]
    public partial class SoftwoodLumberRecipe : Recipe
    {
        public SoftwoodLumberRecipe()
        {
            this.Init(
                name: "SoftwoodLumber",  //noloc
                displayName: Localizer.DoStr("Softwood Lumber"),

                // Defines the ingredients needed to craft this recipe. An ingredient items takes the following inputs
                // type of the item, the amount of the item, the skill required, and the talent used.
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement(typeof(SoftwoodBoardItem), 10, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)),
                    new IngredientElement(typeof(NailItem), 2, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)),
                    new IngredientElement(typeof(FlaxseedOilItem), 0.5f, typeof(CarpentrySkill), typeof(CarpentryLavishResourcesTalent)),
                },

                // Define our recipe output items.
                // For every output item there needs to be one CraftingElement entry with the type of the final item and the amount
                // to create.
                items: new List<CraftingElement>
                {
                    new CraftingElement<SoftwoodLumberItem>(2)
                });
            // Perform post initialization steps for user mods and initialize our recipe instance as a tag product with the crafting system
            this.ModsPostInitialize();
            CraftingComponent.AddTagProduct(typeof(SawmillObject), typeof(LumberRecipe), this);
        }


        /// <summary>Hook for mods to customize RecipeFamily after initialization, but before registration. You can change skill requirements here.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [Solid, Wall, Constructed,BuildRoomMaterialOption]
    [BlockTier(3)]
    [RequiresSkill(typeof(CarpentrySkill), 1)]
        public partial class SoftwoodLumberBlock :
        Block
        , IRepresentsItem
    {
        public virtual Type RepresentedItemType { get { return typeof(SoftwoodLumberItem); } }
    }

    [Serialized]
    [LocDisplayName("Softwood Lumber")]
    [LocDescription("Can be fashioned into various usable equipment.")]
    [MaxStackSize(500)]
    [Weight(10000)]
    [Fuel(4000)][Tag("Fuel")]
    [Ecopedia("Blocks", "Building Materials", createAsSubPage: true)]
    [Tag("Lumber")]
    [Tag("Burnable Fuel")]
    [Tag("Constructable")]
    [Tier(3)]
    public partial class SoftwoodLumberItem :
 
    BlockItem<SoftwoodLumberBlock>
    {
        public override LocString DisplayNamePlural { get { return Localizer.DoStr("Softwood Lumber"); } }

        public override bool CanStickToWalls { get { return false; } }

        private static Type[] blockTypes = new Type[] {
            typeof(SoftwoodLumberStacked1Block),
            typeof(SoftwoodLumberStacked2Block),
            typeof(SoftwoodLumberStacked3Block),
            typeof(SoftwoodLumberStacked4Block)
        };
        
        public override Type[] BlockTypes { get { return blockTypes; } }
    }

    [Tag("Lumber")]
    [Tag("Burnable Fuel")]
    [Tag("Constructable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class SoftwoodLumberStacked1Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Lumber")]
    [Tag("Burnable Fuel")]
    [Tag("Constructable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class SoftwoodLumberStacked2Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Lumber")]
    [Tag("Burnable Fuel")]
    [Tag("Constructable")]
    [Tag(BlockTags.PartialStack)]
    [Serialized, Solid] public class SoftwoodLumberStacked3Block : PickupableBlock, IWaterLoggedBlock { }
    [Tag("Lumber")]
    [Tag("Burnable Fuel")]
    [Tag("Constructable")]
    [Tag(BlockTags.FullStack)]
    [Serialized, Solid,Wall] public class SoftwoodLumberStacked4Block : PickupableBlock, IWaterLoggedBlock { } //Only a wall if it's all 4 SoftwoodLumber
}
