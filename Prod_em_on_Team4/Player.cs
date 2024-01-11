using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;



namespace Prod_em_on_Team4
{
    internal class Player : Sprite
    {
        
        public Player() : base()
        { }

        public Player(Rectangle boundingBox, Vector2 spritePosition, Color spriteColour)
            : base(boundingBox, spritePosition, spriteColour)
        {
            _spritePosition = spritePosition;
            _spriteColour = spriteColour;
            _spriteBox = boundingBox;
        }

        public void LoadContent(ContentManager myContent)
        {
            myContent.RootDirectory = "Content";
            _spriteTexture = myContent.Load<Texture2D>("player2.0");

        }

    }
}
