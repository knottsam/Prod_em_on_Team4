using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Prod_em_on_Team4
{
    public class Bullet : Sprite
    {
        private int _bulletSpeed = 30;
        private bool _IHitSomething = false;
        public bool IHitSomething { get => _IHitSomething; }

        private Vector2 _direction;

        private static Texture2D _bulletTexture;

        public Bullet()
        {
        }
        public Bullet(Rectangle spriteBox, Vector2 spritePosition, Color spriteColour, Vector2 direction) : base(spriteBox, spritePosition, spriteColour)
        {
            _direction = direction;
            direction.Normalize();
        }

        public static new void LoadContent(ContentManager myContent, string fileName) 
        {
            myContent.RootDirectory = "Content";
            _bulletTexture = myContent.Load<Texture2D>(fileName);
        }

        public override void Update(GameTime gameTime)
        {
            int xMove = (int)(_direction.X * _bulletSpeed);
            int yMove = (int)(_direction.Y * _bulletSpeed);

            if (_spritePosition.X + xMove > 0)
            {
                if (_spritePosition.X + xMove < Game1.screenWidth - _bulletTexture.Width)
                {
                    _spritePosition.X += xMove;
                }
                else 
                {
                    _spritePosition.X = Game1.screenWidth - _bulletTexture.Width;
                    _IHitSomething = true;
                }
            }
            else 
            {
                _spritePosition.X = 0;
                _IHitSomething = true;
            }

            if(_spritePosition.Y + yMove > 0)
            {
                if (_spritePosition.Y + yMove < Game1.screenHeight - _bulletTexture.Height)
                {
                    _spritePosition.Y += yMove;
                }
                else
                {
                    _spritePosition.Y = Game1.screenHeight - _bulletTexture.Height;
                    _IHitSomething = true;
                }
            }
            else
            {
                _spritePosition.Y = 0;
                _IHitSomething = true;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bulletTexture, _spritePosition, _spriteColour);
        }
    }
}