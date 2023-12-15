using Microsoft.Xna.Framework;

namespace StellarDuelist.Core.Collision
{
    public sealed class SCollision
    {
        public static SCollision Empty => new(Point.Zero, Point.Zero);

        public Rectangle Rectangle => this._collisionRectangle;
        public Point Position => this._collisionRectangle.Location;
        public Point Size => this._collisionRectangle.Size;

        private Rectangle _collisionRectangle = Rectangle.Empty;

        public SCollision()
        {
            this._collisionRectangle = Rectangle.Empty;
        }

        public SCollision(Point position, Point size)
        {
            Update(position, size);
        }

        public void Update(Point position, Point size)
        {
            SetPosition(position);
            SetSize(size);
        }

        public void SetPosition(Point position)
        {
            this._collisionRectangle.Location = new(position.X - (this.Size.X / 2), position.Y - (this.Size.Y / 2));
        }

        public void SetSize(Point size)
        {
            this._collisionRectangle.Size = size;
        }

        public bool IsColliding(SCollision collision)
        {
            return this._collisionRectangle.Intersects(collision._collisionRectangle);
        }
    }
}
