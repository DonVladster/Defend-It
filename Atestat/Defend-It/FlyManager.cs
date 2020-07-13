using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defend_It.Game_Objects;
using Defend_It.Game_States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It
{
    public class FlyManager
    {
        
        private const int UnlockingLevelJuggernautFly = 3;
        private const int UnlockingLevelMammothFly = 10;
        private const int UnlockingLevelHuskyFly = 7;

        private bool CanCreateSoldierFly = true;
        private bool CanCreateJuggernautFly;
        private bool CanCreateMammothFly;
        private bool CanCreateHuskyFly;

        private const int BaseSpawnTimeSoldierFly = 1000;
        private const int BaseSpawnTimeJuggernautFly = 5000;
        private const int BaseSpawnTimeHuskyFly = 4000;
        private const int BaseSpawnTimeMammothFly = 15000;

        private const int MovementUpdateTime = 20;

        public double SpawnTimeSoldierFly;
        public double SpawnTimeJuggernautFly;
        public double SpawnTimeMammothFly;
        public double SpawnTimeHuskyFly;


        private double elapsedCreateTimeSoldierFly, elapsedCreateTimeJuggernautFly, elapsedCreateTimeMammothFly, elapsedCreateTimeHuskyFly;
        private double elapsedMovementTime;

        public List<Fly> Flies;

        public event EventHandler FlyKilled;
        public event EventHandler FlyReachedTarget;
        

        public FlyManager()
        {
            Flies = new List<Fly>();

            SpawnTimeSoldierFly = BaseSpawnTimeSoldierFly;
            SpawnTimeJuggernautFly = BaseSpawnTimeJuggernautFly;
            SpawnTimeMammothFly = BaseSpawnTimeMammothFly;
            SpawnTimeHuskyFly = BaseSpawnTimeHuskyFly;
        }
        
        public void Update(GameTime gameTime)
        {
            if (elapsedCreateTimeSoldierFly >= SpawnTimeSoldierFly && CanCreateSoldierFly)
            {
                elapsedCreateTimeSoldierFly = 0;
                Flies.Add(new SoldierFly());
            }
            if (elapsedCreateTimeJuggernautFly >= SpawnTimeJuggernautFly && CanCreateJuggernautFly)
            {
                elapsedCreateTimeJuggernautFly = 0;
                Flies.Add(new JuggernautFly());
            }
            if (elapsedCreateTimeHuskyFly >= SpawnTimeHuskyFly && CanCreateHuskyFly)
            {
                elapsedCreateTimeHuskyFly = 0;
                Flies.Add(new HuskyFly());
            }
            if (elapsedCreateTimeMammothFly >= SpawnTimeMammothFly && CanCreateMammothFly)
            {
                elapsedCreateTimeMammothFly = 0;
                Flies.Add(new MammothFly());
            }

            MoveFlies();
            RemoveDeadFlies();
            RemoveFliesReachedTarget();

            UpdateTimers(gameTime);
        }

        public void MoveFlies()
        {
            if (elapsedMovementTime < MovementUpdateTime) return;

            foreach (var fly in Flies)
                fly.Rectangle.Y += fly.MovementIncrement;

            elapsedMovementTime = 0;
        }

        private void UpdateTimers(GameTime gameTime)
        {
            elapsedMovementTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            elapsedCreateTimeHuskyFly += gameTime.ElapsedGameTime.TotalMilliseconds;
            elapsedCreateTimeJuggernautFly += gameTime.ElapsedGameTime.TotalMilliseconds;
            elapsedCreateTimeMammothFly += gameTime.ElapsedGameTime.TotalMilliseconds;
            elapsedCreateTimeSoldierFly += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        
        private void RemoveDeadFlies()
        {
            for (var i = 0; i < Flies.Count; i++)
                if (Flies[i].HealthPoints <= 0)
                {
                    FlyKilled?.Invoke(Flies[i], null);
                }
        }

        private void RemoveFliesReachedTarget()
        {
            for (var i = 0; i < Flies.Count; i++)
                if (Flies[i].Rectangle.Y + Flies[i].Rectangle.Height >= Main.Instance.WindowHeight)
                {
                    FlyReachedTarget?.Invoke(Flies[i], null);
                }
        }

        public void UnlockNewFlies(int level)
        {
            CanCreateJuggernautFly = level >= UnlockingLevelJuggernautFly;
            CanCreateHuskyFly = level >= UnlockingLevelHuskyFly;
            CanCreateMammothFly = level >= UnlockingLevelMammothFly;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var fly in Flies)
                fly.Draw(spriteBatch);
        }
    }
}
