// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Gameplay.Civics
{
    using Eco.Core.Controller;
    using Eco.Core.Items;
    using Eco.Core.Systems;
    using Eco.Core.Utils;
    using Eco.Core.Utils.PropertyScanning;
    using Eco.Gameplay.Civics.Constitutional;
    using Eco.Gameplay.Civics.Elections;
    using Eco.Gameplay.Settlements;
    using Eco.Shared.Localization;
    using Eco.Shared.Networking;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Gameplay.Systems;

#nullable enable

    //Civic actions are things that can be performed by election on specific objects, or by direct execution (depending on the constitution).

    /// <summary>A civic action that lets you change the name of a settlement.</summary>
    [Serialized]
    [AddCivicActionToObjectAttribute(typeof(SettlementFoundationObject))] //Defines the CivicObject which will contain this civic action.
    [Tag(ConstitutionUtils.CanBeInConstitution)]                          //If this tag is set, rules can be made about this civic action in a constitution/constitutional amendment.
    [LocDisplayName("Change Settlement Name"), LocDescription("Rename this settlement, and attached objects if the name still matches (demographic, leader, immigration policy, etc).")]
    [RelatedFeature(nameof(FeatureConfig.UseSettlementSystem))]
    public class CivicAction_ChangeSettlementName : SettlementCivicAction, ICustomValidity
    {
        [Eco, Range(3, 50)] public string? NewName { get; set; }
        [Serialized] string oldName = string.Empty;

        public Result Valid()
        {
            if (!this.NewName.IsSet())                                                                      return Result.FailLocStr($"New name must be set.");
            if (this.NewName == this.Settlement.Name)                                                       return Result.FailLocStr($"New name must be set to a different name the the current settlement name.");
            if (Registrars.Get<Settlement>().IsNameValid(this.NewName) is Result { Success: false } result) return Result.FailLoc($"Name is not valid: {result.Message}");
            if (Registrars.Get<Settlement>().GetByName(this.NewName) != null)                               return Result.FailLoc($"The settlement '{this.NewName}' already exists.");

            return Result.Succeeded;
        }

        //Cache the old name on init.
        public override void Initialize(Settlement settlement)
        {
            base.Initialize(settlement);
            this.oldName = settlement.Name;
        }

        public override Result CanExecute(Players.User user)                 => this.Valid();
        public override LocString Description()                      => Localizer.Do($"On {this.Settlement.MarkedUpName}, rename from '{Text.InfoLight(this.oldName)}' to '{Text.InfoLight(this.NewName)}.");

        //Do the actual civic action.
        public override Result Perform(Players.User user, Election election) => Registrars.Get<Settlement>().Rename(this.Settlement, this.NewName!, false) ? Result.Succeeded : Result.FailLocStr("Rename failed.");
    }
}
