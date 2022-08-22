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

        //Sets a boolean to check if player is dead or alive
        public bool IsDead
        {
            get { return !Player.IsPlayerAlive; }
        }

        //Loads the boss class into the boss level
        Boss1 Boss;

        //Texture for the Projectile
        Texture2D Projectile;

        //Position for the Projectile
        Vector2 ProjectilePos;

        //hitbox for the Projectile
        Rectangle ProjectileHitbox;

        //velocity for the Projectile
        Vector2 ProjectileVel;

        //speed for the Projectile
        float ProjectileSpeed = 30;

        //Healthbar Texture
        Texture2D Healthbar1;

        //Healthbar Position
        Vector2 Healthbar1Pos;

        //Part of the healthbar that shrinks when the player takes damage
        Texture2D Healthbar2;

        //Texture for Flag
        Texture2D Flag;
        //Position for flag
        Vector2 FlagPos;
        //Hitbox for the flag
        Rectangle FlagHitbox;

        //Timer
        float Timer = 0;

        //Loads the font code
        SpriteFont Arial;
        //BossHealth Position
        Vector2 BossHealthPosition;
        //PlayerHealth Position
        Vector2 PlayersHealthPos;

        //Background Texture
        Texture2D BackgroundTexture;
        //Background Position
        Vector2 BackgroundPosition;

        //EndScreen Texture
        Texture2D EndScreen;
        //Endscreen Position
        Vector2 EndScreenPos;

        //PlayerLose Texture
        Texture2D PlayerDeadScreen;
        //PlayerLose Position
        Vector2 PlayerDeadScreenPos;

        //Sound effects
        SoundEffect BossAttack, BossDeath, VictoryTheme;

        //Stops the next level loading by starting the boolean false
        bool EndLevel = false;

        //Ends the current level and loads next level if the boolean is set to true at any point
        public bool EndCurrentLevel
        {
            set { EndLevel = value; }
            get { return EndLevel; }
        }

        //sets a list of the collidables
        List<ICollidable> m_collidables = new List<ICollidable>();

        //Display width for the game using the viewport
        int displaywidth;
        int displayheight;

        //int used for time between the player dying and the game over screen loading
        int gameOverDelayTimer = 1000;

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            gameOverDelayTimer = 1000;

            displaywidth = graphicsDevice.Viewport.Width;
            displayheight = graphicsDevice.Viewport.Height;

            //Loads the background image from the content pipeline
            BackgroundTexture = content.Load<Texture2D>("Textures/BackGround2");
            //Gives the background a position
            BackgroundPosition = new Vector2(0, 0);

            //Loads the end screen image from the content pipeline
            EndScreen = content.Load<Texture2D>("Textures/Win");
            //Gives the end screen a position
            EndScreenPos = new Vector2(0, 0);

            //Loads the Players death screen image from the content pipeline
            PlayerDeadScreen = content.Load<Texture2D>("Textures/PlayerDead");
            //Gives the Players death screen a position
            PlayerDeadScreenPos = new Vector2(0, 0);

            //Loads texture for the healthbar from the content pipeline
            Healthbar1 = content.Load<Texture2D>("Textures/NotMovingHealthBar");
            //Gives the healthbar2 a position
            Healthbar2 = content.Load<Texture2D>("Textures/HealthBar");

            //Loads the texture for the Flag from the pipeline
            Flag = content.Load<Texture2D>("Textures/BossLevelEndpoint");
            //Gives the flag a position
            FlagPos = new Vector2(1400, 744);

            //Hitboxs for the flag
            FlagHitbox = new Rectangle((int)FlagPos.X, (int)FlagPos.Y, Flag.Width, Flag.Height);

            //Healthbar1 position
            Healthbar1Pos = new Vector2(0, 50);

            //player position
            Player = new Player(content, new Vector2(0, 756));

            //Boss position
            Boss = new Boss1(content, new Vector2(1300, 400));

            //defines what happens if the boss is alive
            if (Boss.IsBossAlive)
            {
                //Load the Projectile image from the pipeline if the boss is alive
                Projectile = content.Load<Texture2D>("Textures/Projectile");
                //Gives the Projectile a starting position if the boss is alive
                ProjectilePos.X = -2000;
                ProjectilePos.Y = -1000;
            }

            //Loads the platforms image from the pipeline and gives them a position and adds them to the collidables list
            var Platform = new Platform(content, new Vector2(0, 200));
            m_collidables.Add(Platform);

            Platform = new Platform(content, new Vector2(1100, 550));
            m_collidables.Add(Platform);

            Platform = new Platform(content, new Vector2(500, 550));
            m_collidables.Add(Platform);

            //Loads the platform images that arent being drawn and gives them a position and adds them to the collidables list
            var Level1Platform = new XHitbox(content, new Vector2(0, 840));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(128, 840));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(168, 840));
            m_collidables.Add(Level1Platform);

            //Loads texture for the healthbar from the content pipeline
            Healthbar2 = content.Load<Texture2D>("Textures/Healthbar");

            //Loads the font from the font sheet
            Arial = content.Load<SpriteFont>("Fonts/Font");

            //The coordinates for the Bosses Health displaying on screen
            BossHealthPosition.X = displaywidth / 2 - 150;
            BossHealthPosition.Y = 0;

            //The coordinates for the Players Health displaying on screen
            PlayersHealthPos.X = 0;
            PlayersHealthPos.Y = 0;

            //Loads the sound effects from the content pipeline
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

            //Player attacks if they player is alive and the enter key/ X button is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || padState1.Buttons.X == ButtonState.Pressed && Player.IsPlayerAlive)
            {
                Player.attack();
            }

            //all things that happen if the boss is alive
            if (Boss.IsBossAlive)
            {
                //Creates an int that will be used for the range between the player and the boss
                int range = 1000;
                // boss fires the projectile when within range of the player
                Vector2 distance = Player.PlayerPosition - new Vector2(Boss.Position.X, Boss.Position.Y - 100);
                if (distance.Length() < 550 && (ProjectilePos.X < -range || ProjectilePos.Y < -range || ProjectilePos.Y > displayheight + range) && Player.IsPlayerAlive)
                {
                    //noramalise makes the number generated positive
                    distance.Normalize();
                    //Makes the velocity of the projectile the distance multiplied by the speed
                    ProjectileVel = distance * ProjectileSpeed;
                    //gives the projectile a position
                    ProjectilePos = new Vector2(Boss.Position.X, Boss.Position.Y - 100);
                    //Plays a sound effect when the projectile spawns on screen
                    BossAttack.Play();
                }

                //TO HELP LESS ABLED BODIED PLAYERS
                if (Keyboard.GetState().IsKeyDown(Keys.Add) && Boss.BossLives > 50)
                {
                    Boss.BossLives -= 50;
                }

                ////Daves cheat cause I don't know how to kill nessie
                ////really dave, cheat codes? I see how it is dave
                //if (Keyboard.GetState().IsKeyDown(Keys.K))
                //    Boss.BossLives--;

                //Moving the bosses projectile
                ProjectilePos += ProjectileVel;

                //Hitbox for the bosses projectile
                ProjectileHitbox = new Rectangle((int)ProjectilePos.X, (int)ProjectilePos.Y, Projectile.Width, Projectile.Height);

                //Player taking damage when the boss intersects the player and if the boss is alive
                if (Player.Hitbox.Intersects(Boss.Hitbox) && Boss.IsBossAlive)
                {
                    Player.PlayerLives--;
                }

                //If projectile intersects player, player loses life
                if (Player.Hitbox.Intersects(ProjectileHitbox))
                {
                    Player.PlayerLives -= 10;
                }

                //Code for how boss loses life
                if (Player.Hitbox.Intersects(Boss.Hitbox) && (Keyboard.GetState().IsKeyDown(Keys.Enter) || padState1.Buttons.X == ButtonState.Pressed && Player.IsPlayerAlive))
                {
                    Boss.CheckBossDamage();
                }

                //Code for killing boss if the lives are equal to or below 0
                if (Boss.BossLives <= 0)
                {
                    Boss.IsBossAlive = false;
                    //Plays a sound effectwhen the boss dies
                    BossDeath.Play();
                }
            }
            else
            {
                //Changing Level
                var endpoint = m_collidables.FirstOrDefault(x => x.CollisionType == CollidableType.Endpoint);

                //If the player intersects the endpoint set the end level boolean to true
                if (Player.Hitbox.Intersects(FlagHitbox))
                {
                    EndLevel = true;

                    //Plays a sound effect when the level ends
                    VictoryTheme.Play(0.6f, 0f, 0f);
                }
            }

            //Kills the player if the health is less that 0
            if (Player.PlayerLives < 0)
            {
                Player.IsPlayerAlive = false;

                //begins the game over timer
                gameOverDelayTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }

            //resetting the Player position if it goes off screen on the Y axis
            if (Player.PlayerPosition.Y > displayheight + 500)
            {
                //players loses health
                Player.PlayerLives -= 2;
            }
            //resets the players position
            if (Player.PlayerPosition.Y > displayheight + 1000)
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

            //Draws the player if the player is alive
            if (Player.IsPlayerAlive)
            {
                Player.Draw(spriteBatch);
            }

            //Draws bullet if the bullet is alive 
            if (Boss.IsBossAlive)
            {
                spriteBatch.Draw(Projectile, ProjectilePos, Color.White);
            }

            //Draws the boss if the boss is alive
            if (Boss.IsBossAlive)
            {
                Boss.Draw(spriteBatch);
            }

            //draws all collidables excpet for the last one
            for (int i = 0; i < m_collidables.Count() - 1; i++)
            {
                m_collidables[i].Draw(spriteBatch);
            }

            //adds the endpoint after to the boss dies
            if (!Boss.IsBossAlive)
            {
                spriteBatch.Draw(Flag, FlagHitbox, Color.White);
            }

            //Allows the bosses health to appear on screen
            spriteBatch.DrawString(Arial, "Boss Health: " + Boss.BossLives, BossHealthPosition, Color.White);

            //Allows the timer to draw on screen
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
            spriteBatch.Draw(Healthbar2, new Rectangle(100, 70, (int)healhbarwidth, 50), Color.White);

            //draw losing screen when the game timer is equal or less than 0
            if (gameOverDelayTimer <= 0)
            {
                spriteBatch.Draw(PlayerDeadScreen, PlayerDeadScreenPos, Color.White);
            }

            //Draws the game end screen if the player intersects the flag and the endlevel bool is true
            if (EndLevel)
            {
                spriteBatch.Draw(EndScreen, EndScreenPos, Color.White);
            }

            spriteBatch.End();
        }
    }
}