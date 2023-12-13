using Microsoft.Xna.Framework;

using StellarDuelist.Core.Collections;
using StellarDuelist.Core.Extensions;
using StellarDuelist.Core.Items;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarDuelist.Core.Managers
{
    /// <summary>
    /// Static utility for managing in-game items.
    /// </summary>
    public static class SItemsManager
    {
        /// <summary>
        /// Gets an array of all active items.
        /// </summary>
        public static SItem[] Items => items.ToArray();

        // Templates
        private static readonly Dictionary<Type, SItemDefinition> templates = new();

        // Pool
        private static readonly ObjectPool<SItem> itemPool = new();
        private static readonly List<SItem> items = new();

        /// <summary>
        /// Initializes the item manager by loading item templates.
        /// </summary>
        internal static void Initialize()
        {
            foreach (Type itemTemplateType in SGame.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(SItemDefinition))))
            {
                SItemDefinition template = (SItemDefinition)Activator.CreateInstance(itemTemplateType);
                template.Initialize();

                templates.Add(itemTemplateType, template);
            }
        }

        /// <summary>
        /// Updates all active items.
        /// </summary>
        internal static void Update()
        {
            foreach (SItem item in Items)
            {
                if (item == null)
                {
                    continue;
                }

                item.Update();
            }
        }

        /// <summary>
        /// Draws all active items on the screen.
        /// </summary>
        internal static void Draw()
        {
            foreach (SItem item in Items)
            {
                if (item == null)
                {
                    continue;
                }

                item.Draw();
            }
        }

        /// <summary>
        /// Resets the item manager by removing all items.
        /// </summary>
        internal static void Reset()
        {
            foreach (SItem item in Items)
            {
                itemPool.Add(item);
            }

            items.Clear();
        }

        /// <summary>
        /// Creates a random item at the specified position from available templates.
        /// </summary>
        /// <param name="position">The position where the item will be created.</param>
        /// <returns>The created item instance.</returns>
        public static SItem CreateRandomItem(Vector2 position)
        {
            return Create(templates.Where(x => x.Value.CanSpawn).Select(x => x.Key).SelectRandom(), position);
        }

        /// <summary>
        /// Creates an item of type T at the specified position.
        /// </summary>
        /// <typeparam name="T">The type of item to create.</typeparam>
        /// <param name="position">The position where the item will be created.</param>
        /// <returns>The created item instance.</returns>
        internal static SItem Create<T>(Vector2 position) where T : SItemDefinition
        {
            return Create(typeof(T), position);
        }

        /// <summary>
        /// Creates an item of the specified type at the specified position.
        /// </summary>
        /// <param name="type">The type of item to create.</param>
        /// <param name="position">The position where the item will be created.</param>
        /// <returns>The created item instance.</returns>
        internal static SItem Create(Type type, Vector2 position)
        {
            SItem item = itemPool.Get();
            SItemDefinition template = templates[type];

            item.Build(template, template.Animation, position);
            items.Add(item);
            return item;
        }

        /// <summary>
        /// Removes an item from the manager.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        internal static void Remove(SItem item)
        {
            _ = items.Remove(item);
            itemPool.Add(item);
        }
    }
}
