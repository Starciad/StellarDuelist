using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Background.Layers;

using System.Collections.Generic;

namespace StardustDefender.Core.Background
{
    public abstract class SBackground
    {
        private readonly List<SBackgroundLayer> layers = new();
        private Texture2D texture;

        internal void Initialize()
        {
            OnProcess();
        }
        internal void Update()
        {
            this.layers.ForEach(x => x.Update());
        }
        internal void Draw()
        {
            this.layers.ForEach(x => x.Draw());
        }

        protected void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }
        protected void AddLayer(Rectangle textureRectangle, float parallaxFactor)
        {
            this.layers.Add(new(this.texture, textureRectangle, parallaxFactor));
        }

        protected abstract void OnProcess();
    }
}
