using Microsoft.Xna.Framework;

using StardustDefender.Core.Entities;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.Extensions;
using StardustDefender.Core.Managers;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Controllers
{
    public static partial class SDifficultyController
    {
        private static readonly List<SEntityHeader> enemies = new();

        internal static SEntity CreateRandomEnemy(Vector2 position)
        {
            return SEntityManager.Create(GetRandomEnemyType(), position);
        }

        private static Type GetRandomEnemyType()
        {
            return enemies.Where(x => x.CanSpawn).SelectRandom().EntityType;
        }
    }
}
