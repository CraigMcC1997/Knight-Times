using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knight_Times.Content
{
    public class BossLevel1 : ILevel
    {
        //Loads the classes
        public Player Player;
        public bool IsDead
        {
            get { return !Player.IsPlayerAlive; }
        }

        Boss1 Boss;

        //Projectile
        Texture2D Projectile;
        Vector2 ProjectilePos;
        Rectangle ProjectileHitbox;
        Vector2 ProjectileVel;
        float ProjectileSpeed = 30;

        //Healthbar
        Texture2D Healthbar1;
        Vector2 Healthbar1Pos;
        Texture2D Healthbar2;

        //Timer
        float Timer = 0;

        //Loads the font code
        SpriteFont Arial;
        Vector2 BossHealthPosition;
        Vector2 PlayersHealthPos;

        //Background information
        Texture2D BackgroundTexture;
        Vector2 BackgroundPosition;

        //EndScreen
        Texture2D EndScreen;
        Vector2 EndScreenPos;

        //Players dies screen
        Texture2D PlayerDeadScreen;
        Vector2 PlayerDeadScreenPos;

        //Sound effects
        SoundEffect BossAttack, BossDeath, VictoryTheme;

        //Stops the next level loading
        bool EndLevel = false;
        //Ends the current level and loads next level
        public bool EndCurrentLevel
        {
            set { EndLevel = value; }
            get { return EndLevel; }
        }


        //sets a list of the collidables
        List<ICollidable> m_collidables = new List<ICollidable>();

        int displaywidth;
        int displayheight;

        int gameOverDelayTimer = 1000;

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            displaywidth = graphicsDevice.Viewport.Width;
            displayheight = graphicsDevice.Viewport.Height;

            //Background Texture information
            BackgroundTexture = content.Load<Texture2D>("BackGround2");
            BackgroundPosition = new Vector2(0, 0);

            //EndScreen Texture information
            EndScreen = content.Load<Texture2D>("Win");
            EndScreenPos = new Vector2(0, 0);

            //Player Dead screen information
            PlayerDeadScreen = content.Load<Texture2D>("PlayerDead");
            PlayerDeadScreenPos = new Vector2(0, 0);

            //Loads texture for the healthbar from the content pipeline
            Healthbar1 = content.Load<Texture2D>("NotMovingHealthBar");
            Healthbar2 = content.Load<Texture2D>("HealthBar");

            //Healthbar1 position
            Healthbar1Pos = new Vector2(0, 50);

            //player
            Player = new Player(content, new Vector2(0, 756));

            //Boss
            Boss = new Boss1(content, new Vector2(1300, 400));

            //Bullet
            if (Boss.IsBossAlive)
            {
                Projectile = content.Load<Texture2D>("Projectile");
                ProjectilePos.X = -2000;
                ProjectilePos.Y = -1000;
            }

            //Platforms
            var Platform = new Platform(content, new Vector2(0, 200));
            m_collidables.Add(Platform);

            Platform = new Platform(content, new Vector2(1100, 550));
            m_collidables.Add(Platform);

            Platform = new Platform(content, new Vector2(500, 550));
            m_collidables.Add(Platform);

            //Platforms
            var Level1Platform = new XHitbox(content, new Vector2(0, 840));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(128, 840));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(168, 840));
            m_collidables.Add(Level1Platform);

            //Endpoint
            var endpoint = new EndPoint(content, new Vector2(1400, 744));
            m_collidables.Add(endpoint);

            //Loads texture for the healthbar from the content pipeline

            Healthbar2 = content.Load<Texture2D>("Healthbar");

            //Loads the font from the font sheet
            Arial = content.Load<SpriteFont>("Font");

            //The coordinates for the Bosses Health
            BossHealthPosition.X = displaywidth / 2 - 150;
            BossHealthPosition.Y = 0;

            //The coordinates for the Players Health
            PlayersHealthPos.X = 0;
            PlayersHealthPos.Y = 0;

            //Loads the sound effects
            BossDeath = content.Load<SoundEffect>("BossDeath");
            BossAttack = content.Load<SoundEffect>("BossAttack");
            VictoryTheme = content.Load<SoundEffect>("VictoryTheme");
        }

        public void Update(GameTime gameTime, ref float TimeTaken)
        {
            //For controller support
            GamePadState padState1 = GamePad.GetState(PlayerIndex.One);

            // Time between updates
            float timebetweenupdates = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Jumping code
            var originalPosition = Player.PlayerPosition;

            //Moves the player using predefined keys
            if (Player.IsPlayerAlive)
            {
                Player.MoveMe(Keyboard.GetState(), timebetweenupdates);
            }

            //Player attacks if they player is alive and the enter key is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || padState1.Buttons.X == ButtonState.Pressed && Player.IsPlayerAlive)
            {
                Player.attack();
            }

            //all things that happen if the boss is alive
            if (Boss.IsBossAlive)
            {
                int range = 1000;
                // Fire when within range
                Vector2 distance = Player.PlayerPosition - new Vector2(Boss.Position.X, Boss.Position.Y - 100);
                if (distance.Length() < 550 && (ProjectilePos.X < -range  || ProjectilePos.Y < -range || ProjectilePos.Y > displayheight+range) && Player.IsPlayerAlive)
                {
                    // FIRE
                    distance.Normalize();
                    ProjectileVel = distance *ProjectileSpeed;
                    ProjectilePos = new Vector2(Boss.Position.X, Boss.Position.Y - 100);
                    BossAttack.Play();
                }

                //TO HELP LESS ABLED BODIED PLAYERS
                if (Keyboard.GetState().IsKeyDown(Keys.Add) && Boss.BossLives > 50)
                {
                    Boss.BossLives -= 50;
                }

                // Daves cheat cause I don't know how to kill nessie
                if (Keyboard.GetState().IsKeyDown(Keys.K))
                    Boss.BossLives --;


                    //Moving the bosses projectile
                    ProjectilePos += ProjectileVel;

                //Hitbox for the bosses projectile
                ProjectileHitbox = new Rectangle((int)ProjectilePos.X, (int)ProjectilePos.Y, Projectile.Width, Projectile.Height);

                //Player taking damage
                if (Player.Hitbox.Intersects(Boss.Hitbox) && Boss.IsBossAlive)
                {
                    Player.PlayerLives --;
                }

                //If projectile intersects player, player loses life and moves back
                if (Player.Hitbox.Intersects(ProjectileHitbox))
                {
                    Player.PlayerLives -= 10;
                }

                //Code for how boss loses life
                if (Player.Hitbox.Intersects(Boss.Hitbox) && (Keyboard.GetState().IsKeyDown(Keys.Enter) || padState1.Buttons.X == ButtonState.Pressed && Player.IsPlayerAlive))
                {
                    Boss.CheckBossDamage();
                }

                //Code for killing boss
                if (Boss.BossLives <= 0)
                {
                    Boss.IsBossAlive = false;
                    BossDeath.Play();
                }
            }
            else
            {
                // YOU KILLED THE BOSS!!
                //Lets the game load the next level if the two objects collide

                //Changing Level
                var endpoint = m_collidables.FirstOrDefault(x => x.CollisionType == CollidableType.Endpoint);

                if (Player.Hitbox.Intersects(endpoint.Hitbox))
                {
                    EndLevel = true;
                    VictoryTheme.Play(0.6f, 0f, 0f);
                }
            }

            //Kills the player if the health is less that 0
            if (Player.PlayerLives < 0)
            {
                Player.IsPlayerAlive = false;
                gameOverDelayTimer -= gameTime.ElapsedGameTime.Milliseconds;

            }

            //resetting the Player position if it goes off screen on the Y axis
            //resetting the Player position if it goes off screen on the Y axis
            if (Player.PlayerPosition.Y > displayheight + 500)
            {
                Player.PlayerLives -= 2;
            }
            if (Player.PlayerPosition.Y > displayheight +1000)
            {
                Player.TempPlayerPosition = new Vector2(300, -200);
            }

            //Makes the game update all the hitboxes for objects on screen
            Player.Update(gameTime, m_collidables);

            //Adding to the timer int to allow the boss to move
            Timer += timebetweenupdates;

            //Making the boss follow the player on the Y axis
            if (Math.Abs(Player.PlayerPosition.Y - Boss.Position.Y) > 10)
            {
                if (Player.PlayerPosition.Y < Boss.Position.Y)
                {
                    Boss.Position.Y -= Boss.speed;
                }
                else
                {
                    Boss.Position.Y += Boss.speed;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, float Time)
        {
            spriteBatch.Begin();

            //Draws the background
            spriteBatch.Draw(BackgroundTexture, Vector2.Zero, Color.White);

            //Draws the player
            if (Player.IsPlayerAlive)
            {
                Player.Draw(spriteBatch);
            }

            //Draws bullet
            if (Boss.IsBossAlive)
            {
                spriteBatch.Draw(Projectile, ProjectilePos, Color.White);
            }

            //Draws the boss
            if (Boss.IsBossAlive)
            {
                Boss.Draw(spriteBatch);
            }

            //Allows the game to draw the hitboxes on each object on screen

            //draws all collidables excpet for the last one
            for (int i = 0; i < m_collidables.Count()-1; i++)
            {
                m_collidables[i].Draw(spriteBatch);
            }

            //adds the endpoint after to the boss dies
            if (!Boss.IsBossAlive)
            {
                m_collidables[m_collidables.Count() - 1].Draw(spriteBatch);
            }

            //Allows the bosses health to appear on screen
            spriteBatch.DrawString(Arial, "Boss Health: " + Boss.BossLives, BossHealthPosition, Color.White);
            spriteBatch.DrawString(Arial, "Time: " + (Time / 1000f).ToString("0.0"), new Vector2(600, 0), Color.White);

            //Allows the players health to be named on screen
            spriteBatch.DrawString(Arial, "Health ", PlayersHealthPos, Color.White);

            //Allows the game to stop drawing the sprites
            spriteBatch.End();

            //seperate area for drawing extras
            spriteBatch.Begin();

            //Draws the healthbar
            float healhbarwidth = ((float)Player.PlayerLives / 1000f) * 200f;
            spriteBatch.Draw(Healthbar1, Healthbar1Pos, Color.White);
            spriteBatch.Draw(Healthbar2, new Rectangle(65, 51, (int)healhbarwidth, 50), Color.White);

            // draw losing screen
            if (gameOverDelayTimer <= 0)
            {
                spriteBatch.Draw(PlayerDeadScreen, PlayerDeadScreenPos, Color.White);
            }

            //Draws the game end screen if the player intersects the flag
            if (EndLevel)
            {
                spriteBatch.Draw(EndScreen, EndScreenPos, Color.White);
            }

            spriteBatch.End();
        }
    }
}