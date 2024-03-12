﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Prod_em_on_Team4
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Enemy enemyTest;
        List<Enemy> enemylist = new();
        Song music;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = Globals.screenWidth;
            _graphics.PreferredBackBufferHeight = Globals.screenHeight;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.Content = Content;
            Globals.spriteBatch = _spriteBatch;
            Globals.graphicsDevice = GraphicsDevice;

            music = Content.Load<Song>("new_beat_epic_version");
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;

            enemyTest = new(new Vector2(500, 600), Color.Wheat);
            enemylist.Add(new(new Vector2(6363, 14590), Color.Wheat));
            enemylist.Add(new(new Vector2(16111, 13310), Color.Wheat));
            enemylist.Add(new(new Vector2(17360, 7358), Color.Wheat));
            enemylist.Add(new(new Vector2(10795, 14334), Color.Wheat));
            enemylist.Add(new(new Vector2(13249, 12606), Color.Wheat));
            TileMap.GetTileMap();
            PlayerManager.Spy = new Player(TileMap.playerSpawnPoint, Color.AliceBlue);
            Globals.DefTexture();
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.Update(ref gameTime);

            PlayerManager.Update();
            Camera.Follow(PlayerManager.Spy);
            enemyTest.Update();

            foreach (Enemy enemy in enemylist) 
            {
                enemy.Update();
            }

            if (MediaPlayer.State == MediaState.Stopped) MediaPlayer.Play(music);
            else if(MediaPlayer.State == MediaState.Paused) MediaPlayer.Resume();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        { 
            GraphicsDevice.Clear(Color.Beige);
            Globals.spriteBatch.Begin(transformMatrix:Camera.Transform, samplerState: SamplerState.PointClamp);

            PlayerManager.DrawPlayer();
            TileMap.DrawTiles();
            PlayerManager.DrawPlayerState();

            foreach (Enemy enemy in enemylist)
            {
                enemy.Update();
            }

            enemyTest.Draw();

            Globals.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}