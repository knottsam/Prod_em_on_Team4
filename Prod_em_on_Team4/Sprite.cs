using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;


namespace Prod_em_on_Team4
{
    public class Sprite
    {
        protected Texture2D _spriteTexture;
        protected Vector2 _spritePosition;
        protected Rectangle _spriteBox;
        protected Color _spriteColour;

        public Sprite()
        { }

        public Sprite(Rectangle spriteBox, Vector2 spritePosition, Color spriteColour)
        {
            _spriteBox = spriteBox;
            _spriteColour = spriteColour;
            _spritePosition = spritePosition;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteTexture, _spritePosition, _spriteColour);
        }

        public virtual void LoadContent(ContentManager myContent, string fileName)
        {
            myContent.RootDirectory = "Content";
            _spriteTexture = myContent.Load<Texture2D>(fileName);

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public Color Colour
        {
            get { return _spriteColour; }
            set { _spriteColour = value; }
        }

        public Vector2 Position
        {
            get { return _spritePosition; }
            set { _spritePosition = value; }
        }

        public Rectangle SpriteBox
        {
            get { return _spriteBox; }
            set { _spriteBox = value; }
        }
    }

}
