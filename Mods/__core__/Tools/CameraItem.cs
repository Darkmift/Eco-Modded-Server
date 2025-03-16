// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Core.Controller;
    using Eco.Core.Items;
    using Eco.Core.Utils;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Systems.Messaging.Chat.Commands;
    using Eco.Gameplay.Systems.UserTextures;
    using Eco.Gameplay.Utils;
    using Eco.Shared.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Networking;
    using Eco.Shared.Serialization;
    using Eco.Shared.SharedTypes;
    using PropertyChanged;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Eco.Gameplay.UI;


    public interface IGameCamera
    {
        [RPC] public Task CaptureImage(Player player, byte[] imageData);
        [RPC] public bool ConsumeResources(Player player);
    }

    public class InGameCameraUtils 
    {
        //UI names
        public const string TripodUI = "CameraModeUITripod";
        public const string EaselUI  = "CameraModeUIEasel";

        ///<summary>Creates a PictureItem using the image information and tries to add it to the player's inventory.</summary>
        public static void CaptureImage(Player player, UserTexture texture) 
        {
            //Creates a user texture using the image information and creates a picture item using the user texture
            var picture = new PictureItem(texture);
            var result  = player.User.Inventory.TryAddItem(picture);
            if (result.Success) player.InfoBoxLocStr("Picture added to your inventory.");
            else player.ErrorLocStr(result.Message);
        }

        public static Result ConsumeResources(Player player, Type item) => player.User.Inventory.TryRemoveItem<Item>(player.User);


    }

    /// <summary> Cameras allow users to capture and store images. </summary>
    /// <remarks> It's a 'ToolItem' because it needs durability, but it doesn't interact with any other items directly. </remarks>
    [Category("Hidden"), AddINotifyPropertyChangedInterface]
    public abstract partial class CameraItem : ToolItem, IGameCamera, IInteractor
    {
        [Interaction(InteractionTrigger.LeftClick, canHoldToTrigger: TriBool.True, animationDriven: true, Flags = InteractionFlags.NoTargetRequired)]
        public void CameraInteraction(Player player, InteractionTriggerInfo trigger, InteractionTarget target) { }

        //Called when the player is trying to take a picture, validates the durability and film available. If there is durability and film available
        //reduces them and returns true. If durability or film are not enough returns false
        [RPC] public bool ConsumeResources(Player player)
        {
            if (this.Durability > 0 && player.User.Inventory.TryRemoveItem<CameraFilmItem>(player.User)) 
            {
                this.Durability = Math.Max(0, this.Durability - 1); //Reduce durability 
                return true;
            }
            return false;
        }

        [RPC] public async Task CaptureImage(Player player, byte[] imageData) => InGameCameraUtils.CaptureImage(player, await UserTextureManagerServer.Obj.CreateUserTexture(imageData, player.User, UserTextureType.Screenshot));
    }

    /// <summary> World object with a custom UI (In-Game cameras UI). Allows player to take pictures </summary>
    [RequireComponent(typeof(CameraComponent))]
    [RequireComponent(typeof(MountComponent))]
    public partial class TripodCameraObject : WorldObject, IRepresentsItem, IGameCamera 
    {
        MountComponent mountComponent;

        protected override void PostInitialize() 
        {
            this.mountComponent = this.GetComponent<MountComponent>();
            this.mountComponent.Initialize(1, false);

            this.GetComponent<CameraComponent>().Initialize(this.mountComponent);
        }

        public override void Use(Player player, InteractionTarget target, InteractionTriggerInfo triggerInfo, string ui) 
        {
            if( this.mountComponent.MountSeatOnFree(player) != -1) base.Use(player, target, triggerInfo, InGameCameraUtils.TripodUI);
        }

        [RPC] public bool ConsumeResources(Player player)                 => player.User.Inventory.TryRemoveItem<CameraFilmItem>(player.User);
        [RPC] public async Task CaptureImage(Player player, byte[] image) => InGameCameraUtils.CaptureImage(player, await UserTextureManagerServer.Obj.CreateUserTexture(image, player.User, UserTextureType.Screenshot));
        [RPC] public void Release(Player player)                          => this.mountComponent.Dismount(player);
    }

    ///<summary>World object with a custom UI (In-Game cameras UI). Allows player to take a reference image and then create a picture while painting on the easel</summary>
    [RequireComponent(typeof(CameraComponent))]
    [RequireComponent(typeof(MountComponent))]
    public partial class EaselObject : WorldObject, IRepresentsItem, IGameCamera
    {
        MountComponent mountComponent;

        protected override void PostInitialize() 
        {
            this.mountComponent = this.GetComponent<MountComponent>();
            this.mountComponent.Initialize(1, false);

            this.GetComponent<CameraComponent>().Initialize(this.mountComponent);
        }

        public override void Use(Player player, InteractionTarget target, InteractionTriggerInfo triggerInfo, string ui) 
        {
            if( this.mountComponent.MountSeatOnFree(player) != -1) base.Use(player, target, triggerInfo, InGameCameraUtils.EaselUI);
        }

        [RPC] public bool ConsumeResources(Player player)                 => player.User.Inventory.TryRemoveItem<ArtSuppliesItem>(player.User);
        [RPC] public async Task CaptureImage(Player player, byte[] image) => InGameCameraUtils.CaptureImage(player, await UserTextureManagerServer.Obj.CreateUserTexture(image, player.User, UserTextureType.Screenshot));
        [RPC] public void Release(Player player)                          => this.mountComponent.Dismount(player);
    }

    /// <summary> Component to handle the pickup and avoid players removing the world object while is used </summary>
    [Serialized, CreateComponentTabLoc, HasIcon]
    public class CameraComponent : WorldObjectComponent, IPickupConfirmationComponent 
    {
        MountComponent mountComponent;
        public void Initialize(MountComponent mountComponent) => this.mountComponent = mountComponent;

        public override InventoryMoveResult TryPickup(Player player, InventoryChangeSet invChanges, Inventory targetInventory, bool force)
        {
            if (this.mountComponent.IsMounted) return Result.FailLoc($"Can't pick up while {this.mountComponent.MountedPlayersNames} uses the object.");
            return base.TryPickup(player, invChanges, targetInventory, force);
        }
    }
}
