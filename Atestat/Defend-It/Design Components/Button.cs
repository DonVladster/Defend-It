using System;
using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Design_Components
{
    public class Button
    {
        public Texture2D Texture;
        public float Rotation;
        public Vector2 OriginVector;

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get => rectangle;
            set => rectangle = value;
        }
        public Vector2 Position
        {
            get => new Vector2(rectangle.X, rectangle.Y);
            set
            {
                rectangle.X = (int)value.X;
                rectangle.Y = (int)value.Y;
            }
        }
        public Vector2 Size
        {
            get => rectangle.Size.ToVector2();
            set
            {
                rectangle.Width = (int)value.X;
                rectangle.Height = (int)value.Y;
            }
        }

        private Color color;
        private const int MaxColorAlpha = 255;
        private const int MinColorAlpha = 0;
        private const byte AlphaIncrementer = 3;
        private bool shouldAlphaIncrement;

        public bool Enabled;
        public bool Visible;

        //TODO: ASK SORIN IF THIS IS OK, OR SHOULD I MAKE A STATIC INSTANCE FOR THIS CLASS
        public event EventHandler Click;

        public Button(Texture2D texture, Vector2 position, Vector2 size)
        {
            Texture = texture;
            Position = position;
            Size = size;

            color = new Color(255, 255, 255, 255);
            Enabled = true;
            Visible = true;
        }

        private void RestoreColorAlpha()
        {
            if (color.A >= MaxColorAlpha)
                return;

            color.A += AlphaIncrementer;
        }

        public void Update()
        {
            if (!Enabled) return;

          

            var mouseRectangle =
                new Rectangle(InputHandler.CurrentMouseState.X, InputHandler.CurrentMouseState.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if (InputHandler.LeftClick()) Click?.Invoke(null, null);
                UpdateColorAlpha();
            }
            else
                RestoreColorAlpha();
        }

        private void UpdateColorAlpha()
        {
            if (color.A == MaxColorAlpha) shouldAlphaIncrement = false;
            if (color.A == MinColorAlpha) shouldAlphaIncrement = true;
            if (shouldAlphaIncrement) color.A += AlphaIncrementer;
            else color.A -= AlphaIncrementer;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;


            //TODO : Try to remove the if!!
            if (Rotation == 0.0f)
                spriteBatch.Draw(Texture, rectangle, color);
            else spriteBatch.Draw(Texture, rectangle, null, color, Rotation,
                OriginVector, SpriteEffects.None, 0);
        }
    }
}
