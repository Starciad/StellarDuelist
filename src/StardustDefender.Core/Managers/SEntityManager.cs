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
    public static class SEntityManager
    {
        public static SEntityHeader[] EntityHeaders => entityHeaders.Values.ToArray();
        public static SEntity[] Entities => entities.ToArray();

        // Templates
        private static readonly Dictionary<Type, SEntityHeader> entityHeaders = new();

        // Pool
        private static readonly ObjectPool<SEntity> entityPool = new();
        private static readonly List<SEntity> entities = new();

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
        internal static void Reset()
        {
            foreach (SEntity entity in Entities)
            {
                entityPool.ReturnToPool(entity);
            }

            entities.Clear();
        }

        public static T Create<T>() where T : SEntity
        {
            return Create<T>(Vector2.Zero);
        }
        public static T Create<T>(Vector2 position) where T : SEntity
        {
            return Create<T>(position, Vector2.One);
        }
        public static T Create<T>(Vector2 position, Vector2 scale) where T : SEntity
        {
            return Create<T>(position, scale, 0f);
        }
        public static T Create<T>(Vector2 position, Vector2 scale, float rotation) where T : SEntity
        {
            return (T)Create(typeof(T), position, scale, rotation);
        }

        public static SEntity Create(Type type)
        {
            return Create(type, Vector2.Zero);
        }
        public static SEntity Create(Type type, Vector2 position)
        {
            return Create(type, position, Vector2.One);
        }
        public static SEntity Create(Type type, Vector2 position, Vector2 scale)
        {
            return Create(type, position, scale, 0f);
        }
        public static SEntity Create(Type type, Vector2 position, Vector2 scale, float rotation)
        {
            SEntity entity = entityPool.Get(type) ?? (SEntity)Activator.CreateInstance(type);

            entity.LocalPosition = position;
            entity.WorldPosition = SWorld.GetWorldPosition(position);
            entity.Scale = scale;
            entity.Rotation = rotation;

            entity.Initialize();
            entities.Add(entity);
            return entity;
        }

        internal static void Remove(SEntity entity)
        {
            _ = entities.Remove(entity);
            entityPool.ReturnToPool(entity);
        }
    }
}
