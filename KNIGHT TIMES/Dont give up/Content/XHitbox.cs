using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knight_Times.Content
{
    public class XHitbox : ICollidable
    {
        public Texture2D Texture;
        public Vector2 Position;

        public CollidableType CollisionType { get { return CollidableType.Floor; } }

        private Rectangle m_hitbox;
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            private set { m_hitbox = value; }
        }

        public XHitbox(ContentManager content, Vector2 pos)
        {
            //Texture of the FloorPlatform
            Texture = content.Load<Texture2D>("XHitbox");

            //Starting position of FloorPlatform
            Position = pos;

            //Hitbox for the FloorPlatform
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           //spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}