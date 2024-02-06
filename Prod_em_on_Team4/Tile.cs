using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Prod_em_on_Team4
{
    internal class Tile : Sprite
    {

        public static Texture2D _tileTexture;

        public Dictionary<string, byte> tilesAround = new Dictionary<string, byte>()
        {
            { "t", 0 },
            { "tr",0 },
            { "r", 0 },
            { "br", 0 },
            { "b", 0 },
            { "bl", 0 },
            { "l", 0 },
            { "tl", 0 }
        };

        public int _tileType = 1;

        public Tile(Vector2 spritePosition, Color spriteColour) : base(spritePosition, spriteColour)
        {
            _spriteBox = new RectangleF(spritePosition, TileMap.tileSize, TileMap.tileSize);
        }

        public void SetSourceRect(int type)
        {
            if (type == -3)
            {
                type = TileMap.tileTypes[tileTypeValue()];
                srcRect = new Rectangle((type % TileMap.tileSheetColumns) * TileMap.tileSize, (type / TileMap.tileSheetColumns) * TileMap.tileSize, TileMap.tileSize, TileMap.tileSize);
            }
            else
            {
                srcRect = new Rectangle((type % TileMap.tileSheetColumns) * TileMap.tileSize, (type / TileMap.tileSheetColumns) * TileMap.tileSize, TileMap.tileSize, TileMap.tileSize);
            }
        }

        public byte tileTypeValue()
        {
            byte total = 0;
            if (tilesAround["t"] == 1) { total += (1 << 0); }
            if (tilesAround["tr"] == 1) { total += (1 << 1); }
            if (tilesAround["r"] == 1) { total += (1 << 2); }
            if (tilesAround["br"] == 1) { total += (1 << 3); }
            if (tilesAround["b"] == 1) { total += (1 << 4); }
            if (tilesAround["bl"] == 1) { total += (1 << 5); }
            if (tilesAround["l"] == 1) { total += (1 << 6); }
            if (tilesAround["tl"] == 1) { total += (1 << 7); }
            return total;
        }

        public static void LoadTileTexture(ContentManager myContent, string fileName)
        {
            myContent.RootDirectory = "Content";
            _tileTexture = myContent.Load<Texture2D>(fileName);
        }

        Rectangle srcRect;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_tileType == 0)
            {
                if (TileMap.devMode)
                {
                    spriteBatch.Draw(_tileTexture, _spritePosition, srcRect, _spriteColour);
                }
            }
            else
            {
                spriteBatch.Draw(_tileTexture, _spritePosition, srcRect, _spriteColour);
            }
        }
    }
}
