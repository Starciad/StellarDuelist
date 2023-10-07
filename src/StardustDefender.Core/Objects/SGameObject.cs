﻿using System;

namespace StardustDefender.Core.Objects
{
    internal abstract class SGameObject
    {
        public string Id => this.id;
        private readonly string id;

        public SGameObject()
        {
            this.id = Guid.NewGuid().ToString();
        }

        internal virtual void Awake() { return; }
        internal virtual void Start() { return; }
        internal virtual void Update() { return; }
        internal virtual void Draw() { return; }
        internal virtual void Destroy() { return; }
    }
}