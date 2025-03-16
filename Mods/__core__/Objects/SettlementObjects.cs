// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Objects;
    using Eco.Shared.Items;
    using Eco.Core.Controller;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Settlements;
    using Eco.Gameplay.Civics;
    using Eco.Gameplay.Settlements.Components;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.InitialSpawn;
    using Eco.Shared.Math;

    [RelatedFeature(nameof(FeatureConfig.UseSettlementSystem))] public partial class TownFoundationItem : SettlementFoundationItem<TownFoundationObject> 
    { protected override SettlementType SettlementType => new SettlementType(0); }
    [RelatedFeature(nameof(FeatureConfig.UseSettlementSystem))] public partial class CountryFoundationItem : SettlementFoundationItem<CountryFoundationObject>
    { protected override SettlementType SettlementType => new SettlementType(1); }
    [RelatedFeature(nameof(FeatureConfig.UseSettlementSystem))] public partial class FederationFoundationItem : SettlementFoundationItem<FederationFoundationObject>
    { protected override SettlementType SettlementType => new SettlementType(2); }

    [RequireComponent(typeof(CitizenRosterComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(ConstitutionComponent))]
    [RelatedFeature(nameof(FeatureConfig.UseSettlementSystem))]
    [HasCivicAction(typeof(CivicAction_JoinSettlement))]
    [HasCivicAction(typeof(CivicAction_SecedeFromParentSettlement))]
    [HasCivicAction(typeof(CivicAction_RevokeCitizenship))]
    [HasCivicAction(typeof(CivicAction_DissolveSettlement))]
    [HasCivicAction(typeof(CivicAction_CancelSettlementActions))]
    public partial class TownFoundationObject : SettlementFoundationObject, ISpawnPositionOffset
    {
        public override SettlementType SettlementType      => new SettlementType(0);
        public          Vector2i       SpawnPositionOffset => new Vector2i(-1, 0);
    }

    [RequireComponent(typeof(CitizenRosterComponent))]
    [RequireComponent(typeof(SettlementRosterComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(ConstitutionComponent))]
    [RelatedFeature(nameof(FeatureConfig.UseSettlementSystem))]
    [HasCivicAction(typeof(CivicAction_JoinSettlement))]
    [HasCivicAction(typeof(CivicAction_AddChildSettlement))]
    [HasCivicAction(typeof(CivicAction_CedeChildSettlement))]
    [HasCivicAction(typeof(CivicAction_SecedeFromParentSettlement))]
    [HasCivicAction(typeof(CivicAction_DissolveSettlement))]
    [HasCivicAction(typeof(CivicAction_CancelSettlementActions))]
    [HasCivicAction(typeof(CivicAction_RevokeCitizenship))]
    public partial class CountryFoundationObject : SettlementFoundationObject, ICivicObject, ISpawnPositionOffset
    {
        public override SettlementType SettlementType      => new SettlementType(1);
        public          Vector2i       SpawnPositionOffset => new Vector2i(1, 0);
    }

    [RequireComponent(typeof(CitizenRosterComponent))]
    [RequireComponent(typeof(SettlementRosterComponent))]
    [RequireComponent(typeof(RoomRequirementsComponent))]
    [RequireComponent(typeof(ConstitutionComponent))]
    [RelatedFeature(nameof(FeatureConfig.UseSettlementSystem))]
    [HasCivicAction(typeof(CivicAction_AddChildSettlement))]
    [HasCivicAction(typeof(CivicAction_CedeChildSettlement))]
    [HasCivicAction(typeof(CivicAction_DissolveSettlement))]
    [HasCivicAction(typeof(CivicAction_CancelSettlementActions))]
    [HasCivicAction(typeof(CivicAction_RevokeCitizenship))]
    public partial class FederationFoundationObject : SettlementFoundationObject, ISpawnPositionOffset
    {
        public override SettlementType SettlementType      => new SettlementType(2);
        public          Vector2i       SpawnPositionOffset => new Vector2i(0, 1);
    }

}
