using System;
using Defend_It.Design_Components;
using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Game_States
{
    public class StateMainMenu : GameState
    {
        public Rectangle BackgroundRectangle;
        public Rectangle LogoRectangle;

        public Button BtnLeaderboard;
        public Button BtnNewGame;

        public StateMainMenu() : base("MainMenu")
        {
            BackgroundRectangle = new Rectangle(0, 0, Main.Instance.WindowWidth, Main.Instance.WindowHeight);
            LogoRectangle = new Rectangle(20, 20, 550, 140);
            BtnNewGame = new Button(Assets.GetTexture("fly"),
                new Vector2(Main.Instance.WindowWidth / 1.35f, Main.Instance.WindowHeight / 1.25f), new Vector2(64));
            BtnLeaderboard = new Button(Assets.GetTexture("btnLeaderboard"),
                new Vector2(BtnNewGame.Position.X + BtnNewGame.Size.X + 32, BtnNewGame.Position.Y), new Vector2(64));

            BtnNewGame.Click += BtnNewGameOnClick;
            BtnLeaderboard.Click += BtnLeaderboardOnClick;
        }

        private void BtnNewGameOnClick(object sender, EventArgs eventArgs)
        {
            Main.Instance.FocusOnGameState("Playing");
            ((StatePlaying)Main.Instance.GameStates[Main.Instance.CurrentGameState]).CreateNewGame();
        }

        private void BtnLeaderboardOnClick(object sender, EventArgs eventArgs)
        {
            //TODO: ADD LEADERBOARD
            //if (System.Windows.Forms.Application.OpenForms["FormLeaderboard"] == null)
            //{
            //    BtnNewGame.Enabled = false;
            //    var form = new FormLeaderboard();
            //    form.Show();
            //}
        }
        
        public override void Update(GameTime gameTime)
        {
            BtnNewGame.Update();
            BtnLeaderboard.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.GetTexture("background"), BackgroundRectangle, Color.White);
            spriteBatch.Draw(Assets.GetTexture("logo"), LogoRectangle, Color.White);

            BtnNewGame.Draw(spriteBatch);
            BtnLeaderboard.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
