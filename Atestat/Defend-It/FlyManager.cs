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

        public const int UnlockingLevelSoldierFly = 1;
        public const int UnlockingLevelJuggernautFly = 3;
        public const int UnlockingLevelMammothFly = 10;
        public const int UnlockingLevelHuskyFly = 7;

        public bool CanCreateSoldierFly = true;
        public bool CanCreateJuggernautFly;
        public bool CanCreateMammothFly;
        public bool CanCreateHuskyFly;

        private const int BaseSpawnTimeSoldierFly = 1000;
        private const int BaseSpawnTimeJuggernautFly = 5000;
        private const int BaseSpawnTimeHuskyFly = 4000;
        private const int BaseSpawnTimeMammothFly = 15000;

        private const int MovementUpdateTime = 20;

        private double SpawnTimeSoldierFly;
        private double SpawnTimeJuggernautFly;
        private double SpawnTimeMammothFly;
        private double SpawnTimeHuskyFly;


        private double elapsedCreateTimeSoldierFly, elapsedCreateTimeJuggernautFly, elapsedCreateTimeMammothFly, elapsedCreateTimeHuskyFly;
        private double elapsedMovementTime;

        public List<Fly> Flies = new List<Fly>();

        private static FlyManager instance;

        public static FlyManager Instance
        {
            get
            {
                if (instance == null) instance = new FlyManager();
                return instance;
            }
        }
       

        public event EventHandler FlyKilled;
        public event EventHandler FlyReachedTarget;
        

        public FlyManager()
        {
            SpawnTimeSoldierFly = BaseSpawnTimeSoldierFly;
            SpawnTimeJuggernautFly = BaseSpawnTimeJuggernautFly;
            SpawnTimeMammothFly = BaseSpawnTimeMammothFly;
            SpawnTimeHuskyFly = BaseSpawnTimeHuskyFly;
            
            StatePlaying.Instance.IncreaseSpawnTime += OnIncreaseSpawnTime;
        }

        private void OnIncreaseSpawnTime(object sender, EventArgs eventArgs)
        {
            if ((int)sender % 3 == 0) SpawnTimeSoldierFly *= 0.9;
            if ((int)sender % 5 == 0) SpawnTimeJuggernautFly *= 0.7;
            if ((int)sender % 7 == 0) SpawnTimeHuskyFly *= 0.8;
            if ((int)sender % 10 == 0) SpawnTimeMammothFly *= 0.6;
        }
        
        public void Update(GameTime gameTime, int currentLevel)
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

        //TODO: CHANGE THE NAMES
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
                if (Flies[i].Rectangle.Y + Flies[i].Rectangle.Height >= Main.WindowHeight)
                {
                    FlyReachedTarget?.Invoke(Flies[i], null);
                }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var fly in Flies)
                fly.Draw(spriteBatch);
        }
    }
}
