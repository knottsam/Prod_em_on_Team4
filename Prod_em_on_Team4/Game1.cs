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

        private Bullet _playerBullet;


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
            firstPlayer = new Player(new Rectangle(), new Vector2(_graphics.PreferredBackBufferWidth / 2, 0), Color.AliceBlue);

            _playerBullet = new Bullet();
            _playerBullet.Colour = Color.White;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            firstPlayer.LoadContent(Content, "player2.0");
            _playerBullet.LoadContent(Content, "GameBullet");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //Debug.Write(Keyboard.GetState().IsKeyDown(Keys.Right)); Debug.Write(Keyboard.GetState().IsKeyDown(Keys.Left));
            firstPlayer.Update(gameTime);
            // TODO: Add your update logic here


            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _playerBullet.moveUp = true;
                _playerBullet.moveDown = false;
                _playerBullet.moveLeft = false;
                _playerBullet.moveRight = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _playerBullet.moveUp = false;
                _playerBullet.moveDown = true;
                _playerBullet.moveLeft = false;
                _playerBullet.moveRight = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _playerBullet.moveUp = false;
                _playerBullet.moveDown = false;
                _playerBullet.moveLeft = false;
                _playerBullet.moveRight = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _playerBullet.moveUp = false;
                _playerBullet.moveDown = false;
                _playerBullet.moveLeft = true;
                _playerBullet.moveRight = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                _playerBullet.moveUp = true;
                _playerBullet.moveDown = false;
                _playerBullet.moveLeft = true;
                _playerBullet.moveRight = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                _playerBullet.moveUp = true;
                _playerBullet.moveDown = false;
                _playerBullet.moveLeft = false;
                _playerBullet.moveRight = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.CapsLock))
            {
                _playerBullet.moveUp = false;
                _playerBullet.moveDown = true;
                _playerBullet.moveLeft = true;
                _playerBullet.moveRight = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                _playerBullet.moveUp = false;
                _playerBullet.moveDown = true;
                _playerBullet.moveLeft = false;
                _playerBullet.moveRight = true;
            }


            MouseState mouseState = Mouse.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.B))
            {
                _playerBullet._startMoving = true;
                _playerBullet.Position = new Vector2(mouseState.X, mouseState.Y);
                Debug.WriteLine("mouse");
            }

            //_playerBullet.SpritePosition;
            // TODO: Add your update logic here

            _playerBullet.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _playerBullet.Draw(_spriteBatch);
            firstPlayer.Draw(_spriteBatch);
            _spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}