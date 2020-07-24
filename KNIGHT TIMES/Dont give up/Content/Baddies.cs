using Knight_Times.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knight_Times
{
    public class Baddies : ICollidable
    {
        //Gives the Ranged enemy a texture, position and hitbox
     //   public Texture2D Texture;

        //Enemy animations
        private animation WalkAnimRight;
        private animation WalkAnimLeft;
        private animation AttackRight;
        private animation AttackLeft;
        bool facingleft = false;
        public bool isattacking = false;
        public Vector2 Position;

       // public bool facingRight;
        private Rectangle m_hitbox;
        public int Health;
        public int Damage;
        public bool IsEnemyAlive = true;

        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            set { m_hitbox = value; }
        }

        //Gives the enemies a speed using a float
        public float speed = 2f;

        //A boolean to set the each enemy to dead or alive
        public bool IsAlive = true;

        public Baddies(ContentManager content, string filename, Vector2 pos, float setspeed, int Health2, int Damage2)
        {
            //Texture for the Ranged enemy
            //Texture = content.Load<Texture2D>(filename);

            WalkAnimRight = new animation(content, filename + "Walk", 0, 0, 1f, Color.White, true, 24, 1, 4, true, false, false);
            WalkAnimLeft = new animation(content, filename + "Walk", 0, 0, 1f, Color.White, true, 24, 1, 4, true, false, true);
            AttackRight = new animation(content, filename + "Attack", 0, 0, 1f, Color.White, false, 12, 1, 4, false, false, false);
            AttackLeft = new animation(content, filename + "Attack", 0, 0, 1f, Color.White, false, 12, 1, 4, false, false, true);

            //Names the position and health for the enemies
            Position = pos;
            speed = setspeed;
            Health = Health2;
            Damage = Damage2;

            WalkAnimLeft.start(Position);
            WalkAnimRight.start(Position);

        }

        public CollidableType CollisionType
        {
            get { return CollidableType.Wall; }
        }

        public void update(Player player, float gtime)
        {
            //Allows the enemy to follow the player on the X axis
            //Changed which way the enemy is facing depending on the players position on the X axis
            if (Math.Abs(player.PlayerPosition.X - Position.X) > 20)
            {
                if (player.PlayerPosition.X < Position.X)
                {
                    Position.X -= speed;
                    facingleft = true;
                }
                else
                {
                    Position.X += speed;
                    facingleft = false;
                }
            }

            if (AttackLeft.visible)
                AttackLeft.update(gtime);
            if (AttackRight.visible)
                AttackRight.update(gtime);

            if (!facingleft)
            {
                WalkAnimRight.update(gtime);
            }
            else
            {
                WalkAnimLeft.update(gtime);
            }

            if (isattacking && !AttackLeft.visible && !AttackRight.visible)
            {
                isattacking = false;
            }

            if (Math.Abs(Position.X - player.PlayerPosition.X) < 250)
            {
                if (!isattacking)
                {
                    isattacking = true;
                    if (facingleft)
                        AttackLeft.start();
                    else
                        AttackRight.start();
                }
            }

            //Hitbox for the Ranged enemy
            //    Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            Hitbox = new Rectangle((int)Position.X - WalkAnimRight.rect.Width / 2, (int)Position.Y - WalkAnimRight.rect.Height / 2, WalkAnimRight.rect.Width, WalkAnimRight.rect.Height);
        }

        //Player attacking
        public void TakeDamage()
        {
            Health--;

            if (Health <= 0)
            {
                IsAlive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the enemies on screen if they are alive
            if (IsAlive)
            {
                //                spriteBatch.Draw(Texture, Position, null, Color.White, 0, Vector2.Zero, 1, facingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                if (facingleft)
                {
                    if (!isattacking)
                    {
                        WalkAnimLeft.drawme(spriteBatch, Position);
                    }
                    else
                    {
                        AttackLeft.drawme(spriteBatch, Position);
                    }
                }
                else
                {
                    if (!isattacking)
                    {
                        WalkAnimRight.drawme(spriteBatch, Position);
                    }
                    else
                    {
                        AttackRight.drawme(spriteBatch, Position);
                    }
                }
            }
        }
    }
}
