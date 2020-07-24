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
        public Texture2D Texture;
        public Vector2 Position;

        private Rectangle m_hitbox;
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            set { m_hitbox = value; }
        }

        public CollidableType CollisionType
        {
            get { return CollidableType.Wall; }
        }

        public Wall(ContentManager content, Vector2 position)
        {
            //Texture for the Wall
            Texture = content.Load<Texture2D>("Wall");

            //Sets starting position for Wall
            Position = position;

            //Hitbox for the Wall
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public bool Update(Rectangle playerHitbox)
        {
            return playerHitbox.Intersects(Hitbox);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
