// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using Eco.Shared.Math;
// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

using Eco.WorldGenerator;
using Eco.World;
using Eco.World.Blocks;
using Eco.Shared.Utils;
using Eco.Gameplay;

using Vector3 = System.Numerics.Vector3;

public class DolphinShrine : IWorldGenFeature
{
    private const int NumberOfShrines = 1;
    private const int ShrineSize = 12;

    public void Generate(Random seed, Vector3 voxelSize, WorldSettings settings)
    {
        for (int i = 0; i < NumberOfShrines; ++i)
        {
            var location = (WorldPosition3i)WrappedWorldPosition3i.Clamp((WrappedPosition3i)World.GetRandomLandPos() + (Vector3i.Down * (ShrineSize * 2)));

            location.SpiralOutXZIter(ShrineSize).ForEach(x =>
            {
                var height = Math.Min(ShrineSize * 0.5f, (ShrineSize * 0.6f) - WorldPosition3i.Distance(x, location));
                for (int j = 0; j < height; ++j)
                {
                    if(!World.GetBlock((Vector3i)x + (Vector3i.Up * j)).Is<Impenetrable>())
                        World.DeleteBlock((Vector3i)x + (Vector3i.Up * j));
                    if ((int)WorldPosition3i.Distance(x, location) != 0 && !World.GetBlock((Vector3i)x + (Vector3i.Down * j)).Is<Impenetrable>())
                        World.SetBlock<WaterBlock>((Vector3i)x + (Vector3i.Down * j));
                }
            });
            WorldObjectDebugUtil.Spawn("EckoStatueObject", null, (Vector3i)location);
        }
    }
}
