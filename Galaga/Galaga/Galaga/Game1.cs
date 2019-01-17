using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    /// peepee hurty
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D galagaSpriteSheet;
        Texture2D background;
        KeyboardState old;

        int timer;

        //shooting things
        List<Rectangle> playerShots;
        List<Rectangle> enemyShots;

        int fireTime;
        int efireTime;

        //sprite location on the screen
        Rectangle ship;
        Rectangle spaceFly;
        Rectangle birdy;
        Rectangle playBullet; //player's bullet
        Rectangle enBullet; //enemy bullet
        Rectangle explosion;

        //sprite location on sprite sheet
        Rectangle spaceFly1;
        Rectangle butterboi1;
        Rectangle birdy1;

        Rectangle pBull1;
        Rectangle eBull1;
        List<Rectangle> enemyExplosion;
        Rectangle enemyExplosion1, enemyExplosion2, enemyExplosion3, enemyExplosion4, enemyExplosion5;
        List<Rectangle> playerExplosion;
        Rectangle playerExplosion1, playerExplosion2, playerExplosion3, playerExplosion4;


        //Booleans determine which explosion to draw
        bool explodePlayer;
        bool explodeEnemy;
        int explosionTracker;

        //list of enemies
        List<Enemy> enemys;
        int move;

        //lives and score
        SpriteFont font;
        int life;
        int highScore;
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

            timer = 0;

            playerShots = new List<Rectangle>();
            fireTime = 0;
            enemyShots = new List<Rectangle>();
            efireTime = 0;
            enemys = new List<Enemy>();

            //on screen
            ship = new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 90, 35, 35);
            spaceFly = new Rectangle(10, 50, 35, 35);
            birdy = new Rectangle(90, 50, 35, 35);

            playBullet = new Rectangle();
            enBullet = new Rectangle();
            explosion = new Rectangle();


            //on sprite sheet
            spaceFly1 = new Rectangle(158, 174, 20, 20);
            butterboi1 = new Rectangle(158, 152, 20, 20);
            birdy1 = new Rectangle(158, 200, 20, 20);

            pBull1 = new Rectangle(364, 193, 10, 20);
            eBull1 = new Rectangle(372, 49, 10, 20);

            enemyExplosion = new List<Rectangle>();
            enemyExplosion1 = new Rectangle(208, 187, 20, 20);
            enemyExplosion.Add(enemyExplosion1);
            enemyExplosion2 = new Rectangle(229, 187, 20, 20);
            enemyExplosion.Add(enemyExplosion2);
            enemyExplosion3 = new Rectangle(251, 187, 20, 20);
            enemyExplosion.Add(enemyExplosion3);
            enemyExplosion4 = new Rectangle(278, 187, 20, 20);
            enemyExplosion.Add(enemyExplosion4);
            enemyExplosion5 = new Rectangle(314, 187, 20, 20);
            enemyExplosion.Add(enemyExplosion5);

            playerExplosion = new List<Rectangle>();
            playerExplosion1 = new Rectangle(210, 45, 40, 40);
            playerExplosion.Add(playerExplosion1);
            playerExplosion2 = new Rectangle(246, 45, 40, 40);
            playerExplosion.Add(playerExplosion2);
            playerExplosion3 = new Rectangle(285, 45, 40, 40);
            playerExplosion.Add(playerExplosion3);
            playerExplosion4 = new Rectangle(327, 45, 40, 40);
            playerExplosion.Add(playerExplosion4);

            explodePlayer = false;
            explodeEnemy = false;
            explosionTracker = 0;

            //enemy list
            enemys.Add(new Enemy(spaceFly, spaceFly1));
            enemys.Add(new Enemy(new Rectangle(50, 54, 35, 35), butterboi1));
            enemys.Add(new Enemy(birdy, birdy1));
            move = 3;
            
            //life and score
            life = 2;
            highScore = 20000;
            score = 0;
            font = this.Content.Load<SpriteFont>("SpriteFont1");

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
            background = this.Content.Load<Texture2D>("download (5)");

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
            timer++;
            int seconds = timer / 60;

            if (seconds % 10 == 0 && explodePlayer || explodeEnemy)
            {
                explosionTracker++;
            }
            else
            {
                explosionTracker = 0;
            }

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
                    playerShots.Add(new Rectangle(ship.X + 12, ship.Y + 5, 20, 30));
                }
                fireTime++;
            }
            shoot();

            enemyShoot();
            handleCollissions();
            shipMovement(kb);

            enemyMovement();


            //new high score
            if (score > highScore)
            {
                highScore = score;
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
            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            for (int i = 0; i < playerShots.Count; i++)
                spriteBatch.Draw(galagaSpriteSheet, playerShots[i], pBull1, Color.SkyBlue);

            for (int i = 0; i < enemyShots.Count; i++)
                spriteBatch.Draw(galagaSpriteSheet, enemyShots[i], eBull1, Color.White);

            if (life > -1)
                spriteBatch.Draw(galagaSpriteSheet, ship, new Rectangle(181, 53, 20, 20), Color.White);
            else
                spriteBatch.DrawString(font, "GAME OVER", new Vector2(GraphicsDevice.Viewport.Width / 2 - 50, GraphicsDevice.Viewport.Height / 2), Color.Turquoise);
            //spriteBatch.Draw(galagaSpriteSheet, ship, new Rectangle(181, 53, 20, 20), Color.White);

            spriteBatch.Draw(galagaSpriteSheet, playBullet, pBull1, Color.White);
            spriteBatch.Draw(galagaSpriteSheet, enBullet, eBull1, Color.White);
            spriteBatch.DrawString(font, "1UP", new Vector2(20, 10), Color.Red);
            spriteBatch.DrawString(font, "HIGH SCORE", new Vector2(GraphicsDevice.Viewport.Width / 2 - 50, 10), Color.Red);
            spriteBatch.DrawString(font, "" + score, new Vector2(20, 25), Color.White);
            spriteBatch.DrawString(font, "" + highScore, new Vector2(GraphicsDevice.Viewport.Width / 2 - 50, 25), Color.White);

            for (int i = 0; i < life; i++)
            {
                if ((score == 20000 || score % 70000 == 0) && score != 0)
                {
                    life++;
                }
                spriteBatch.Draw(galagaSpriteSheet, new Rectangle(0 + (i * 35), GraphicsDevice.Viewport.Height - ship.Height, 35, 35), new Rectangle(181, 53, 20, 20), Color.White);
            }
            for (int i = 0; i < enemys.Count; i++)
                spriteBatch.Draw(galagaSpriteSheet, enemys[i].pos, enemys[i].spritePos, Color.White);


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
        if (kb.IsKeyDown(Keys.Right) && ship.X + ship.Width < GraphicsDevice.Viewport.Width)
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
        for (int i = 0; i < enemys.Count; i++)
        {

            enemyShots.Add(new Rectangle(enemys[i].pos.X + 12, enemys[i].pos.Y, 20, 30));
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
            for (int k = enemys.Count - 1; k >= 0; k--)
            {
                if (playerShots[i].Intersects(enemys[k].pos))
                {
                    score += enemys[k].value;
                    playerShots.Remove(playerShots[i]);
                    enemys.Remove(enemys[k]);
                    break;

                }
            }
        }
        for (int i = enemyShots.Count - 1; i >= 0; i--)
        {
            if (enemyShots[i].Intersects(ship))
            {
                enemyShots.Remove(enemyShots[i]);
                life--;
                explodePlayer = true;
            }
        }
    }

    public void enemyMovement()
    {
        for (int i = 0; i < enemys.Count; i++)
        {

            enemys[i].pos.X += move;

            if (enemys[i].pos.X + enemys[i].pos.Width > GraphicsDevice.Viewport.Width || enemys[i].pos.X < 5)
            {
                move *= -1;
            }
        }
    }
}
}