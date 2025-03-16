// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods
{
    using System.Collections.Generic;
    using System.Linq;
    using Eco.Core.Agents;
    using Eco.Core.Tests;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Systems.Messaging.Chat.Commands;
    using Eco.Shared.IoC;
    using Eco.Shared.Networking;
    using Eco.Shared.Math;
    using Eco.World;

    using Vector3 = System.Numerics.Vector3;
	using Eco.Gameplay.Utils;

	[ChatCommandHandler]
    public static class AvatarCommands
    {
        private static List<WorldObserver> dummyPlayers = new List<WorldObserver>();

        [ChatCommand("Shows commands for avatars manipulation.")]
        public static void Avatar(User user) { }

        [CITest(clientDependent: true)]
        [ChatSubCommand("Avatar", "Spawns a dummy avatar", ChatAuthorizationLevel.DevTier)]
        public static void Dummy(User user, int count = 1)
        {
            count = count > 1 ? count : 1;
            for (int i = 0; i < count; ++i)
            {
                MakeDummy(user.Player);
            }
        }

        [CITest(clientDependent: true)]
        [ChatSubCommand("Avatar", "Spawns passed number of clones of your avatar", ChatAuthorizationLevel.DevTier)]
        public static void MeTime(User user, int count = 1)
        {
            count = count > 1 ? count : 1;
            for (int i = 0; i < count; ++i)
            {
                MakeDummy(user.Player, user);
            }
        }

        [CITest(clientDependent: true)]
        [ChatSubCommand("Avatar", "Kills all spawned dummys", ChatAuthorizationLevel.DevTier)]
        public static void LastPlayerOnEarth()
        {
            while (dummyPlayers.Count > 0)
                KillDummy(dummyPlayers.Last() as Player);
        }

        [CITest(clientDependent: true)]
        [ChatSubCommand("Avatar", "Toggles Third Person Camera", ChatAuthorizationLevel.User)]
        public static void ThirdPerson(User user)
        {
            user.Player.ClientRPC("ToggleThirdPerson");
        }

        [ChatSubCommand("Avatar", "Enables unrestricted avatar customization in game", ChatAuthorizationLevel.DevTier)]
        public static void Customize(User user) => user.Player.ClientRPC("CustomizeAvatarUnrestricted");
        public static Player MakeDummy(Player player, User sourceUser = null)
        {
            var name = "Dummy #" + dummyPlayers.Count;
            var user = TestUtils.MakeTestUser(name);
            user.Position = World.GetRandomLandPosNear(player.User.Position.XYZi()) + new Vector3(0.0f, 0.5f, 0.0f);
            user.MarkDirty();
            var dummy = new Player(user, 30f, null);

            if (sourceUser != null)
            {
                dummy.User.OverrideInventory(sourceUser.Inventory);
                dummy.User.OverrideAvatar(sourceUser.Avatar);
            }

            dummyPlayers.Add(dummy);

            return dummy;
        }

        static void KillDummy(Player dummy)
        {
            ServiceHolder<INetObjectManager>.Obj.Remove(dummy);
            dummy.User.Logout();
            dummyPlayers.Remove(dummy);
        }
    }
}
