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
        public Texture2D FloorPlatformTexture;
        public Vector2 FloorPlatformPosition;

        public CollidableType CollisionType { get { return CollidableType.Floor; } }

        private Rectangle m_hitbox;
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
            spriteBatch.Draw(FloorPlatformTexture, FloorPlatformPosition, Color.White);
        }
    }
}