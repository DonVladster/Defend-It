using System;
using System.Collections.Generic;
using System.Linq;
using Defend_It.Design_Components;
using Defend_It.Game_Objects;
using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Game_States
{
    public class StatePlaying : GameState
    {
        private Dictionary<string, Label> labels = new Dictionary<string, Label>();
        private FlyManager flyManager;
        private Player player;

        private int level;
        public int Level
        {
            get => level;
            set
            {
                level = value;
                flyManager.Flies.Clear();
                flyManager.UnlockNewFlies(Level);

                if (Level % 3 == 0) flyManager.SpawnTimeSoldierFly *= 0.9;
                if (Level % 5 == 0) flyManager.SpawnTimeJuggernautFly *= 0.7;
                if (Level % 7 == 0) flyManager.SpawnTimeHuskyFly *= 0.8;
                if (Level % 10 == 0) flyManager.SpawnTimeMammothFly *= 0.6;

                labels["Level"].Show(3000);
                ShouldReceiveDollarBonus = 1;
            }
        }

        public int ShouldReceiveDollarBonus = 1;
        public int DollarBonus => 10 * Level;
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
        
        public StatePlaying() : base("Playing")
        {
            labels["Level"] = new Label("Nivel: " + Level, new Vector2(0), 3000);
            labels["Level"].Position = new Vector2(
                Main.Instance.WindowWidth / 2 - (int)labels["Level"].Size.X / 2,
                (float)Main.Instance.WindowHeight / 4);

            labels["Score"] = new Label("Scor: " + Score, new Vector2(5, 0));
            labels["Lives"] = new Label("Vieti: " + Lives, new Vector2(5, 25));
            labels["Dollars"] = new Label("Dolari: " + Dollars, new Vector2(5, 50));

            CreateNewGame();
        }

        public void CreateNewGame()
        {
            player = new Player();
            flyManager = new FlyManager();

            Level = 1;
            Dollars = 0;
            Score = 0;
            Lives = 3;

            flyManager.FlyKilled += OnFlyKilled;
            flyManager.FlyReachedTarget += OnFlyReachedTarget;
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
            Assets.GetSoundEffect("flyDeath").Play(volume, Pitch, Pan);
            flyManager.Flies.Remove(fly);
        }

        private void UpdateLabels(GameTime gameTime)
        {
            labels["Score"].Text = "Scor: " + Score;
            labels["Lives"].Text = "Vieti: " + Lives;
            labels["Level"].Text = "Nivel: " + Level;
            labels["Dollars"].Text = "Dolari: " + Dollars;

            labels["Level"].Update(gameTime);

        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            flyManager.Update(gameTime);
            UpdateLabels(gameTime);

            CheckMissileFlyCollision();
            UpdateLevelOnScore();
            CheckGameOver();
            

            if(!Main.Instance.IsMouseOnScreen()) Main.Instance.FocusOnGameState("Paused");
        }

        private void CheckMissileFlyCollision()
        {
            foreach (var missile in player.AmmoManager.Missiles)
            foreach (var fly in flyManager.Flies.Where(fly => fly.Rectangle.Intersects(missile.Rectangle)))
            {
                fly.HealthPoints -= AmmoManager.MissilePower;
                if (fly.HealthPoints > 0)
                    Assets.GetSoundEffect("rocketExplosion").Play(volume, Pitch, Pan);
                missile.IsDestroyed = true;
            }
        }

        public void UpdateLevelOnScore()
        {
            if (Score < 60 * Level * Level + 200 * Level) return;

            Level++;

            Dollars += (Score - 60 * (Level - 2) * (Level - 2) - 200 * (Level - 2)) / 10;
            if (ShouldReceiveDollarBonus == 1)
                Dollars += DollarBonus;

            // Main.Instance.FocusOnGameState("Shop");
        }

        public void CheckGameOver()
        {
            if (Lives <= 0)
            {
                Main.Instance.FocusOnGameState("EndGame");
                ((StateEndGame)Main.Instance.GameStates[Main.Instance.CurrentGameState]).Score = Score;
            }
        }

        

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.GetTexture("grass"), Vector2.Zero,
                new Rectangle(0, 0, Main.Instance.WindowWidth, Main.Instance.WindowHeight), Color.White, 0, Vector2.Zero, 1f,
                SpriteEffects.None, 0);

            flyManager.Draw(spriteBatch);
            player.Draw(spriteBatch);

            foreach (var label in labels)
                label.Value.Draw(spriteBatch);
        }
    }
}
