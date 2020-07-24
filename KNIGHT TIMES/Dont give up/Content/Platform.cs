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
        public Texture2D PlatformTexture;
        public Vector2 PlatformPosition;

        public CollidableType CollisionType { get { return CollidableType.Floor; } }

        private Rectangle m_hitbox;
        public Rectangle Hitbox
        {
            get { return m_hitbox; }
            private set { m_hitbox = value; }
        }
           public Platform(ContentManager content, Vector2 pos)
        {
            //Texture of the FloorHouse1
            PlatformTexture = content.Load<Texture2D>("platform");

            //Starting position of FloorHouse1
            PlatformPosition = pos;

            //Hitbox for the FloorHouse1
            Hitbox = new Rectangle((int)PlatformPosition.X, (int)PlatformPosition.Y, PlatformTexture.Width, PlatformTexture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlatformTexture, PlatformPosition, Color.White);
        }
    }
}
    
