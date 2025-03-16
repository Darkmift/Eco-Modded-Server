// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;


    [RequireComponent(typeof(MintComponent))]
    public partial class MintObject : WorldObject { }

    [MaxStackSize(1)]
    public partial class MintItem : WorldObjectItem<MintObject>
    {
    }
}
