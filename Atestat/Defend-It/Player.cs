using System.Collections.Generic;
using Defend_It.Game_Objects;
using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It
{
    public class Player
    {
        public Texture2D Texture;
        private Rectangle rectangle;
        public int X
        {
            get => rectangle.X;
            set => rectangle.X = value;
        }
        public int Y
        {
            get => rectangle.Y;
            set => rectangle.Y = value;
        }
        public Point Position
        {
            get => new Point(X, Y);

            set
            {
                rectangle.X = value.X;
                rectangle.Y = value.Y;
            }
        }

        public Point TopCenterPosition => new Point(X + rectangle.Width / 2, Y);

        public AmmoManager AmmoManager;

        public Player()
        {
            Texture = Assets.GetTexture("frog");

            rectangle.Width = 64;
            rectangle.Height = 64;
            rectangle.X = Main.Instance.WindowWidth / 2 - rectangle.Width;
            rectangle.Y = Main.Instance.WindowHeight - rectangle.Height;

            AmmoManager = new AmmoManager();
           
        }

        public void Update(GameTime gameTime)
        {
            UpdatePosition();
            if (InputHandler.LeftClick()) AmmoManager.CreateMissile(TopCenterPosition);
            if(InputHandler.RightClick()) AmmoManager.CreateTripleMissile(TopCenterPosition); 
            AmmoManager.Update(gameTime);
        }

        public void UpdatePosition()
        {
            if(Main.Instance.IsMouseOnScreen())
                X = InputHandler.CurrentMouseState.X - rectangle.Width / 2;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            AmmoManager.Draw(spriteBatch);
            spriteBatch.Draw(Texture, rectangle, Color.White);
        }


    }
    public class AmmoManager
    {
        private const int TimeBetweenShots = 200;
        private const int DefaultMovementUnits = 10;

        private const double MoveMissileDelayTime = 5;
        private double elapsedMovementTime, elapsedCreateTime;

        public List<Missile> Missiles;

        public static int MissilePower;
        public static int TripleShotsQuantity;

        public AmmoManager()
        {
            MissilePower = 1;
            TripleShotsQuantity = 0;
            Missiles = new List<Missile>();
        }

        public void Update(GameTime gameTime)
        {
            MoveAmmo();
            RemoveDestroyedMissiles();

            elapsedMovementTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            elapsedCreateTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void MoveAmmo()
        {
            if (elapsedMovementTime < MoveMissileDelayTime) return;

            foreach (var missile in Missiles)
            {
                missile.Rectangle.Y -= DefaultMovementUnits;

                missile.Rectangle.X = missile.Rectangle.X + (int)missile.Rotation * DefaultMovementUnits / 2;
                missile.Rectangle.X = missile.Rectangle.X + (int)missile.Rotation * DefaultMovementUnits / 2;
            }

            elapsedMovementTime = 0;
        }

        public void RemoveDestroyedMissiles()
        {
            for (var i = 0; i < Missiles.Count; i++)
                if (Missiles[i].IsDestroyed) Missiles.RemoveAt(i);
        }

        public void CreateMissile(Point creationPoint)
        {
            if (elapsedCreateTime < TimeBetweenShots) return;

            creationPoint.X -= Missile.DefaultSize.X / 2;
            Missiles.Add(new Missile(creationPoint, Missile.YRotation.None));

            elapsedCreateTime = 0;
        }

        public void CreateTripleMissile(Point creationPoint)
        {
            if (TripleShotsQuantity == 0 || elapsedCreateTime < TimeBetweenShots) return;

            creationPoint.X -= Missile.DefaultSize.X / 2;
            Missiles.Add(new Missile(creationPoint, Missile.YRotation.None));

            creationPoint.Y += 10;
            Missiles.Add(new Missile(creationPoint, Missile.YRotation.Left));

            creationPoint.Y -= 10;
            Missiles.Add(new Missile(creationPoint, Missile.YRotation.Right));

            TripleShotsQuantity--;
            elapsedCreateTime = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var missile in Missiles)
                missile.Draw(spriteBatch);
        }

    }
}
