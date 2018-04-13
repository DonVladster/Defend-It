using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Game_Objects
{
    public class Fly
    {
        public Rectangle Rectangle;
        protected Texture2D SelectedTexture;
        protected Color Color = Color.White;

        private const int BaseMovementIncrement = 4;
        protected double Speed;
        public int MovementIncrement => (int)(BaseMovementIncrement * Speed);

        private Random rand = new Random();
        private int GetFlyNextPositionX => rand.Next(0, Main.Instance.WindowWidth - Rectangle.Width);

        public int PointsGiven;
        public int HealthPoints;


        public Fly(Texture2D texture)
        {
            Rectangle = new Rectangle(GetFlyNextPositionX, 0, 32, 32);
            SelectedTexture = texture;

            HealthPoints = 1;
            Speed = 1;
            PointsGiven = 0;
        }

        protected Fly(Point size)
        {
            Rectangle.Width = size.X;
            Rectangle.Height = size.Y;
            Rectangle.X = GetFlyNextPositionX;
            Rectangle.Y = 0;
        }



        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SelectedTexture, Rectangle, Color);
        }
    }

    public class SoldierFly : Fly
    {
        public SoldierFly() : base(new Point(32))
        {
            SelectedTexture = Assets.GetTexture("fly");

            HealthPoints = 1;
            Speed = 1;
            PointsGiven = 20;
        }

    }

    public class JuggernautFly : Fly
    {
        public JuggernautFly() : base(new Point(48))
        {
            SelectedTexture = Assets.GetTexture("fly");

            HealthPoints = 3;
            Speed = 1;
            PointsGiven = 25;
        }

    }

    public class MammothFly : Fly
    {
        public MammothFly() : base(new Point(64))
        {
            SelectedTexture = Assets.GetTexture("flyMammoth");

            HealthPoints = 9;
            Speed = 0.5;
            PointsGiven = 80;

        }
    }

    public class HuskyFly : Fly
    {
        public HuskyFly() : base(new Point(32))
        {
            SelectedTexture = Assets.GetTexture("flyHusky");

            HealthPoints = 1;
            Speed = 2.5;
            PointsGiven = 30;

        }
    }
}
