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
        //Animations for facing right or left
        private animation PlayerAnimRight;
        private animation PlayerAnimLeft;
        private animation PlayerAttackRight;
        private animation PlayerAttackLeft;
        private SoundEffect PlayerAttackSound;

        //  public Texture2D PlayerTexture;
        public Vector2 TempPlayerPosition;
        public Vector2 PlayerPosition;
        //Player lives
        public int PlayerLives = 1000;
        public Vector2 Oldposition;
        public int Playerscore = 0;
        const float PlayerSpeed = 9.8f;
        float startY;
        float jumpSpeed;
        bool jumping;
        Rectangle m_hitbox;
        bool facingleft = false;
        public bool isattacking = false;
        //boolean for killing the player
        public bool IsPlayerAlive = true;

        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            private set { m_hitbox = value; }
        }

        public CollidableType CollisionType
        {
            get { return CollidableType.Player; }
        }

        public Player(ContentManager content, Vector2 pos)
        {
            //Texture for the Player
            //     PlayerTexture = content.Load<Texture2D>("Player");
            PlayerAnimRight = new animation(content, "PlayerWalk", 0, 0, 1f, Color.White, true, 24, 1, 4, true, false, false);
            PlayerAnimLeft = new animation(content, "PlayerWalk", 0, 0, 1f, Color.White, true, 24, 1, 4, true, false, true);
            PlayerAttackRight = new animation(content, "PlayerAttack", 0, 0, 1f, Color.White, false, 12, 1, 4, false, false, false);
            PlayerAttackLeft = new animation(content, "PlayerAttack", 0, 0, 1f, Color.White, false, 12, 1, 4, false, false, true);
            PlayerAttackSound = content.Load<SoundEffect>("PlayerAttackSound");

            //Sets starting position for Player
            PlayerPosition = pos;
            TempPlayerPosition = pos;

            PlayerAnimLeft.start();
            PlayerAnimRight.start();
            PlayerLives = 1000;
        }

        public void MoveMe(KeyboardState keys, float gtime)
        {
            //For controller support
            GamePadState padState1 = GamePad.GetState(PlayerIndex.One);

            //which keys move player left and right

            TempPlayerPosition.X += PlayerSpeed * padState1.ThumbSticks.Left.X;

            if (padState1.ThumbSticks.Left.X > 0)
            {
                facingleft = false;
            }

            if (padState1.ThumbSticks.Left.X < 0)
            {
                facingleft = true;
            }

            if (keys.IsKeyDown(Keys.A))
            {
                TempPlayerPosition.X -= PlayerSpeed;
                facingleft = true;
            }
            else if (keys.IsKeyDown(Keys.D))
            {
                TempPlayerPosition.X += PlayerSpeed;
                facingleft = false;
            }

            if (TempPlayerPosition.X <= 0) TempPlayerPosition.X = 0;


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

            //Jumping update
            if (keys.IsKeyDown(Keys.Space) || padState1.Buttons.A == ButtonState.Pressed)
            {
                Jump();
            }

            if (isattacking && !PlayerAttackLeft.visible && !PlayerAttackRight.visible)
            {
                isattacking = false;
            }
        }

        public void attack()
        {
            if (!isattacking)
            {
                isattacking = true;
                if (facingleft)
                    PlayerAttackLeft.start();
                else
                    PlayerAttackRight.start();

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
            // apply some gravity
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
                    TempPlayerPosition.Y = collidable.Hitbox.Y + Hitbox.Height/2;
                }

                //Hit From Top
                else
                {
                    TempPlayerPosition.Y = collidable.Hitbox.Y - Hitbox.Height/2;
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
            if (Hitbox.Intersects(collidable.Hitbox))
            {
                TempPlayerPosition = PlayerPosition;
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
            //            spriteBatch.Draw(PlayerTexture, PlayerPosition, null,Color.White,0,Vector2.Zero,1,facingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
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
            //            spriteBatch.Draw(PlayerTexture, PlayerPosition, null,Color.White,0,Vector2.Zero,1,facingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }


        //Gives the player a position
        public void Reset(Vector2 pos)
        {
            PlayerPosition = pos;
            TempPlayerPosition = pos;
        }
    }
}