using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Game_States
{
    public class StatePaused : GameState
    {
        public StatePaused() : base("Paused")
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Main.Instance.IsMouseOnScreen())
                Main.Instance.FocusOnGameState("Playing");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach(var state in Main.Instance.GameStates.Where(state => state.Name == "Playing"))
                state.Draw(spriteBatch);

            
        }
    }
}
