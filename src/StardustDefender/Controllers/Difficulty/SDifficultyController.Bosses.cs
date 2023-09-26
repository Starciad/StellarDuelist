using StardustDefender.Entities.Bosses;
using StardustDefender.Entities.Player;
using StardustDefender.Managers;
using StardustDefender.Extensions;

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using StardustDefender.Engine;

namespace StardustDefender.Controllers
{
    internal static partial class SDifficultyController
    {
        private static float delayForNextBoss = 3;
        private static float currentDelayForNextBoss = 0;

        private static readonly Dictionary<Type, Func<SPlayerEntity, bool>> bosses = new()
        {
            [typeof(SBoss_01)] = new Func<SPlayerEntity, bool>(Boss01_Checker),
        };

        internal static bool TryGetRandomBossType(out Type bossType)
        {
            // === DEBUG (FORCE A BOSS TO APPEAR) ===
            // bossType = typeof(SBoss_01);
            // return true;

            // === APPLY DELAY ===
            if (currentDelayForNextBoss > 0)
            {
                currentDelayForNextBoss--;
                bossType = default;
                return false;
            }

            // === GAME (SELECT A BOSS BASED ON VARIOUS CONDITIONS) ===
            bossType = bosses.Where(x => x.Value.Invoke(SLevelController.Player)).SelectRandom().Key;
            return bossType != null;
        }
        internal static SBossEntity CreateBossOfType(Type bossType, Vector2 position)
        {
            delayForNextBoss = SRandom.Range(3, 7);
            currentDelayForNextBoss = delayForNextBoss;

            return (SBossEntity)SEntityManager.Create(bossType, position);
        }

        #region BOSS CHECKERS
        // All methods check whether it is possible to spawn the
        // boss in a certain phase to face the player.

        private static bool Boss01_Checker(SPlayerEntity player)
        {
            return (difficultyRate >= 2.5f && SLevelController.Level >= 5) &&
                   (player.BulletSpeed >= 3.6f && player.BulletLifeTime >= 3.6f);
        }
        #endregion
    }
}
