using System;
using System.Collections.Generic;
using Defend_It.Design_Components;
using Defend_It.IO_Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Defend_It.Game_States
{
    public class StateShop : GameState
    {
        private readonly Dictionary<string, Label> labels = new Dictionary<string, Label>();
        private readonly Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private readonly Vector2 defaultButtonSize = new Vector2(64);
        private const int DefaultPriceLife = 50;
        private const int ModifierPriceLife = 2;
        private int PriceLife => DefaultPriceLife * (int)Math.Pow(ModifierPriceLife, Lives - 1);

        private const int PriceTripleAmmo = 40;
        private const int PricePower = 400;

        private int Dollars
        {
            get
            {
                if((StatePlaying)Main.Instance.GetGameState("Playing") != null)
                    return ((StatePlaying)Main.Instance.GetGameState("Playing")).Dollars;
                return 0;
            } 
            set
            {
                if ((StatePlaying)Main.Instance.GetGameState("Playing") != null)
                   ((StatePlaying)Main.Instance.GetGameState("Playing")).Dollars = value;
            }
        }
        private int Lives
        {
            get
            {
                if ((StatePlaying)Main.Instance.GetGameState("Playing") != null)
                    return ((StatePlaying)Main.Instance.GetGameState("Playing")).Lives;
                return 0;
            }
            set
            {
                if ((StatePlaying)Main.Instance.GetGameState("Playing") != null)
                    ((StatePlaying)Main.Instance.GetGameState("Playing")).Lives = value;
            }
        }
        private int TripleShotsQuantity
        {
            get
            {
                if ((StatePlaying)Main.Instance.GetGameState("Playing") != null)
                    return ((StatePlaying)Main.Instance.GetGameState("Playing")).Player.AmmoManager.TripleShotsQuantity;
                return 0;
            }
            set
            {
                if ((StatePlaying)Main.Instance.GetGameState("Playing") != null)
                    ((StatePlaying)Main.Instance.GetGameState("Playing")).Player.AmmoManager.TripleShotsQuantity = value;
            }
        }
        private int MissilePower
        {
            get
            {
                if ((StatePlaying)Main.Instance.GetGameState("Playing") != null)
                    return ((StatePlaying)Main.Instance.GetGameState("Playing")).Player.AmmoManager.MissilePower;
                return 0;
            }
            set
            {
                if ((StatePlaying)Main.Instance.GetGameState("Playing") != null)
                    ((StatePlaying)Main.Instance.GetGameState("Playing")).Player.AmmoManager.MissilePower = value;
            }
        }

        public StateShop() : base("Shop")
        {

            labels["ShopName"] = new Label("Mayhem Shop", Vector2.Zero);
            labels["ShopName"].Position = new Vector2((float)Main.Instance.WindowWidth / 2 - labels["ShopName"].Size.X / 2, 16);
            labels["Upgrades"] = new Label("Upgraduri: ", new Vector2(32, 128));
            labels["Consumables"] = new Label("Consumabile: ", new Vector2(32, 256));
            labels["Dollars"] = new Label("Dolari: " + Dollars, Vector2.Zero);
            labels["Dollars"].Position = new Vector2((float)Main.Instance.WindowWidth / 2 - labels["Dollars"].Size.X / 2,
                Main.Instance.WindowHeight - 100);
            buttons["Continue"] = new Button(Assets.GetTexture("arrow"),
                new Vector2(Main.Instance.WindowWidth - 64, Main.Instance.WindowHeight - 64), defaultButtonSize)
            {
                Rotation = (float)Math.PI / 2,
                OriginVector = new Vector2(0, 64)
            };

            buttons["Upgrade_Power"] = new Button(Assets.GetTexture("rocket"),
                new Vector2(labels["Upgrades"].Position.X + labels["Upgrades"].Size.X,
                    labels["Upgrades"].Position.Y - 16),
                defaultButtonSize);
            labels["Upgrade_Power"] = new Label("Putere: " + MissilePower, Vector2.Zero);
            labels["Upgrade_Power"].Position = new Vector2(
                buttons["Upgrade_Power"].Position.X + buttons["Upgrade_Power"].Size.X / 2 -
                labels["Upgrade_Power"].Size.X / 2,
                buttons["Upgrade_Power"].Position.Y + buttons["Upgrade_Power"].Size.Y);
            labels["Price_Power"] = new Label(PricePower + "$",
                new Vector2(buttons["Upgrade_Power"].Position.X + buttons["Upgrade_Power"].Size.X / 2 - 15,
                    buttons["Upgrade_Power"].Position.Y + buttons["Upgrade_Power"].Size.Y +
                    labels["Upgrade_Power"].Size.Y));

            buttons["Buy_TripleAmmo"] = new Button(Assets.GetTexture("tripleRocket"),
                new Vector2(labels["Consumables"].Position.X + labels["Consumables"].Size.X,
                    labels["Consumables"].Position.Y - 16),
                defaultButtonSize);
            labels["Amount_TripleAmmo"] = new Label("Cantitate: " + TripleShotsQuantity, Vector2.Zero);
            labels["Amount_TripleAmmo"].Position = new Vector2(
                buttons["Buy_TripleAmmo"].Position.X + buttons["Buy_TripleAmmo"].Size.X / 2 -
                labels["Amount_TripleAmmo"].Size.X / 2,
                buttons["Buy_TripleAmmo"].Position.Y + buttons["Buy_TripleAmmo"].Size.Y);
            labels["Price_TripleAmmo"] = new Label(PriceTripleAmmo + "$",
                new Vector2(buttons["Buy_TripleAmmo"].Position.X + buttons["Buy_TripleAmmo"].Size.X / 2 - 15,
                    buttons["Buy_TripleAmmo"].Position.Y + buttons["Buy_TripleAmmo"].Size.Y +
                    labels["Amount_TripleAmmo"].Size.Y));

            buttons["Buy_Lives"] = new Button(Assets.GetTexture("heart"),
                new Vector2(labels["Consumables"].Position.X + labels["Consumables"].Size.X + 96,
                    labels["Consumables"].Position.Y - 16),
                defaultButtonSize);
            labels["Amount_Lives"] = new Label("Vieti: " + Lives, new Vector2());
            labels["Amount_Lives"].Position = new Vector2(
                buttons["Buy_Lives"].Position.X + buttons["Buy_Lives"].Size.X / 2 -
                labels["Amount_Lives"].Size.X / 2,
                buttons["Buy_Lives"].Position.Y + buttons["Buy_Lives"].Size.Y);
            labels["Price_Lives"] = new Label(PriceLife + "$", new Vector2(
                buttons["Buy_Lives"].Position.X + buttons["Buy_Lives"].Size.X / 2 - 15,
                buttons["Buy_Lives"].Position.Y + buttons["Buy_Lives"].Size.Y + labels["Amount_Lives"].Size.Y));

            buttons["Continue"].Click += delegate
            {
                Main.Instance.FocusOnGameState("Playing");
            };
            buttons["Buy_TripleAmmo"].Click += delegate
            {
                if (SpendMoney(PriceTripleAmmo))
                    TripleShotsQuantity += 10;
            };
            buttons["Buy_Lives"].Click += delegate
            {
                if (SpendMoney(PriceLife))
                    Lives++;
            };
            buttons["Upgrade_Power"].Click += delegate
            {
                if (SpendMoney(PricePower))
                    MissilePower++;
            };

        }

        private bool SpendMoney(int amount)
        {
            if (Dollars < amount)
                return false;

            Dollars -= amount;
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in buttons)
                button.Value.Update();

            UpdateLabels();
        }

        public void UpdateLabels()
        {
            labels["Dollars"].Text = "Dolari: " + Dollars;
            labels["Amount_TripleAmmo"].Text = "Munitie: " + TripleShotsQuantity;
            labels["Amount_Lives"].Text = "Vieti: " + Lives;
            labels["Price_Lives"].Text = PriceLife + "$";
            labels["Upgrade_Power"].Text = "Putere: " + MissilePower;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.GetTexture("grass"), Vector2.Zero,
                new Rectangle(0, 0, Main.Instance.WindowWidth, Main.Instance.WindowHeight), Color.White, 0,
                Vector2.Zero, 1f, SpriteEffects.None, 0);


            foreach (var label in labels)
                label.Value.Draw(spriteBatch);

            foreach (var button in buttons)
                button.Value.Draw(spriteBatch);
        }


    }
}
