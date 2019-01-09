using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Invaders2
{
    public class Bomb
    {
        public Vector2 Position;
        public Texture2D Texture;
        public int Id;
        public BoundingBox BoundingBox;

        public Bomb(Texture2D tex, Vector2 pos, int id)
        {
            this.Texture = tex;
            this.Position = pos;
            this.Id = id;
            this.BoundingBox = new BoundingBox(new Vector3(this.Position.X + 9, this.Position.Y + 7, 0),
                                               new Vector3(this.Position.X + 15, this.Position.Y + 21, 0));
        }


        public void UpdateBoundingBox()
        {
            this.BoundingBox.Min.X = this.Position.X + 9;
            this.BoundingBox.Min.Y = this.Position.Y + 7;
            this.BoundingBox.Max.X = this.Position.X + 15;
            this.BoundingBox.Max.Y = this.Position.Y + 21;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Bomb b = (Bomb)obj;
                return this.Id == b.Id;
            }
        }
    }
}