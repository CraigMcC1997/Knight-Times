using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knight_Times
{
    public class Boss1 : ICollidable
    {
        //Gives the boss a texture, position and hitbox
        public Texture2D Texture;
        public Vector2 Position;
        private Rectangle m_hitbox;
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            set { m_hitbox = value; }
        }

        //Gives the boss a speed using a float
        public float speed = 2f;

        //boss health
        public int BossLives = 300;

        //boss life boolean
        public bool IsBossAlive = true;

        //Gives the boss a Collision box using the pre-defined "wall" hitbox
        //Can be found in the "interfaces" and "Player" classes
        public CollidableType CollisionType
        {
            get { return CollidableType.Wall; }
        }

        //Take away from the bosses health when damaged
        public void CheckBossDamage()
        {
            BossLives--;
        }

        public Boss1(ContentManager content, Vector2 pos)
        {
            //Texture for the Boss1
            Texture = content.Load<Texture2D>("Boss");

            //Sets starting position for Boss1
            Position = pos;

            //Hitbox for the Boss1
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsBossAlive)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
        }
    }
}