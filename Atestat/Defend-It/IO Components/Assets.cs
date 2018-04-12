using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.IO_Components
{
    public static class Assets
    {
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public static Dictionary<string, SoundEffect> SoundEffect = new Dictionary<string, SoundEffect>();

        public static bool LoadTextures(ContentManager content)
        {
            try
            {
                Textures["arrow"] = content.Load<Texture2D>("arrowUp");
                Textures["heart"] = content.Load<Texture2D>("heart");
                Textures["nuke"] = content.Load<Texture2D>("skull");
                Textures["grass"] = content.Load<Texture2D>("grass");
                Textures["fly"] = content.Load<Texture2D>("fly");
                Textures["flyHusky"] = content.Load<Texture2D>("flyHusky");
                Textures["flyMammoth"] = content.Load<Texture2D>("flyMammoth");
                Textures["frog"] = content.Load<Texture2D>("frog");
                Textures["rocket"] = content.Load<Texture2D>("rocket");
                Textures["tripleRocket"] = content.Load<Texture2D>("tripleRocket");
                Textures["btnPlay"] = content.Load<Texture2D>("btnPlay");
                Textures["btnContinue"] = content.Load<Texture2D>("btnContinue");
                Textures["btnLeaderboard"] = content.Load<Texture2D>("btnLeaderboard");
                Textures["background"] = content.Load<Texture2D>("background");
                Textures["logo"] = content.Load<Texture2D>("logo");
                Textures["textbox"] = content.Load<Texture2D>("textbox");

                Fonts["calibri18"] = content.Load<SpriteFont>("calibri18");
                Fonts["calibri26"] = content.Load<SpriteFont>("calibri26");

                SoundEffect["flyDeath"] = content.Load<SoundEffect>("slimeSplash");
                SoundEffect["rocketExplosion"] = content.Load<SoundEffect>("rocketExplosionSound");
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Write("Eroare la incarcarea texturilor: " + ex.Message);
                return false;
            }

        }
    }
}
