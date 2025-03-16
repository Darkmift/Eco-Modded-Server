// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System.ComponentModel;
    using Eco.Core.Controller;
    using Eco.Core.Items;
    using Eco.Gameplay.Auth;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Wires;
    using Eco.Gameplay.Settlements;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Networking;
    using Eco.Shared.Serialization;
    using Eco.Simulation.Time;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Aliases;
    using Eco.Shared.Items;
    using Eco.Shared.IoC;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Shared.SharedTypes;
    using Eco.Gameplay.Economy.Transfer;
	using Eco.Gameplay.Systems.EnvVars;

	//This Toll object demonstrates the use of auto-generated UI for components, check out the attributes assigned to the members and compare to the UI it creates.
	//A toll object will, when triggered (either by a user paying for it or by the owner using it), send an activation to any touching objects, using wires to transmit.  
	//Use '/give TollItem' to get an example in-game.
	[Serialized, LocDisplayName("Toll"), LocDescription("Toggle on any touching wires and electronic objects."), Category("Hidden"), NoIcon]
    public class TollItem : WorldObjectItem<TollObject>, IHasEnvVars
    {
    }

    [Serialized]
    [RequireComponent(typeof(TollComponent))]
    [RequireComponent(typeof(AttachmentComponent))]
    [RequireComponent(typeof(MustBeOwnedComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    public class TollObject : WorldObject
    {
        static TollObject()
        {
            AddOccupancyList(typeof(TollObject), new BlockOccupancy(Vector3i.Zero, typeof(BuildingWorldObjectBlock)));
        }

        protected override void OnCreatePostInitialize()
        {
            base.OnCreatePostInitialize();
            this.GetComponent<PropertyAuthComponent>().SetPublic();
        }

        public override LocString DisplayName { get { return Localizer.DoStr("Toll"); } }
    }


    [Tag("Economy"), Category("Hidden"), NoIcon, Serialized, AutogenClass, LocDisplayName("Toll")]
    public class TollComponent : SwitchComponent
    {
        // --------------------- Defining the UI of a component ---------------------
        // Properties that are marked with the 'Autogen' and 'SyncToView' attribute will be shown in the UI.
        // Properties that are marked with the 'AutoRPC' attribute will be editable from that UI.
        [SyncToView, Autogen, AutoRPC] public Currency Currency                     { get { return this.currencyHandle; } set { this.currencyHandle = value; } }
        [SyncToView, Autogen, AutoRPC, Serialized] public float TollAmount          { get; set; }
        [SyncToView, Autogen, AutoRPC, Serialized] public float TimeBeforeSwitchOff { get; set; }

        // The 'Eco' attribute is a shortcut to imply all above attributes, and will make the property appear and be editable.
        [Eco] public BankAccount TargetBankAccount { get; set; }

        // The following method will be represented with a button on the Component's UI, because it is marked with the 'Autogen' attribute.
        // The 'OwnerHidden' attribute hides it from the object's Owner, and the 'GuestEditable' attribute makes it appear for guests.
        // When the Player presses that button, the code inside this method will be executed.
        [RPC, Autogen, OwnerHidden, GuestEditable] public void PayToll(Player player)
        {
            if (!this.Enabled)                   { player.ErrorLocStr("Toll is disabled. Check status."); return; }
            if (this.HasFreeToll(player))        { player.ErrorLocStr("Since you're authorized, toll is not required."); return; }
            if (this.Parent.Owners == null)      { player.ErrorLocStr("Object does not have an owner, cannot be used."); return; }
            if (this.On)                         { player.ErrorLocStr("Toll is already activated, wait for it to expire (owner can turn off manually)."); return; }

            if (Transfers.TransferNow(player.User, new TransferData {
                Amount                = this.TollAmount,
                SourceAccount         = player?.User.BankAccount,
                TargetAccount         = this.TargetBankAccount,
                Sender                = player?.User,
                TransferType          = TransferType.TollPayment,
                TransferDescription   = Localizer.Do($"Toll at {this.Parent.UILink()}"),
                Currency              = this.Currency,
                TaxSettlementScopes   = SettlementUtils.GetSettlementsAtPos(this.Parent.Position3i)
            })) this.DoSwitch(player);
        }


        // The following variables are not gonna be displayed in the UI.
        [Serialized] double worldTimeToReactivate = 0f;
        [Serialized] Currency currencyHandle;

        public TollComponent()
        {
            this.TollAmount = 1f;               // Default toll amount.
            this.TimeBeforeSwitchOff = 10f;     // Default time to switch off after being switched on.
            this.OnChanged.Add(OnStateChanged); // Start the timer to switch off.
            void OnStateChanged(bool on) { if (on && this.TimeBeforeSwitchOff != 0) this.worldTimeToReactivate = WorldTime.Seconds + this.TimeBeforeSwitchOff; }
        }
        public override void OnCreate() => this.Currency = CurrencyManager.GetPlayerCurrency(this.Parent.NameOfCreator);
        public override void Tick() { if (this.worldTimeToReactivate > 0f && this.On && WorldTime.Seconds > this.worldTimeToReactivate) this.DoSwitch(this.Parent.Creator?.Player); }


        // --------------------- Interactions workflow ---------------------
        // The key concepts of creating Component-based interactions are:
        //      1) Declare the Interaction Definitions:
        //          These define the 'Possible Interactions' that the Player can do when looking at the WorldObject that contains this Component.
        //          They can be shown conditionally based on whether specific 'Interaction Parameters' are present in said Player's context.
        //          Potential Interactions that pass all conditions will be displayed in the in-game UI when a Player targets the object.
        //          When the Player triggers the interaction, the code of the interaction will be executed with that Player and their Context as parameters.
        //      2) Declare the Interaction Parameters:
        //          Parameters are strings that may or may not be present in a player's context.
        //          By themselves, they're just strings, but they can act as conditions for specific interactions to only appear when we want them to.
        //          (e.g.: Only show 'Change Ownership' on WorldObjects if the player is the object's creator).
        //          The parameters can also be passed from client side, when looking at specific parts of a GameObject (see 'Client:SpecificInteractable.cs').
        //          (e.g.: Only show 'Mount' when targeting a vehicle's Mount Seat)
        

        // -------- Declare the 'Interaction Parameters' this component generates --------
        // - Mark this method with the 'InteractionParameter' attribute, so the 'hasFreeToll' parameter will be generated for players that are Authed.
        // - The parameter will be evaluated on a per-player basis, the first time the player looks at the WorldObject that contains this component.
        // - If a parameter is 'Changed', it will be re-evaluated the next time the player targets the WorldObject.
        // - This specific parameter definition will add 'hasFreeToll' if it evaluates to 'true', and will add no parameters when it evaluates to 'false'.
        // - You can find additional info about the different 'InteractionParameter' attributes and how they work by reading the comments in 'InteractionParameterAttribute.cs'.
        [EnvVar] public bool HasFreeToll(Player player) => ServiceHolder<IAuthManager>.Obj.IsAuthorized(this.Parent, player?.User, AccessType.FullAccess);

        // -------- Do any necessary subscriptions to declare the conditions of 'updating' the parameters we defined -------- 
        public override void Initialize()
        {
            base.Initialize();

            var authComponent = this.Parent.GetComponent<AuthComponent>();
            authComponent.OwnerChanged.Add(_=>OnAuthChanged());                          // Subscribe to make auth changes clear the cached parameters.
            authComponent.AuthChanged.Add((_) => OnAuthChanged());                  // ..the parameter will then be re-evaluated per interacting player.
            void OnAuthChanged() => this.Changed(nameof(this.HasFreeToll));  // We trigger a parameter cache clear by calling the 'Changed' method for a parameter property.
        }

        // -------- Declare the 'Possible Interactions' this component generates --------
        // - Notice the interactions here have a priority of -1. This is done to prioritize them over the interactions of parent type 'SwitchComponent'.
        // - Whenever multiple interactions have the exact same trigger and their conditions match, only the one with the higher priority will appear.
        //      1) This interaction has no parameters and will always be visible for all users.
        [Interaction(InteractionTrigger.RightClick, "Pay Toll", priority: -1, authRequired: AccessType.ConsumerAccess)]
        public void PayTollAndSwitch(Player player, InteractionTriggerInfo trigger, InteractionTarget target) => this.PayToll(player);

        //      2) This interaction will only appear if the 'hasFreeToll' parameter is there.
        [Interaction(InteractionTrigger.LeftClick, "Open Without Toll", priority: -1, authRequired: AccessType.ConsumerAccess, requiredEnvVars: new[] { nameof(HasFreeToll) })]
        public void OpenWithoutToll(Player player, InteractionTriggerInfo trigger, InteractionTarget target)
        {
            if (this.HasFreeToll(player)) this.DoSwitch(player); // Authorize the interaction, and, if all go well, perform the switch.
            else                                 player.ErrorLocStr("You don't have access to directly switch this; you must pay the toll.");
        }
    }
}
