using System;

namespace StardustDefender.Objects
{
    internal abstract class SGameObject
    {
        public string Id => id;
        private readonly string id;

        public SGameObject()
        {
            id = Guid.NewGuid().ToString();
        }

        internal virtual void Awake() { return; }
        internal virtual void Start() { return; }
        internal virtual void Update() { return; }
        internal virtual void Draw() { return; }
        internal virtual void Destroy() { return; }
    }
}
