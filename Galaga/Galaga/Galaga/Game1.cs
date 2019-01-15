using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Galaga
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D galagaSpriteSheet;
        KeyboardState old;

        //shooting things
        List<Rectangle> playerShots;
        List<Rectangle> enemyShots;
        int fireTime;
        int efireTime;

        //sprite location on the screen
        Rectangle ship;
        Rectangle spaceFly;
        Rectangle playBullet; //player's bullet
        Rectangle enBullet; //enemy bullet
        Rectangle explosion;

        //sprite location on picture
        Rectangle spaceFly1;
        Rectangle pBull1;
        Rectangle eBull1;
        Rectangle explosion1;
        Rectangle explosion2;
        Rectangle explosion3;
        Rectangle explosion4;
        Rectangle explosion5;

        //list of enemies
        List<Rectangle> enemySprites;
        List<Rectangle> enemyLocations;
        int move;

        //lives and score
        int life;
        int score;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 650;
            graphics.ApplyChanges();
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
            // TODO: Add your initialization logic here
            old = Keyboard.GetState();

            playerShots = new List<Rectangle>();
            fireTime = 0;
            enemyShots = new List<Rectangle>();
            efireTime = 0;

            //on screen
            ship = new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 50, 35, 35);
            spaceFly = new Rectangle(10, 10, 35, 35);

            playBullet = new Rectangle();
            enBullet = new Rectangle();
            explosion = new Rectangle();


            //on sprite sheet
            
            spaceFly1 = new Rectangle(158, 174, 20, 20);

            pBull1 = new Rectangle(364, 193, 20, 20);
            eBull1 = new Rectangle(372, 49, 20, 20);

            explosion1 = new Rectangle(208, 187, 20, 20);
            explosion2 = new Rectangle(229, 187, 20, 20);
            explosion3 = new Rectangle(251, 187, 20, 20);
            explosion4 = new Rectangle(278, 187, 20, 20);
            explosion5 = new Rectangle(314, 187, 20, 20);


            //enemy list
            enemySprites = new List<Rectangle>(); //on spriteSheet locations
            enemyLocations = new List<Rectangle>(); //on screen locations
            enemySprites.Add(spaceFly1);
            enemyLocations.Add(spaceFly);
            move = 3;

            
            //life and score
            life = 3;
            score = 0;
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
            galagaSpriteSheet = this.Content.Load<Texture2D>("Galaga Textures");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
        /// 
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            KeyboardState kb = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            //shoots with space bar
            if (kb.IsKeyDown(Keys.Space))
            {
                if (fireTime % 10 == 0)
                {
                    playerShots.Add(new Rectangle(ship.X + 16, ship.Y + 5, 15, 20));
                }
                fireTime++;
            }
            shoot();
            
            enemyShoot();
            handleCollissions();
            shipMovement(kb);
            
            enemyMovement();

            //extra life based on scoring
            if (score == 20000 || score % 70000 == 0)
            {
                life++;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for (int i = 0; i < playerShots.Count; i++)
                spriteBatch.Draw(galagaSpriteSheet, playerShots[i], pBull1, Color.White);
            for (int i = 0; i < enemyShots.Count; i++)
                spriteBatch.Draw(galagaSpriteSheet, enemyShots[i], eBull1, Color.White);
            spriteBatch.Draw(galagaSpriteSheet, ship, new Rectangle(181, 53, 20, 20), Color.White);
            spriteBatch.Draw(galagaSpriteSheet, playBullet, pBull1, Color.White);
            spriteBatch.Draw(galagaSpriteSheet, enBullet, eBull1, Color.White);
            for (int i = 0; i < enemySprites.Count; i++)
                spriteBatch.Draw(galagaSpriteSheet, enemyLocations[i], enemySprites[i], Color.White);
            spriteBatch.End();
            base.Draw(gameTime);

        }

        public void shoot()
        {
            for (int i = playerShots.Count - 1; i >= 0; i--)
            {
                playerShots[i] = new Rectangle(playerShots[i].X, playerShots[i].Y - 20, playerShots[i].Width, playerShots[i].Height);
            }
        }

        public void shipMovement(KeyboardState kb)
        {



            if (kb.IsKeyDown(Keys.Right) && ship.X <= GraphicsDevice.Viewport.Width)
            {
                ship.X += 5;
            }
            if (kb.IsKeyDown(Keys.Left) && ship.X >= 0)
            {
                ship.X -= 5;
            }

        }
        public void eshoot()
        {
            for (int i = 0; i < enemyLocations.Count; i++)
            {

                enemyShots.Add(new Rectangle(enemyLocations[i].X + 16, enemyLocations[i].Y, 15, 20));
            }
            
            
        }

        public void enemyShoot()
        {
            efireTime++;
            if (efireTime % 240 == 0)
                eshoot();
            for (int i = 0; i < enemyShots.Count; i++)
                enemyShots[i] = new Rectangle(enemyShots[i].X, enemyShots[i].Y + 7, enemyShots[i].Width, enemyShots[i].Height);
        }

        public void handleCollissions()
        {

            for (int i = playerShots.Count - 1; i >= 0; i--)
            {
                if(playerShots[i].Intersects(spaceFly))
                {
                    playerShots.Remove(playerShots[i]);
                    score += 100;
                }
            }

            for(int i = enemyShots.Count - 1; i >= 0; i--)
            {
                if (enemyShots[i].Intersects(ship))
                {
                    enemyShots.Remove(enemyShots[i]);
                }
            }
        }

        public void enemyMovement()
        {
            for (int i = 0; i < enemyLocations.Count; i++)
            {
                Rectangle currentEnemy = enemyLocations[i];
                currentEnemy.X += move;
                enemyLocations[i] = currentEnemy;

                if (enemyLocations[i].X + enemyLocations[i].Width > GraphicsDevice.Viewport.Width || enemyLocations[i].X < 10)
                {
                    move *= -1;
                }
            }
        }
    }
}
