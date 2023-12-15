using Microsoft.Xna.Framework;

using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Templates.Dangerous;
using StellarDuelist.Core.Extensions;
using StellarDuelist.Core.Managers;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarDuelist.Core.Controllers
{
    public static partial class SDifficultyController
    {
        private static float delayForNextBoss = 3;
        private static float currentDelayForNextBoss = 0;

        private static SEntityDefinition[] allBosses = Array.Empty<SEntityDefinition>();
        private static readonly List<SEntityDefinition> remainingBosses = new();

        /// <summary>
        /// Attempts to retrieve a random boss type for spawning.
        /// </summary>
        /// <param name="bossType">The retrieved boss type, if successful.</param>
        /// <returns><c>true</c> if a boss type is retrieved; otherwise, <c>false</c>.</returns>
        internal static bool TryGetRandomBossType(out Type bossType)
        {
            // === DEBUG (FORCE A BOSS TO APPEAR) ===
            // bossType = remainingBosses.First().EntityTargetType;
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

            SEntityDefinition definition = remainingBosses.Where(x => x.CanSpawn).SelectRandom() ?? default;

            if (definition == null)
            {
                bossType = default;
                return false;
            }

            _ = remainingBosses.Remove(definition);
            bossType = definition.EntityTargetType;
            return true;
        }

        /// <summary>
        /// Creates a boss entity of the specified type at the given position.
        /// </summary>
        /// <param name="bossType">The type of boss entity to create.</param>
        /// <param name="position">The position at which to create the boss.</param>
        /// <returns>The created boss entity.</returns>
        internal static SBossEntity CreateBossOfType(Type bossType, Vector2 position)
        {
            delayForNextBoss = SRandom.Range(3, 7);
            currentDelayForNextBoss = delayForNextBoss;

            return (SBossEntity)SEntityManager.Create(bossType, position);
        }
    }
}
