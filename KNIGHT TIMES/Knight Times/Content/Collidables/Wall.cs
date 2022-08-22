using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knight_Times.Content
{
    public class Wall : ICollidable
    {
        //Gives the wall a texture
        public Texture2D Texture;

        //Gives the wall a position
        public Vector2 Position;

        //Gives the wall a private hitbox
        private Rectangle m_hitbox;

        //Gives the wall a public hitbox
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            set { m_hitbox = value; }
        }

        //Hnadles the Collidables for the wall
        public CollidableType CollisionType
        {
            get { return CollidableType.Wall; }
        }

        public Wall(ContentManager content, Vector2 position)
        {
            //Loads the wall texture from the content pipeline 
            Texture = content.Load<Texture2D>("Textures/Wall");

            //Sets starting position for Wall
            Position = position;

            //Hitbox for the Wall
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        //Updates the wall using the player class
        public bool Update(Rectangle playerHitbox)
        {
            return playerHitbox.Intersects(Hitbox);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the wall on screen
            //spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
