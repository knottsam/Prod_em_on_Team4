using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;



namespace Prod_em_on_Team4
{
    internal class Player : Sprite
    {
        private bool moveLeft;
        private bool moveRight;

        public Player() : base()
        { }

        public Player(Rectangle boundingBox, Vector2 spritePosition, Color spriteColour)
            : base(boundingBox, spritePosition, spriteColour)
        {
            _spritePosition = spritePosition;
            _spriteColour = spriteColour;
            _spriteBox = boundingBox;
        }
        public override void Update(GameTime gameTime, bool gameStarted, int ScreenWidth)
        {
            _spritePosition = new Vector2(_spritePosition.X, _spritePosition.Y);

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                moveLeft = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                moveRight = true;
            }

            if (moveLeft == true && _spritePosition.X > 0)
            {
                _spritePosition.X -= 10;
                moveLeft = false;
            }

            if (moveRight == true && _spritePosition.X < ScreenWidth - _spriteTexture.Width)
            {
                _spritePosition.X += 10;
                moveRight = false;
            }
            //do stuff using the keyboard
        }

        public void LoadContent(ContentManager myContent)
        {
            myContent.RootDirectory = "Content";
            _spriteTexture = myContent.Load<Texture2D>("player2.0");

        }

    }
}
