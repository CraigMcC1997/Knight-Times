//using Knight_Times;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

namespace Knight_Times.Content
{
    public class Level1 : ILevel
    {
        //Loads the classes
        public Player Player;

        public bool IsDead
        {
            get { return !Player.IsPlayerAlive; }
        }

        //Loads the font
        SpriteFont Arial;
        Vector2 PlayersHealthPos;

        //Players healthbar
        Texture2D Healthbar1;
        Vector2 Healthbar1Pos;
        Texture2D Healthbar2;

        //List of the enemies 
        List<Baddies> enemies = new List<Baddies>();

        //Loads the camera class into the level for use
        private Camera Camera;

        //Defines the Height and Width of the screen
        public const int WindowWidth = 2000;
        public const int WindowHeight = 1000;

        //Timer
        float Timer = 0;

        //Background information
        Texture2D BackgroundTexture;
        Vector2 BackgroundPosition;

        //Background clouds
        Texture2D Cloud1;
        Vector2 Cloud1Pos;

        Texture2D Cloud2;
        Vector2 Cloud2Pos;

        Texture2D Cloud3;
        Vector2 Cloud3Pos;

        Texture2D Cloud4;
        Vector2 Cloud4Pos;

        Texture2D Cloud5;
        Vector2 Cloud5Pos;

        Texture2D Cloud6;
        Vector2 Cloud6Pos;

        Texture2D Cloud7;
        Vector2 Cloud7Pos;

        Texture2D Cloud8;
        Vector2 Cloud8Pos;

        //Players dies screen
        Texture2D PlayerDeadScreen;
        Vector2 PlayerDeadScreenPos;

        //Statue texture and position
        Texture2D statue;
        Vector2 statuePosition;

        //Castle texture and position
        Texture2D castle;
        Vector2 castlePosition;

        //Castle2 texture and position
        Texture2D castle2;
        Vector2 castlePosition2;

        //Hills
        Texture2D hills1;
        Vector2 hills1Position;

        Texture2D hills2;
        Vector2 hills2Position;

        //House 1 texture and position
        Texture2D House1;
        Vector2 House1Position;

        //House 2 texture and position
        Texture2D House2;
        Vector2 House2Position;

        //House 3 texture and position
        Texture2D House3;
        Vector2 House3Position;

        // PLEASE NEXT TIME USE A LIST OR ARRAY FOR YOUR COLLECTABLES!!!!!!!!

        //Coin texture and position
        Vector2 CoinsTextPos;
        Texture2D Coin;
        Vector2 CoinPos;
        Rectangle CoinHitbox;
        bool IsCoinAlive = true;

        Texture2D Coin2;
        Vector2 CoinPos2;
        Rectangle CoinHitbox2;
        bool IsCoinAlive2 = true;

        Texture2D Coin3;
        Vector2 CoinPos3;
        Rectangle CoinHitbox3;
        bool IsCoinAlive3 = true;

        Texture2D Coin4;
        Vector2 CoinPos4;
        Rectangle CoinHitbox4;
        bool IsCoinAlive4 = true;

        Texture2D Coin5;
        Vector2 CoinPos5;
        Rectangle CoinHitbox5;
        bool IsCoinAlive5 = true;

        Texture2D Coin6;
        Vector2 CoinPos6;
        Rectangle CoinHitbox6;
        bool IsCoinAlive6 = true;

        Texture2D Coin7;
        Vector2 CoinPos7;
        Rectangle CoinHitbox7;
        bool IsCoinAlive7 = true;

        Texture2D coin8;
        Vector2 coinPosition8;
        Rectangle coinHitbox8;
        bool isCoinAlive8 = true;

        //Game over delay time
        int gameOverDelayTimer = 1000;

        //Stops the next level loading
        bool EndLevel = false;
        //Ends the current level and loads next level
        public bool EndCurrentLevel
        {
            set { EndLevel = value; }
            get { return EndLevel; }
        }


        //Names the song
        Song BackgroundMusic;
        SoundEffect CoinPickup;

