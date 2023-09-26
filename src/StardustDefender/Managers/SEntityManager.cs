using Microsoft.Xna.Framework;

using StardustDefender.Collections;
using StardustDefender.Effects;
using StardustDefender.Entities;
using StardustDefender.World;

using System;
using System.Collections.Generic;

namespace StardustDefender.Managers
{
    internal static class SEntityManager
    {
        public static SEntity[] Entities => entities.ToArray();

        private static readonly ObjectPool<SEntity> entityPool = new();
        private static readonly List<SEntity> entities = new();

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

        internal static T Create<T>() where T : SEntity
        {
            return Create<T>(Vector2.Zero);
        }
        internal static T Create<T>(Vector2 position) where T : SEntity
        {
            return Create<T>(position, Vector2.One);
        }
        internal static T Create<T>(Vector2 position, Vector2 scale) where T : SEntity
        {
            return Create<T>(position, scale, 0f);
        }
        internal static T Create<T>(Vector2 position, Vector2 scale, float rotation) where T : SEntity
        {
            return (T)Create(typeof(T), position, scale, rotation);
        }

        internal static SEntity Create(Type type)
        {
            return Create(type, Vector2.Zero);
        }
        internal static SEntity Create(Type type, Vector2 position)
        {
            return Create(type, position, Vector2.One);
        }
        internal static SEntity Create(Type type, Vector2 position, Vector2 scale)
        {
            return Create(type, position, scale, 0f);
        }
        internal static SEntity Create(Type type, Vector2 position, Vector2 scale, float rotation)
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
