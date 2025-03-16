// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Core.Plugins.Interfaces;
    using Eco.Gameplay.Civics.Constitutional;
    using Eco.Gameplay.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.SharedTypes;
    using Eco.Shared.SharedTypes.Items;

    /// <summary> Contains all <see cref="TagDefinition"/> for Mods. </summary>
    public class TagDefinitions : IModInit
    {
        private static readonly TagDefinition[] Definitions =
        {
            // Categories for the Item Filter
            new TagDefinition("Wood")               { ShowInFilter = true, PluralName = Localizer.DoStr("Wood") },
            new TagDefinition("Ore")                { ShowInFilter = true },
            new TagDefinition("Food")               { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Housing")            { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false, PluralName = Localizer.DoStr("Housing") },
            new TagDefinition("Vehicles")           { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Metal")              { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Crop")               { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Seeds")              { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Tool")               { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Power")              { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Fertilizer")         { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Clothes")            { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Crafting Table")     { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Block")              { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Upgrade")            { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Crop Seed")          { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Rock")               { ShowInFilter = true },
            new TagDefinition("Research")           { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Skill Scrolls")      { ShowInFilter = true },
            new TagDefinition("Animal")             { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Fish")               { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Fuel")               { ShowInFilter = true, AutoHighlight = false, ShowInEcopedia = false, PluralName = Localizer.DoStr("Fuel") },

            // Tags that share a name with an item
            new TagDefinition("Bread")              { AutoHighlight = false, PluralName = Localizer.DoStr("Bread") },
            new TagDefinition("Coal")               { AutoHighlight = false, PluralName = Localizer.DoStr("Coal") },
            new TagDefinition("Lumber")             { AutoHighlight = false, PluralName = Localizer.DoStr("Lumber") },

            // Tags that have a unique plural name
            new TagDefinition("AshlarStone")        { PluralName = Localizer.DoStr("Ashlar Stone") },
            new TagDefinition("HardwoodLumber")     { PluralName = Localizer.DoStr("Hardwood Lumber") },
            new TagDefinition("SofwoodLumber")      { PluralName = Localizer.DoStr("Softwood Lumber") },
            new TagDefinition("CompositeLumber")    { PluralName = Localizer.DoStr("Composite Lumber") },
            new TagDefinition("Hardwood")           { PluralName = Localizer.DoStr("Hardwood") },
            new TagDefinition("Softwood")           { PluralName = Localizer.DoStr("Softwood") },
            new TagDefinition("MortaredStone")      { PluralName = Localizer.DoStr("Mortared Stone") },
            new TagDefinition("Silica")             { PluralName = Localizer.DoStr("Silica") },
            new TagDefinition("Fat")                { PluralName = Localizer.DoStr("Fat") },
            new TagDefinition("Fungus")             { PluralName = Localizer.DoStr("Fungi") },
            new TagDefinition("Concrete")           { PluralName = Localizer.DoStr("Concrete") },
            new TagDefinition("Advanced Research")  { PluralName = Localizer.DoStr("Advanced Research") },
            new TagDefinition("Basic Research")     { PluralName = Localizer.DoStr("Basic Research") },
            new TagDefinition("Modern Reasearch")   { PluralName = Localizer.DoStr("Modern Research") },
           
            // Hidden Tags: All tags below should not be visible to players. They are used for behind the scenes gameplay mechanics

            // Specialties: For use in skill system; specialties are displayed elsewhere in Skills UI & Ecopedia
            new TagDefinition("Carpenter Specialty")            { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Chef Specialty")                 { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Engineer Specialty")             { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Farmer Specialty")               { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Hunter Specialty")               { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Mason Specialty")                { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Smith Specialty")                { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Survivalist Specialty")          { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Tailor Specialty")               { AutoHighlight = false, ShowInEcopedia = false },

            // Tools: Tags that define tool actions
            new TagDefinition("Construction")                   { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Excavation")                     { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Harvester")                      { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Logging")                        { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Planter")                        { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Plow")                           { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Tamper")                         { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Tools")                          { AutoHighlight = false, ShowInEcopedia = false },

            // Blocks: Tags for interaction context with blocks
            new TagDefinition("Block")                          { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Liquid")                         { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Constructable")                  { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Excavatable")                    { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("RoadType")                       { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Harvestable")                    { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Currency")                       { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Choppable")                      { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Clearable")                      { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Diggable")                       { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Pickupable")                     { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("FastPickupable")                 { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Minable")                        { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Reapable")                       { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Samplable")                      { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Tillable")                       { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Tilled")                         { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("UnderWater")                     { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Usable")                         { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition(BlockTags.PlaceableOnWalls)       { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition(BlockTags.PartialStack)           { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition(BlockTags.FullStack)              { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },

            // Objects: Tags for interaction context with objects
            new TagDefinition("Economy")                        { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Housing Objects")                { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Modules")                        { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Polluter")                       { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Storage Container")              { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Tech")                           { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("World Object")                   { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Elevator")                       { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("ElevatorCallPost")               { AutoHighlight = false, ShowInEcopedia = false },

            // Items: Tags for interaction context with items
            new TagDefinition("Product")                        { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Torch")                          { AutoHighlight = false, ShowInEcopedia = false },

            // Misc: Tags that should be hidden but do not fit a defined category
            new TagDefinition("Climate")                             { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Ingredient")                          { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("NotAllowedInInventories")             { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("NotInBrowser")                        { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("PlaceableOnUnownedLand")              { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("Animal")                              { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("Plant")                               { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Tree")                                { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Marine Life")                         { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("NonPlant")                            { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("Education")                           { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("Teachable")                           { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition(ConstitutionUtils.CanBeInConstitution) { AutoHighlight = false, ShowInEcopedia = false, ShowInFilter = false, Hidden = true },
            new TagDefinition(ItemTags.WasteProductTag)              { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("Hoer")                                { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },

            new TagDefinition("Bed")                            { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("Boat")                           { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("CanBeRoad")                      { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("Gear")                           { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("OnOff")                          { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("Root")                           { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("Wardrobe")                       { AutoHighlight = false, ShowInEcopedia = false, Hidden = true },
            new TagDefinition("HasFloorSurface")                { AutoHighlight = false, ShowInEcopedia = false, ShowInFilter = false, Hidden = true },
            new TagDefinition("CanBeOnTableTop")                { AutoHighlight = false, ShowInEcopedia = false, ShowInFilter = false, Hidden = true },

            // Law: tag for law trigger property
            new TagDefinition("Specialty")                      { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Profession")                     { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Plants")                         { AutoHighlight = false, ShowInEcopedia = false },
            new TagDefinition("Animals")                        { AutoHighlight = false, ShowInEcopedia = false },
        };

        /// <summary> Called on Mods initialization for the marker interface <see cref="IModInit"/>. Registers all <see cref="TagDefinition"/> from <see cref="Definitions"/>. </summary>
        public static void Initialize()
        {
            foreach (var definition in Definitions)
                TagDefinition.Register(definition);
        }
    }
}
