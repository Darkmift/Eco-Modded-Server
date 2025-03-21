﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// <auto-generated BlockFromGroupTemplate.tt/>

namespace Eco.Mods.TechTree
{
    using System;
    using Eco.Gameplay.Blocks;
    using Eco.Shared.Localization;

    /// <summary>
    /// <para>
    /// Server side definition for the "Basic" block form group. 
    /// This object inherits the FormGroup base class.
    /// </para>
    /// <para>More information about FormGroup objects can be found at https://docs.play.eco/api/server/eco.gameplay/Eco.Gameplay.Blocks.FormGroup.html</para>
    /// <remarks>
    /// This is an auto-generated class. Don't modify it! All your changes will be wiped with next update! Use Mods* partial methods instead for customization. 
    /// If you wish to modify this class, please create a new partial class or follow the instructions in the "UserCode" folder to override the entire file.
    /// </remarks>
    /// </summary>
    public partial class BasicFormGroup : FormGroup
    {
        /// <summary>Defines the name of the form group.</summary>
        public override string Name => "Basic"; //noloc
        /// <summary>The pural localization name for block form group.</summary> 
        public override LocString DisplayName => Localizer.DoStr("Basic");
        /// <summary>The tooltip description for the food item.</summary>
        public override LocString DisplayDescription => Localizer.DoStr("Basic");
        /// <summary>Defines the sort order used by this block forum group</summary>
        public override int SortOrder => 1;
    }
}
