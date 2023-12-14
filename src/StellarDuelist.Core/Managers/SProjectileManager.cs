using StellarDuelist.Core.Collections.Generic;
using StellarDuelist.Core.Projectiles;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarDuelist.Core.Managers
{
    /// <summary>
    /// Static utility for managing in-game projectiles.
    /// </summary>
    public static class SProjectileManager
    {
        /// <summary>
        /// Gets an array of all active projectiles.
        /// </summary>
        internal static SProjectile[] ActiveProjectiles => activeProjectiles.ToArray();

        private static readonly ObjectPool<SProjectile> projectilePool = new();
        private static readonly List<SProjectile> activeProjectiles = new();

        /// <summary>
        /// Updates all active projectiles.
        /// </summary>
        internal static void Update()
        {
            foreach (SProjectile projectile in ActiveProjectiles)
            {
                if (projectile == null)
                {
                    continue;
                }

                projectile.Update();
            }
        }

        /// <summary>
        /// Draws all active projectiles on the screen.
        /// </summary>
        internal static void Draw()
        {
            foreach (SProjectile projectile in ActiveProjectiles)
            {
                if (projectile == null)
                {
                    continue;
                }

                projectile.Draw();
            }
        }

        /// <summary>
        /// Resets the projectile manager by removing all projectiles.
        /// </summary>
        internal static void Reset()
        {
            foreach (SProjectile projectile in ActiveProjectiles)
            {
                projectilePool.Add(projectile);
            }

            activeProjectiles.Clear();
        }

        /// <summary>
        /// Creates a projectile using the provided builder.
        /// </summary>
        /// <param name="builder">The builder used to create the projectile.</param>
        public static void Create(SProjectileBuilder builder)
        {
            SProjectile projectile;

            do
            {
                projectile = projectilePool.Get();
            } while (activeProjectiles.Contains(projectile));

            projectile.Build(builder);
            projectile.Initialize();

            activeProjectiles.Add(projectile);
        }

        /// <summary>
        /// Removes a projectile from the manager.
        /// </summary>
        /// <param name="projectile">The projectile to remove.</param>
        internal static void Remove(SProjectile projectile)
        {
            _ = activeProjectiles.Remove(projectile);
            projectilePool.Add(projectile);
        }
    }
}
