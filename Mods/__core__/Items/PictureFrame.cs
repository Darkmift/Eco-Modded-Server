// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Core.Utils;
    using Eco.Gameplay.Aliases;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Culture.CultureItems;
	using Eco.Gameplay.Economy;
	using Eco.Gameplay.GameActions;
    using Eco.Gameplay.Housing.PropertyValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
	using Eco.Gameplay.Players;
	using Eco.Gameplay.Property;
    using Eco.Gameplay.Settlements.Culture;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
	using System;
	using static Eco.Gameplay.Components.ForSaleComponent;

    ///<summary>WorldObject that uses <see cref="PictureFrameComponent"/> to display images in the world.  We make it implemewnt
    ///the IHasDynamicHomeFurnishingValue so that it can apply the value of the artwork to the property value.</summary>
    [RequireComponent(typeof(PictureFrameComponent))]
    [RequireComponent(typeof(ForSaleComponent))]
    [Serialized, NotSpawnable]
    public abstract partial class PictureFrameObject : WorldObject, IHasDynamicHomeFurnishingValue
    {
        protected override void ComponentsInitialized()
		{
            var comp = this.GetComponent<PictureFrameComponent>();

            comp.PictureChangedEvent.Add(ResetForSale); //Turn off for-sale everytime picture changes
            if (!comp.HasPicture) ResetForSale();

            this.GetComponent<ForSaleComponent>().SetConfig(SaleTypes.PickupInventoryOfObject, DescribeArtwork, GetPictureItem);

            void ResetForSale()   => this.GetComponent<ForSaleComponent>().ForSale = false;
            Item GetPictureItem() => this.GetComponent<PictureFrameComponent>().GetPictureItem;
            LocString DescribeArtwork() 
            { 
                var comp = this.GetComponent<PictureFrameComponent>();
                return Localizer.Do($"{Text.InfoLight(comp.CurrentTex?.MarkedUpName)} by {comp.CurrentTex?.Creator?.MarkedUpName} (current value: {CultureUtils.AsCulture(comp.CalcArtValue)})");
            }
        }


		float     IHasDynamicHomeFurnishingValue.DynamicFurnishingValue => this.GetComponent<PictureFrameComponent>().CalcArtValue;     //The dynamic value of the artwork contributes to the total furnishing value.
		LocString IHasDynamicHomeFurnishingValue.DynamicFurnishingTitle => this.CachedSettlementAtPos != null ? Localizer.Do($"Artwork Value in {this.CachedSettlementAtPos.MarkedUpName}") : Localizer.DoStr("Artwork Value"); 
		object    IHasDynamicHomeFurnishingValue.UniqueObject           => this.GetComponent<PictureFrameComponent>().CurrentTex?.Creator;  //And we determine 'uniqueness' (for the sake of diminishing returns on repeats) based on who the artist is.
        LocString IHasDynamicHomeFurnishingValue.UniqueObjectName       => Localizer.DoStr("artist");
    }

    [NotSpawnable]
    public abstract partial class PictureFrameItem : WorldObjectItem<PictureFrameObject> { }
}
