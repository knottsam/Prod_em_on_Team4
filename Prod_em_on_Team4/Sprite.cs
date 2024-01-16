﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;


namespace Prod_em_on_Team4
{
    public class Sprite : Game1
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

        public void LoadContent(ContentManager myContent, string fileName)
        {
            myContent.RootDirectory = "Content";
            _spriteTexture = myContent.Load<Texture2D>(fileName);

        }

        public virtual void Update(GameTime gameTime, bool gameStarted, int ScreenWidth) 
        {
            
        }
    }

}