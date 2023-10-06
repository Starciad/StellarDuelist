using Microsoft.Xna.Framework;

using StardustDefender.Core.Collections;
using StardustDefender.Core.Extensions;
using StardustDefender.Core.Items;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Core.Managers
{
    public static class SItemsManager
    {
        internal static SItem[] Items => items.ToArray();

        // Templates
        private static readonly Dictionary<Type, SItemRegister> templates = new();

        // Pool
        private static readonly ObjectPool<SItem> itemPool = new();
        private static readonly List<SItem> items = new();

        internal static void Initialize()
        {
            foreach (Type itemTemplateType in SGame.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(SItemRegister))))
            {
                SItemRegister template = (SItemRegister)Activator.CreateInstance(itemTemplateType);
                template.Initialize();

                templates.Add(itemTemplateType, template);
            }
        }
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
        internal static void Reset()
        {
            foreach (SItem item in Items)
            {
                itemPool.ReturnToPool(item);
            }

            items.Clear();
        }

        public static SItem CreateRandomItem(Vector2 position)
        {
            return Create(templates.Keys.SelectRandom(), position);
        }

        internal static SItem Create<T>(Vector2 position) where T : SItemRegister
        {
            return Create(typeof(T), position);
        }
        internal static SItem Create(Type type, Vector2 position)
        {
            SItem item = itemPool.Get(type) ?? new();
            SItemRegister template = templates[type];

            item.Build(template, template.Animation, position);

            items.Add(item);
            return item;
        }

        internal static void Remove(SItem item)
        {
            _ = items.Remove(item);
        }
    }
}
