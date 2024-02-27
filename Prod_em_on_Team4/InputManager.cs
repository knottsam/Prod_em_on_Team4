using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Prod_em_on_Team4
{
    internal static class InputManager
    {
        private static Point _direction;
        public static Point Direction => _direction;

        private static Dictionary<Keys, bool> pressedKeys = new();

        public static void LastKeysDown()
        {
            foreach (Keys k in pressedKeys.Keys)
            {
                if (pressedKeys[k] == true)
                {
                    pressedKeys[k] = Keyboard.GetState().IsKeyDown(k);
                }
            }
        }

        public static bool HaveIJustPressed(Keys keyToCheck)
        {
            if (pressedKeys.ContainsKey(keyToCheck))
            {
                if (pressedKeys[keyToCheck] == false) 
                {
                    pressedKeys[keyToCheck] = Keyboard.GetState().IsKeyDown(keyToCheck);
                    return pressedKeys[keyToCheck];
                }
                return false;
            }
            else
            {
                pressedKeys.Add(keyToCheck, Keyboard.GetState().IsKeyDown(keyToCheck));
                return pressedKeys[keyToCheck];
            }
        }

        public static void Update()
        {
            _direction = Point.Zero;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.GetPressedKeyCount() > 0)
            {
                if (keyboardState.IsKeyDown(Keys.Left)) _direction.X--;
                if (keyboardState.IsKeyDown(Keys.Right)) _direction.X++;
                if (keyboardState.IsKeyDown(Keys.Up)) _direction.Y--;
                if (keyboardState.IsKeyDown(Keys.Down)) _direction.Y++;
            }

            LastKeysDown();
        }
    }
}
