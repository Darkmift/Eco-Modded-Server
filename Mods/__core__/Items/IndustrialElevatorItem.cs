// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Occupancy;
    using Eco.Shared.Math;
    using System.Collections.Generic;
    using Eco.Shared.Localization;
    using Eco.Shared.SharedTypes;

    public partial class IndustrialElevatorItem : WorldObjectItem<IndustrialElevatorObject>
    {
        protected override OccupancyContext GetOccupancyContext => new PositionsRequirementContext(
            new List<PositionsRequirement>
            {
                //Part of the Elevator requires ground below it to be solid.
                //..these are not really parts of its occupancy, since Y = -1, more like requirements.
                new PositionsRequirement(
                    positions: new List<Vector3i>
                    {
                        new Vector3i(7, -1, -11),
                        new Vector3i(7, -1, -5),
                        new Vector3i(7, -1, -6),
                        new Vector3i(7, -1, -7),
                        new Vector3i(7, -1, -10),
                        new Vector3i(7, -1, -4),
                        new Vector3i(7, -1, 0),
                        new Vector3i(7, -1, -1),
                        new Vector3i(7, -1, -2),
                        new Vector3i(7, -1, -3),
                        new Vector3i(7, -1, -9),
                        new Vector3i(7, -1, -8),
                        new Vector3i(0, -1, -11),
                        new Vector3i(0, -1, -5),
                        new Vector3i(0, -1, -7),
                        new Vector3i(0, -1, -10),
                        new Vector3i(0, -1, -4),
                        new Vector3i(0, -1, 0),
                        new Vector3i(0, -1, -1),
                        new Vector3i(0, -1, -2),
                        new Vector3i(0, -1, -3),
                        new Vector3i(0, -1, -9),
                        new Vector3i(0, -1, -8),
                        new Vector3i(0, -1, -6),
                        new Vector3i(6, -1, 0),
                        new Vector3i(5, -1, 0),
                        new Vector3i(2, -1, 0),
                        new Vector3i(1, -1, 0),
                        new Vector3i(4, -1, 0),
                        new Vector3i(3, -1, 0),
                        new Vector3i(6, -1, -11),
                        new Vector3i(5, -1, -11),
                        new Vector3i(2, -1, -11),
                        new Vector3i(1, -1, -11),
                        new Vector3i(4, -1, -11),
                        new Vector3i(3, -1, -11),
                    },
                    requirement:      PositionRequirementType.OnSolidGround,
                    partName:         Localizer.DoStr("Base"),
                    placementMessage: Localizer.DoStr("on solid ground")
                ),
                //The moving part needs to have empty ground below it.
                //..these are not parts of the occupancy either, just requirements.
                new PositionsRequirement(
                    positions: new List<Vector3i>
                    {
                        new Vector3i(1, -1, -1),
                        new Vector3i(2, -1, -1),
                        new Vector3i(5, -1, -1),
                        new Vector3i(6, -1, -1),
                        new Vector3i(3, -1, -1),
                        new Vector3i(4, -1, -1),
                        new Vector3i(1, -1, -4),
                        new Vector3i(2, -1, -4),
                        new Vector3i(5, -1, -4),
                        new Vector3i(6, -1, -4),
                        new Vector3i(3, -1, -4),
                        new Vector3i(4, -1, -4),
                        new Vector3i(1, -1, -5),
                        new Vector3i(2, -1, -5),
                        new Vector3i(5, -1, -5),
                        new Vector3i(6, -1, -5),
                        new Vector3i(3, -1, -5),
                        new Vector3i(4, -1, -5),
                        new Vector3i(1, -1, -6),
                        new Vector3i(2, -1, -6),
                        new Vector3i(5, -1, -6),
                        new Vector3i(6, -1, -6),
                        new Vector3i(3, -1, -6),
                        new Vector3i(4, -1, -6),
                        new Vector3i(1, -1, -7),
                        new Vector3i(2, -1, -7),
                        new Vector3i(5, -1, -7),
                        new Vector3i(6, -1, -7),
                        new Vector3i(3, -1, -7),
                        new Vector3i(4, -1, -7),
                        new Vector3i(1, -1, -8),
                        new Vector3i(2, -1, -8),
                        new Vector3i(5, -1, -8),
                        new Vector3i(6, -1, -8),
                        new Vector3i(3, -1, -8),
                        new Vector3i(4, -1, -8),
                        new Vector3i(1, -1, -9),
                        new Vector3i(2, -1, -9),
                        new Vector3i(5, -1, -9),
                        new Vector3i(6, -1, -9),
                        new Vector3i(3, -1, -9),
                        new Vector3i(4, -1, -9),
                        new Vector3i(1, -1, -10),
                        new Vector3i(2, -1, -10),
                        new Vector3i(5, -1, -10),
                        new Vector3i(6, -1, -10),
                        new Vector3i(3, -1, -10),
                        new Vector3i(4, -1, -10),
                        new Vector3i(1, -1, -3),
                        new Vector3i(2, -1, -3),
                        new Vector3i(5, -1, -3),
                        new Vector3i(6, -1, -3),
                        new Vector3i(3, -1, -3),
                        new Vector3i(4, -1, -3),
                        new Vector3i(1, -1, -2),
                        new Vector3i(2, -1, -2),
                        new Vector3i(5, -1, -2),
                        new Vector3i(6, -1, -2),
                        new Vector3i(3, -1, -2),
                        new Vector3i(4, -1, -2),
                    },
                    requirement:      PositionRequirementType.OnEmptySpace,
                    partName:         Localizer.DoStr("Shaft"),
                    placementMessage: Localizer.DoStr("on empty space")
                )
            }
        );
    }
}