        //sets a list of the collidables
        List<ICollidable> m_collidables = new List<ICollidable>();

        bool playerHurting = false;
        int hurtingFlashTimer = 0;

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            gameOverDelayTimer = 1000;
            //Camera code
            Camera = new Camera
            {
                ViewportWidth = graphicsDevice.Viewport.Width,
                ViewportHeight = graphicsDevice.Viewport.Height
            };

            //Loads the font from the font sheet
            Arial = content.Load<SpriteFont>("Font");

            //The coordinates for the Players Health
            PlayersHealthPos.X = 0;
            PlayersHealthPos.Y = 0;

            //The coordinates for the Players Score
            CoinsTextPos.X = 200;
            CoinsTextPos.Y = 0;

            //Sets the zoom for the camera
            Camera.AdjustZoom(0.25f);

            //Background Texture information
            BackgroundTexture = content.Load<Texture2D>("Background");
            BackgroundPosition = new Vector2(-4000, -4000);

            //Player Dead screen information
            PlayerDeadScreen = content.Load<Texture2D>("PlayerDead");

            //FIX THIS OI FIX IT GET IT FIXED OI OI OI FIXY TIME BEFORE U FORGET AND NOT GET IT FIXED LIKE YOU NEED IT TO BE
            PlayerDeadScreenPos = new Vector2(2000, 500);

            //Loads texture for the healthbars from the content pipeline
            Healthbar1 = content.Load<Texture2D>("NotMovingHealthBar");
            Healthbar2 = content.Load<Texture2D>("HealthBar");

            //Healthbar1 position
            Healthbar1Pos = new Vector2(0, 50);

            //Loads the content from the classes
            //Player
            Player = new Player(content, new Vector2(2270, 1750));

            //Adding an enemy from the baddies class and giving it a position //speed, health, damage
            enemies.Clear();
            enemies.Add(new Baddies(content, "Heavy", new Vector2(8000, 1744), 1.5f, 100, 4));
            enemies.Add(new Baddies(content, "Heavy", new Vector2(9000, 1744), 1f, 100, 4));
            enemies.Add(new Baddies(content, "Heavy", new Vector2(10000, 1744), 0.7f, 100, 4));
            enemies.Add(new Baddies(content, "Sheep", new Vector2(7000, 1824), 5f, 50, 3));
            enemies.Add(new Baddies(content, "Sheep", new Vector2(7500, 1824), 4f, 50, 3));
            enemies.Add(new Baddies(content, "Sheep", new Vector2(8000, 1824), 3f, 50, 3));

            //Floor
            m_collidables.Clear();
            var floorPlatform = new FloorPlatform(content, new Vector2(2000, 1904));
            m_collidables.Add(floorPlatform);

            floorPlatform = new FloorPlatform(content, new Vector2(4000, 1904));
            m_collidables.Add(floorPlatform);

            floorPlatform = new FloorPlatform(content, new Vector2(6000, 1904));
            m_collidables.Add(floorPlatform);

            floorPlatform = new FloorPlatform(content, new Vector2(1000, 1904));
            m_collidables.Add(floorPlatform);

            floorPlatform = new FloorPlatform(content, new Vector2(8000, 1904));
            m_collidables.Add(floorPlatform);

            floorPlatform = new FloorPlatform(content, new Vector2(10000, 1904));
            m_collidables.Add(floorPlatform);

