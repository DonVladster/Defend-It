using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Game_Objects
{
    public class Missile
    {
        public Texture2D Texture;
        public Rectangle Rectangle;
        public static Point DefaultSize = new Point(32);

        public enum YRotation
        {
            Left = -1,
            Right = 1,
            None = 0
        }
        public YRotation Rotation;

        private Color color = Color.White;

        public bool IsDestroyed;

        public Missile(Point location, YRotation rotation)
        {
            Texture = Assets.GetTexture("rocket");
            Rectangle = new Rectangle(location, DefaultSize);
            IsDestroyed = false;

            Rotation = rotation;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Rotation == YRotation.None)
                spriteBatch.Draw(Texture, Rectangle, color);

            if (Rotation == YRotation.Left)
                spriteBatch.Draw(Texture, Rectangle, null, color, -0.50f, new Vector2(0), SpriteEffects.None, 0);

            if (Rotation == YRotation.Right)
                spriteBatch.Draw(Texture, Rectangle, null, color, 0.50f, new Vector2(0), SpriteEffects.None, 0);
        }

    }

}
