using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Prod_em_on_Team4
{
    internal class Enemy : Sprite
    {
        public Enemy(Vector2 spritePosition, Color spriteColour) : base(ref spritePosition, ref spriteColour)
        {
            _spriteBox = new RectangleF(ref _spritePosition);
            string enemyTexture = "player2.0";
            LoadContent(ref enemyTexture);
        }

        private void Gravity()
        {

        }

        public void Update()
        {

        }

    }
}
