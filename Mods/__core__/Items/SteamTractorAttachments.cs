// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System.Linq;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Items;
    using Eco.Shared.Math;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.Gameplay.GameActions;
    using Eco.Core.Items;
    using Eco.Gameplay.Plants;

    [Tag("Plow")]
    public partial class SteamTractorPlowItem : VehicleToolItem
    {
        // Item's area of effect (current position will be alternately shifted by these values to cause a block change).
        private static Vector3i[] area = new Vector3i[] { new Vector3i(0, -1, -2), new Vector3i(1, -1, -2), new Vector3i(-1, -1, -2) };

        public override void ApplyBlockInteraction(WrappedWorldPosition3i pos, Quaternion rot, VehicleComponent vehicle, Inventory inv = null)
        {
            AtomicActions.ChangeBlockNow(newType: typeof(TilledDirtBlock),  // Create a pack, fill it with plow actions and try to perform.
                context: new MultiblockActionContext() {
                    Player                = vehicle.Driver,
                    ToolUsed              = this,
                    GameActionConstructor = () => new PlowField(),
                    ActionDescription     = GameActionDescription.DoStr("plow a block", "plowing a block"),
                    Area                  = area.MoveAndRotate((Vector3i)pos, rot)    // Rotate area of effect and shift it to current position (and exclude repetitions from the result).
                                                .Where(position => World.GetBlock(position).Is<Tillable>()), // Exclude untillable blocks.
                    RepairableItem        = this
                });
        }
    }

    [Tag("Harvester")]
    public partial class SteamTractorHarvesterItem : VehicleToolItem
    {
        private static Vector3i[] area = new Vector3i[] { new Vector3i(0, 0, 3), new Vector3i(1, 0, 3), new Vector3i(-1, 0, 3) };

        public override void ApplyBlockInteraction(WrappedWorldPosition3i pos, Quaternion rot, VehicleComponent vehicle, Inventory inv = null)
        {
            AtomicActions.HarvestPlantNow(  // Create a pack, fill it with harvest actions and try to perform.
                harvestTo:    inv, 
                reapableOnly: false,        // Harvest every plant.
                context:      new MultiblockActionContext() {
                    Player            = vehicle.Driver,
                    ToolUsed          = this,
                    ActionDescription = GameActionDescription.DoStr("harvest a plant", "harvesting a plant"),
                    Area              = area.MoveAndRotate((Vector3i)pos, rot), // Rotate area of effect and shift it to current position (and exclude repetitions from the result).
                    RepairableItem    = this
                });
        }
    }


    [Tag("Planter")]
    public partial class SteamTractorSowerItem : VehicleToolItem
    {
        private static Vector3i[] area = new Vector3i[] { new Vector3i(0, 0, 3), new Vector3i(1, 0, 3), new Vector3i(-1, 0, 3) };

        // TODO: create atomic actions covering the case and utilize them.
        public override void ApplyBlockInteraction(WrappedWorldPosition3i pos, Quaternion rot, VehicleComponent vehicle, Inventory inv = null)
        {
            if (inv == null)
                return;

            var seeder = vehicle.Driver.User;
            foreach (var offset in area)
            {
                // Check if this inventory is capable of selection then check that the selected stack is a seed, if so use that stack to plant, if not then iterate through the non-empty stacks to obtain the first seed we find.
                var stack = (inv is SelectionInventory selectionInv && selectionInv.SelectedStack != null && selectionInv.SelectedStack?.Item is SeedItem) ? selectionInv.SelectedStack : inv.NonEmptyStacks.FirstOrDefault(x => x.Item is SeedItem);

                if (stack == null)
                    return;

                var seed      = (SeedItem)stack.Item;
                var targetPos = (rot.RotateVector(offset) + (Vector3i)pos).XYZi();
                if (World.GetBlock(targetPos + Vector3i.Down).Is<Tilled>() && World.GetBlock(targetPos).Is<Empty>())
                {
                    seed!.TrySeedFromInventory(stack, inv, targetPos, seeder, this);
                    this.UseDurability(1, vehicle.Driver, true);
                }
            }
        }
    }
}
