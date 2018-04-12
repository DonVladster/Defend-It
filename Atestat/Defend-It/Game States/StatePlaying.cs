using System;
using System.Linq;
using Defend_It.Game_Objects;
using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Game_States
{
    public class StatePlaying : GameState
    {
        private FlyManager flyManager;
        private Player player;

        public int Level;

        public int ShouldReceiveDollarBonus = 1;
        public int DollarBonus => 10 * Level * ShouldReceiveDollarBonus;
        public int Score;
        public static int Dollars;

        private float volume = 0.05f;
        private const float Pitch = 0.0f, Pan = 0.0f;

        public static int Lives;

        private static StatePlaying instance;
        public static StatePlaying Instance
        {
            get
            {
                if (instance == null) instance = new StatePlaying();
                return instance;
            }
            set => instance = value;
        }

       // public event EventHandler LevelUpdated;
        //public event EventHandler GameOver;
        
        public StatePlaying() : base("Playing")
        {
            player = new Player();
            flyManager = new FlyManager();

            Dollars = 0;
            Score = 0;
            Level = 1;
            Lives = 3;

            FlyManager.Instance.FlyKilled += OnFlyKilled;
            FlyManager.Instance.FlyReachedTarget += OnFlyReachedTarget;
        }

        private void OnFlyReachedTarget(object sender, EventArgs eventArgs)
        {
            var fly = (Fly)sender;
            Lives--;
            ShouldReceiveDollarBonus = 0;
            flyManager.Flies.Remove(fly);
        }

        private void OnFlyKilled(object sender, EventArgs eventArgs)
        {
            var fly = (Fly)sender;

            Score += fly.PointsGiven;
            Assets.SoundEffect["Fly_death"].Play(volume, Pitch, Pan);
            flyManager.Flies.Remove(fly);
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            flyManager.Update(gameTime, Level);

            CheckMissileFlyCollision();
            UpdateLevelOnScore();

            //if (Lives <= 0) GameOver?.Invoke(null, null);
        }

        private void CheckMissileFlyCollision()
        {
            foreach (var missile in player.AmmoManager.Missiles)
            foreach (var fly in flyManager.Flies.Where(fly => fly.Rectangle.Intersects(missile.Rectangle)))
            {
                fly.HealthPoints -= AmmoManager.MissilePower;
                if (fly.HealthPoints > 0)
                    Assets.SoundEffect["RocketExplosion"].Play(volume, Pitch, Pan);
                missile.IsDestroyed = true;
            }
        }

        public void UpdateLevelOnScore()
        {
            if (Score < 60 * Level * Level + 200 * Level) return;

            Level++;
            flyManager.Flies.Clear();

            if (Level == FlyManager.UnlockingLevelJuggernautFly) flyManager.CanCreateJuggernautFly = true;
            if (Level == FlyManager.UnlockingLevelHuskyFly) flyManager.CanCreateHuskyFly = true;
            if (Level == FlyManager.UnlockingLevelMammothFly) flyManager.CanCreateMammothFly = true;

            if (Level % 3 == 0) flyManager.SpawnTimeSoldierFly *= 0.9;
            if (Level % 5 == 0) flyManager.SpawnTimeJuggernautFly *= 0.7;
            if (Level % 7 == 0) flyManager.SpawnTimeHuskyFly *= 0.8;
            if (Level % 10 == 0) flyManager.SpawnTimeMammothFly *= 0.6;
            //LevelUpdated?.Invoke(null, null);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            flyManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }
    }
}
