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

    public partial class TownBellItem : WorldObjectItem<TownBellObject>
    {
        protected override OccupancyContext GetOccupancyContext => new PositionsRequirementContext(
            new List<PositionsRequirement>{
				//Part of the TownBell requires ground below it to be solid.
                //..these are not really parts of its occupancy, since Y = -1, more like requirements.
				new PositionsRequirement(
                    positions : new List<Vector3i>{
                    new Vector3i(0, -1, 0),
                    new Vector3i(0, -1, 1),
                    new Vector3i(0, -1, 2),
                    new Vector3i(1, -1, 0),
                    new Vector3i(3, -1, 0),
                    new Vector3i(3, -1, 1),
                    new Vector3i(3, -1, 2),
                    new Vector3i(2, -1, 0),
                    new Vector3i(0, -1, 3),
                    new Vector3i(1, -1, 3),
                    new Vector3i(2, -1, 3),
                    new Vector3i(3, -1, 3),
                },
                requirement:      PositionRequirementType.OnSolidGround,
                partName:         Localizer.DoStr("base"),
                placementMessage: Localizer.DoStr("on solid ground")
                ),
				//The moving part needs to have empty ground below them.
               //..these are not parts of the occupancy either, just requirements.
				new PositionsRequirement(
                    positions: new List<Vector3i>{
                    new Vector3i(1, -1, 1),
                    new Vector3i(1, -1, 2),
                    new Vector3i(2, -1, 1),
                    new Vector3i(2, -1, 2),
                },
                requirement:      PositionRequirementType.OnEmptySpace,
                partName:         Localizer.DoStr("center"),
                placementMessage: Localizer.DoStr("over empty space")
                ),
            });
	}
}
