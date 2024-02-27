using Microsoft.Xna.Framework;

namespace Prod_em_on_Team4
{
    public static class Camera
    {
        public static Matrix Transform { get; private set; }
        public static Vector3 Position = new();

        private static float moveAmount = 0.1f;
        private static int maxYOffset = 300;

        private static float Lerp(float start,  float end, ref float amt) 
        {
            return start + (end - start) * amt;
        }

        public static void Follow(Sprite target)
        {
            InputManager.LastKeysDown();

            //Position.X = MathHelper.Clamp( Lerp(Position.X, (-target.Center.X), moveAmount), -1548, -500);
            //Position.Y = MathHelper.Clamp( Lerp(Position.Y, (-target.Center.Y), moveAmount), -1136, -592);
            Position.X = Lerp(Position.X, (-target.Center.X), ref moveAmount);
            Position.Y = Lerp(Position.Y, (-target.Center.Y), ref moveAmount);

            Matrix position = Matrix.CreateTranslation(Position);

            Matrix offset = Matrix.CreateTranslation(
                Globals.screenWidth / 2,
                Globals.screenHeight / 2,
                0);

            Transform = position * offset;

            /*if ((System.Math.Abs((-target.Center.Y) - Position.Y) > maxYOffset))
            {
                Position.Y -= (System.Math.Abs((-target.Center.Y) - Position.Y) - maxYOffset);
            }*/
        }

    }
}