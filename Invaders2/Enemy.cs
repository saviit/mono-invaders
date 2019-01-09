using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Invaders2
{


    public class Enemy
    {
        public Vector2 Position; // { get; set; }
        public Texture2D Texture; // { get; set; }
        public Vector2[] MovePattern; // { get; set; }
        public float MoveSpeed; // { get; set; }
        public int CurrentGoal; // { get; set; }
        public BoundingBox BoundingBox;

        public int LastDropped = 0;
        //private Microsoft.Xna.Framework.Graphics. shape;

        public Enemy() { this.CurrentGoal = 0; }

        public Enemy(Texture2D tex, Vector2 pos, float speed)
        {
            this.Position = pos;
            this.Texture = tex;
            this.MoveSpeed = speed;
            //if (!(pattern == null)) this.MovePattern = pattern;
            this.CurrentGoal = 0;
            this.BoundingBox = new BoundingBox(new Vector3(this.Position.X + 1, this.Position.Y + 1, 0),
                                               new Vector3(this.Position.X + this.Texture.Width - 1, this.Position.Y + 51, 0));
        }

        public void UpdateBoundingBox()
        {
            this.BoundingBox.Min.X = this.Position.X + 1;
            this.BoundingBox.Min.Y = this.Position.Y + 1;
            this.BoundingBox.Max.X = this.Position.X + 63;
            this.BoundingBox.Max.Y = this.Position.Y + 51;
        }


        /*
         * Initialize the Enemy
         */
        public void Initialize(Vector2 pos, Texture2D tex, Vector2[] pattern = null, float speed = 1.0f)
        {
            this.Position = pos;
            this.Texture = tex;
            this.MoveSpeed = speed;
            if (!(pattern == null)) this.MovePattern = pattern;
            this.CurrentGoal = 0;
        }

        // Update position etc.
        public void Update(GameTime time)
        {
            if(MovePattern != null ) 
            {
                //if( ValuesEqual(Position, MovePattern[CurrentGoal]) ) { CurrentGoal++; } //start moving towards next target
                if (CurrentGoal < MovePattern.Length)
                {
                    Vector2 goal = MovePattern[CurrentGoal];
                    Vector2 current = Position;
                    //Vector2 dist = Distance(goal, current);
                    //float dx1 = dist.X;
                    //float dy1 = dist.Y;
                    //float dx2 = current.X + MoveSpeed;
                    //float dy2 = (dx2 * dy1) / dx1;

                    //if (float.IsInfinity(dy2) || float.IsNaN(dy2)) { dy2 = 0f; }; //BAD

                    //Position = new Vector2(dx2, current.Y + dy2);

                    //float factorX = 0;

                    //if (goal.X > current.X) { factorX = (goal.X - current.X) / (current.Y - goal.Y); }
                    //if (goal.X < current.X) { factorX = (goal.X - current.X) / (goal.Y - current.Y); }


                    //float factorY = 0;
                    //if (goal.Y > current.Y) { factorY = 1; }
                    //if (goal.Y < current.Y) { factorY = -1; }
                    //if (goal.Y == current.Y) {
                    //    factorY = 0;
                    //    if (goal.X > current.X) { factorX = 1; }
                    //    if (goal.X < current.X) { factorX = -1; }
                    //}
                    //double dx = goal.X - current.X;
                    //double dy = goal.Y - current.Y;
                    //double d = Math.Sqrt((dx * dx) + (dy * dy));
                    //float incX = (float)Math.Round((double)MoveSpeed * (dx / d), 0);
                    //float incY = (float)Math.Round((double)MoveSpeed * (dy / d), 0);

                    float dx = goal.X - current.X;
                    float dy = goal.Y - current.Y;
                    float d = (float)Math.Sqrt((dx * dx) + (dy * dy));

                    Position = new Vector2(Position.X + (MoveSpeed * (dx / d)), Position.Y + (MoveSpeed * (dy / d)));

                    if (d < MoveSpeed) CurrentGoal++;

                } else if (CurrentGoal >= MovePattern.Length) { CurrentGoal = 0; }
            }
        }


        // Handles drawing of the Enemy on the specified SpriteBatch
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Position, Color.White);
        }



        private bool ValuesEqual(Vector2 v1, Vector2 v2)
        {
            if (v1.X == v2.X && v1.Y == v2.Y) return true;
            return false;
        }

        // Returns the absolute distance between two points (vectors) as a Vector2
        private Vector2 Distance(Vector2 v1, Vector2 v2)
        {
            return new Vector2(Math.Abs(Math.Max(v1.X, v2.X) - Math.Min(v1.X, v2.X)), Math.Abs(Math.Max(v1.Y, v2.Y) - Math.Min(v1.Y, v2.Y)));
        }
    }


}