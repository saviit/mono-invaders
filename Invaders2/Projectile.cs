using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Invaders2
{
    public class Projectile
    {
        public Vector2 Position;
        public Texture2D Texture;
        public int Id;
        public BoundingBox BoundingBox;

        public Projectile(Texture2D tex, Vector2 pos, int id)
        {
            this.Texture = tex;
            this.Position = pos;
            this.Id = id;
            this.BoundingBox = new BoundingBox(new Vector3(this.Position.X + 7, this.Position.Y + 1, 0),
                                               new Vector3(this.Position.X + 9, this.Position.Y + 12, 0));
        }

        public void UpdateBoundingBox()
        {
            this.BoundingBox.Min.X = this.Position.X + 7;
            this.BoundingBox.Min.Y = this.Position.Y + 1;
            this.BoundingBox.Max.X = this.Position.X + 9;
            this.BoundingBox.Max.Y = this.Position.Y + 12;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }


        public override bool Equals(object obj)
        {
            if( obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else 
            {
                Projectile p = (Projectile)obj;
                return this.Id == p.Id;
            }
        }
    }

}