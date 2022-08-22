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
        //Gives the Endpoint a texture
        public Texture2D EndPointTexture;

        //Gives the Endpoint a position
        public Vector2 EndPointPosition;

        //Gives the end point a private hitbox
        private Rectangle m_hitbox;

        //Gives the end point a pulic hitbox
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            set { m_hitbox = value; }
        }

        //Handles the collidables for the end point
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

        //Updates the end point when then player intersects it
        public bool Update(Rectangle playerHitbox)
        {
            return playerHitbox.Intersects(Hitbox);
        }

        //Draws the end point
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(EndPointTexture, EndPointPosition, Color.White);
        }
    }
}
