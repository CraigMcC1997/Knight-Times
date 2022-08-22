using Knight_Times;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knight_Times.Content
{
    public class Player : ICollidable
    {
        //Player facing right
        private animation PlayerAnimRight;

        //Player facing left
        private animation PlayerAnimLeft;

        //Player attacking right
        private animation PlayerAttackRight;

        //Player attacking left
        private animation PlayerAttackLeft;

        //Sound effect for when the player attacks
        private SoundEffect PlayerAttackSound;

        //temporary position for the player
        public Vector2 TempPlayerPosition;

        //Position for the player
        public Vector2 PlayerPosition;

        //Player lives
        public int PlayerLives = 1000;

        //Old position for the player
        public Vector2 Oldposition;

        //gives the player a score
        public int Playerscore = 0;

        //Gives the player a speed for used for falling as this is gravity
        const float PlayerSpeed = 9.8f;

        //For when the player jumps 
        float startY;

        //Speed at which the player jumps
        float jumpSpeed;

        //boolean to check if the player is jumping or not
        bool jumping;

        //Hitbox for the player
        Rectangle m_hitbox;

        //Boolean to check if the player is facing left or right
        bool facingleft = false;

        //boolean to check if the player is attacking or not
        public bool isattacking = false;

        //boolean for killing the player
        public bool IsPlayerAlive = true;

        //hitbox for the player
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            private set { m_hitbox = value; }
        }

        //Handles the collidables for the player
        public CollidableType CollisionType
        {
            get { return CollidableType.Player; }
        }

        public Player(ContentManager content, Vector2 pos)
        {
            //Loads the animation for the player walking from the content pipeline
            PlayerAnimRight = new animation(content, "Textures/PlayerWalk", 0, 0, 1f, Color.White, true, 24, 1, 4, true, false, false);

            //Loads the animation for the player walking from the content pipeline
            PlayerAnimLeft = new animation(content, "Textures/PlayerWalk", 0, 0, 1f, Color.White, true, 24, 1, 4, true, false, true);

            //Loads the animation for the player attacking from the content pipeline
            PlayerAttackRight = new animation(content, "Textures/PlayerAttack", 0, 0, 1f, Color.White, false, 12, 1, 4, false, false, false);

            //Loads the animation for the player attacking from the content pipeline
            PlayerAttackLeft = new animation(content, "Textures/PlayerAttack", 0, 0, 1f, Color.White, false, 12, 1, 4, false, false, true);

            //Loads the soubd effect for the player attacking from the content pipeline
            PlayerAttackSound = content.Load<SoundEffect>("SoundFiles/PlayerAttackSound");

            //Sets starting position for Player
            PlayerPosition = pos;
            TempPlayerPosition = pos;

            PlayerAnimLeft.start();
            PlayerAnimRight.start();

            //Gives the players lives 
            PlayerLives = 1000;
        }

        public void MoveMe(KeyboardState keys, float gtime)
        {
            //For controller support
            GamePadState padState1 = GamePad.GetState(PlayerIndex.One);

            //moves the player using the left thumbstick plus the players speed
            TempPlayerPosition.X += PlayerSpeed * padState1.ThumbSticks.Left.X;

            //Player facing right
            if (padState1.ThumbSticks.Left.X > 0)
            {
                facingleft = false;
            }

            //Player facing left
            if (padState1.ThumbSticks.Left.X < 0)
            {
                facingleft = true;
            }

            //Moves the player to the left and makes the player face left
            if (keys.IsKeyDown(Keys.A))
            {
                TempPlayerPosition.X -= PlayerSpeed;
                facingleft = true;
            }

            //Moves the player to the right and makes the player face right
            else if (keys.IsKeyDown(Keys.D))
            {
                TempPlayerPosition.X += PlayerSpeed;
                facingleft = false;
            }

            //Stop the player moving off screen
            if (TempPlayerPosition.X <= 0) TempPlayerPosition.X = 0;

            //updates the player facing right or left
            if (PlayerAttackLeft.visible)
                PlayerAttackLeft.update(gtime);
            if (PlayerAttackRight.visible)
                PlayerAttackRight.update(gtime);

            if (!facingleft)
            {
                PlayerAnimRight.update(gtime);
            }
            else
            {
                PlayerAnimLeft.update(gtime);
            }

            //Maked the player jump when space/A is pressed
            if (keys.IsKeyDown(Keys.Space) || padState1.Buttons.A == ButtonState.Pressed)
            {
                Jump();
            }

            //Stops the player attacking 
            if (isattacking && !PlayerAttackLeft.visible && !PlayerAttackRight.visible)
            {
                isattacking = false;
            }
        }
        //Player attacking
        public void attack()
        {
            if (!isattacking)
            {
                isattacking = true;
                if (facingleft)
                    PlayerAttackLeft.start();
                else
                    PlayerAttackRight.start();

                //Plays the sound effect when the player attacks
                PlayerAttackSound.Play(0.2f, 0f, 0f);
            }
        }

        //Jumping controls
        public void Jump()
        {
            if (!jumping)
            {
                startY = TempPlayerPosition.Y;

                jumping = true;

                //Give it upward thrust
                jumpSpeed = -35;
            }
        }

        public void Update(GameTime gameTime, List<ICollidable> collidables)
        {
            //apply some gravity
            TempPlayerPosition.Y += 9.8f;

            if (jumping)
            {
                //Making it go up
                TempPlayerPosition.Y += jumpSpeed;

                // Decrease the jumping speed gradually
                jumpSpeed += 1;
            }

            PlayerPosition = TempPlayerPosition;
            HandleCollisions(collidables);
            PlayerPosition = TempPlayerPosition;
        }

        //Handles collidables within the player class
        private void HandleCollisions(List<ICollidable> collidables)
        {
            if (collidables == null || collidables.Count == 0)
            {
                return;
            }

            //Hitbox for the Player
            Hitbox = new Rectangle((int)PlayerPosition.X - PlayerAnimRight.rect.Width / 2, (int)PlayerPosition.Y - PlayerAnimRight.rect.Height / 2, PlayerAnimRight.rect.Width, PlayerAnimRight.rect.Height);

            foreach (var collidable in collidables)
            {
                switch (collidable.CollisionType)
                {
                    case CollidableType.Floor:
                        CheckFloorCollision(collidable);
                        break;
                    case CollidableType.Wall:
                        CheckWallCollision(collidable);
                        break;
                }
            }
        }

        //Stops the player falling through the top of objects
        private void CheckFloorCollision(ICollidable collidable)
        {
            //If it's farther than ground
            if (Hitbox.Intersects(collidable.Hitbox))
            {
                //Hit From Bottom
                if (Hitbox.Y > collidable.Hitbox.Y)
                {
                    TempPlayerPosition.Y = collidable.Hitbox.Y + Hitbox.Height / 2;
                }

                //Hit From Top
                else
                {
                    TempPlayerPosition.Y = collidable.Hitbox.Y - Hitbox.Height / 2;
                }

                //Jumping code
                if (jumping)
                {
                    jumping = false;
                }
            }
        }

        private void CheckWallCollision(ICollidable collidable)
        {
            //If it's farther than ground
            if (Hitbox.Intersects(collidable.Hitbox))
            {
                //Hit From Bottom
                if (Hitbox.X > collidable.Hitbox.X)
                {
                    TempPlayerPosition.X = collidable.Hitbox.X + Hitbox.Width;
                }

                //Hit From Top
                else
                {
                    TempPlayerPosition.X = collidable.Hitbox.X - Hitbox.Width;
                }
            }
        }

        //Allows the game to draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (facingleft)
            {
                if (!isattacking)
                {
                    PlayerAnimLeft.drawme(spriteBatch, PlayerPosition);
                }
                else
                {
                    PlayerAttackLeft.drawme(spriteBatch, PlayerPosition);
                }
            }
            else
            {
                if (!isattacking)
                {
                    PlayerAnimRight.drawme(spriteBatch, PlayerPosition);
                }
                else
                {
                    PlayerAttackRight.drawme(spriteBatch, PlayerPosition);
                }
            }
        }

        //Allows the game to draw the player
        public void Draw(SpriteBatch spriteBatch, Color drawColour)
        {
            if (facingleft)
            {
                if (!isattacking)
                {
                    PlayerAnimLeft.colour = drawColour;
                    PlayerAnimLeft.drawme(spriteBatch, PlayerPosition);
                    PlayerAnimLeft.colour = Color.White;
                }
                else
                {
                    PlayerAttackLeft.colour = drawColour;
                    PlayerAttackLeft.drawme(spriteBatch, PlayerPosition);
                    PlayerAttackLeft.colour = Color.White;
                }
            }
            else
            {
                if (!isattacking)
                {
                    PlayerAnimRight.colour = drawColour;
                    PlayerAnimRight.drawme(spriteBatch, PlayerPosition);
                    PlayerAnimRight.colour = Color.White;
                }
                else
                {
                    PlayerAttackRight.colour = drawColour;
                    PlayerAttackRight.drawme(spriteBatch, PlayerPosition);
                    PlayerAttackRight.colour = Color.White;
                }
            }
        }

        //Gives the player a position
        public void Reset(Vector2 pos)
        {
            PlayerPosition = pos;
            TempPlayerPosition = pos;
        }
    }
}