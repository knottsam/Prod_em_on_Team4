using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Prod_em_on_Team4
{
    internal class Tile : Sprite
    {

        public Tile(Rectangle spriteBox, Vector2 spritePosition, Color spriteColour) : base(spriteBox, spritePosition, spriteColour)
        {
        }

        public override void LoadContent(ContentManager myContent, string fileName)
        {
            base.LoadContent(myContent, fileName);
            _spriteBox = new Rectangle((int)_spritePosition.X, (int)_spritePosition.Y, _spriteTexture.Width, _spriteTexture.Height);
        }
    }
}
