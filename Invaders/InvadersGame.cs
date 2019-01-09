using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Invaders
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class InvadersGame : Game
    {
        GraphicsDeviceManager graphics;
        private const int ResolutionX = 1280;
        private const int ResolutionY = 800;
        
        SpriteBatch spriteBatch;
        Random random;


        // Resources
        SpriteFont fontAgency;
        SpriteFont fontAgency16;
        SpriteFont fontArial;

        Texture2D playerTex;
        Texture2D enemyTex;
        Texture2D bunkerTex;
        Texture2D[] bunker_hit;
        Texture2D bombTex;
        Texture2D bulletTex;
        //Texture2D groundTex;

        // Game objects
        Player player;
        List<Enemy> enemies;
        List<Bunker> bunkers;
        List<Bomb> bombs;
        List<Projectile> bullets;
        Ground ground;

        // Global variables
        readonly int numEnemies = 11;
        readonly int numBunkers = 4;
        readonly int bombDropChance = 10; // inverse -- ie. 1/x, value of 10 == 10%, 20 == 1/20 == 5%
        readonly int bombDropCooldown = 800; //ms

        readonly int playerFireCooldown = 350; //ms
        int lastFired = 0;



        public InvadersGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = ResolutionX,
                PreferredBackBufferHeight = ResolutionY
            };

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            random = new Random();
            // TODO: Add your initialization logic here
            player = new Player();
            enemies = new List<Enemy>(numEnemies);
            bunkers = new List<Bunker>(numBunkers);
            bombs = new List<Bomb>();
            bullets = new List<Projectile>();

            for(int i = 0; i < numEnemies; i++) { enemies.Add(new Enemy()); }
            for(int i = 0; i < numBunkers; i++) { bunkers.Add(new Bunker()); }

            ground = new Ground(
                new Texture2D(graphics.GraphicsDevice, 1, 1),
                new Rectangle(0, ResolutionY - 20,
                              ResolutionX, 20));
            ground.Texture.SetData(new[] { Color.White });

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Load fonts
            fontAgency = Content.Load<SpriteFont>("AgencyFB");
            fontAgency16 = Content.Load<SpriteFont>("AgencyFB16");
            fontArial = Content.Load<SpriteFont>("Arial");
            // Textures
            playerTex = Content.Load<Texture2D>("tank");
            player.Texture = playerTex;
            player.Position = new Vector2(ResolutionX / 2 - playerTex.Width / 2,
                                          ResolutionY - playerTex.Height - 40);
            enemyTex = Content.Load<Texture2D>("alien2");

            for (int i = 1; i <= enemies.Count; i++)
            {
                Enemy enemy = enemies[i - 1];
                enemy.Texture = enemyTex;
                enemy.Position = new Vector2( (ResolutionX / (enemies.Count + 1)) * i - enemyTex.Width / 2, 
                                               200 - enemyTex.Height / 2);
                enemy.MovePattern = new Vector2[] {
                                 new Vector2(enemy.Position.X - ((ResolutionX / (enemies.Count + 1)) 
                                             - enemyTex.Width / 2),
                                             enemy.Position.Y),
                                 new Vector2(enemy.Position.X + ((ResolutionX / (enemies.Count + 1)) - enemyTex.Width / 2),
                                             enemy.Position.Y) };
                enemy.MoveSpeed = 1;
            }

            bunkerTex = Content.Load<Texture2D>("bunker");
            for (int i = 1; i <= bunkers.Count; i++)
            {
                Bunker b = bunkers[i - 1];
                b.Texture = bunkerTex;
                b.Position = new Vector2((ResolutionX / (bunkers.Count + 1)) * i - bunkerTex.Width / 2, 
                                          ResolutionY - bunkerTex.Height - 200);
                b.BoundingBox = new BoundingBox(new Vector3(b.Position.X, b.Position.Y + 17, 0),
                                               new Vector3(b.Position.X + b.Texture.Width, b.Position.Y + 31, 0));
            }
            bunker_hit = new Texture2D[3];
            bunker_hit[0] = Content.Load<Texture2D>("bunker_broken03");
            bunker_hit[1] = Content.Load<Texture2D>("bunker_broken02");
            bunker_hit[2] = Content.Load<Texture2D>("bunker_broken01");

            bombTex = Content.Load<Texture2D>("alienbomb");
            bulletTex = Content.Load<Texture2D>("playerbullet");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // PLAYER CONTROLS
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) { Exit(); }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (player.Position.X > 4)
                {
                    player.Position.X -= 4;
                    player.UpdateBoundingBox();
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
                if (player.Position.X + playerTex.Width < ResolutionX)
                {
                    player.Position.X += 4;
                    player.UpdateBoundingBox();
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if ( (gameTime.TotalGameTime.TotalMilliseconds - playerFireCooldown) > lastFired)
                {
                    FireBullet(player.Position, gameTime);
                    lastFired = (int)gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
            //----------------------------------------


            // ENEMY PROCEDURES
            foreach (Enemy enemy in enemies)
            {
                // Move enemy
                if(enemy.Position.X - enemy.MovePattern[enemy.CurrentGoal].X < 0)
                {
                    enemy.Position.X += enemy.MoveSpeed;
                    enemy.UpdateBoundingBox();
                }
                else if (enemy.Position.X - enemy.MovePattern[enemy.CurrentGoal].X > 0)
                {
                    enemy.Position.X -= enemy.MoveSpeed;
                    enemy.UpdateBoundingBox();
                }
                else //current move goal reached, change move goal
                {
                    if (enemy.CurrentGoal + 1 == enemy.MovePattern.Length) { enemy.CurrentGoal = 0; }
                    else enemy.CurrentGoal++;
                }
                // Detertime bomb dropping
                if ( gameTime.TotalGameTime.TotalMilliseconds - enemy.LastDropped > bombDropCooldown)
                {
                    if (random.Next(1, bombDropChance + 1) == bombDropChance) DropBomb(enemy.Position, gameTime);
                    enemy.LastDropped = (int)gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            //----------------------------------------





            // ADVANCE PROJECTILES AND CHECK COLLISIONS

            
            for (int i = 0; i < bullets.Count; i++)
            {
                Projectile p = bullets[i];
                p.Position.Y -= 10;
                p.UpdateBoundingBox();
                if (p.Position.Y < -(p.Texture.Height))
                {
                    bullets.Remove(p);
                    continue;
                }
                // Collision detection
                for(int j = 0; j < bunkers.Count; j++)
                {
                    Bunker b = bunkers[j];
                    if (p.BoundingBox.Intersects(b.BoundingBox))
                    {
                        bullets.Remove(p);
                        b.Health--;
                        if (b.Health < 0) { bunkers.Remove(b); }
                        else { b.Texture = bunker_hit[b.Health]; }
                        break;
                    }
                }
                for (int k = 0; k < enemies.Count; k++)
                {
                    Enemy enemy = enemies[k];
                    if (p.BoundingBox.Intersects(enemy.BoundingBox))
                    {
                        enemies.Remove(enemy);
                        bullets.Remove(p);
                        break;
                    }
                }
            }


            for (int i = 0; i < bombs.Count; i++)
            {
                Bomb b = bombs[i];
                b.Position.Y += 10;
                b.UpdateBoundingBox();

                // Collision detection
                for (int j = 0; j < bunkers.Count; j++)
                {
                    Bunker bunker = bunkers[j];
                    if (b.BoundingBox.Intersects(bunker.BoundingBox))
                    {
                        bombs.Remove(b);
                        bunker.Health--;
                        if (bunker.Health < 0) { bunkers.Remove(bunker); }
                        else { bunker.Texture = bunker_hit[bunker.Health]; }
                        break;
                    }
                }

                if (b.BoundingBox.Intersects(ground.BoundingBox)
                 || b.Position.Y > ResolutionY)
                {
                    bombs.Remove(b);
                }
            }
            

            //----------------------------------------


            

            

            base.Update(gameTime);
        }


        private void FireBullet(Vector2 playerPos, GameTime gameTime)
        {
            Projectile p = new Projectile(bulletTex, 
                new Vector2(playerPos.X + playerTex.Width / 2 - bulletTex.Width / 2,
                            playerPos.Y - bulletTex.Height),
                            (int)gameTime.TotalGameTime.TotalMilliseconds);
            bullets.Add(p);
        }


        private void DropBomb(Vector2 enemyPos, GameTime gameTime)
        {
            Bomb b = new Bomb(bombTex,
                new Vector2(enemyPos.X + enemyTex.Width / 2 - bombTex.Width / 2,
                            enemyPos.Y + enemyTex.Height),
                            (int)gameTime.TotalGameTime.TotalMilliseconds);
            bombs.Add(b);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //spriteBatch.DrawString(fontAgency16, callOrder, new Vector2(10, 10), Color.Red);
            spriteBatch.DrawString(fontAgency16, graphics.GraphicsDevice.Viewport.Width.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(fontAgency16, player.Position.X.ToString(), new Vector2(10, 40), Color.Wheat);
            spriteBatch.DrawString(fontAgency16, bullets.Count.ToString(), new Vector2(10, 60), Color.Pink);
            //spriteBatch.DrawString(fontAgency16, lastFired.ToString(), new Vector2(10, 60), Color.Pink);
            //spriteBatch.DrawString(fontAgency16, gameTime.TotalGameTime.TotalMilliseconds.ToString(), new Vector2(10, 80), Color.LightBlue);
            spriteBatch.Draw(player.Texture, player.Position, Color.White);
            foreach (var enemy in enemies) { spriteBatch.Draw(enemy.Texture, enemy.Position, Color.White); }
            foreach (var bunker in bunkers) { spriteBatch.Draw(bunker.Texture, bunker.Position, Color.White); }
            foreach (var bullet in bullets) { spriteBatch.Draw(bullet.Texture, bullet.Position, Color.White); }
            foreach (var bomb in bombs) { spriteBatch.Draw(bomb.Texture, bomb.Position, Color.White); }
            spriteBatch.Draw(ground.Texture, ground.Bounds, Color.Green);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
