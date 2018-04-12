using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Game_Objects
{
    public class Frog
    {
        public Texture2D Texture;

        public Rectangle Rectangle;
        public Point Position
        {
            get => new Point(Rectangle.X, Rectangle.Y);

            set
            {
                Rectangle.X = value.X;
                Rectangle.Y = value.Y;
            }
        }

        public Point TopCenterPosition => new Point(Position.X + Rectangle.Width / 2, Position.Y);
        
      

        public Frog()
        {
            Texture = Assets.Textures["Frog"];

            Rectangle.Width = 64;
            Rectangle.Height = 64;
            Rectangle.X = Main.WindowWidth / 2 - Rectangle.Width;
            Rectangle.Y = Main.WindowHeight - Rectangle.Height;
            
        }

        public void Update(GameTime gameTime)
        {
            Rectangle.X = InputHandler.Instance.CurrentMouseState.X - Rectangle.Width / 2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }


    }
}
