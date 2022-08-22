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
        //Enemy animations
        //For facing right when walking
        private animation WalkAnimRight;

        //For facing left when walking
        private animation WalkAnimLeft;

        //For facing right when attacking
        private animation AttackRight;

        //For facing left when attacking
        private animation AttackLeft;

        //Boolean to set the enemies facing left or right
        bool facingleft = false;

        //Boolean to set the enemies attacking or not attacking
        public bool isattacking = false;

        //Sets the position for the enemies
        public Vector2 Position;

        //Private hitbox for the class
        private Rectangle m_hitbox;

        //Gives the enemies a health
        public int Health;

        //gives the enemies a damage
        public int Damage;

        //2nd boolean to set if the enemies are alive or dead
        public bool IsEnemyAlive = true;

        //Public hitbox for the enemies
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
            //Texture for the enemies
            //takes the file names and adds whatever is in quatation marks to that filename
            WalkAnimRight = new animation(content, filename + "Walk", 0, 0, 1f, Color.White, true, 24, 1, 4, true, false, false);
            WalkAnimLeft = new animation(content, filename + "Walk", 0, 0, 1f, Color.White, true, 24, 1, 4, true, false, true);
            AttackRight = new animation(content, filename + "Attack", 0, 0, 1f, Color.White, false, 12, 1, 4, false, false, false);
            AttackLeft = new animation(content, filename + "Attack", 0, 0, 1f, Color.White, false, 12, 1, 4, false, false, true);

            //Sets the enemies position, speed, health and damages
            Position = pos;
            speed = setspeed;
            Health = Health2;
            Damage = Damage2;

            //sets the positions for whe the enemies are alking left or right
            WalkAnimLeft.start(Position);
            WalkAnimRight.start(Position);

        }

        //Handles the colliables from the "interface"
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

            //Updates the enemies when attacking
            if (AttackLeft.visible)
                AttackLeft.update(gtime);
            if (AttackRight.visible)
                AttackRight.update(gtime);

            // if the enemy is facing left when walking animation is playing, update
            if (!facingleft)
            {
                WalkAnimRight.update(gtime);
            }
            //else if the enemy is facing right when walking animation is playing, update
            else
            {
                WalkAnimLeft.update(gtime);
            }

            //set the enemy to attack if the bellow requirements are met 
            if (isattacking && !AttackLeft.visible && !AttackRight.visible)
            {
                isattacking = false;
            }

            //changes which way the enemy is facing when attacking depending on what side of the player they are
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

            //Hitbox for the enemies
            Hitbox = new Rectangle((int)Position.X - WalkAnimRight.rect.Width / 2, (int)Position.Y - WalkAnimRight.rect.Height / 2, WalkAnimRight.rect.Width, WalkAnimRight.rect.Height);
        }

        //enemy health int goes down when the player is attacking them
        public void TakeDamage()
        {
            //Lowers the enemies health
            Health--;

            //sets the boolean to false when the enemies health is less than or equal to 0
            //(kills the enemy
            if (Health <= 0)
            {
                IsAlive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the enemies on screen if they are alive and defines if they are attacking or walking left/right
            if (IsAlive)
            {
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
