using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Prod_em_on_Team4
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //public static Enemy enemyTest;

        public static List<Enemy> enemylist = new();
        Song music;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = Globals.screenWidth;
            _graphics.PreferredBackBufferHeight = Globals.screenHeight;

            music = Content.Load<Song>("new_beat_epic_version");
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;

            
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

            //enemyTest = new(new Vector2(500, 600), Color.Wheat);

            TileMap.GetTileMap();
            PlayerManager.Spy = new Player(TileMap.playerSpawnPoint, Color.AliceBlue);
            Globals.DefTexture();

            enemylist.Add(new(new Vector2(500, 600), Color.Wheat));
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.Update(ref gameTime);

            PlayerManager.Update();
            if (PlayerManager.Spy.HP > 0 && enemylist.Count > 0) Camera.Follow(PlayerManager.Spy);
            //enemyTest.Update();


            for (int i = 0; i < enemylist.Count; i++) 
            {
                enemylist[i].Update();

                if (enemylist[i].HP <= 0)
                {
                    enemylist.RemoveAt(i);
                    i --;
                }
            }

            if (MediaPlayer.State == MediaState.Stopped) MediaPlayer.Play(music);
            else if (MediaPlayer.State == MediaState.Paused) MediaPlayer.Resume();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        { 
            GraphicsDevice.Clear(Color.Beige);
            Globals.spriteBatch.Begin(transformMatrix:Camera.Transform, samplerState: SamplerState.PointClamp);



            if (PlayerManager.Spy.HP > 0)
            {
                if (enemylist.Count > 0) 
                {
                    PlayerManager.DrawPlayerState();
                    PlayerManager.DrawPlayer();
                    TileMap.DrawTiles();
                }
                else
                {
                    Globals.spriteBatch.DrawString(PlayerManager.stateFont,
                    "VICTORY",
                    PlayerManager.Spy.Position
                    - (PlayerManager.stateFont.MeasureString(PlayerManager.Spy.State) / 2)
                    + new Vector2(PlayerManager.Spy.SpriteBox.Width / 2, 0),
                    Color.Red, 0, Vector2.Zero, 5, SpriteEffects.None, 0);
                }
                
            }
            else
            {
                Globals.spriteBatch.DrawString(PlayerManager.stateFont,
                "YOU DIED BRUH...",
                PlayerManager.Spy.Position
                - (PlayerManager.stateFont.MeasureString(PlayerManager.Spy.State) / 2)
                + new Vector2(PlayerManager.Spy.SpriteBox.Width / 2, 0),
                Color.Red, 0, Vector2.Zero, 5, SpriteEffects.None, 0);
            }

            //enemyTest.Draw();

            foreach (Enemy enemy in enemylist)
            {
                enemy.Draw();
            }

            Globals.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}