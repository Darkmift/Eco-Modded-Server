// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
namespace Eco.Mods.TechTree
{
	using Eco.Core.Controller;
	using Eco.Gameplay.Civics;
	using Eco.Gameplay.Civics.Constitutional;
	using Eco.Gameplay.Civics.Demographics;
	using Eco.Gameplay.Civics.Districts;
	using Eco.Gameplay.Civics.Elections;
	using Eco.Gameplay.Civics.Immigration;
	using Eco.Gameplay.Civics.Laws;
	using Eco.Gameplay.Civics.Laws.ExecutiveActions;
	using Eco.Gameplay.Civics.Objects;
	using Eco.Gameplay.Civics.Titles;
	using Eco.Gameplay.Components;
	using Eco.Gameplay.Components.Auth;
	using Eco.Gameplay.Items;
	using Eco.Gameplay.Objects;
	using Eco.Gameplay.Placement;
	using Eco.Gameplay.Settlements.Components;
	using Eco.Gameplay.Systems;
	using Eco.Gameplay.Systems.InitialSpawn;
	using Eco.Shared.Math;
	using System;
	using static Eco.Gameplay.Civics.Elections.Internal.ElectionPoll;

	//A component for performing specific civic actions
	public partial class BallotBoxItem : WorldObjectItem<BallotBoxObject> { }

    [RequireComponent(typeof(PerformCivicActionComponent))]
    [RequireComponent(typeof(JurisdictionComponent))]
    [HasCivicAction(typeof(CivicAction_Vote))]
    [HasCivicAction(typeof(CivicAction_Veto))]
    [HasCivicAction(typeof(CivicAction_StartCandidateElection))]
    [HasCivicAction(typeof(CivicAction_EnterElection))]
    [HasCivicAction(typeof(CivicAction_StartPoll))]
    [HasCivicAction(typeof(CivicAction_WithdrawFromElection))]
    public partial class BallotBoxObject : CivicObject 
    {
        //Ballot box needs crafting and storage components to be active even when there is no jurisdiction so new settlements can be created by crafting foundation items.
        //Because of that jurisdiction component shouldn't make world object disabled when no jurisdiction is selected like it does by default.
        //To still prevent civic actions from being pefrformed PerformCivicActionComponent has logic that disables all actions
        //if jurisdiction component is attached to world object but jurisdiction is not yet selected.
        partial void ModsPostInitialize() => this.GetComponent<JurisdictionComponent>().DisableWhenInvalid = false;
    }

    [RequireComponent(typeof(NameDataTrackerComponent))]
    [RequireComponent(typeof(AuthDataTrackerComponent))]
    [RequireComponent(typeof(ConstitutionComponent))] 
    public partial class CapitolObject : WorldObject, IPersistentData, ICivicObject, IMoveableWithinDeedAndInfluence
    {       
        protected override void OnCreatePostInitialize()
        {
            base.OnCreatePostInitialize();
            this.GetComponent<PropertyAuthComponent>().SetPublic();
            ConstitutionManager.ConstitutionChangedEvent.Add(c => this.Changed(nameof(this.Founded)));
        }

        public  object PersistentData { get; set; }

        //To allow reveal anim on cloth
        [SyncToView] public bool Founded => this.GetComponent<ConstitutionComponent>().Constitution?.IsActive ?? false;
    }

    [RequireComponent(typeof(PerformCivicActionComponent))]
    [RequireComponent(typeof(JurisdictionComponent))]
    [HasCivicAction(typeof(CivicAction_PerformExecutiveAction))]
    public partial class ExecutiveOfficeObject : CivicObject { }

    [RequireComponent(typeof(PerformCivicActionComponent))]
    [HasCivicAction(typeof(CivicAction_ResignFromOffice))]
    [HasCivicAction(typeof(CivicAction_RemoveFromOffice))]
    [RequireComponent(typeof(CivicObjectComponent))] public partial class GovernmentOfficeObject : CivicObject { public override Type[] CivicSlotTypes => new[] { typeof(ElectedTitle)            }; }
    [RequireComponent(typeof(CivicObjectComponent))] public partial class CensusBureauObject     : CivicObject { public override Type[] CivicSlotTypes => new[] { typeof(Demographic)             }; }
    [RequireComponent(typeof(CivicObjectComponent))] public partial class SmallCourtObject       : CivicObject { public override Type[] CivicSlotTypes => new[] { typeof(Law)                     }; public override int SlotCount => 3; }
    [RequireComponent(typeof(CivicObjectComponent))] public partial class BoardOfElectionsObject : CivicObject { public override Type[] CivicSlotTypes => new[] { typeof(ElectionProcess)         }; }
    [RequireComponent(typeof(CivicObjectComponent))] public partial class ZoningOfficeObject     : CivicObject { public override Type[] CivicSlotTypes => new[] { typeof(DistrictMap)             }; }
    [RequireComponent(typeof(CivicObjectComponent))] public partial class AmendmentsObject       : CivicObject { public override Type[] CivicSlotTypes => new[] { typeof(ConstitutionalAmendment) }; }

    [RequireComponent(typeof(CivicObjectComponent), "Laws"), RequireComponent(typeof(CivicObjectComponent), "Injunctions")]
    public partial class LargeCourtObject : CivicObject { public override Type[] CivicSlotTypes => new[] { typeof(Law), typeof(Injunction) }; public override int SlotCount => 5; }


    [RelatedFeature(nameof(FeatureConfig.UseSettlementSystem))]
    [RequireComponent(typeof(CivicObjectComponent))] public partial class ImmigrationDeskObject  : CivicObject { public override Type[] CivicSlotTypes => new[] { typeof(ImmigrationPolicy)       }; public override int SlotCount => 1; }

    [RelatedFeature(nameof(FeatureConfig.UseSettlementSystem))]
    [RequireComponent(typeof(AnnexationComponent))]
    [RequireComponent(typeof(CultureControlComponent))]
    public partial class EmbassyDeskObject      : WorldObject, IMoveableWithinDeedAndInfluence { }

    public partial class ImmigrationDeskItem    : UniqueSettlementCivicItem<ImmigrationDeskObject> 
    {
        public override Type CivicObjectType => typeof(ImmigrationPolicy);
    }

    public partial class ImmigrationDeskObject : ISpawnPositionOffset { public Vector2i SpawnPositionOffset => new Vector2i(0, 2); }
}
