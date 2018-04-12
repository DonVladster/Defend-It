using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Design_Components
{
    public class Label
    {
        public string Text;
        public Vector2 Position;

        public bool Visible;

        private double visibilityTime;
        private double elapsedTime;

        private Color color = Color.Black;
        public SpriteFont Font;
        public Vector2 Size => Font.MeasureString(Text);

        /// <summary>
        /// The label created will only be visible for "visibilityTime" miliseconds.
        /// To make the label visible again the method Show(double timeVisible) must be called.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="visibilityTime">Time in which the label is visible</param>
        public Label(string text, Vector2 position, double visibilityTime)
        {
            Font = Assets.Fonts["Calibri18"];
            Text = text;
            Position = position;
            Show(visibilityTime);
        }

        public Label(string text, Vector2 position)
        {
            Font = Assets.Fonts["Calibri18"];
            Text = text;
            Position = position;
            Visible = true;
            visibilityTime = -1;
        }

        public void Show(double timeVisible)
        {
            Visible = true;
            visibilityTime = timeVisible;
            elapsedTime = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!Visible) return;

            visibilityTime -= elapsedTime;

            if (visibilityTime <= 0)
                Visible = false;

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            spriteBatch.DrawString(Font, Text, Position, color);
        }


    }
}
