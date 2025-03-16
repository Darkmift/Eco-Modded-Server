// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Core.Controller;
    using Eco.Core.PropertyHandling;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Storage;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Gameplay.Systems.NewTooltip.TooltipLibraryFiles;
    using Eco.Shared.Graphics;
    using Eco.Shared.Items;
    using Eco.Shared.Localization;
	using Eco.Shared.Serialization;
	using Eco.Shared.Services;
    using Eco.Shared.Utils;

    [ItemGroup("Paint")]
    public partial class PaintBucketItem
    {
        public override LocString Label => Localizer.DoStr("Paint Bucket");
        protected override string ColoredOverlayName => "PaintBucketPaint";

        /// <summary> On paint bucket right click - select its color for painting</summary>
		public override string OnUsed(Player player, ItemStack itemStack)
        {
            // Can be selected only from inside users inventory
            if (!player.User.Inventory.Contains(itemStack.SingleItemAsEnumerable()))
            {
                player.MsgLocStr("Can select color only from your inventory!", NotificationStyle.Error);
                return string.Empty;
            }

            var color = (itemStack.Item as PaintBucketItem)?.Color ?? ByteColor.White;

            player.User.Avatar.ToolState.SelectedColor = color;
            return string.Empty;
        }
    }

    [RequireComponent(typeof(MixingComponent))]
    [RequireComponent(typeof(PublicStorageComponent))]
    public partial class PaintMixerObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            var storageComponent = this.GetComponent<PublicStorageComponent>();

            storageComponent.Initialize(24);
            storageComponent.Storage.AddInvRestriction(new PaintItemRestriction());
            storageComponent.Storage.AddInvRestriction(new StackLimitRestriction(500));
            storageComponent.Storage.AddInvRestriction(new NotCarriedRestriction()); // can't store block or large items
        }
    }
}
