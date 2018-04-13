using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Design_Components
{
    public class Cursor
    {
        private Texture2D texture;
        private Vector2 position;
        private Color color = Color.Black;

        public Cursor(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update()
        {
            if (Main.Instance.IsMouseOnScreen())
            {
                position.X = InputHandler.CurrentMouseState.X - 16;
                position.Y = InputHandler.CurrentMouseState.Y - 16;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }

    }
}
