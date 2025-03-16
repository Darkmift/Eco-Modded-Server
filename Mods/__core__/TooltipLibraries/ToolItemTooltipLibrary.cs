// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Gameplay.Systems.NewTooltip.TooltipLibraryFiles;
    using Eco.Shared.Items;
    using Eco.Shared.Utils;
    using Eco.Shared.Localization;
    using Eco.World.Blocks;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    //ToolItem can not be refrenced in Eco.Gameplay so its tooltip library should be declared here.
    [TooltipLibrary]
    public static class ToolItemTooltipLibrary
    {
        public static void Initialize() { }

        [NewTooltip(CacheAs.User | CacheAs.SubType, 200, overrideType: typeof(PickaxeItem))]
        public static LocString MinablesTooltip(Type type, User user, TooltipOrigin origin)
        {
            var item = Item.Get(type) as PickaxeItem;
            var myHardness = item.PerkDamage.GetCurrentValue(user) + item.Damage.GetCurrentValue(user);
            var minableBlockTypes = Block.BlockTypesWithAttribute(typeof(Minable)).Select(x => new KeyValuePair<Type, float>(x, Block.Get<Minable>(x).Hardness)).ToList();

            if (!minableBlockTypes.Any()) return LocString.Empty;

            //We need to get all blocks, but not the hidden ones. Blocks from "Hidden" category should not be displayed in tooltips.
            var allBlocks = Item.AllItemsExceptHidden.OfType<BlockItem>();

            var resList = new List<LocString>();
            minableBlockTypes.OrderBy(item => item.Value).ForEach(x =>
            {
                var targetItem = allBlocks.FirstOrDefault(item => item.OriginType == x.Key);
                var hitCount = (int)Math.Ceiling(x.Value / myHardness);
                if (targetItem != null) resList.Add(Localizer.NotLocalized($"{targetItem.UILink()}: {Localizer.Plural("hit", hitCount)}"));
            });

            return new TooltipSection(Localizer.DoStr("Can mine"), resList.FoldoutListLoc("item", origin));
        }

        [NewTooltip(CacheAs.SubType, 200, overrideType: typeof(ToolItem))]
        public static LocString SubtypesTooltip(Type toolType, TooltipOrigin origin)
        {
            IEnumerable<ToolItem> concreteItems = Item.AllItemsExceptHidden.OfType<ToolItem>().Where(item => item.CanItemExistInInventories() && toolType.IsAssignableFrom(item.GetType()));

            // If there are any concrete items besides the item we're viewing the tooltip for, then show this list
            if (concreteItems.Any(item => item.GetType() != toolType))
                return new TooltipSection(Localizer.DoStr("Variants"), concreteItems.OrderByDescending(item => item.OriginalMaxDurability).Select(item => item.UILink()).FoldoutListLoc("item", origin));
            else
                return LocString.Empty;
        }
    }
}
