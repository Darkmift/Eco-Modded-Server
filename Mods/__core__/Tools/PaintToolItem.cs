// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Linq;
using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.DynamicValues;
using Eco.Gameplay.GameActions;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Mods.TechTree;
using Eco.Shared.Graphics;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Networking;
using Eco.Shared.Serialization;
using Eco.Shared.SharedTypes;
using Eco.World.Color;

/// <summary>
/// Base class for item that can paint things.
/// </summary>
[Serialized]
[Category("Hidden"), Tag("Painter")]
public class PaintToolItem : ToolItem, IInteractor
{
    [SyncToView] public bool CanHoldToPaint => false;

    [RPC] public virtual bool PaintBlock(Player player, InteractionTarget target, byte coat)
    {
        var color = player.User.Avatar.ToolState.SelectedColor;
        if (!this.TryCreateMultiblockContext(out var context, target, player, tagsTargetable: BlockTags.NonPlant, gameActionConstructor: () => new BlockPaint())) return false;

        var bucketStacks = player.User.Inventory.GetStacks<PaintBucketItem>(bucket => bucket != null && bucket.Color.EqualsNoAlpha(color)).ToList();
        return AtomicActions.PaintBlockNow(context, color, coat, bucketStacks).Success;
    }
    
    [RPC] public virtual bool PaintWorldObject(Player player, InteractionTarget target, WorldObject worldObj, int channel, byte coat)
    {
        var color = player.User.Avatar.ToolState.SelectedColor.WithAlpha(coat);
        var bucketStacks = player.User.Inventory.GetStacks<PaintBucketItem>(bucket => bucket != null && bucket.Color.EqualsNoAlpha(color)).ToList();

        var context = this.CreateMultiblockContext(player, true, worldObj.Position.XYZi(), gameActionConstructor: () => new ObjectPaint());
        return AtomicActions.PaintObjectNow(context, worldObj, color, coat, channel, bucketStacks).Success;
    }
    
    [RPC] public virtual bool ClearPaintWorldObject(Player player, InteractionTarget target, WorldObject worldObj, int channel)
    {
        var context = this.CreateMultiblockContext(player, true, worldObj.Position.XYZi(), gameActionConstructor: () => new ObjectPaintCleanup());
        return AtomicActions.ClearObjectPaintNow(context, worldObj, channel).Success;
    }
    
    [RPC] public virtual bool ClearPaint(Player player, InteractionTarget target)
    {
        if (!this.TryCreateMultiblockContext(out var context, target, player, tagsTargetable: BlockTags.NonPlant, gameActionConstructor: () => new BlockPaintCleanup())) return false;
        return AtomicActions.ClearBlockPaintNow(context).Success;
    }
}

[Serialized]
[LocDisplayName("Dev Paint Tool")]
[LocDescription("Paint tool! It paints with some developer features.")]
[Category("Hidden")]
public class DevPaintToolItem : PaintToolItem
{
    public override bool PaintBlock(Player player, InteractionTarget target, byte coat)
    {
        if (!target.BlockPosition.HasValue) return false;
        
        // For dev tool ignore laws and auth, so we just directly paint blocks
        var coatedColor = player.User.Avatar.ToolState.SelectedColor.WithAlpha(coat);
        BlockColorManager.Obj.SetColor(target.BlockPosition.Value, coatedColor);
        return true;
    }
    
    public override bool PaintWorldObject(Player player, InteractionTarget target, WorldObject worldObj, int channel, byte coat)
    {
        var paintable = worldObj?.GetComponent<PaintableComponent>();
        if (paintable == null) return false;

        var color = player.User.Avatar.ToolState.SelectedColor.WithAlpha(coat);
        paintable.SetColor(channel, color);
        return true;
    }
    
    public override bool ClearPaintWorldObject(Player player, InteractionTarget target, WorldObject worldObj, int channel)
    {
        var paintable = worldObj?.GetComponent<PaintableComponent>();
        if (paintable == null) return false;

        return paintable.ClearColor(channel);
    }

    public override bool ClearPaint(Player player, InteractionTarget target)
    {
        // For dev tool ignore laws and auth, so we just directly clear colors
        return target.BlockPosition.HasValue && BlockColorManager.Obj.ClearColor(target.BlockPosition.Value);
    }
}
