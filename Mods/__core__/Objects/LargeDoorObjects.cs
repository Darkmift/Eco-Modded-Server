// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Components;
    using Eco.Shared.Localization;
    using Eco.Shared.Items;

    public partial class LargeLumberDoorObject : WorldObject
    {
        protected override void PostInitialize() => LargeDoorUtils.InitializeDoor(this);
    }

    public partial class LargeWindowedLumberDoorObject : WorldObject
    {
        protected override void PostInitialize() => LargeDoorUtils.InitializeDoor(this);
    }

    public partial class LargeCorrugatedSteelDoorObject : WorldObject
    {
        protected override void PostInitialize() => LargeDoorUtils.InitializeDoor(this);
    }
	
	    public partial class WoodenSlidingDoorObject : WorldObject
    {
        protected override void PostInitialize() => LargeDoorUtils.InitializeDoor(this);
    }
	
	    public partial class ShojiDoorObject : WorldObject
    {
        protected override void PostInitialize() => LargeDoorUtils.InitializeDoor(this);
    }

    public static class LargeDoorUtils
    {
        /// <summary> Custom messages container for the OnOffComponent of Large Doors.</summary>
        class LargeDoorMessagesContainer : OnOffComponent.IOnOffMessagesContainer
        {
            public LocString TurnOnMessage        => Localizer.DoStr("Close");
            public LocString TurnOffMessage       => Localizer.DoStr("Open");
            public LocString TurnedOnMessage      => Localizer.DoStr("Closed");
            public LocString TurnedOffMessage     => Localizer.DoStr("Open");
            public LocString NotAuthedMessage     => Localizer.DoStr("You are not authorized to Open/Close this door.");
            public LocString InvalidStatusMessage => Localizer.DoStr("This door has an invalid status and cannot be opened.");
        }

        static LargeDoorMessagesContainer msgContainer = new LargeDoorMessagesContainer();

        public static void InitializeDoor(WorldObject door) => door.GetComponent<OnOffComponent>().Setup(null, AccessType.ConsumerAccess, true, msgContainer);
    }
}
