using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Knight_Times
{
    public interface ILevel
    {
        void LoadContent(ContentManager content, GraphicsDevice graphicsDevice);
        void Update(GameTime gameTime, ref float TimeTaken);
        void Draw(SpriteBatch spriteBatch, float Time);

        bool EndCurrentLevel { set;  get; }
        bool IsDead { get; }
    }

    public enum CollidableType
    {
        Player,
        Floor,
        Wall,
        Endpoint,
        Coin
    }

    public interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch);
    }

    public interface ICollidable : IDrawable
    {
        Rectangle Hitbox { get; }
        CollidableType CollisionType { get; }
    }
}
