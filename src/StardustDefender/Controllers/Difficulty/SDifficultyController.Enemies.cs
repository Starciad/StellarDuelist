using StardustDefender.Entities.Aliens;
using StardustDefender.Entities;
using StardustDefender.Managers;
using StardustDefender.Extensions;

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace StardustDefender.Controllers
{
    internal static partial class SDifficultyController
    {
        // <Type, (difficultyRate)>
        private static readonly Dictionary<Type, float> enemies = new()
        {
            [typeof(SAlien_01)] = 0f,
            [typeof(SAlien_02)] = 3f,
        };

        internal static SEntity CreateRandomEnemy(Vector2 position)
        {
            return SEntityManager.Create(GetRandomEnemyType(), position);
        }

        private static Type GetRandomEnemyType()
        {
            return enemies.Where(x => x.Value <= difficultyRate).SelectRandom().Key;
        }
    }
}
