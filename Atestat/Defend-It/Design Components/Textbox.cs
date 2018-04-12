using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Defend_It.Design_Components
{
    public class Textbox : Label
    {
        private Texture2D texture;
        public new string Text
        {
            get { return base.Text; }
            set
            {
                if (Font.MeasureString(value).X + Spacing.X > rectangle.Size.X) return;

                if (Font.MeasureString(value).Y + Spacing.Y > rectangle.Size.Y)
                    rectangle.Size = new Point(rectangle.Size.X, (int)Font.MeasureString(value).Y);

                base.Text = value;

            }
        }

        private Rectangle rectangle;
        public new Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                rectangle.X = (int)value.X;
                rectangle.Y = (int)value.Y;
            }

        }
        private static Point Spacing = new Point(5, 3);

        private Color color = Color.White;

        private bool isFocused;
        public bool IsFocused
        {
            get => isFocused;
            set
            {
                if (value) color.A = 255;
                else color.A = 50;

                isFocused = value;
            }
        }

        public Textbox(string text, Rectangle rectangle) : base(string.Empty, new Vector2(rectangle.X + Spacing.X, rectangle.Y + Spacing.Y))
        {
            texture = Assets.Textures["Textbox"];
            IsFocused = false;
            this.rectangle = rectangle;
            Text = text;
        }
        
        public override void Update(GameTime gameTime)
        {
            UpdateKeyboardInput();

            if (InputHandler.LeftClick())
            {
                var mouseRectangle = new Rectangle(InputHandler.CurrentMouseState.Position, new Point(1, 1));

                if (mouseRectangle.Intersects(rectangle))
                    IsFocused = true;
                else
                    IsFocused = false;
            }

            base.Update(gameTime);
        }

        private void UpdateKeyboardInput()
        {
            if (!IsFocused) return;

            if (InputHandler.IsKeyPressed(Keys.Back) && Text.Length > 0)
                Text = Text.Remove(Text.Length - 1);

            if (InputHandler.CurrentKeyboardState.GetPressedKeys().Length <= 0) return;

            foreach (var key in InputHandler.CurrentKeyboardState.GetPressedKeys())
            {
                if (!InputHandler.IsKeyPressed(key)) return;
                if (!KeyIsAllowedInName(key)) return;

                var textToAdd = key.ToString();
                
                Text += textToAdd;
            }
        }

        private bool KeyIsAllowedInName(Keys key)
        {
            return key >= Keys.A && key <= Keys.Z;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);

            base.Draw(spriteBatch);
        }

    }
}
