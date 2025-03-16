// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System;
	using Eco.Gameplay.Objects;
    using Eco.Gameplay.Settlements.ClaimStakes;
	using Eco.Gameplay.Settlements.Components;
	using Eco.Shared.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;

    //Define claim stakes for each type of settlement
    [Serialized, LocDisplayName("Town Claim Stake")]        public class TownClaimStakeItem       : SettlementClaimStakeItem { public override SettlementType SettlementType => SettlementType.All[0]; public override Type WorldObjectType => typeof(TownClaimStakeObject); }
    [Serialized, LocDisplayName("Country Claim Stake")]     public class CountryClaimStakeItem    : SettlementClaimStakeItem { public override SettlementType SettlementType => SettlementType.All[1]; public override Type WorldObjectType => typeof(CountryClaimStakeObject); }
    [Serialized, LocDisplayName("Federation Claim Stake")]  public class FederationClaimStakeItem : SettlementClaimStakeItem { public override SettlementType SettlementType => SettlementType.All[2]; public override Type WorldObjectType => typeof(FederationClaimStakeObject); }


    [Serialized] 
    public class TownClaimStakeObject : SettlementClaimStakeObject 
    { 
        public override SettlementType SettlementType => SettlementType.All[0];
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
    }

    [RequireComponent(typeof(InfluenceLimiterComponent))] //Not on lowest since it cant have children limited.
    [Serialized] 
    public class CountryClaimStakeObject : SettlementClaimStakeObject
    { 
        public override SettlementType SettlementType => SettlementType.All[1];
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
    }
    [RequireComponent(typeof(InfluenceLimiterComponent))]
    [Serialized] 
    public class FederationClaimStakeObject : SettlementClaimStakeObject
    { 
        public override SettlementType SettlementType => SettlementType.All[2];
        public override TableTextureMode TableTexture => TableTextureMode.Stone;
    }
}