            //Platforms
            //SET OF 3
            var Level1Platform = new XHitbox(content, new Vector2(2174, 1200));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(2302, 1200));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(2430, 1200));
            m_collidables.Add(Level1Platform);
            //SET OF 3
            Level1Platform = new XHitbox(content, new Vector2(2830, 1480));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(2712, 1480));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(2584, 1480));
            m_collidables.Add(Level1Platform);
            //SET OF 4
            Level1Platform = new XHitbox(content, new Vector2(3260, 1430));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(3132, 1430));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(3004, 1430));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(3860, 1600));
            m_collidables.Add(Level1Platform);
            //SET OF 2
            Level1Platform = new XHitbox(content, new Vector2(6050, 1650));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(6556, 1450));
            m_collidables.Add(Level1Platform);
            //SET OF 4
            Level1Platform = new XHitbox(content, new Vector2(6200, 1450));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(6328, 1450));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(6456, 1450));
            m_collidables.Add(Level1Platform);
            //SET OF 4
            Level1Platform = new XHitbox(content, new Vector2(7428, 1450));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(7556, 1450));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(7656, 1450));
            m_collidables.Add(Level1Platform);

            Level1Platform = new XHitbox(content, new Vector2(7756, 1450));
            m_collidables.Add(Level1Platform);

            //Castle
            castle = content.Load<Texture2D>("Castle");
            castlePosition = new Vector2(2000, 1045);

            castle2 = content.Load<Texture2D>("Castle2");
            castlePosition2 = new Vector2(2850, 1171);

            //statue
            statue = content.Load<Texture2D>("Statue");
            statuePosition = new Vector2(3800, 1540);

            //House1
            House1 = content.Load<Texture2D>("House1");
            House1Position = new Vector2(4500, 1350);

            //House2
            House2 = content.Load<Texture2D>("House2");
            House2Position = new Vector2(6000, 950);

            //House3
            House3 = content.Load<Texture2D>("House3");
            House3Position = new Vector2(8460, 1120);

            //Coins
            Coin = content.Load<Texture2D>("Coin");
            CoinPos = new Vector2(2250, 1110);

            Coin2 = content.Load<Texture2D>("Coin");
            CoinPos2 = new Vector2(2600, 1400);

            Coin3 = content.Load<Texture2D>("Coin");
            CoinPos3 = new Vector2(3900, 1520); 

            Coin4 = content.Load<Texture2D>("Coin");
            CoinPos4 = new Vector2(6060, 1560); 

            Coin5 = content.Load<Texture2D>("Coin");
            CoinPos5 = new Vector2(6600, 1350);

            Coin6 = content.Load<Texture2D>("Coin");
            CoinPos6 = new Vector2(7450, 1350); 

            Coin7 = content.Load<Texture2D>("Coin");
            CoinPos7 = new Vector2(10100, 1470);

            coin8 = content.Load<Texture2D>("Coin");
            coinPosition8 = new Vector2(3000, 100);

            //Hitboxs for the coins
            CoinHitbox = new Rectangle((int)CoinPos.X, (int)CoinPos.Y, Coin.Width, Coin.Height);
            CoinHitbox2 = new Rectangle((int)CoinPos2.X, (int)CoinPos2.Y, Coin2.Width, Coin2.Height);
            CoinHitbox3 = new Rectangle((int)CoinPos3.X, (int)CoinPos3.Y, Coin3.Width, Coin3.Height);
            CoinHitbox4 = new Rectangle((int)CoinPos4.X, (int)CoinPos4.Y, Coin4.Width, Coin4.Height);
            CoinHitbox5 = new Rectangle((int)CoinPos5.X, (int)CoinPos5.Y, Coin5.Width, Coin5.Height);
            CoinHitbox6 = new Rectangle((int)CoinPos6.X, (int)CoinPos6.Y, Coin6.Width, Coin6.Height);
            CoinHitbox7 = new Rectangle((int)CoinPos7.X, (int)CoinPos7.Y, Coin7.Width, Coin7.Height);
            coinHitbox8 = new Rectangle((int)coinPosition8.X, (int)coinPosition8.Y, coin8.Width, coin8.Height);

            IsCoinAlive = true;
            IsCoinAlive2 = true;
            IsCoinAlive3 = true;
            IsCoinAlive4 = true;
            IsCoinAlive5 = true;
            IsCoinAlive6 = true;
            IsCoinAlive7 = true;
            isCoinAlive8 = true;

            //Hills
            hills1 = content.Load<Texture2D>("hills1");
            hills1Position = new Vector2(4000, 1395);

            hills2 = content.Load<Texture2D>("hills2");
            hills2Position = new Vector2(1000, 1395);

            //Clouds
            Cloud1 = content.Load<Texture2D>("Cloud1");
            Cloud1Pos = new Vector2(9000, 1200);

            Cloud2 = content.Load<Texture2D>("Cloud1");
            Cloud2Pos = new Vector2(11000, 1300);

            Cloud3 = content.Load<Texture2D>("Cloud1");
            Cloud3Pos = new Vector2(8040, 1400);

            Cloud4 = content.Load<Texture2D>("Cloud1");
            Cloud4Pos = new Vector2(14000, 1470);

            Cloud5 = content.Load<Texture2D>("Cloud1");
            Cloud5Pos = new Vector2(8500, 1590);

            Cloud6 = content.Load<Texture2D>("Cloud1");
            Cloud6Pos = new Vector2(7846, 1109);

            Cloud7 = content.Load<Texture2D>("Cloud1");
            Cloud7Pos = new Vector2(11960, 1407);

            Cloud8 = content.Load<Texture2D>("Cloud1");
            Cloud8Pos = new Vector2(9574, 1389);

            //Walls
            var wall = new Wall(content, new Vector2(2000, 1244));
            m_collidables.Add(wall);

            wall = new Wall(content, new Vector2(11000, 1244));
            m_collidables.Add(wall);

            //End
            var endpoint = new EndPoint(content, new Vector2(10700, 1744));
            m_collidables.Add(endpoint);

            //Loads the background music
            BackgroundMusic = content.Load<Song>("Sköll");

            //Loads the sound file for collecting the coins 
            CoinPickup = content.Load<SoundEffect>("CoinPickup");

            //Plays the song
            MediaPlayer.Play(BackgroundMusic);

            //PLaces the player in the center of the camera
            Camera.CenterOn(Player.PlayerPosition);
        }


        public void Update(GameTime gameTime, ref float TimeTaken)
        {
            //For controller support
            GamePadState padState1 = GamePad.GetState(PlayerIndex.One);

            //Time between updates (used by enemies)
            float timebetweenupdates = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Jumping code
            var originalPosition = Player.PlayerPosition;

            //Moves the player using predefined keys
            if (Player.IsPlayerAlive)
            {
                Player.MoveMe(Keyboard.GetState(), timebetweenupdates);
            }


            //Player attacks if they player is alive and the enter key is pressed or X on the gamepad
            if ((Keyboard.GetState().IsKeyDown(Keys.Enter) || padState1.Buttons.X == ButtonState.Pressed) && Player.IsPlayerAlive )
            {
                Player.attack();
            }

            //Player attacking the enemy when enter is pressed and the enemy is near the player
            playerHurting = false;
            foreach (Baddies enemy in enemies)
            {
                if (enemy.Hitbox.Intersects(Player.Hitbox) && Player.isattacking)
                {
                    enemy.TakeDamage();
                }

                //If the enemy collides with the player, player and enemy take damage
                if (Player.Hitbox.Intersects(enemy.Hitbox) && enemy.isattacking)
                {
                    Player.PlayerLives--;
                    playerHurting = true;
                }

                //Updates the enemies
                enemy.update(Player, timebetweenupdates);
            }

            //Kills the player if the health is less that 0
            if (Player.PlayerLives < 0)
            {
                Player.IsPlayerAlive = false;

            //Kills the player if the health is less that 0
                gameOverDelayTimer -= gameTime.ElapsedGameTime.Milliseconds;
             }

            //Kills the enemy if the health is less that 0
            for (int i = enemies.Count - 1; i >= 0; --i)
            {
                if (enemies[i].Health < 0)
                {
                    enemies[i].IsEnemyAlive = false;
                    enemies.RemoveAt(i);
                    TimeTaken -= 3500;
                }
            }


            //resetting the Player position if it goes off screen on the Y axis
            if (Player.PlayerPosition.Y > WindowHeight +1500)
            {
                Player.PlayerLives -= 2;
            }
            if (Player.PlayerPosition.Y > WindowHeight + 3000)
            {
                Player.TempPlayerPosition = new Vector2(1650,800);
            }

            //Makes the game update all the hitboxes for ojects on screen
            Player.Update(gameTime, m_collidables);

            //Changing Level
            var endpoint = m_collidables.FirstOrDefault(x => x.CollisionType == CollidableType.Endpoint);

            //Lets the game load the next level if the two objects collide
            if (Player.Hitbox.Intersects(endpoint.Hitbox) && Player.Playerscore >= 7)
            {
                EndLevel = true;
            }


            // If these were in a list then you could just do a FOR LOOP and does this code once
            //Picking up the coins
            if (Player.Hitbox.Intersects(CoinHitbox) && IsCoinAlive)
            {
                Player.Playerscore++;
                IsCoinAlive = false;
                CoinPickup.Play();
            }

            if (Player.Hitbox.Intersects(CoinHitbox2) && IsCoinAlive2)
            {
                Player.Playerscore++;
                IsCoinAlive2 = false;
                CoinPickup.Play();
            }

            if (Player.Hitbox.Intersects(CoinHitbox3) && IsCoinAlive3)
            {
                Player.Playerscore++;
                IsCoinAlive3 = false;
                CoinPickup.Play();
            }

            if (Player.Hitbox.Intersects(CoinHitbox4) && IsCoinAlive4)
            {
                Player.Playerscore++;
                IsCoinAlive4 = false;
                CoinPickup.Play();
            }

            if (Player.Hitbox.Intersects(CoinHitbox5) && IsCoinAlive5)
            {
                Player.Playerscore++;
                IsCoinAlive5 = false;
                CoinPickup.Play();
            }

            if (Player.Hitbox.Intersects(CoinHitbox6) && IsCoinAlive6)
            {
                Player.Playerscore++;
                IsCoinAlive6 = false;
                CoinPickup.Play();
            }

            if (Player.Hitbox.Intersects(CoinHitbox7) && IsCoinAlive7)
            {
                Player.Playerscore++;
                IsCoinAlive7 = false;
                CoinPickup.Play();
            }

            if (Player.Hitbox.Intersects(coinHitbox8) && isCoinAlive8)
            {
                Player.Playerscore++;
                isCoinAlive8 = false;
                CoinPickup.Play();
            }

            //Adding to the timer int to allow the Light enemy to move
            Timer += timebetweenupdates;

            //Moving the clouds across the screen from right to left
            Cloud1Pos.X --;
            Cloud2Pos.X -=1.5f;
            Cloud3Pos.X -=1;
            Cloud4Pos.X -= 1.3f;
            Cloud5Pos.X -= 1.6f;
            Cloud6Pos.X -=1.8f;
            Cloud7Pos.X -= 1.2f;
            Cloud8Pos.X -= 1.1f;

            //PLaces the player in the center of the camera
            Camera.CenterOn(Player.PlayerPosition);
        }

        public void Draw(SpriteBatch spriteBatch, float TimeTaken)
        {
            //The transform matrix used by the camera
            spriteBatch.Begin(transformMatrix: Camera.TranslationMatrix);

            //Draws the background
            spriteBatch.Draw(BackgroundTexture, Vector2.Zero, Color.White);


            //Draws the clouds
            spriteBatch.Draw(Cloud1, Cloud1Pos, Color.White);
            spriteBatch.Draw(Cloud2, Cloud2Pos, Color.White);
            spriteBatch.Draw(Cloud3, Cloud3Pos, Color.White);
            spriteBatch.Draw(Cloud4, Cloud4Pos, Color.White);
            spriteBatch.Draw(Cloud5, Cloud5Pos, Color.White);
            spriteBatch.Draw(Cloud6, Cloud6Pos, Color.White);
            spriteBatch.Draw(Cloud7, Cloud7Pos, Color.White);
            spriteBatch.Draw(Cloud8, Cloud8Pos, Color.White);

            //Draws the hills
            spriteBatch.Draw(hills1, hills1Position, Color.White);
            spriteBatch.Draw(hills2, hills2Position, Color.White);

            //Draws House1
            spriteBatch.Draw(House1, House1Position, Color.White);

            //Draws house 2
            spriteBatch.Draw(House2, House2Position, Color.White);

            //Draws house 3
            spriteBatch.Draw(House3, House3Position, Color.White);

            //Draws the statue
            spriteBatch.Draw(statue, statuePosition, Color.White);

            //Draws the castle
            spriteBatch.Draw(castle2, castlePosition2, Color.White);

            //Draws the enemies if they are alive
            foreach (Baddies enemy in enemies)
            {
                if (enemy.IsEnemyAlive)
                {
                    enemy.Draw(spriteBatch);
                }
            }

            //Draws the coins
            if (IsCoinAlive)
            {
                spriteBatch.Draw(Coin, CoinPos, Color.White);
            }

            if (IsCoinAlive2)
            {
                spriteBatch.Draw(Coin2, CoinPos2, Color.White);
            }

            if (IsCoinAlive3)
            {
                spriteBatch.Draw(Coin3, CoinPos3, Color.White);
            }

            if (IsCoinAlive4)
            {
                spriteBatch.Draw(Coin4, CoinPos4, Color.White);
            }

            if (IsCoinAlive5)
            {
                spriteBatch.Draw(Coin5, CoinPos5, Color.White);
            }

            if (IsCoinAlive6)
            {
                spriteBatch.Draw(Coin6, CoinPos6, Color.White);
            }

            if (IsCoinAlive7)
            {
                spriteBatch.Draw(Coin7, CoinPos7, Color.White);
            }

            if(isCoinAlive8)
            {
                spriteBatch.Draw(coin8, coinPosition8, Color.White);
            }

            //Draws the player
            if (Player.IsPlayerAlive)
            {
                if (playerHurting)
                {
                    hurtingFlashTimer += 1;
                    if (hurtingFlashTimer < 15)
                    {
                        Player.Draw(spriteBatch, Color.Red);
                    }
                    else
                    {
                        Player.Draw(spriteBatch, Color.White);
                        if (hurtingFlashTimer > 30) hurtingFlashTimer = 0;
                    }
                }
                else
                {
                    Player.Draw(spriteBatch);
                }
            }

            //Draws the platforms
            foreach (var Level1Platform in m_collidables)
            {
                Level1Platform.Draw(spriteBatch);
            }

            //Draws the castle
            spriteBatch.Draw(castle, castlePosition, Color.White);

            //Allows the game to draw the hitboxes on each object on screen
            foreach (var platform in m_collidables)
            {
                platform.Draw(spriteBatch);
            }

            //Allows the game to stop drawing the sprites
            spriteBatch.End();
            
            //seperate area for drawing text
            spriteBatch.Begin();

            //Allows the players health to be named on screen
            spriteBatch.DrawString(Arial, "Health ", PlayersHealthPos, Color.White);

            //Allows the players score to display on screen
            spriteBatch.DrawString(Arial, "Coins Collected: " + Player.Playerscore, CoinsTextPos, Color.White);
            spriteBatch.DrawString(Arial, "Time: " + (TimeTaken / 1000f).ToString("0.0"), new Vector2(600,0), Color.White);

            //Draws the healthbar
            float healhbarwidth = ((float)Player.PlayerLives / 1000f) * 200f;
            spriteBatch.Draw(Healthbar1, Healthbar1Pos ,Color.White);
            spriteBatch.Draw(Healthbar2, new Rectangle(100, 70, (int)healhbarwidth, 50), Color.White);

            // draw losing screen
            if (!Player.IsPlayerAlive && gameOverDelayTimer <= 0)
            {
                spriteBatch.Draw(PlayerDeadScreen, new Rectangle(0,0, WindowWidth,WindowHeight), Color.White);
            }

            spriteBatch.End();
        }
    }
}