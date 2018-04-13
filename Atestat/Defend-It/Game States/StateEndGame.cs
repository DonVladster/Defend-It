using System;
using Defend_It.Design_Components;
using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Game_States
{
    public class StateEndGame : GameState
    {
        private Textbox txtName;
        private Label lblName;
        private Label lblScore;
        private int score;
        public int Score
        {
            get => score;
            set
            {
                lblScore.Text = "Score: " + value;
                score = value;
            }
        }

        public Button BtnReturn;

        public StateEndGame() : base("EndGame")
        {
            lblName = new Label("Your Name:", new Vector2(Main.Instance.WindowWidth / 2 - 64, 180));
            txtName = new Textbox("PLAYER",
                new Rectangle((int) lblName.Position.X, (int) lblName.Position.Y + (int) lblName.Size.Y, 128, 32));
            BtnReturn = new Button(Assets.GetTexture("btnContinue"),
                new Vector2(txtName.Position.X - 2, txtName.Position.Y + txtName.Size.Y), new Vector2(128, 32));
            lblScore = new Label("Score: ",
                new Vector2(txtName.Position.X - 2, txtName.Position.Y + txtName.Size.Y + BtnReturn.Size.Y + 5));

            BtnReturn.Click += BtnReturnOnClick;
        }

        private void BtnReturnOnClick(object sender, EventArgs eventArgs)
        {
            Main.Instance.FocusOnGameState("MainMenu");


            //foreach (var ovr in Main.Instance.GameStates.Where(ovr => ovr.Name == "Game"))
              //  LeaderboardManager.SaveNewScore(txtName.Text, ((GameOverlay)ovr).Score);
        }

        public override void Update(GameTime gameTime)
        {
            txtName.Update(gameTime);

            BtnReturn.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.GetTexture("grass"), Vector2.Zero,
                new Rectangle(0, 0, Main.Instance.WindowWidth, Main.Instance.WindowHeight), Color.White, 0, Vector2.Zero, 1f,
                SpriteEffects.None, 0);

            lblScore.Draw(spriteBatch);
            lblName.Draw(spriteBatch);

            txtName.Draw(spriteBatch);

            BtnReturn.Draw(spriteBatch);
        }
    }
}
