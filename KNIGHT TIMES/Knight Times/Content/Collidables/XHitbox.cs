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
        //Gives the XHitbox a texture
        public Texture2D Texture;

        //Gives the XHitbox a position
        public Vector2 Position;

        //Handles the collidables for the XHitbox
        public CollidableType CollisionType { get { return CollidableType.Floor; } }

        //gives the XHitbox a private hitbox
        private Rectangle m_hitbox;

        //Gives the XHitbox public hitbox
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            private set { m_hitbox = value; }
        }

        public XHitbox(ContentManager content, Vector2 pos)
        {
            //Loads the texture for the XHitbox from the content pipeline
            Texture = content.Load<Texture2D>("Textures/XHitbox");

            //Starting position of XHitbox
            Position = pos;

            //Hitbox for the XHitbox
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the XHitbox on screen
            //REMEMBER AND COMMENT THIS OUT AS THESE ARE NOT SUPPOSED TO BE DRAWN
            //spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}