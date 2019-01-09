using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Invaders2
{
    public class Bunker
    {
        public Vector2 Position;// { get; set; }
        public Texture2D Texture;// { get; set; }
        public BoundingBox BoundingBox;
        public int Health;

        public Bunker()
        {
            this.Health = 3;
        }

        public Bunker(Texture2D tex, Vector2 pos)
        {
            this.Texture = tex;
            this.Position = pos;
            this.BoundingBox = new BoundingBox(new Vector3(this.Position.X, this.Position.Y + 17, 0),
                                               new Vector3(this.Position.X + this.Texture.Width, this.Position.Y + 31, 0));
            this.Health = 3;
        }
    }
}