using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;

namespace Prod_em_on_Team4
{
    internal class Player : Sprite
    {
        float playerYVelocity = 0, playerXVelocity = 0, wallJumpAmount = 10;
        int jumpsAvailable = 0, moveSpeed = 10;
        double shootDelay = 0;

        bool keyDown, clingingToWall;
        bool coyoteTime = true;
        int framesPassed = 0;
        

        

        List<Bullet> bullets = new List<Bullet>();
        Vector2 shootDirection = new Vector2(1,0); 

        // Constant values that will not be changed at runtime
        const float jumpAmount = 20f, wallFriction = 0.9f, terminalVelocity = 64;
        const int maxJumps = 2;

        public Player(Vector2 spritePosition, Color spriteColour) : base(spritePosition, spriteColour) {}
        
        void MovePlayer(GameTime gameTime)
        {
            #region Vertical Movement
            _spriteBox.Y += playerYVelocity;
            bool thereWasAYCollision = false; 
            // Checks all the tiles that are within 2 tiles around you, to see if you collide with them
            foreach (Tile t in TileMap.GetTilesAround(_spritePosition.ToPoint()))
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
            foreach (Tile t in TileMap.GetTilesAround(_spritePosition.ToPoint())) 
            {
                if (_spriteBox.Intersects(t.SpriteBox)) // Collision!
                {
                    // If you were moving right, you go to the left of the tile. If you were moving left, you go to the right of the tile
                    _spriteBox.X = (horizontalMovement > 0 && Math.Abs(playerXVelocity) == 0) || (playerXVelocity > 0 && Math.Abs(playerXVelocity) > 0) ? t.SpriteBox.Left - _spriteBox.Width : _spriteBox.X = t.SpriteBox.Right;
                    
                    playerXVelocity = 0;
                    if (!thereWasAYCollision && Keyboard.GetState().IsKeyDown(Keys.Left) || !thereWasAYCollision && Keyboard.GetState().IsKeyDown(Keys.Right))
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
            // code for the coyoteTime however using it disables double jump and i cant figure out a workaround that allows for both.

            //if(!thereWasAYCollision && !clingingToWall)
            //{
                //framesPassed += 1;
                //if(framesPassed > 8)
                //{
                    //coyoteTime = false;
                //}
               
            //}
            //if (thereWasAYCollision) { coyoteTime = true; framesPassed = 0; }   
            
            // You can only jump if:
            // - You have the just pressed the up button
            // - You have jumps available
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && jumpsAvailable > 0 && keyDown != true) //&& coyoteTime == true)
            {
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
        }

        void ShootBullet(GameTime gameTime) 
        {
            if (shootDelay >= 0.5)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
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
            MovePlayer(gameTime);
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
