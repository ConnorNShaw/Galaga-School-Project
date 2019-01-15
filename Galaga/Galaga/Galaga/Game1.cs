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
        int fireTime;

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
            enemySprites.Add(spaceFly);
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
        public void enemyMovement()
        {
            //for (int i = 0; i < enemies.Length; i++)
            //{
            //    enemies[i].X += move;
            //    if (enemies[i].X + enemies[i].Width > GraphicsDevice.Viewport.Width || enemies[i].X < 10)
            //    {
            //        move *= -1;
            //    }
            //}
            spaceFly.X += move;
            if (spaceFly.X + spaceFly.Width > GraphicsDevice.Viewport.Width || spaceFly.X < 0)
            {
                move *= -1;
            }
        }
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            KeyboardState kb = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            if (kb.IsKeyDown(Keys.Space))
            {
                if (fireTime % 10 == 0)
                {
                    playerShots.Add(new Rectangle(ship.X + 10, ship.Y, 15, 20));
                }
                fireTime++;
            }

            enemyMovement();

            //extra life based on scoring
            if (score == 30000 || score == 80000)
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(galagaSpriteSheet, ship, new Rectangle(181, 53, 20, 20), Color.White);
            spriteBatch.Draw(galagaSpriteSheet, playBullet, pBull1, Color.White);
            spriteBatch.Draw(galagaSpriteSheet, enBullet, eBull1, Color.White);
            spriteBatch.Draw(galagaSpriteSheet, spaceFly, spaceFly1, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
