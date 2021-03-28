using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knight_Times.Content
{
    public class FloorPlatform : ICollidable
    {
        //Gives the Floor Platform a texture
        public Texture2D FloorPlatformTexture;

        //Gives the Floor Platform a position
        public Vector2 FloorPlatformPosition;

        //Handles the collidables for the floor platform 
        public CollidableType CollisionType { get { return CollidableType.Floor; } }

        //Gives the floor a private hitbox
        private Rectangle m_hitbox;

        //Gives the floor a public hitbox
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            private set { m_hitbox = value; }
        }

        public FloorPlatform(ContentManager content, Vector2 pos)
        {
            //Texture of the FloorPlatform
            FloorPlatformTexture = content.Load<Texture2D>("FloorPlatform");

            //Starting position of FloorPlatform
            FloorPlatformPosition = pos;

            //Hitbox for the FloorPlatform
            Hitbox = new Rectangle((int)FloorPlatformPosition.X, (int)FloorPlatformPosition.Y, FloorPlatformTexture.Width, FloorPlatformTexture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws the floor platform
            spriteBatch.Draw(FloorPlatformTexture, FloorPlatformPosition, Color.White);
        }
    }
}