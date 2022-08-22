using Knight_Times;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knight_Times
{
    public class Platform : ICollidable
    {
        //Gives the Platform a texture
        public Texture2D PlatformTexture;

        //Gives the platform a position
        public Vector2 PlatformPosition;

        //Handles the collidables for the platform
        public CollidableType CollisionType { get { return CollidableType.Floor; } }

        //Gives the platform a private hitbox
        private Rectangle m_hitbox;

        //Gives the platform a public hitbox
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            private set { m_hitbox = value; }
        }

        public Platform(ContentManager content, Vector2 pos)
        {
            //Loads the texture for tha platform from the content pipeline
            PlatformTexture = content.Load<Texture2D>("Textures/platform");

            //Starting position of platform
            PlatformPosition = pos;

            //Hitbox for the platform
            Hitbox = new Rectangle((int)PlatformPosition.X, (int)PlatformPosition.Y, PlatformTexture.Width, PlatformTexture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the platform on screen
            spriteBatch.Draw(PlatformTexture, PlatformPosition, Color.White);
        }
    }
}

