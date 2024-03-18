using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        private float size;

        public Tile(Vector2 spritePosition, Color spriteColour) : base(ref spritePosition, ref spriteColour)
        {
            size = TileMap.tileSize;
            _spriteBox = new RectangleF(ref spritePosition, ref size, ref size);
        }

        public void SetSourceRect()
        {
            if (_tileType == -3)
            {
                int __tileType = TileMap.tileTypes[tileTypeValue()];
                srcRect = new Rectangle((__tileType % TileMap.tileSheetColumns) * TileMap.tileSize, (__tileType / TileMap.tileSheetColumns) * TileMap.tileSize, TileMap.tileSize, TileMap.tileSize);
            }
            else
            {
                srcRect = new Rectangle((_tileType % TileMap.tileSheetColumns) * TileMap.tileSize, (_tileType / TileMap.tileSheetColumns) * TileMap.tileSize, TileMap.tileSize, TileMap.tileSize);
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

        public static void LoadTileTexture(string fileName)
        {
            _tileTexture = Globals.Content.Load<Texture2D>(fileName);
        }

        Rectangle srcRect;
        public void Draw(Color tileColor = default(Color))
        {
            if (tileColor == default(Color))
            {
                tileColor = Color.White;
            }

            if (_tileType == 0 && TileMap.devMode)
            {
                Globals.spriteBatch.Draw(_tileTexture, _spritePosition, srcRect, tileColor);
            }
            else
            {
                Globals.spriteBatch.Draw(_tileTexture, _spritePosition, srcRect, tileColor);
            }
        }
    }
}