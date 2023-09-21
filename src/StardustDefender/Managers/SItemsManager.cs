﻿using Microsoft.Xna.Framework;

using SharpDX.Direct3D9;

using StardustDefender.Collections;
using StardustDefender.Effects;
using StardustDefender.Entities.Items;
using StardustDefender.Extensions;
using StardustDefender.Items;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Controllers
{
    internal static class SItemsManager
    {
        public static SItem[] Items => items.ToArray();

        // Templates
        private static readonly Dictionary<Type, SItemTemplate> templates = new();

        // Pool
        private static readonly ObjectPool<SItem> itemPool = new();
        private static readonly List<SItem> items = new();

        internal static void Initialize()
        {
            foreach (Type itemTemplateType in SGame.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(SItemTemplate))))
            {
                SItemTemplate template = (SItemTemplate)Activator.CreateInstance(itemTemplateType);
                template.Initialize();

                templates.Add(itemTemplateType, template);
            }
        }
        internal static void Update()
        {
            foreach (SItem item in Items)
            {
                item?.Update();
            }
        }
        internal static void Draw()
        {
            foreach (SItem item in Items)
            {
                item?.Draw();
            }
        }

        internal static SItem GetRandomItem(Vector2 position)
        {
            return Create(templates.Keys.SelectRandom(), position);
        }

        internal static SItem Create<T>(Vector2 position) where T : SItemTemplate
        {
            return Create(typeof(T), position);
        }
        internal static SItem Create(Type type, Vector2 position)
        {
            SItem item = itemPool.Get() ?? new();
            SItemTemplate template = templates[type];

            item.Build(template, template.Animation, position);
            
            items.Add(item);
            return item;
        }
        internal static void Remove(SItem item)
        {
            items.Remove(item);
        }
    }
}