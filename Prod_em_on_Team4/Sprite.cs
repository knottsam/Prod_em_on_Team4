using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Prod_em_on_Team4
{
    public class Sprite
    {
        protected Texture2D _spriteTexture;
        protected Vector2 _spritePosition;
        protected Rectangle _spriteBox;
        protected Color _spriteColour;

        public Sprite()
        {

        }

        public Sprite(Vector2 spritePosition, Rectangle spriteBox, Color spriteColour)
        {
            _spriteBox = spriteBox;
            _spriteColour = spriteColour;
            _spritePosition = spritePosition;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {   
            spriteBatch.Draw(_spriteTexture, _spritePosition, _spriteColour);

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public void LoadContent(ContentManager myContent, string fileName)
        {
            myContent.RootDirectory = "Content";
            _spriteTexture = myContent.Load<Texture2D>(fileName);

        }
    }
}
