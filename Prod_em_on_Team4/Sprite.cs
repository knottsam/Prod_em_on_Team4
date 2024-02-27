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
        protected RectangleF _spriteBox;
        protected Color _spriteColour;

        public Vector2 Center { get => new Vector2(_spritePosition.X + (_spriteBox.Width / 2), _spritePosition.Y + (_spriteBox.Height / 2)); }

        public Sprite()
        { }

        public Sprite(ref Vector2 spritePosition, ref Color spriteColour)
        {
            _spriteColour = spriteColour;
            _spritePosition = spritePosition;
        }

        public virtual void Draw()
        {
            Globals.spriteBatch.Draw(_spriteTexture, _spritePosition, _spriteColour);
        }

        public virtual void LoadContent(ref string fileName)
        {
            _spriteTexture = Globals.Content.Load<Texture2D>(fileName);
            _spriteBox = new RectangleF(ref _spritePosition, ref _spriteTexture);
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

        public RectangleF SpriteBox
        {
            get { return _spriteBox; }
            set { _spriteBox = value; }
        }
    }

}