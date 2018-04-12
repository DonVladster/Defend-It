using System;
using Microsoft.Xna.Framework.Input;

namespace Defend_It.IO_Components
{
    public class InputHandler
    {

        public MouseState CurrentMouseState;
        public MouseState LastMouseState;
        public KeyboardState CurrentKeyboardState;
        public KeyboardState LastKeyboardState;


        public event EventHandler LeftClick;
        public event EventHandler RightClick;

        private static InputHandler instance;
        public static InputHandler Instance
        {
            get
            {
                if (instance == null) instance = new InputHandler();

                return instance;
            }
            set => instance = value;
        }

        public void Update()
        {
            UpdateStates();

            if (CurrentMouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton == ButtonState.Released)
                LeftClick?.Invoke(null, null);

            if (CurrentMouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton == ButtonState.Released)
                RightClick?.Invoke(null, null);
        }

        public void UpdateStates()
        {
            LastMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && !LastKeyboardState.IsKeyDown(key);
        }


    }
}

