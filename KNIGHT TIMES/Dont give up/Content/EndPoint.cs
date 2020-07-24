using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knight_Times.Content
{
    public class EndPoint : ICollidable
    {
        public Texture2D EndPointTexture;
        public Vector2 EndPointPosition;

        private Rectangle m_hitbox;
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            set { m_hitbox = value; }
        }

        public CollidableType CollisionType
        {
            get { return CollidableType.Endpoint; } 
        }

        public EndPoint(ContentManager content, Vector2 position)
        {
            //Texture for the EndPoint
            EndPointTexture = content.Load<Texture2D>("EndPoint");

            //Sets starting position for EndPoint
            EndPointPosition = position;

            //Hitbox for the EndPoint
            Hitbox = new Rectangle((int)EndPointPosition.X, (int)EndPointPosition.Y, EndPointTexture.Width, EndPointTexture.Height);
        }

        public bool Update(Rectangle playerHitbox)
        {
            return playerHitbox.Intersects(Hitbox);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(EndPointTexture, EndPointPosition, Color.White);
        }
    }
}
