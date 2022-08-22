using Knight_Times;
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
    public class GameStartScreen : ILevel
    {
        //Loads the camera class into the level for use
        private Camera Camera;

        //Checks for if the player is dead or alive
        public bool IsDead
        {
            get { return false; }
        }

        //Defines the Height and Width of the screen
        public const int WindowWidth = 2000;
        public const int WindowHeight = 1000;

        //3 booleans to see if the gamescreens have to be drawn or not
        bool GameScreen1 = true;
        bool GameScreen2 = false;
        bool GameScreen3 = false;

        //time between when the player is allowed to press the button
        int Timer = 1000;
        int Timer2 = 1000;
        int Timer3 = 1000;

        //Texture for background1
        Texture2D background;
        //Position for background1
        Vector2 backgroundPos;

        //Texture for background2
        Texture2D background2;
        //Position for background2
        Vector2 background2Pos;

        //Texture for background3
        Texture2D background3;
        //Position for background3
        Vector2 background3Pos;

        //Stops the next level loading
        bool EndLevel = false;

        //Sound effects
        Song BackGroundSong;

        //Ends the current level and loads next level
        public bool EndCurrentLevel
        {
            set { EndLevel = value; }
            get { return EndLevel; }
        }

        //Loads the player class
        Player Player;

        //sets a list of the collidables
        List<ICollidable> m_collidables = new List<ICollidable>();

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            //Camera code
            Camera = new Camera
            {
                ViewportWidth = graphicsDevice.Viewport.Width,
                ViewportHeight = graphicsDevice.Viewport.Height
            };

            //adjusts the camera zoom
            Camera.AdjustZoom(0.25f);

            //Loads background1 image from the content pipeline
            background = content.Load<Texture2D>("Textures/menubackgroundcopy");
            //Gives background1 a position
            backgroundPos = new Vector2(0, 0);

            //Loads background2 image from the content pipeline
            background2 = content.Load<Texture2D>("Textures/Story");
            //Gives background2 a position
            background2Pos = new Vector2(170, 75);

            //Loads background3 image from the content pipeline
            background3 = content.Load<Texture2D>("Textures/Controls");
            //Gives background3 a position
            background3Pos = new Vector2(170, 50);

            //Uses player to position the camera
            Player = new Player(content, new Vector2(970, 740));

            //Loads the background music
            BackGroundSong = content.Load<Song>("SoundFiles/Song2");

            //Plays the song
            MediaPlayer.Play(BackGroundSong);

            //centers the camera on the player
            Camera.CenterOn(Player.PlayerPosition);
        }

        //PlayerScore returns 0 as the score doesnt start until level 1
        public int GetPlayerScore()
        {
            return 0;
        }

        public void Update(GameTime gameTime, ref float TimeTaken)
        {
            //For controller support
            GamePadState padState1 = GamePad.GetState(PlayerIndex.One);

            //Time between updates (used by enemies)
            float timebetweenupdates = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Starts a countdown using game time
            Timer -= gameTime.ElapsedGameTime.Milliseconds;

            //Starts a countdown when the second screen is drawing
            if (GameScreen2)
            {
                Timer2 -= gameTime.ElapsedGameTime.Milliseconds;
            }

            //Starts a countdown when the third screen is drawing
            if (GameScreen3)
            {
                Timer3 -= gameTime.ElapsedGameTime.Milliseconds;
            }

            //Loads the next level when enter/A is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && Timer <= 0 || padState1.Buttons.Start == ButtonState.Pressed && Timer <= 0)
            {
                GameScreen1 = false;
                GameScreen2 = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && (GameScreen2) && Timer2 <= 0 || padState1.Buttons.Start == ButtonState.Pressed && (GameScreen2) && Timer2 <= 0)
            {
                GameScreen2 = false;
                GameScreen3 = true;
            }

            //Loads the next level when enter is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && (GameScreen3) && Timer3 <= 0 || padState1.Buttons.Start == ButtonState.Pressed && (GameScreen3) && Timer3 <= 0)
            {
                EndLevel = true;
            }

            //Changing Level
            var endpoint = m_collidables.FirstOrDefault(x => x.CollisionType == CollidableType.Endpoint);
        }

        public void Draw(SpriteBatch spriteBatch, float Time)
        {
            //The transform matrix used by the camera
            spriteBatch.Begin(transformMatrix: Camera.TranslationMatrix);

            //Draws each platform from the platform class
            foreach (var platform in m_collidables)
            {
                platform.Draw(spriteBatch);
            }

            //Draws the player
            Player.Draw(spriteBatch);

            //Draws the background if the boolean for screen1 is true
            if (GameScreen1)
            {
                spriteBatch.Draw(background, backgroundPos, Color.White);
            }

            //Draws the background if the boolean for screen2 is true
            if (GameScreen2)
            {
                spriteBatch.Draw(background2, background2Pos, Color.White);
            }

            //Draws the background if the boolean for screen3 is true
            if (GameScreen3)
            {
                spriteBatch.Draw(background3, background3Pos, Color.White);
            }

            //Allows the game to stop drawing the sprites
            spriteBatch.End();
        }
    }
}