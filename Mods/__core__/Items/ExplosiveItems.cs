// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Shared.Serialization;

    // Tier 1 explosion item
    public partial class DynamiteItem : WorldObjectItem<DynamiteObject> { }

    // Tier 2 explosion item
    public partial class MiningChargeItem : WorldObjectItem<MiningChargeObject> { }

    // Tier 2 explosion tool for detonation
    public partial class RemoteDetonatorItem : DetonatorBaseItem { }
}
