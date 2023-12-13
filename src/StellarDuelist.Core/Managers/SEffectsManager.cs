using Microsoft.Xna.Framework;

using StellarDuelist.Core.Collections;
using StellarDuelist.Core.Effects;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarDuelist.Core.Managers
{
    /// <summary>
    /// Static utility for managing in-game effects.
    /// </summary>
    public static class SEffectsManager
    {
        /// <summary>
        /// Gets an array of all active effects.
        /// </summary>
        public static SEffect[] Effects => effects.ToArray();

        // Templates
        private static readonly Dictionary<Type, SEffectDefinition> templates = new();

        // Pool
        private static readonly ObjectPool<SEffect> effectPool = new();
        private static readonly List<SEffect> effects = new();

        /// <summary>
        /// Initializes the effects manager by loading effect templates.
        /// </summary>
        internal static void Initialize()
        {
            foreach (Type type in SGame.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(SEffectDefinition))))
            {
                SEffectDefinition template = (SEffectDefinition)Activator.CreateInstance(type);
                template.Initialize();

                templates.Add(type, template);
            }
        }

        /// <summary>
        /// Updates all active effects.
        /// </summary>
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

        /// <summary>
        /// Draws all active effects on the screen.
        /// </summary>
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

        /// <summary>
        /// Resets the effects manager by removing all effects.
        /// </summary>
        internal static void Reset()
        {
            foreach (SEffect effect in Effects)
            {
                effectPool.Add(effect);
            }

            effects.Clear();
        }

        /// <summary>
        /// Creates a new effect of type T with default parameters.
        /// </summary>
        /// <typeparam name="T">The type of effect to create.</typeparam>
        /// <returns>The created effect instance.</returns>
        public static SEffect Create<T>() where T : SEffectDefinition
        {
            return Create<T>(Vector2.Zero);
        }

        /// <summary>
        /// Creates a new effect of type T with the specified world position.
        /// </summary>
        /// <typeparam name="T">The type of effect to create.</typeparam>
        /// <param name="worldPosition">The world position of the effect.</param>
        /// <returns>The created effect instance.</returns>
        public static SEffect Create<T>(Vector2 worldPosition) where T : SEffectDefinition
        {
            return Create<T>(worldPosition, Vector2.One);
        }

        /// <summary>
        /// Creates a new effect of type T with the specified worldPosition and scale.
        /// </summary>
        /// <typeparam name="T">The type of effect to create.</typeparam>
        /// <param name="worldPosition">The world position of the effect.</param>
        /// <param name="scale">The scale of the effect.</param>
        /// <returns>The created effect instance.</returns>
        public static SEffect Create<T>(Vector2 worldPosition, Vector2 scale) where T : SEffectDefinition
        {
            return Create<T>(worldPosition, scale, 0f, Color.White);
        }

        /// <summary>
        /// Creates a new effect of type T with the specified world position, scale, and rotation.
        /// </summary>
        /// <typeparam name="T">The type of effect to create.</typeparam>
        /// <param name="worldPosition">The world position of the effect.</param>
        /// <param name="scale">The scale of the effect.</param>
        /// <param name="rotation">The rotation of the effect.</param>
        /// <returns>The created effect instance.</returns>
        public static SEffect Create<T>(Vector2 worldPosition, Vector2 scale, float rotation) where T : SEffectDefinition
        {
            return Create(typeof(T), worldPosition, scale, rotation, Color.White);
        }

        /// <summary>
        /// Creates a new effect of type T with the specified world position, scale, rotation, and color.
        /// </summary>
        /// <typeparam name="T">The type of effect to create.</typeparam>
        /// <param name="worldPosition">The world position of the effect.</param>
        /// <param name="scale">The scale of the effect.</param>
        /// <param name="rotation">The rotation of the effect.</param>
        /// <param name="color">The color of the effect.</param>
        /// <returns>The created effect instance.</returns>
        public static SEffect Create<T>(Vector2 worldPosition, Vector2 scale, float rotation, Color color) where T : SEffectDefinition
        {
            return Create(typeof(T), worldPosition, scale, rotation, color);
        }

        /// <summary>
        /// Creates a new effect of the specified type with default parameters.
        /// </summary>
        /// <param name="type">The type of effect to create.</param>
        /// <returns>The created effect instance.</returns>
        public static SEffect Create(Type type)
        {
            return Create(type, Vector2.Zero);
        }

        /// <summary>
        /// Creates a new effect of the specified type with the specified world position.
        /// </summary>
        /// <param name="type">The type of effect to create.</param>
        /// <param name="worldPosition">The world position of the effect.</param>
        /// <returns>The created effect instance.</returns>
        public static SEffect Create(Type type, Vector2 worldPosition)
        {
            return Create(type, worldPosition, Vector2.One);
        }

        /// <summary>
        /// Creates a new effect of the specified type with the specified world position and scale.
        /// </summary>
        /// <param name="type">The type of effect to create.</param>
        /// <param name="worldPosition">The world position of the effect.</param>
        /// <param name="scale">The scale of the effect.</param>
        /// <returns>The created effect instance.</returns>
        public static SEffect Create(Type type, Vector2 worldPosition, Vector2 scale)
        {
            return Create(type, worldPosition, scale, 0f);
        }

        /// <summary>
        /// Creates a new effect of the specified type with the specified world position, scale, and rotation.
        /// </summary>
        /// <param name="type">The type of effect to create.</param>
        /// <param name="worldPosition">The world position of the effect.</param>
        /// <param name="scale">The scale of the effect.</param>
        /// <param name="rotation">The rotation of the effect.</param>
        /// <returns>The created effect instance.</returns>
        public static SEffect Create(Type type, Vector2 worldPosition, Vector2 scale, float rotation)
        {
            return Create(type, worldPosition, scale, rotation, Color.White);
        }

        /// <summary>
        /// Creates a new effect of the specified type with the specified world position, scale, rotation, and color.
        /// </summary>
        /// <param name="type">The type of effect to create.</param>
        /// <param name="worldPosition">The world position of the effect.</param>
        /// <param name="scale">The scale of the effect.</param>
        /// <param name="rotation">The rotation of the effect.</param>
        /// <param name="color">The color of the effect.</param>
        /// <returns>The created effect instance.</returns>
        public static SEffect Create(Type type, Vector2 worldPosition, Vector2 scale, float rotation, Color color)
        {
            SEffect effect = effectPool.Get();

            if (effects.Contains(effect))
            {
                effect = Activator.CreateInstance<SEffect>();
                effect.Reset();
            }

            effect.Build(templates[type].Animation);
            effect.Position = worldPosition;
            effect.Scale = scale;
            effect.Rotation = rotation;
            effect.Color = color;

            effects.Add(effect);
            return effect;
        }

        /// <summary>
        /// Removes an effect from the manager.
        /// </summary>
        /// <param name="effect">The effect to remove.</param>
        internal static void Remove(SEffect effect)
        {
            _ = effects.Remove(effect);
            effectPool.Add(effect);
        }
    }
}
