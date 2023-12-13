using Microsoft.Xna.Framework;

using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Extensions;
using StellarDuelist.Core.Managers;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarDuelist.Core.Controllers
{
    public static partial class SDifficultyController
    {
        private static readonly List<SEntityDefinition> enemies = new();

        /// <summary>
        /// Creates a random enemy entity at the specified position.
        /// </summary>
        /// <param name="position">The position at which to create the enemy.</param>
        /// <returns>The created enemy entity.</returns>
        internal static SEntity CreateRandomEnemy(Vector2 position)
        {
            return SEntityManager.Create(GetRandomEnemyType(), position);
        }

        /// <summary>
        /// Retrieves a random enemy type for spawning.
        /// </summary>
        /// <returns>The retrieved enemy type.</returns>
        private static Type GetRandomEnemyType()
        {
            return enemies.Where(x => x.CanSpawn).SelectRandom().EntityTargetType;
        }
    }
}
