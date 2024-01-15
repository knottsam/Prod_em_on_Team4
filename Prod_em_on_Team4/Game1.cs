using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prod_em_on_Team4
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Bullet _playerBullet;

        public static int screenHeight;
        public static int screenWidth;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight =  _graphics.PreferredBackBufferHeight;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //this doesnt work
            _playerBullet = new Bullet();
            _playerBullet.Colour = Color.White; 

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _playerBullet.LoadContent(Content, "GameBullet");
          

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Keyboard.GetState().IsKeyDown(Keys.Up))
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
           else  if (Keyboard.GetState().IsKeyDown(Keys.Right))
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
            else if (Keyboard.GetState().IsKeyDown (Keys.Q))
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

            if (Mouse.GetState().LeftButton == ButtonState.Pressed) 
            {
                _playerBullet._startMoving = true;
                _playerBullet.Position = new Vector2(mouseState.X, mouseState.Y);
            }

            //_playerBullet.SpritePosition;
            // TODO: Add your update logic here

            _playerBullet.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _playerBullet.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}