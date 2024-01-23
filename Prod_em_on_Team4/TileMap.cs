using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;

namespace Prod_em_on_Team4
{
    internal static class TileMap
    {

        private static Tile[,] tiles;

        public static void GetTileMap(ContentManager Content)
        {
            using (StreamReader sr = new StreamReader("Tiles.txt"))
            {
                int numberOfLines = 0;
                string line;
                int maximumLineLength = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    numberOfLines++;
                    if (line.Length > maximumLineLength)
                    {
                        maximumLineLength = line.Length;
                    }
                }

                tiles = new Tile[numberOfLines, maximumLineLength];
            }

            using (StreamReader sr = new StreamReader("Tiles.txt"))
            {
                string line;
                int lineNumber = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        tiles[lineNumber, i] = new Tile(new Rectangle(), new Vector2(i * 64, lineNumber * 64), Color.White);
                        tiles[lineNumber, i].LoadContent(Content, "Tile");
                        Debug.WriteLine(1);
                    }
                    lineNumber++;
                }
            }
        }

        public static void DrawTiles(SpriteBatch SB) 
        {
            foreach (Tile t in tiles) 
            {
                if (t != null) 
                {
                    t.Draw(SB);
                }
            }
        }
    }
}
