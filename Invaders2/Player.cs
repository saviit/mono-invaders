using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Invaders2
{

    public class Player
    {
        public Texture2D Texture;
        public Vector2 Position;
        public BoundingBox BoundingBox;
        
        public Player() { }

        public Player(Texture2D tex, Vector2 pos)
        {
            this.Texture = tex;
            this.Position = pos;
            this.BoundingBox = new BoundingBox(new Vector3(this.Position.X, this.Position.Y + 14, 0),
                                               new Vector3(this.Position.X + this.Texture.Width, this.Position.Y + 18, 0));
        }

        public void UpdateBoundingBox()
        {
            this.BoundingBox.Min.X = this.Position.X;
            this.BoundingBox.Min.Y = this.Position.Y + 14;
            this.BoundingBox.Max.X = this.Position.X + this.Texture.Width;
            this.BoundingBox.Max.Y = this.Position.Y + 18;
        }
    }
}