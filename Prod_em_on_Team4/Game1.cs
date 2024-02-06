using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Prod_em_on_Team4
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player Spy;

        public static int screenWidth, screenHeight;
        public static float gravityAmount = 1;
        public static float airResistance = 1;
      

        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 800;

            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Bullet.LoadContent(Content, "GameBullet");
            // TODO: use this.Content to load your game content here

            TileMap.GetTileMap(Content);
            Spy = new Player(TileMap.playerSpawnPoint, Color.AliceBlue);
            Spy.LoadContent(Content, "player2.0");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // TODO: Add your update logic here

            Spy.Update(gameTime);

            Camera.Follow(Spy);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        { 
            GraphicsDevice.Clear(Color.Beige);
            _spriteBatch.Begin(transformMatrix:Camera.Transform, samplerState: SamplerState.PointClamp);

            Spy.Draw(_spriteBatch);
            TileMap.DrawTiles(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}