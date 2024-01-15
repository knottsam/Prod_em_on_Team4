using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prod_em_on_Team4
{
    public class Bullet : Sprite
    {
        private Sprite _OwnerSprite;

        private int _bulletSpeed = 20;

        public bool moveUp = true;
        public bool moveDown = true;
        public bool moveRight = true;
        public bool moveLeft = true;

        public bool _startMoving = false;

        public Bullet()
        {
        }
        public Bullet(Vector2 spritePosition,Rectangle spriteBox, Color spriteColour ):base (spritePosition,spriteBox,spriteColour)
        {
            _spriteBox = spriteBox;
            _spriteColour = spriteColour;
            _spritePosition = spritePosition;
        }
        public void ResetToOwner(Sprite ownerSprite)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if(_startMoving == true && moveUp == true)
            {
                _spritePosition.Y -= _bulletSpeed;
            }
            if (_startMoving == true && moveDown == true)
            {
                _spritePosition.Y += _bulletSpeed;
            }
            if (_startMoving == true && moveRight == true)
            {
                _spritePosition.X += _bulletSpeed;
            }
            if (_startMoving == true && moveLeft == true)
            {
                _spritePosition.X -= _bulletSpeed;
            }
            
         

            if (_spritePosition.X > Game1.screenWidth || (_spritePosition.X + _spriteTexture.Width) < 0 || _spritePosition.Y > Game1.screenHeight || (_spritePosition.Y + _spriteTexture.Height) < 0) 
            {
                _startMoving = false;
            }

          

            base.Update(gameTime);
        }

        public Sprite Owner
        {
            get { return _OwnerSprite; }
            set { _OwnerSprite = value; }
        }
    }
}
