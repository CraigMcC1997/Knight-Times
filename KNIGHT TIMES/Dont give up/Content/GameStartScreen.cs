 using Knight_Times;
using Microsoft.Xna.Framework;
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

        public bool IsDead
        {
            get { return false; }
        }

        //Defines the Height and Width of the screen
        public const int WindowWidth = 2000;
        public const int WindowHeight = 1000;

        public bool GameScreen1 = true;
        bool GameScreen2 = false;
        bool GameScreen3 = false;

        //Time between when the player is allowed to press the button
        int Timer = 1000;
        int Timer2 = 1000;
        int Timer3 = 1000;

        //Information for the 3 images
        Texture2D background;
        Vector2 backgroundPos;

        Texture2D background2;
        Vector2 background2Pos;

        Texture2D background3;
        Vector2 background3Pos;

        //Stops the next level loading
        bool EndLevel = false;
        //Ends the current level and loads next level
        public bool EndCurrentLevel
        {
            set { EndLevel = value; }
            get { return EndLevel; }
        }

        //Loads the player classs
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

            //adjusts the camera
            Camera.AdjustZoom(0.25f);

            //loads the 3 images from the content pipeline and gives them a position
            background = content.Load<Texture2D>("menubackgroundcopy");
            backgroundPos = new Vector2(0, 0);

            background2 = content.Load<Texture2D>("Story");
            background2Pos = new Vector2(190, 75);

            background3 = content.Load<Texture2D>("Controls");
            background3Pos = new Vector2(180, 50);

            //Uses player for the camera
            Player = new Player(content, new Vector2(970, 740));

            //centers the camera on the player
            Camera.CenterOn(Player.PlayerPosition);

            //Loads the content from the classes
             GameScreen1 = true;
       GameScreen2 = false;
        GameScreen3 = false;

        //Time between when the player is allowed to press the button
        Timer = 1000;
        Timer2 = 1000;
        Timer3 = 1000;
    }

        //PlayerScore
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

            //Starts a countdown
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

            //Loads the next level when enter is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && Timer <= 0 || padState1.Buttons.A == ButtonState.Pressed && Timer <= 0)
            {
                GameScreen1 = false;
                GameScreen2 = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && (GameScreen2) && Timer2 <= 0 || padState1.Buttons.A == ButtonState.Pressed && (GameScreen2) && Timer2 <= 0)
            {
                GameScreen2 = false;
                GameScreen3 = true;
            }

            //Loads the next level when enter is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && (GameScreen3) && Timer3 <= 0 || padState1.Buttons.A == ButtonState.Pressed && (GameScreen3) && Timer3 <= 0)
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

            //Allows the game to draw the hitboxes on each object on screen
            foreach (var platform in m_collidables)
            {
                platform.Draw(spriteBatch);
            }

            Player.Draw(spriteBatch);

            if (GameScreen1)
            {
                spriteBatch.Draw(background, backgroundPos, Color.White);
            }

            if (GameScreen2)
            {
                spriteBatch.Draw(background2, background2Pos, Color.White);
            }

            if (GameScreen3)
            {
                spriteBatch.Draw(background3, background3Pos, Color.White);
            }

            //Allows the game to stop drawing the sprites
            spriteBatch.End();
        }
    }
}