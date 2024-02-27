using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Prod_em_on_Team4
{
    public static class PlayerManager
    {
        public static Player Spy;

        private static SpriteFont stateFont = Globals.Content.Load<SpriteFont>("StateFont");

        private static Vector2 stateTextOffset = new Vector2(0, 8);

        public static void Update()
        {
            InputManager.Update();
            Spy.Update();

            if (InputManager.HaveIJustPressed(Keys.Escape))
            {
                Spy.Position = TileMap.playerSpawnPoint;
                Spy.SpriteBox.Location = TileMap.playerSpawnPoint;
            }
        }

        public static void DrawPlayerState()
        {
            Globals.spriteBatch.DrawString(stateFont,
                Spy.State,
                Spy.Position - stateTextOffset
                - (stateFont.MeasureString(Spy.State) / 2)
                + new Vector2(Spy.SpriteBox.Width / 2, 0),
                Color.Red);
        }

        public static void DrawPlayer() 
        {
            Spy.Draw();
        }
    }
}
