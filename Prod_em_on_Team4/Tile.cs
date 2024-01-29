using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Prod_em_on_Team4
{
    internal class Tile : Sprite
    {

        public static Texture2D _tileTexture;

        public Tile(Vector2 spritePosition, Color spriteColour) : base(spritePosition, spriteColour)
        {
            _spriteBox = new RectangleF(spritePosition, _tileTexture.Width, _tileTexture.Height);
        }

        public static void LoadTileTexture(ContentManager myContent, string fileName)
        {
            myContent.RootDirectory = "Content";
            _tileTexture = myContent.Load<Texture2D>(fileName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_tileTexture, _spritePosition, _spriteColour);
        }
    }
}
