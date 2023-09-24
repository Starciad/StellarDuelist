using StardustDefender.Entities.Bosses;
using StardustDefender.Entities.Player;
using StardustDefender.Entities;
using StardustDefender.Managers;
using StardustDefender.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace StardustDefender.Controllers
{
    internal static partial class SDifficultyController
    {
        // <Type, (difficultyRate, levelRequired, spawnCheckAction)>
        private static readonly Dictionary<Type, (float, int, Func<SPlayerEntity, bool>)> bosses = new()
        {
            [typeof(SBoss_01)] = (5f, 2, new Func<SPlayerEntity, bool>(Boss01_Checker)),
        };

        internal static bool TryCreateRandomBoss(Vector2 position, out SBossEntity value)
        {
            Type targetType = GetRandomBossType();

            if (targetType == null)
            {
                value = default;
                return false;
            }

            value = (SBossEntity)SEntityManager.Create(targetType, position);
            return true;
        }
        private static Type GetRandomBossType()
        {
            return typeof(SBoss_01);

            /*
            return bosses.Where(x => x.Value.Item1 <= difficultyRate)
                         .Where(x => x.Value.Item2 >= SLevelController.Level)
                         .Where(x => x.Value.Item3.Invoke(SLevelController.Player))
                         .SelectRandom().Key;
            */
        }

        #region BOSS CHECKERS
        // All methods check whether it is possible to spawn the
        // boss in a certain phase to face the player.

        private static bool Boss01_Checker(SPlayerEntity player)
        {
            return true;
        }
        #endregion
    }
}
