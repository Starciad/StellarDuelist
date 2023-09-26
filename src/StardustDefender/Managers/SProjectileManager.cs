using StardustDefender.Collections;
using StardustDefender.Projectiles;

using System.Collections.Generic;

namespace StardustDefender.Managers
{
    internal static class SProjectileManager
    {
        public static SProjectile[] Projectiles => projectiles.ToArray();

        private static readonly ObjectPool<SProjectile> projectilePool = new();
        private static readonly List<SProjectile> projectiles = new();

        internal static void Update()
        {
            foreach (SProjectile projectile in Projectiles)
            {
                if (projectile == null)
                {
                    continue;
                }

                projectile?.Update();
            }
        }
        internal static void Draw()
        {
            foreach (SProjectile projectile in Projectiles)
            {
                if (projectile == null)
                {
                    continue;
                }

                projectile?.Draw();
            }
        }
        internal static void Reset()
        {
            foreach (SProjectile projectile in Projectiles)
            {
                projectilePool.ReturnToPool(projectile);
            }

            projectiles.Clear();
        }

        internal static void Create(SProjectileBuilder builder)
        {
            SProjectile projectile = projectilePool.Get<SProjectile>();

            if (projectile == null)
            {
                projectile = new();
                projectile.Initialize();
            }

            projectile.Build(builder);
            projectiles.Add(projectile);
        }
        internal static void Remove(SProjectile projectile)
        {
            _ = projectiles.Remove(projectile);
            projectilePool.ReturnToPool(projectile);
        }
    }
}
