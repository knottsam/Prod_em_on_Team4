﻿using Microsoft.Xna.Framework.Graphics;
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
<<<<<<< Updated upstream
        private float playerYVelocity = 0;
        private float jumpAmount = 20;
        private float jumpCount = 0;
        private int maxJumps = 2;
        private int jumpsAvailable = 0;
        private bool keyDown;
=======
        float playerYVelocity = 0, playerXVelocity = 0, wallJumpAmount = 10;
        int jumpsAvailable = 0, moveSpeed = 10;
        double shootDelay = 0;
>>>>>>> Stashed changes

        bool keyDown, clingingToWall;

        List<Bullet> bullets = new List<Bullet>();
        Vector2 shootDirection = new Vector2(1,0); 

        // Constant values that will not be changed at runtime
        const float jumpAmount = 20f, wallFriction = 0.9f, terminalVelocity = 64;
        const int maxJumps = 2;

        public Player(Vector2 spritePosition, Color spriteColour) : base(spritePosition, spriteColour) {}

        void MovePlayer()
        {
            #region Vertical Movement
            _spriteBox.Y += playerYVelocity;
            bool thereWasAYCollision = false; 
            // Checks all the tiles that are within 2 tiles around you, to see if you collide with them
            foreach (Tile t in TileMap.GetTilesAround(_spritePosition))
            {
                if (_spriteBox.Intersects(t.SpriteBox)) // Collision!
                {
                    thereWasAYCollision = true;

                    // If you were moving up, you go to the bottom of the tile. If you were moving down, you go to the top of the tile
                    _spriteBox.Y = (playerYVelocity > 0) ? (t.SpriteBox.Top - _spriteBox.Height) : t.SpriteBox.Bottom;

                    // If you are now standing on top of a tile, your jumps get reset so that you can double jump again
                    if (playerYVelocity > 0) { jumpsAvailable = maxJumps; }
                    playerYVelocity = 0;

                    break;
                }
            }
            if (!thereWasAYCollision) { playerYVelocity += (playerYVelocity < terminalVelocity) ? Game1.gravityAmount : (terminalVelocity - playerYVelocity); } // Player accelerates down (GRAVITY!!!)
            #endregion

            #region Horizontal Movement
            /* Explaing Horizontal Movement User Input
             
             If you are Pressing Right, 
                - (Convert.ToInt32(Keyboard.GetState().IsKeyDown(Keys.Right)) returns 1
             If you are not pressing right,
                - (Convert.ToInt32(Keyboard.GetState().IsKeyDown(Keys.Right)) returns 0

            This works with the subtrahend → (Convert.ToInt32(Keyboard.GetState().IsKeyDown(Keys.Right))

            If you press right,
                - horizontalMovement = (1 - 0) * moveSpeed
            If you press left,
                - horizontalMovement = (0 - 1) * moveSpeed
            If you press both left and right,
                - horizontalMovement = (1 - 1) * moveSpeed
            If you press neither left nor right,
                - horizontalMovement = (0 - 0) * moveSpeed
             */
            int horizontalMovement =
                (Convert.ToInt32(Keyboard.GetState().IsKeyDown(Keys.Right))
                - Convert.ToInt32(Keyboard.GetState().IsKeyDown(Keys.Left)))
                * moveSpeed;

            // If the player velocity is zero, the player mvoes by the horizontal movement.
            // If there is a non-zero player velocity, the player moves by the player velocity
            _spriteBox.X += (Math.Abs(playerXVelocity) > 0) ? playerXVelocity : horizontalMovement;

            // Checks all the tiles that are within 2 tiles around you, to see if you collide with them
            foreach (Tile t in TileMap.GetTilesAround(_spritePosition)) 
            {
                if (_spriteBox.Intersects(t.SpriteBox)) // Collision!
                {
                    // If you were moving right, you go to the left of the tile. If you were moving left, you go to the right of the tile
                    _spriteBox.X = (horizontalMovement > 0 && Math.Abs(playerXVelocity) == 0) || (playerXVelocity > 0 && Math.Abs(playerXVelocity) > 0) ? t.SpriteBox.Left - _spriteBox.Width : _spriteBox.X = t.SpriteBox.Right;
                    
                    playerXVelocity = 0;
                    if (!thereWasAYCollision) 
                    {
                        clingingToWall = true;
                        jumpsAvailable = 1;
                        if (playerYVelocity > 0) { playerYVelocity -= wallFriction; }
                    }
                    break;
                }
            }
            if (thereWasAYCollision) { clingingToWall = false; }
            if (Math.Abs(playerXVelocity) > 0) { playerXVelocity -= Math.Sign(playerXVelocity) * Game1.airResistance; }
            #endregion

            #region Jump
            // You can only jump if:
            // - You have the just pressed the up button
            // - You have jumps available
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && jumpsAvailable > 0 && keyDown != true)
            {
<<<<<<< Updated upstream
                _spritePosition.Y += playerYVelocity;
                playerYVelocity += Game1.gravityAmount;
                if (playerYVelocity > 0 && _spritePosition.X == 0 ||  playerYVelocity > 0 &&_spritePosition.X == Game1.screenWidth - _spriteTexture.Width)
                {
                    playerYVelocity -= 0.8f;
                }
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
                if(_spritePosition.X == 0 || _spritePosition.X == Game1.screenWidth - _spriteTexture.Width )
                { jumpsAvailable = 1; }
                else { jumpsAvailable -= 1; }
                keyDown = true;
            }

            if(keyDown) 
            {
                keyDown = !Keyboard.GetState().IsKeyUp(Keys.Up);
            }
            //jumping code
=======
                playerYVelocity = -jumpAmount;
                jumpsAvailable -= 1;
                keyDown = true;

                if(clingingToWall)
                {
                    // Makes the player jump away from the wall by checking whether you are on a wall to the left or to the right
                    playerXVelocity = (horizontalMovement < 0) ? wallJumpAmount : -wallJumpAmount;
                }
            }
            if (keyDown) { keyDown = !Keyboard.GetState().IsKeyUp(Keys.Up); }

            // Brings the player's X Velocity to 0, whether it's positive or negative
            #endregion

            // Sets the sprites actual Position.
            // We draw the player using its _spritePosition.
            // All collision checks are made using the player's bounding box, not the actual player position.
            _spritePosition.X = _spriteBox.X;
            _spritePosition.Y = _spriteBox.Y;

            Debug.WriteLine(playerYVelocity);
>>>>>>> Stashed changes
        }

        void ShootBullet(GameTime gameTime) 
        {
            if (shootDelay >= 0.5)
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
                        new Vector2(_spritePosition.X + _spriteTexture.Width, _spritePosition.Y + (int)(0.5 * _spriteTexture.Height)), 
                        Color.Red, 
                        shootDirection
                    )
                    );
                    shootDelay = 0;
                }
            }
            else 
            {
                shootDelay += (1 / gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        public override void Update(GameTime gameTime)
        {
            MovePlayer();
            ShootBullet(gameTime);
            for (int i = 0; i < bullets.Count; i++) { bullets[i].Update(gameTime); }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
                if (bullets[i].IHitSomething) { bullets.RemoveAt(i); }
            }
            base.Draw(spriteBatch);
        }
    }
}
