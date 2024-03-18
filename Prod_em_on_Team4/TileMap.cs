using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Prod_em_on_Team4
{
    internal static class TileMap
    {
        static readonly List <Tuple<int, int>> tileAroundPositions = new() 
        {
            Tuple.Create(-2, -2),
            Tuple.Create(-1, -2),
            Tuple.Create(0, -2),
            Tuple.Create(-2, -1),
            Tuple.Create(0, -1),
            Tuple.Create(-2, 0),
            Tuple.Create(-1, 0),
            Tuple.Create(0, 0),
        };
        static readonly string levelName = "Testing";
        static readonly Dictionary<Point, Tile> tiles = new ();
        static readonly Dictionary<string, Vector2> tilesAroundTile = new()
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

        public static readonly Dictionary<byte, int> tileTypes = new()
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
        public static readonly bool devMode = false;
        public static Vector2 playerSpawnPoint = Vector2.Zero;
        public static readonly int tileSize = 64, tileSheetColumns = 12;

        static void ConfigureTileTypes()
        {
            foreach (Point tilePos in tiles.Keys)
            {
                SetSurroundingsDataForTileAt(tilePos);
                if (tiles[tilePos]._tileType != 0 && tiles[tilePos]._tileType != -3)
                {
                    tiles[tilePos]._tileType = tileTypes[tiles[tilePos].tileTypeValue()];
                }

                tiles[tilePos].SetSourceRect();
            }
        }

        public static void DrawTiles() 
        {
            foreach (Tile t in tiles.Values) t.Draw();
        }

        #region Get The Tiles Around A Point Functions
        static bool AcknowledgeTile(ref string inclusionParamater, ref int normalisedVelocity, Tuple<int, int> tilePos)
        {
            switch (inclusionParamater)
            {
                case "Only-Y":
                    if (normalisedVelocity + tilePos.Item2 == -1)
                    {
                        return false;
                    }
                    if (tilePos.Item2 == -1)
                    {
                        return false;
                    }
                    break;
                case "Only-X":
                    if (normalisedVelocity + tilePos.Item1 == -1)
                    {
                        return false;
                    }
                    if (tilePos.Item1 == -1)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }
        static void ParseTileProposition(ref List<Tile> tilesAround, ref Point proposedTilePos)
        {
            Tile checkTile = (tiles.ContainsKey(proposedTilePos)) ? tiles[proposedTilePos] : null;
            if (checkTile != null && checkTile._tileType != -3)
            {
                tilesAround.Add(checkTile);
            }
        }
        public static List<Tile> GetTilesAround(Point point, string inclusionParamater = "", int normalisedVelocity = 0)
        {
            List<Tile> tilesAround = new();
            Point proposedTile = new();

            foreach (Tuple<int, int> tilePos in tileAroundPositions)
            {
                if (!AcknowledgeTile(ref inclusionParamater, ref normalisedVelocity, tilePos)) continue;

                proposedTile.X = ((((point.X + tileSize) / tileSize) + tilePos.Item1) * tileSize);
                proposedTile.Y = ((((point.Y + tileSize) / tileSize) + tilePos.Item2) * tileSize);

                ParseTileProposition(ref tilesAround, ref proposedTile);
            }

            return tilesAround;
        }
        #endregion

        #region Set Data For Surrounding Tiles Functions
        static bool ThereAreTilesAboveOrBelow(ref string tilePosition, Point checkTilePos)
        {
            switch (tilePosition[0])
            {
                case 't':
                    checkTilePos.Y -= tileSize;
                    if (tiles.ContainsKey(checkTilePos))
                    {
                        if (tiles[checkTilePos]._tileType != 0) return true;
                    }
                    break;
                case 'b':
                    checkTilePos.Y += tileSize;
                    if (tiles.ContainsKey(checkTilePos))
                    {
                        if (tiles[checkTilePos]._tileType != 0) return true;
                    }
                    break;
            }
            return false;
        }
        static bool ThereAreTilesLeftOrRight(ref string tilePosition, Point checkTilePos)
        {
            switch (tilePosition[1])
            {
                case 'l':
                    checkTilePos.X += tileSize;
                    if (tiles.ContainsKey(checkTilePos))
                    {
                        if (tiles[checkTilePos]._tileType != 0) return true;
                    }
                    break;
                case 'r':
                    checkTilePos.X -= tileSize;
                    if (tiles.ContainsKey(checkTilePos))
                    {
                        if (tiles[checkTilePos]._tileType != 0) return true;
                    }
                    break;
            }
            return false;
        }
        static void HandleCornerTile(ref Point checkTilePos, ref Point tilePositionKey, ref string positionAroundTile)
        {
            bool cornerCanBeAllowed =
                        ThereAreTilesAboveOrBelow(ref positionAroundTile, checkTilePos)
                        &&
                        ThereAreTilesLeftOrRight(ref positionAroundTile, checkTilePos);

            if (cornerCanBeAllowed
                && (tiles.ContainsKey(checkTilePos) && tiles[checkTilePos]._tileType != 0))
            {
                tiles[tilePositionKey].tilesAround[positionAroundTile] = (byte)1;
            }
            else
            {
                tiles[tilePositionKey].tilesAround[positionAroundTile] = 0;
            }
        }
        static void HandleNonCornerTile(ref Point checkTilePos, ref Point tilePositionKey, ref string positionAroundTile)
        {
            if (tiles.ContainsKey(checkTilePos) && tiles[checkTilePos]._tileType != 0)
            {
                tiles[tilePositionKey].tilesAround[positionAroundTile] = (byte)1;
            }
            else
            {
                tiles[tilePositionKey].tilesAround[positionAroundTile] = (byte)0;
            }
        }
        static void SetSurroundingsDataForTileAt(Point tilePositionKey)
        {
            foreach (KeyValuePair<string, Vector2> kvp in tilesAroundTile)
            {
                int checkTileX = (int)(tiles[tilePositionKey].Position.X + (kvp.Value.X * tileSize));
                int checkTileY = (int)(tiles[tilePositionKey].Position.Y + (kvp.Value.Y * tileSize));
                Point checkTilePos =  new(checkTileX, checkTileY);
                string positionAroundTile = kvp.Key;

                if (positionAroundTile.Length == 2)
                {
                    HandleCornerTile(
                        ref checkTilePos, 
                        ref tilePositionKey, 
                        ref positionAroundTile);
                }
                else
                {
                    HandleNonCornerTile(ref 
                        checkTilePos, 
                        ref tilePositionKey, 
                        ref positionAroundTile);
                }
            }
        }
        #endregion

        #region Load Map From File Functions
        static void AssignTileByByte(int byteValueToInt, Point tilePos, ref int row, ref int column)
        {
            switch (byteValueToInt)
            {
                case 1:
                    tiles.Add(tilePos, new Tile(new Vector2(column * tileSize, row * tileSize), Color.White));
                    tiles[tilePos]._tileType = 0;
                    column++;
                    break;
                case 2:
                    tiles.Add(tilePos, new Tile(new Vector2(column * tileSize, row * tileSize), Color.White));
                    column++;
                    break;
                case 3:
                    playerSpawnPoint = new Vector2(column * tileSize + 0.5f * tileSize, row * tileSize);
                    column++;
                    break;
                case 4:
                    tiles.Add(tilePos, new Tile(new Vector2(column * tileSize, row * tileSize), Color.White));
                    tiles[tilePos]._tileType = -3;
                    column++;
                    break;
                case 15:
                    row++;
                    column = 0;
                    break;
                default:
                    column++;
                    break;
            }
        }
        static void GetTilesFromFile(BinaryReader binaryReader)
        {
            try
            {
                int lineColumn = 0, lineNumber = 1;
                Point tilePos;
                while (true)
                {
                    tilePos = new(lineColumn * tileSize, lineNumber * tileSize);
                    AssignTileByByte((int)binaryReader.ReadByte(), tilePos, ref lineNumber, ref lineColumn);
                }
            }
            catch (EndOfStreamException) { }
        }
        public static void GetTileMap()
        {
            using (FileStream stream = File.Open($"{levelName}.tilemap", FileMode.Open))
            {
                using (BinaryReader br = new(stream))
                {
                    GetTilesFromFile(br);
                    Tile.LoadTileTexture("SAND DONE-1.png");
                    ConfigureTileTypes();
                }
            }
        }
        #endregion
    }
}
