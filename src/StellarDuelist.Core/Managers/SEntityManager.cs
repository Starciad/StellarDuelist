using Microsoft.Xna.Framework;

using StellarDuelist.Core.Collections;
using StellarDuelist.Core.Entities;
using StellarDuelist.Core.Entities.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StellarDuelist.Core.Managers
{
    /// <summary>
    /// Static utility for in-game entity management.
    /// </summary>
    public static class SEntityManager
    {
        /// <summary>
        /// Gets an array of all entity headers.
        /// </summary>
        public static SEntityDefinition[] EntityDefinitions => entityDefinitions.Values.ToArray();

        /// <summary>
        /// Gets an array of all active entities.
        /// </summary>
        public static SEntity[] Entities => entities.ToArray();

        // Templates
        private static readonly Dictionary<Type, SEntityDefinition> entityDefinitions = new();

        // Pool
        private static readonly Dictionary<Type, ObjectPool<SEntity>> entityPool = new();
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

                SEntityDefinition definition = registerAttribute.GetEntityDefinition();
                definition.Build(type);

                entityDefinitions.Add(type, definition);
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
                AddEntityToObjectPool(entity);
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
        /// <returns>The created entity instance.</returns>
        public static SEntity Create(Type type, Vector2 position, Vector2 scale, float rotation)
        {
            SEntity entity = GetEntityFromObjectPool(type);

            entity.EntityDefinition = entityDefinitions[type];
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
            AddEntityToObjectPool(entity);
        }

        private static void AddEntityToObjectPool(SEntity entity)
        {
            Type type = entity.GetType();

            if (!entityPool.ContainsKey(type))
            {
                entityPool.Add(type, new());
            }

            entityPool[type].Add(entity);
        }

        private static SEntity GetEntityFromObjectPool(Type entityType)
        {
            if (!entityPool.ContainsKey(entityType))
            {
                entityPool.Add(entityType, new());
            }

            return entityPool[entityType].Get();
        }
    }
}