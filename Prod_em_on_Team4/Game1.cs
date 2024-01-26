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
        private Player firstPlayer;

        public static int screenWidth;
        public static int screenHeight;
        public static float gravityAmount = 1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 832;

            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            firstPlayer = new Player(new Rectangle(), new Vector2(_graphics.PreferredBackBufferWidth / 2, 0), Color.AliceBlue);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            firstPlayer.LoadContent(Content, "player2.0");
            Bullet.LoadContent(Content, "GameBullet");
            // TODO: use this.Content to load your game content here

            TileMap.GetTileMap(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            firstPlayer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        { 

            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            TileMap.DrawTiles(_spriteBatch);
            firstPlayer.Draw(_spriteBatch);

            _spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}