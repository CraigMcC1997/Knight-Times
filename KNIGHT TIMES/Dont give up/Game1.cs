using Knight_Times.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.IO;
using KeybordFunctions;

namespace Knight_Times
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Loads the font
        SpriteFont Arial;


        //Loads the levels
        int m_currentLevel = 0;
        List<ILevel> Levels = new List<ILevel>();

        //Timer for game score
        public float TimeTaken = 0;

        KeyboardState lastkeystate;
        Boolean keyboardreleased = true;
        float keycounter = 0;           // Counter for delay between key strokes
        const float keystrokedelay = 200;   // Delay between key strokes in milliseconds

        const int numberofhighscores = 10;                              // Number of high scores to store
        float[] highscores = new float[numberofhighscores];                 // Array of high scores
        string[] highscorenames = new string[numberofhighscores];       // Array of high score names
        const int maxnamelength = 30;   // Maximum name length for high score table
        int lasthighscore = numberofhighscores - 1;
        Boolean newhighscore = false;



        //Defines the Height and Width of the screen
        public const int WindowWidth = 2000;
        public const int WindowHeight = 1000;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            Content.RootDirectory = "Content";

            //Shows the mouse on top of the game screen
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //List of all the levels in the game
            Levels.Add(new GameStartScreen());
            //Levels.Add(new GameStartScreen2());
            Levels.Add(new Level1());
            Levels.Add(new BossLevel1());

            //Loads the font from the font sheet
            Arial = Content.Load<SpriteFont>("Font");

            // Load in high scores
            if (File.Exists(@"highscore.txt")) // This checks to see if the file exists
            {
                StreamReader sr = new StreamReader(@"highscore.txt");	// Open the file

                String line;		// Create a string variable to read each line into
                for (int i = 0; i < numberofhighscores && !sr.EndOfStream; i++)
                {
                    line = sr.ReadLine();	// Read the first line in the text file
                    highscorenames[i] = line.Trim(); // Read high score name

                    if (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();	// Read the first line in the text file
                        line = line.Trim(); 	// This trims spaces from either side of the text
                        highscores[i] = (float)Convert.ToDecimal(line);	// This converts line to numeric
                    }
                }
                sr.Close();			// Close the file
            }
            // SORT HIGH SCORE TABLE
            Array.Sort(highscores, highscorenames);

            //Loads the levels in order assigned
            Levels[m_currentLevel].LoadContent(Content, GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            // Save high scores
            StreamWriter sw = new StreamWriter(@"highscore.txt");
            for (int i = 0; i < numberofhighscores; i++)
            {
                sw.WriteLine(highscorenames[i]);
                sw.WriteLine(highscores[i].ToString("0"));
            }
            sw.Close();
        }

        protected override void Update(GameTime gameTime)
        {
            // Variable to hold keyboard state
            KeyboardState keys = Keyboard.GetState();
            keyboardreleased = (keys != lastkeystate);      // Has keyboard input changed

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keys.IsKeyDown(Keys.Escape))
                Exit();

            float timebetweenupdates = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Loads the next level
            Levels[m_currentLevel].Update(gameTime, ref TimeTaken);

            if (Levels[m_currentLevel].EndCurrentLevel)
            {
                if (m_currentLevel < Levels.Count - 1)
                {
                    // Load next level
                    LoadNextLevel();
                }
                else
                {
                    // You have completed the last level
                    if (TimeTaken < highscores[lasthighscore])
                    {
                        highscorenames[lasthighscore] = "";
                        highscores[lasthighscore] = TimeTaken;
                        newhighscore = true;
                    }
                    else if (newhighscore)
                    {
                        keycounter -= timebetweenupdates; // Counter to delay between keys of the same value being entered
                        if (keyboardreleased)
                        {
                            if (keys.IsKeyDown(Keys.Back) && highscorenames[lasthighscore].Length > 0)
                            {
                                highscorenames[lasthighscore] = highscorenames[lasthighscore].Substring(0, highscorenames[lasthighscore].Length - 1);
                            }
                            else
                            {
                                char nextchar = kfunctions.getnextkey();
                                char lastchar = '!';
                                if (highscorenames[lasthighscore].Length > 0)
                                    lastchar = Convert.ToChar(highscorenames[lasthighscore].Substring(highscorenames[lasthighscore].Length - 1, 1));
                                if (nextchar != '!' && (nextchar != lastchar || keycounter < 0))
                                {
                                    keycounter = keystrokedelay;
                                    highscorenames[lasthighscore] += nextchar;
                                    if (highscorenames[lasthighscore].Length > maxnamelength)
                                        highscorenames[lasthighscore] = highscorenames[lasthighscore].Substring(0, maxnamelength);
                                }
                            }
                        }
                    }
                }
            }

            else
            {
                Level1 copyOfLevel1 = (Level1)Levels[1];
                //Scores adds when player is alive
                if (m_currentLevel == 1 && copyOfLevel1.Player != null && copyOfLevel1.Player.IsPlayerAlive)
                {
                    TimeTaken += timebetweenupdates;
                }

                BossLevel1 copyOfLevel2 = (BossLevel1)Levels[2];
                if (m_currentLevel == 2 && copyOfLevel2.Player != null && copyOfLevel2.Player.IsPlayerAlive)
                {
                    TimeTaken += timebetweenupdates;
                }
            }

            if (Levels[m_currentLevel].EndCurrentLevel || Levels[m_currentLevel].IsDead)
            {                // Allow game to return to the start
                if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || keys.IsKeyDown(Keys.Enter))
                {
                    // Sort the high score table
                    Array.Sort(highscores, highscorenames);
                    reset();
                }
            }


            lastkeystate = keys;                     // Read keyboard

            base.Update(gameTime);
        }

        void reset()
        {
            for (int i = 0; i < Levels.Count; i++)
            {
                Levels[i].EndCurrentLevel = false;
                Levels[i].LoadContent(Content, GraphicsDevice);
            }

            TimeTaken = 0;
            m_currentLevel = -1;
            LoadNextLevel();
        }

        //Loads the next level if current level is completed
        private void LoadNextLevel()
        {
            m_currentLevel++;

            if (m_currentLevel >= Levels.Count)
            {
                m_currentLevel = Levels.Count - 1;
            }
            else
            {
                Levels[m_currentLevel].LoadContent(Content, GraphicsDevice);
                newhighscore = false;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Draws each object in currently loaded level
            Levels[m_currentLevel].Draw(spriteBatch, TimeTaken);

            spriteBatch.Begin();

            // Draw top ten high scores
            if (m_currentLevel == 2 && Levels[m_currentLevel].EndCurrentLevel && newhighscore)
            {
                int starty = 650;
                int maxlength = 15;

                spriteBatch.DrawString(Arial, "TOP TIMES", new Vector2(100, starty-60), Color.White, MathHelper.ToRadians(0), new Vector2(0, 0), 1f, SpriteEffects.None, 0);

                for (int i = 0; i < numberofhighscores; i++)
                {
                    if (highscorenames[i].Length >= maxlength)
                        spriteBatch.DrawString(Arial, (i + 1).ToString("0") + ". " + highscorenames[i].Substring(0, maxlength), new Vector2(60, starty + (i * 30)),
                            Color.White, MathHelper.ToRadians(0), new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    else
                        spriteBatch.DrawString(Arial, (i + 1).ToString("0") + ". " + highscorenames[i], new Vector2(60, starty + (i * 30)),
                            Color.White, MathHelper.ToRadians(0), new Vector2(0, 0), 1f, SpriteEffects.None, 0);

                    spriteBatch.DrawString(Arial, (highscores[i]/1000).ToString("0.0"), new Vector2(WindowWidth - 1500, starty + (i * 30)),
                        Color.White, MathHelper.ToRadians(0), new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                }

                if (newhighscore)
                {
                    spriteBatch.DrawString(Arial, "New High Score Enter Name", new Vector2(WindowWidth / 2 - (int)(Arial.MeasureString("New High Score Enter Name").X * (1f / 2f)), WindowHeight-100),
                            Color.White, MathHelper.ToRadians(0), new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(Arial, highscorenames[numberofhighscores - 1], new Vector2(WindowWidth / 2 - (int)(Arial.MeasureString("New High Score Enter Name").X * (1f / 2f)), WindowHeight - 60),
                            Color.AliceBlue, MathHelper.ToRadians(0), new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}