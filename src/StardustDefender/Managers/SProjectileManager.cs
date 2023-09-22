using StardustDefender.Collections;
using StardustDefender.Projectiles;

using System.Collections.Generic;

namespace StardustDefender.Managers
{
    internal static class SProjectileManager
    {
        private static readonly ObjectPool<SProjectile> projectilePool = new();
        private static readonly List<SProjectile> projectiles = new();

        internal static void Update()
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update();
            }
        }
        internal static void Draw()
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw();
            }
        }

        internal static void Create(SProjectileBuilder builder)
        {
            SProjectile projectile = projectilePool.Get();

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
            projectiles.Remove(projectile);
            projectilePool.ReturnToPool(projectile);
        }
    }
}
