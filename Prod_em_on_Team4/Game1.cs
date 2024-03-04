using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prod_em_on_Team4
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Enemy enemyTest;

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

            enemyTest = new(new Vector2(500, 600), Color.Wheat);

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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        { 
            GraphicsDevice.Clear(Color.Beige);
            Globals.spriteBatch.Begin(transformMatrix:Camera.Transform, samplerState: SamplerState.PointClamp);

            PlayerManager.DrawPlayer();
            TileMap.DrawTiles();
            PlayerManager.DrawPlayerState();

            enemyTest.Draw();

            Globals.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}