using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Prod_em_on_Team4
{
    internal class Player : Sprite
    {
        private float playerYVelocity = 0;
        private float jumpAmount = 20;
        private float jumpCount = 0;
        private int maxJumps = 2;
        private int jumpsAvailable = 0;
        private bool keyDown;


        private int moveSpeed = 10;

        private double canShoot = 0;

        private List<Bullet> bullets = new List<Bullet>();

        private Vector2 shootDirection = new Vector2(1,0);



        public Player() : base()
        { }

        public Player(Rectangle boundingBox, Vector2 spritePosition, Color spriteColour)
            : base(boundingBox, spritePosition, spriteColour)
        {
            _spritePosition = spritePosition;
            _spriteColour = spriteColour;
            _spriteBox = boundingBox;
        }

        private void MovePlayer()
        {
            int horizontalMovement =
                (Convert.ToInt32(Keyboard.GetState().IsKeyDown(Keys.Right))
                - Convert.ToInt32(Keyboard.GetState().IsKeyDown(Keys.Left)))
                * moveSpeed;

            if (horizontalMovement == 0) 
            {

            }
            else if (horizontalMovement < 0)
            {
                if (_spritePosition.X + horizontalMovement > 0)
                {
                    _spritePosition.X += horizontalMovement;
                }
                else
                {
                    _spritePosition.X = 0;
                }

                shootDirection.X = -1;
            }
            else
            {
                if (_spritePosition.X + horizontalMovement < Game1.screenWidth - _spriteTexture.Width)
                {
                    _spritePosition.X += horizontalMovement;
                }
                else
                {
                    _spritePosition.X = Game1.screenWidth - _spriteTexture.Width;
                }

                shootDirection.X = 1;
            }

            if (_spritePosition.Y + playerYVelocity < Game1.screenHeight - _spriteTexture.Height)
            {
                _spritePosition.Y += playerYVelocity;
                playerYVelocity += Game1.gravityAmount;
            }
            //adds gravity to the player
            else
            {
                _spritePosition.Y = Game1.screenHeight - _spriteTexture.Height;
                playerYVelocity = 0;
                jumpsAvailable = maxJumps;
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && jumpsAvailable > 0 && keyDown != true)
            {
                playerYVelocity = -jumpAmount;
                jumpCount += 1;
                jumpsAvailable -= 1;
                keyDown = true;
            }

            if(keyDown) 
            {
                keyDown = !Keyboard.GetState().IsKeyUp(Keys.Up);
            }
            //jumping code
        }

        private void ShootBullet(GameTime gameTime) 
        {
            if (canShoot >= 0.5)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    MouseState mouseState = Mouse.GetState();
                    int mouseX = mouseState.X;
                    int mouseY = mouseState.Y;

                    shootDirection = new Vector2(mouseX - _spritePosition.X - _spriteTexture.Width, mouseY - _spritePosition.Y);
                    shootDirection.Normalize();
                    bullets.Add(
                    new Bullet
                     (
                        new Rectangle(), 
                        new Vector2(_spritePosition.X + _spriteTexture.Width, _spritePosition.Y + (int)(0.5 * _spriteTexture.Height)), 
                        Color.White, 
                        shootDirection
                    )
                    );
                    canShoot = 0;
                }
            }
            else 
            {
                canShoot += (1 / gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        public override void Update(GameTime gameTime)
        {
            MovePlayer();
            ShootBullet(gameTime);

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gameTime);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bullets.Count; i++) 
            {
                bullets[i].Draw(spriteBatch);
                if (bullets[i].IHitSomething) 
                {
                    bullets.RemoveAt(i);
                }
            }

            base.Draw(spriteBatch);
        }
    }
}
