using Microsoft.Xna.Framework;

using StardustDefender.Collections;
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
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i]?.Update();
            }
        }
        internal static void Draw()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i]?.Draw();
            }
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
            SEntity entity = entityPool.Get();

            if (entity == null)
            {
                entity = (SEntity)Activator.CreateInstance(type);
                entity.Initialize();
            }

            entity.LocalPosition = position;
            entity.WorldPosition = SWorld.GetWorldPosition(position);
            entity.Scale = scale;
            entity.Rotation = rotation;

            entities.Add(entity);
            return entity;
        }

        internal static void Remove(SEntity entity)
        {
            entities.Remove(entity);
            entityPool.ReturnToPool(entity);
        }
    }
}
