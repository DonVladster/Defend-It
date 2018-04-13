using System;
using System.Collections.Generic;
using Defend_It.Design_Components;
using Defend_It.Game_States;
using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Defend_It
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;


        public int WindowHeight = 600;
        public int WindowWidth = 800;

        private static Main instance;
        public static Main Instance
        {
            get
            {
                if (instance == null) instance = new Main();
                return instance;
            }
            set => instance = value;
        }

        public List<GameState> GameStates;
        public int CurrentGameState;
        private Cursor crosshair;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            GameStates = new List<GameState>();

            Content.RootDirectory = "Content";

            Exiting += delegate { Dispose(); };
        }
        
        public bool IsMouseOnScreen()
        {
            if (InputHandler.CurrentMouseState.X <= 0) return false;
            if (InputHandler.CurrentMouseState.X >= WindowWidth) return false;
            if (InputHandler.CurrentMouseState.Y <= 0) return false;
            if (InputHandler.CurrentMouseState.Y >= WindowHeight) return false;
            return true;
        }

        public void FocusOnGameState(string gameStateName)
        {
            foreach (var state in GameStates)
                if (state.Name == gameStateName)
                    CurrentGameState = GameStates.IndexOf(state);
        }

        protected override void Initialize()
        {
            if (!Assets.LoadTextures(Content)) Exit();
            crosshair = new Cursor(Assets.GetTexture("crosshair"));

            GameStates.AddRange(new GameState[]
            {
                new StateMainMenu(),
                new StatePlaying(),
                new StateEndGame() 
            });
            FocusOnGameState("MainMenu");

            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            InputHandler.Update();
            crosshair.Update();
            GameStates[CurrentGameState]?.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap);

            GameStates[CurrentGameState]?.Draw(spriteBatch);
            crosshair.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
