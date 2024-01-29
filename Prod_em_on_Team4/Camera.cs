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
        static Vector3 Position = new();

        private static float moveAmount = 0.1f;

        public static void Follow(Sprite target)
        {
            float posXDelta = ((-target.Center.X) - Position.X) * moveAmount;
            float posYDelta = ((-target.Center.Y) - Position.Y) * moveAmount;

            Position.X = (Math.Abs(posXDelta) < 0.1) ? -target.Center.X : Position.X  + posXDelta;
            Position.Y = (Math.Abs(posYDelta) < 0.1) ? -target.Center.Y : Position.Y + posYDelta;

            Matrix position = Matrix.CreateTranslation(Position);

            Matrix offset = Matrix.CreateTranslation(
                Game1.screenWidth / 2,
                Game1.screenHeight / 2,
                0);

            Transform = position * offset;
        }

    }
}
