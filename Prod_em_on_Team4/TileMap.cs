using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Prod_em_on_Team4
{
    internal static class TileMap
    {
        static Tuple<int, int>[] tileAroundPositions = new Tuple<int, int>[21];
        public static bool devMode = false;

        public static int tileSize = 64;
        public static int tileSheetColumns = 12;

        static Dictionary<string, Vector2> tilesAroundTile = new Dictionary<string, Vector2>()
        {
            { "t", new Vector2(0, 1) },
            { "tr", new Vector2(1, 1) },
            { "r", new Vector2(1, 0) },
            { "br", new Vector2(1, -1) },
            { "b", new Vector2(0, -1) },
            { "bl", new Vector2(-1, -1) },
            { "l", new Vector2(-1, 0) },
            { "tl", new Vector2(-1, 1) }
        };
        public static Dictionary<byte, int> tileTypes = new Dictionary<byte, int>()
        {
            { 0, 1 }, { 255, 2 }, { 199, 3 }, { 124, 4 },
            { 31, 5 }, { 241 , 6 }, { 193, 7 }, { 7, 8 },
            { 112, 9 }, { 28, 10 }, { 85, 11 }, { 215, 12 },
            { 125, 13 }, { 95, 14 }, { 245, 15 }, { 247, 16 },
            { 223, 17 }, { 127, 18 }, { 253, 19 }, { 17, 20 },
            { 68, 21 }, { 1, 22 }, { 16, 23 }, { 4, 24 },
            { 64, 25}, { 65, 26 }, { 5, 27 }, { 20, 28 },
            { 80, 29 }, { 23, 30 }, { 209, 31 }, { 113, 32 },
            { 29, 33 }, { 116, 34 }, { 92, 35 }, { 197, 36 },
            { 71, 37 }, { 87, 38 }, { 213, 39 }, { 117, 40 },
            { 93, 41 },{ 221, 42 }, { 119, 43 }, { 81, 44 },
            { 21, 45 }, { 84 ,46 }, { 69, 47 }
        };

        private static Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

        public static Vector2 playerSpawnPoint = Vector2.Zero;

        /*static Tile CheckIfTile(Point proposedPosition) 
        {
            foreach (KeyValuePair<string, Tile> kvpT in tiles)
            {
                if (kvpT.Key == $"{proposedPosition.X},{proposedPosition.Y}")
                {
                    return kvpT.Value;
                }
            }
            return null;
        }*/

        //static List<Tile> drawTiles = new();

        public static List<Tile> GetTilesAround(Point point)
        {
            List<Tile> tilesAround = new List<Tile>();
            //drawTiles = new();

            Point proposedTile = new() ;
            Tile checkTile;

            foreach (Tuple<int, int> tilePos in tileAroundPositions)
            {
                proposedTile.X = ((((point.X + tileSize) / tileSize) + tilePos.Item1) * tileSize);
                proposedTile.Y = ((((point.Y + tileSize) / tileSize) + tilePos.Item2) * tileSize);

                //drawTiles.Add(new Tile(new Vector2(proposedTile.X, proposedTile.Y), Color.Blue));

                checkTile = (tiles.ContainsKey($"{proposedTile.X},{proposedTile.Y}")) ? tiles[$"{proposedTile.X},{proposedTile.Y}"] : null;
                if (checkTile != null && checkTile._tileType != -3) 
                {
                    tilesAround.Add(checkTile);
                }
            }

            return tilesAround;
        }

        static string levelName = "Bishop_Level";

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

            Tile.LoadTileTexture(Content, "UnVincedTilesV2");

            int fileMaxLines = File.ReadAllBytes($"{levelName}.tilemap").Count(s => s == 15);

            using (var stream = File.Open($"{levelName}.tilemap", FileMode.Open))
            {  
                using (BinaryReader br = new BinaryReader(stream))
                {
                    try
                    {
                        int i = 0;
                        int lineNumber = 1;
                        while (true)
                        {
                            switch ((int)br.ReadByte())
                            {
                                case 1:
                                    tiles.Add($"{i * tileSize},{lineNumber * tileSize}", new Tile(new Vector2(i * tileSize, lineNumber * tileSize), Color.White));
                                    tiles[$"{i * tileSize},{lineNumber * tileSize}"]._tileType = 0;
                                    i++;
                                    break;
                                case 2:
                                    tiles.Add($"{i * tileSize},{lineNumber * tileSize}", new Tile(new Vector2(i * tileSize, lineNumber * tileSize), Color.White));
                                    i++;
                                    break;
                                case 3:
                                    playerSpawnPoint = new Vector2(i * tileSize + 0.5f * tileSize, lineNumber * tileSize);
                                    i++;
                                    break;
                                case 4:
                                    tiles.Add($"{i * tileSize},{lineNumber * tileSize}", new Tile(new Vector2(i * tileSize, lineNumber * tileSize), Color.White));
                                    tiles[$"{i * tileSize},{lineNumber * tileSize}"]._tileType = -3;
                                    i++;
                                    break;
                                case 15:
                                    lineNumber++;
                                    i = 0;
                                    break;
                                default:
                                    i++;
                                    break;
                            }
                        }
                    }
                    catch (EndOfStreamException e) { }

                    ConfigureTileTypes();
                }
            }
        }

        public static void DrawTiles(SpriteBatch SB) 
        {
            foreach (Tile t in tiles.Values)
            {
                t.Draw(SB);
            }
        }

        static void CheckAroundTileAt(string tilePositionKey)
        {
            foreach (KeyValuePair<string, Vector2> kvp in tilesAroundTile)
            {
                int checkTileX = (int)(tiles[tilePositionKey].Position.X + (kvp.Value.X * tileSize));
                int checkTileY = (int)(tiles[tilePositionKey].Position.Y + (kvp.Value.Y * tileSize));

                if (kvp.Key.Length == 2)
                {
                    int allowCorner = 0;

                    switch (kvp.Key[0])
                    {
                        case 't':
                            if (tiles.ContainsKey($"{checkTileX},{checkTileY - tileSize}"))
                            {
                                if (tiles[$"{checkTileX},{checkTileY - tileSize}"]._tileType != 0) allowCorner++;
                            }
                            break;
                        case 'b':
                            if (tiles.ContainsKey($"{checkTileX},{checkTileY + tileSize}"))
                            {
                                if (tiles[$"{checkTileX},{checkTileY + tileSize}"]._tileType != 0) allowCorner++;
                            }
                            break;
                    }

                    switch (kvp.Key[1])
                    {
                        case 'l':
                            if (tiles.ContainsKey($"{checkTileX + tileSize},{checkTileY}"))
                            {
                                if (tiles[$"{checkTileX + tileSize},{checkTileY}"]._tileType != 0) allowCorner++;
                            }
                            break;
                        case 'r':
                            if (tiles.ContainsKey($"{checkTileX - tileSize},{checkTileY}"))
                            {
                                if (tiles[$"{checkTileX - tileSize},{checkTileY}"]._tileType != 0) allowCorner++;
                            }
                            break;
                    }

                    if (allowCorner == 2)
                    {
                        tiles[tilePositionKey].tilesAround[kvp.Key] = (tiles.ContainsKey($"{checkTileX},{checkTileY}") && tiles[$"{checkTileX},{checkTileY}"]._tileType != 0) ? (byte)1 : (byte)0;
                    }
                    else
                    {
                        tiles[tilePositionKey].tilesAround[kvp.Key] = 0;
                    }
                }
                else
                {
                    tiles[tilePositionKey].tilesAround[kvp.Key] = (tiles.ContainsKey($"{checkTileX},{checkTileY}") && tiles[$"{checkTileX},{checkTileY}"]._tileType != 0) ? (byte)1 : (byte)0;
                }
            }
        }

        public static void LastKeyDown()
        {
            if (Keyboard.GetState().GetPressedKeyCount() == 0)
            {
                lastKeyDown = Keys.None;
            }
        }

        static void ConfigureTileTypes()
        {
            foreach (string tilePos in tiles.Keys)
            {
                if (tilePos != null)
                {
                    CheckAroundTileAt(tilePos);
                    if (tiles[tilePos]._tileType != 0 && tiles[tilePos]._tileType != -3)
                    {
                        tiles[tilePos]._tileType = tileTypes[tiles[tilePos].tileTypeValue()];
                    }

                    if (tiles[tilePos]._tileType == -3)
                    {
                        tiles[tilePos].SetSourceRect(-3);
                    }
                    else
                    {
                        tiles[tilePos].SetSourceRect(tiles[tilePos]._tileType);
                    }
                }
            }
        }

        static Keys lastKeyDown;
        public static bool HaveIJustPressed(Keys keyToCheck)
        {
            if (Keyboard.GetState().IsKeyDown(keyToCheck) && lastKeyDown != keyToCheck)
            {
                lastKeyDown = keyToCheck;
                return true;
            }
            return false;
        }

    }
}
