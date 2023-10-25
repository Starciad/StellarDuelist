using Microsoft.Xna.Framework;

using StardustDefender.Core.Collections;
using StardustDefender.Core.Entities;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.World;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StardustDefender.Core.Managers
{
    /// <summary>
    /// Static utility for in-game entity management.
    /// </summary>
    public static class SEntityManager
    {
        /// <summary>
        /// Gets an array of all entity headers.
        /// </summary>
        public static SEntityHeader[] EntityHeaders => entityHeaders.Values.ToArray();

        /// <summary>
        /// Gets an array of all active entities.
        /// </summary>
        public static SEntity[] Entities => entities.ToArray();

        // Templates
        private static readonly Dictionary<Type, SEntityHeader> entityHeaders = new();

        // Pool
        private static readonly ObjectPool<SEntity> entityPool = new();
        private static readonly List<SEntity> entities = new();

        /// <summary>
        /// Initializes the entity manager by loading entity templates.
        /// </summary>
        internal static void Initialize()
        {
            foreach (Type type in SGame.Assembly.GetTypes())
            {
                SEntityRegisterAttribute registerAttribute = type.GetCustomAttribute<SEntityRegisterAttribute>();
                if (registerAttribute == null)
                {
                    continue;
                }

                SEntityHeader header = registerAttribute.CreateHeader();
                header.Build(type);

                entityHeaders.Add(type, header);
            }
        }

        /// <summary>
        /// Updates all active entities.
        /// </summary>
        internal static void Update()
        {
            foreach (SEntity entity in Entities)
            {
                if (entity == null)
                {
                    continue;
                }

                entity.Update();
            }
        }

        /// <summary>
        /// Draws all active entities on the screen.
        /// </summary>
        internal static void Draw()
        {
            foreach (SEntity entity in Entities)
            {
                if (entity == null)
                {
                    continue;
                }

                entity.Draw();
            }
        }

        /// <summary>
        /// Resets the entity manager by removing all entities.
        /// </summary>
        internal static void Reset()
        {
            foreach (SEntity entity in Entities)
            {
                entityPool.Add(entity);
            }

            entities.Clear();
        }

        /// <summary>
        /// Creates a new entity of type T with default parameters.
        /// </summary>
        /// <typeparam name="T">The type of entity to create.</typeparam>
        /// <returns>The created entity instance.</returns>
        public static T Create<T>() where T : SEntity
        {
            return Create<T>(Vector2.Zero);
        }

        /// <summary>
        /// Creates a new entity of type T with the specified position.
        /// </summary>
        /// <typeparam name="T">The type of entity to create.</typeparam>
        /// <param name="position">The position of the entity.</param>
        /// <returns>The created entity instance.</returns>
        public static T Create<T>(Vector2 position) where T : SEntity
        {
            return Create<T>(position, Vector2.One);
        }

        /// <summary>
        /// Creates a new entity of type T with the specified position and scale.
        /// </summary>
        /// <typeparam name="T">The type of entity to create.</typeparam>
        /// <param name="position">The position of the entity.</param>
        /// <param name="scale">The scale of the entity.</param>
        /// <returns>The created entity instance.</returns>
        public static T Create<T>(Vector2 position, Vector2 scale) where T : SEntity
        {
            return Create<T>(position, scale, 0f);
        }

        /// <summary>
        /// Creates a new entity of type T with the specified position, scale, and rotation.
        /// </summary>
        /// <typeparam name="T">The type of entity to create.</typeparam>
        /// <param name="position">The position of the entity.</param>
        /// <param name="scale">The scale of the entity.</param>
        /// <param name="rotation">The rotation of the entity.</param>
        /// <returns>The created entity instance.</returns>
        public static T Create<T>(Vector2 position, Vector2 scale, float rotation) where T : SEntity
        {
            return (T)Create(typeof(T), position, scale, rotation);
        }

        /// <summary>
        /// Creates a new entity of the specified type with default parameters.
        /// </summary>
        /// <param name="type">The type of entity to create.</param>
        /// <returns>The created entity instance.</returns>
        public static SEntity Create(Type type)
        {
            return Create(type, Vector2.Zero);
        }

        /// <summary>
        /// Creates a new entity of the specified type with the specified position.
        /// </summary>
        /// <param name="type">The type of entity to create.</param>
        /// <param name="position">The position of the entity.</param>
        /// <returns>The created entity instance.</returns>
        public static SEntity Create(Type type, Vector2 position)
        {
            return Create(type, position, Vector2.One);
        }

        /// <summary>
        /// Creates a new entity of the specified type with the specified position and scale.
        /// </summary>
        /// <param name="type">The type of entity to create.</param>
        /// <param name="position">The position of the entity.</param>
        /// <param name "scale">The scale of the entity.</param>
        /// <returns>The created entity instance.</returns>
        public static SEntity Create(Type type, Vector2 position, Vector2 scale)
        {
            return Create(type, position, scale, 0f);
        }

        /// <summary>
        /// Creates a new entity of the specified type with the specified position, scale, rotation, and color.
        /// </summary>
        /// <param name="type">The type of entity to create.</param>
        /// <param name="position">The position of the entity.</param>
        /// <param name "scale">The scale of the entity.</param>
        /// <param name "rotation">The rotation of the entity.</param>
        /// <param name "color">The color of the entity.</param>
        /// <returns>The created entity instance.</returns>
        public static SEntity Create(Type type, Vector2 position, Vector2 scale, float rotation)
        {
            SEntity entity = entityPool.Get(type);

            entity.LocalPosition = position;
            entity.Scale = scale;
            entity.Rotation = rotation;

            entity.Initialize();
            entities.Add(entity);
            return entity;
        }

        /// <summary>
        /// Removes an entity from the manager.
        /// </summary>
        internal static void Remove(SEntity entity)
        {
            _ = entities.Remove(entity);
            entityPool.Add(entity);
        }
    }
}
