using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace Prod_em_on_Team4
{
    public static class Globals
    {
        public static ContentManager Content { get; set; }
        public static SpriteBatch spriteBatch { get; set; }
        public static float TotalSeconds { get; set; }

        public static GraphicsDevice graphicsDevice { get; set; }
        public static float TotalMilliseconds { get; set; }

        public static readonly int screenWidth = 1300, screenHeight = 800;
        public static float gravityAmount = 60, airResistance = 60;

        public static Texture2D defTexture;
        public static void DefTexture()
        {
            defTexture = new Texture2D(graphicsDevice, 1, 1);
            defTexture.SetData<Color>(new[] { Color.White });
        }

        public static Texture2D LoadTexture(string fileName)
        {
            return Content.Load<Texture2D>(fileName);
        }

        public static void Update(ref GameTime GT)
        {
            TotalSeconds = (float)GT.ElapsedGameTime.TotalSeconds;
            TotalMilliseconds = (float)GT.ElapsedGameTime.TotalMilliseconds;
        }
    }
}
