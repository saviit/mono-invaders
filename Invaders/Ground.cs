using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Invaders
{
    public class Ground
    {
        public Texture2D Texture;
        public Rectangle Bounds;
        public BoundingBox BoundingBox;

        public Ground(Texture2D tex, Rectangle bounds)
        {
            this.Texture = tex;
            this.Bounds = bounds;
            this.BoundingBox = new BoundingBox(new Vector3(this.Bounds.X, this.Bounds.Y, 0),
                                               new Vector3(this.Bounds.Width, this.Bounds.Height, 0));
        }
    }
}