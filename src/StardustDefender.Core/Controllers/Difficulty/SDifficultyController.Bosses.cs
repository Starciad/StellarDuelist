using Microsoft.Xna.Framework;

using StardustDefender.Core.Components;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Extensions;
using StardustDefender.Core.Managers;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Core.Controllers
{
    public static partial class SDifficultyController
    {
        private static float delayForNextBoss = 3;
        private static float currentDelayForNextBoss = 0;

        private static SEntityHeader[] allBosses = Array.Empty<SEntityHeader>();
        private static readonly List<SEntityHeader> remainingBosses = new();

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
            if (remainingBosses.Count == 0)
            {
                remainingBosses.AddRange(allBosses);
            }

            SEntityHeader entityHeader = remainingBosses.Where(x => x.CanSpawn).SelectRandom() ?? default;

            if (entityHeader == null)
            {
                bossType = default;
                return false;
            }

            _ = remainingBosses.Remove(entityHeader);
            bossType = entityHeader.EntityType;
            return true;
        }
        internal static SBossEntity CreateBossOfType(Type bossType, Vector2 position)
        {
            delayForNextBoss = SRandom.Range(3, 7);
            currentDelayForNextBoss = delayForNextBoss;

            return (SBossEntity)SEntityManager.Create(bossType, position);
        }
    }
}
