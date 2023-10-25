using Microsoft.Xna.Framework;

using StardustDefender.Core.Collections;
using StardustDefender.Core.Effects;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Core.Managers
{
    public static class SEffectsManager
    {
        public static SEffect[] Effects => effects.ToArray();

        // Templates
        private static readonly Dictionary<Type, SEffectRegister> templates = new();

        // Pool
        private static readonly ObjectPool<SEffect> effectPool = new();
        private static readonly List<SEffect> effects = new();

        internal static void Initialize()
        {
            foreach (Type type in SGame.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(SEffectRegister))))
            {
                SEffectRegister template = (SEffectRegister)Activator.CreateInstance(type);
                template.Initialize();

                templates.Add(type, template);
            }
        }
        internal static void Update()
        {
            foreach (SEffect effect in Effects)
            {
                if (effect == null)
                {
                    continue;
                }

                effect.Update();
            }
        }
        internal static void Draw()
        {
            foreach (SEffect effect in Effects)
            {
                if (effect == null)
                {
                    continue;
                }

                effect.Draw();
            }
        }
        internal static void Reset()
        {
            foreach (SEffect effect in Effects)
            {
                effectPool.ReturnToPool(effect);
            }

            effects.Clear();
        }

        public static SEffect Create<T>() where T : SEffectRegister
        {
            return Create<T>(Vector2.Zero);
        }
        public static SEffect Create<T>(Vector2 position) where T : SEffectRegister
        {
            return Create<T>(position, Vector2.One);
        }
        public static SEffect Create<T>(Vector2 position, Vector2 scale) where T : SEffectRegister
        {
            return Create<T>(position, scale, 0f, Color.White);
        }
        public static SEffect Create<T>(Vector2 position, Vector2 scale, float rotation, Color color) where T : SEffectRegister
        {
            return Create(typeof(T), position, scale, rotation, color);
        }

        public static SEffect Create(Type type)
        {
            return Create(type, Vector2.Zero);
        }
        public static SEffect Create(Type type, Vector2 position)
        {
            return Create(type, position, Vector2.One);
        }
        public static SEffect Create(Type type, Vector2 position, Vector2 scale)
        {
            return Create(type, position, scale, 0f, Color.White);
        }
        public static SEffect Create(Type type, Vector2 position, Vector2 scale, float rotation, Color color)
        {
            SEffect effect = effectPool.Get(type) ?? new();

            effect.Build(templates[type].Animation);
            effect.Position = position;
            effect.Scale = scale;
            effect.Rotation = rotation;
            effect.Color = color;

            effects.Add(effect);
            return effect;
        }

        internal static void Remove(SEffect effect)
        {
            _ = effects.Remove(effect);
            effectPool.ReturnToPool(effect);
        }
    }
}
