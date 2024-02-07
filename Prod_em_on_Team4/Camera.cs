using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;

namespace Prod_em_on_Team4
{
    public static class Camera
    {
        public static Matrix Transform { get; private set; }
        public static Vector3 Position = new();

        private static float moveAmount = 0.2f;
        private static int maxYOffset = 300;

        private static float Lerp(float start,  float end, float amt) 
        {
            return start + (end - start) * amt;
        }

        public static void Follow(Sprite target)
        {
            TileMap.LastKeyDown();

            Position.X = Lerp(Position.X, (-target.Center.X), moveAmount);
            Position.Y = Lerp(Position.Y, (-target.Center.Y), moveAmount);

            Matrix position = Matrix.CreateTranslation(Position);

            Matrix offset = Matrix.CreateTranslation(
                Game1.screenWidth / 2,
                Game1.screenHeight / 2,
                0);

            Transform = position * offset;

            /*if ((Math.Abs((-target.Center.Y) - Position.Y) > maxYOffset))
            {
                Position.Y -= (Math.Abs((-target.Center.Y) - Position.Y) - maxYOffset);
            }*/
        }

    }
}