using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Prod_em_on_Team4
{
    internal static class TileMap
    {
        static Tile[,] tiles;
        static Tuple<int, int>[] tileAroundPositions = new Tuple<int, int>[21];
       
        static Tile CheckIfTile(Vector2 proposedPosition) 
        {
            foreach (Tile tile in tiles) 
            {
                if (tile != null)
                {
                    if (tile.Position == proposedPosition)
                    {
                        return tile;
                    }
                }
            }
            return null;
        }

        static List<Tile> drawTiles = new();

        public static List<Tile> GetTilesAround(Vector2 point)
        {
            List<Tile> tilesAround = new List<Tile>();
            drawTiles = new();

            Vector2 proposedTile = new() ;
            Tile checkTile;

            

            foreach (Tuple<int, int> tilePos in tileAroundPositions)
            {
                proposedTile.X = ((((int)(point.X + Tile._tileTexture.Width) / Tile._tileTexture.Width) + tilePos.Item1) * Tile._tileTexture.Width);
                proposedTile.Y = ((((int)(point.Y + Tile._tileTexture.Height) / Tile._tileTexture.Height) + tilePos.Item2) * Tile._tileTexture.Height);

                drawTiles.Add(new Tile(new Vector2(proposedTile.X, proposedTile.Y), Color.Blue));

                checkTile = CheckIfTile(proposedTile);
                if (checkTile != null) 
                {
                    tilesAround.Add(checkTile);
                }
            }

            return tilesAround;
        }

        public static void GetTileMap(ContentManager Content)
        {
            int lowerBound = -3;
            int upperBound = 1;

            int count = 0;
            for (int i = lowerBound; i < (upperBound + 1); i++) 
            {
                for (int j = lowerBound; j < (upperBound + 1); j++) 
                {
                    if (i == lowerBound || i == upperBound)
                    {
                        if (j == lowerBound || j == upperBound)
                        {
                            continue;
                        }
                    }

                    tileAroundPositions[count] = Tuple.Create(i, j);
                    count++;
                }
            }

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
                Tile.LoadTileTexture(Content, "Tile");

                while ((line = sr.ReadLine()) != null)
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        switch (line[i])
                        {
                            case '1':
                                tiles[lineNumber, i] = new Tile(new Vector2(i * 64, lineNumber * 64), Color.White);
                            break;
                            default: break;
                        }
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

            foreach (Tile t in drawTiles) 
            {
                //t.Draw(SB);
            }
        }
    }
}
