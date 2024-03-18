using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Prod_em_on_Team4
{
    public class Player : Sprite
    {
        float jumpOffWallAmount = 600f, glideAmount = 0.95f;
        int jumpsAvailable = 0, moveSpeed = 600;
        double shootDelay = 0;
        bool canDash = false;
        public string State { get => playerStates.Max().ToString(); }
        HashSet<PlayerState> playerStates = new();
        public enum PlayerState : sbyte
        {
            Airborne = -1,
            Jumping,
            Falling,
            Standing,
            Walking,
            Running,
            Gliding,
            WallClimbing,
            Dashing
        }
        public Dictionary<PlayerState, bool> playerUseState = new() { { PlayerState.Dashing, false }, { PlayerState.Gliding, false } };
        List<Bullet> bullets = new List<Bullet>();
        Vector2 dashVel = Vector2.Zero;
        public Vector2 Velocity = Vector2.Zero, facingDirection = new Vector2(1, 0);
        const float jumpAmount = 1200f, wallFriction = 0.9f; //terminalVelocity = 320;
        const int maxJumps = 2, dashSpeed = 48;

        private Texture2D HealthBar;

        private readonly int maxHP = 10;
        public int HP;

        AnimationManager myAnimations;

        public Player(Vector2 spritePosition, Color spriteColour) : base(ref spritePosition, ref spriteColour)
        {
            _spriteBox = new RectangleF(ref _spritePosition);

            HealthBar = Globals.Content.Load<Texture2D>("HealthBar");
            HP = maxHP;

            myAnimations = new(new Dictionary<string, Animation>()
            {
                {"Standing", new Animation(Globals.LoadTexture("Spy1 COMPLETE"), 1, 0)},
                {"Walking" , new Animation(Globals.LoadTexture("Spy Base Walk (f8)"), 8, 100)},
                {"Running" , new Animation(Globals.LoadTexture("FULLY VINCED UP SPY-Sheet"), 9, 75)},
            });

            SetAnimation("Standing");

            Bullet.LoadContent("GameBullet");
        }

        public void Update()
        {
            Move();
            ShootBullet();
            AttackDash();

            ManageAnimation();

            foreach (Bullet bullet in bullets) { bullet.Update(); }
        }

        public override void Draw()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw();
                if (bullets[i].IHitSomething) { bullets.RemoveAt(i); }
            }

            myAnimations.Draw(ref _spritePosition, (facingDirection.X == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

            Globals.spriteBatch.Draw(HealthBar, Position - new Vector2(0, 16), Color.Red);
            Globals.spriteBatch.Draw(HealthBar, Position - new Vector2(0, 16), new Rectangle(0, 0, HealthBar.Width * HP / maxHP, HealthBar.Height), Color.Green);
        }

        void AttackDash()
        {
            if (InputManager.HaveIJustPressed(Keys.X) && canDash)
            {
                Vector2 dashDirection = InputManager.Direction.ToVector2();

                if (((dashDirection.X != 0) ^ (dashDirection.Y != 0)) && dashDirection.Y >= 0)
                {
                    dashDirection.Normalize();
                }
                else
                {
                    dashDirection = facingDirection;
                }

                playerUseState[PlayerState.Dashing] = true;

                jumpsAvailable = 0;
                Velocity = dashVel = dashDirection * dashSpeed;
                canDash = false;
            }
        }

        void ShootBullet()
        {
            if (shootDelay >= 0.1)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                {
                    bullets.Add(
                    new Bullet
                    (
                        new Vector2(_spritePosition.X + _spriteBox.Width, _spritePosition.Y + (int)(0.5 * _spriteBox.Height)),
                        Color.Red,
                        ref facingDirection
                    )
                    );
                    shootDelay = 0;
                }
            }
            else
            {
                shootDelay += (1 / Globals.TotalMilliseconds);
            }
        }

        #region Movement Functions
        RectangleF shadowYCollision()
        {
            float Y1 = (Velocity.Y > 0) ? _spriteBox.Bottom : _spriteBox.Top;
            return new RectangleF(new Vector2(_spriteBox.Left, Y1), new Vector2(_spriteBox.Right, Y1 + Velocity.Y));
        }
        void HandleVerticalMovement(ref bool yCollision)
        {
            RectangleF collisionArea = shadowYCollision();
            playerStates.Add((Velocity.Y > 0) ? PlayerState.Falling : PlayerState.Jumping);
            int signOfYVelocity = Math.Sign(Velocity.Y);
            Point collisionAreaLocation = new Point((int)collisionArea.X, (int)collisionArea.Y);

            for (int l = -1; (l + 1) < collisionArea.Height / 64; l++)
            {
                foreach (Tile t in TileMap.GetTilesAround(new Point(collisionAreaLocation.X, collisionAreaLocation.Y + (64 * l * signOfYVelocity)), "Only-Y", signOfYVelocity))
                {
                    if (t.SpriteBox.Intersects(ref collisionArea))
                    {
                        yCollision = true;
                        if (Velocity.Y > 0)
                        {
                            canDash = true;
                            playerStates.Add(PlayerState.Standing);
                            playerUseState[PlayerState.Gliding] = false;
                            jumpsAvailable = maxJumps;

                            _spriteBox.Y = (t.SpriteBox.Top - _spriteBox.Height);
                        }
                        else
                        {
                            _spriteBox.Y = t.SpriteBox.Bottom;
                        }

                        Velocity.Y = 1;
                        dashVel.Y = 0;
                        break;
                    }
                }
                if (yCollision) { break; }
            }
            if (!yCollision)
            {
                _spriteBox.Y += Velocity.Y;
                if (dashVel.X == 0)
                {
                    Velocity.Y += Globals.gravityAmount * Globals.TotalSeconds;  // Player accelerates down (GRAVITY!!!)
                }
            }
        }
        void HandleHorizontalMovement(ref bool thereWasAYCollision, out float horizontalMovement)
        {
            bool amWalking = Keyboard.GetState().IsKeyDown(Keys.S);

            horizontalMovement = (Velocity.X == 0) ? InputManager.Direction.X * (amWalking ? moveSpeed / 2 : moveSpeed) * Globals.TotalSeconds : 0;
            float xMove = Velocity.X + horizontalMovement;

            playerUseState[PlayerState.WallClimbing] = false;
            if (xMove != 0)
            {
                myAnimations.UpdateAnimation();
                if (horizontalMovement != 0)
                {
                    if (!amWalking)
                    {
                        playerStates.Add(PlayerState.Running);
                    }
                    else
                    {
                        playerStates.Add(PlayerState.Walking);
                    }

                    facingDirection.X = Math.Sign(horizontalMovement);
                    _spriteBox.X += horizontalMovement;
                }
                else { _spriteBox.X += Velocity.X; }

                foreach (Tile t in TileMap.GetTilesAround(_spritePosition.ToPoint(), "Only-X", Math.Sign(xMove)))
                {
                    if (_spriteBox.Intersects(t.SpriteBox)) // Collision!
                    {
                        _spriteBox.X = ((xMove) > 0) ? t.SpriteBox.Left - _spriteBox.Width : t.SpriteBox.Right;

                        if (!thereWasAYCollision)
                        {
                            playerUseState[PlayerState.WallClimbing] = true;
                            playerStates.Add(PlayerState.WallClimbing);
                            canDash = true;

                            jumpsAvailable = 1;
                            if (Velocity.Y > 0) { Velocity.Y -= wallFriction; }
                        }
                        Velocity.X = dashVel.X = 0;
                        break;
                    }
                }
            }
            if (Velocity.X != 0)
            {
                float xVelocityDelta = Math.Sign(Velocity.X) * Globals.airResistance * Globals.TotalSeconds * ((playerUseState[PlayerState.Dashing]) ? 6f : 1);
                Velocity.X -= (Math.Abs(Velocity.X) - xVelocityDelta > 0) ? xVelocityDelta : Velocity.X;
            }

            if ((xMove) == 0 && (Velocity.Y > 0 && thereWasAYCollision))
            {
                playerStates.Add(PlayerState.Standing);
            }
        }
        bool HandleJump(ref float horizontalMovement)
        {
            if (InputManager.HaveIJustPressed(Keys.Up) && jumpsAvailable > 0)
            {
                Velocity.Y = -jumpAmount * Globals.TotalSeconds;
                jumpsAvailable -= 1;

                if (playerUseState[PlayerState.WallClimbing])
                {
                    Velocity.X = (horizontalMovement < 0) ? jumpOffWallAmount * Globals.TotalSeconds : -jumpOffWallAmount * Globals.TotalSeconds;
                }
                return true;
            }
            return false;
        }
        void HandleGlide()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && jumpsAvailable == 0 && Velocity.Y > 0)
            {
                if (playerUseState[PlayerState.Gliding])
                {
                    Velocity.Y = -Velocity.Y * Globals.TotalSeconds;
                    playerUseState[PlayerState.Gliding] = true;
                }
                Velocity.Y -= glideAmount * Globals.gravityAmount * Globals.TotalSeconds;
                playerStates.Add(PlayerState.Gliding);
            }
        }
        void HandleDash()
        {
            if (playerUseState[PlayerState.Dashing])
            {
                if (Velocity.X == 0)
                {
                    dashVel.X = 0;
                }
                if (Velocity.Y == 1)
                {
                    dashVel.Y = 0;
                }

                if (dashVel == Vector2.Zero)
                {
                    playerUseState[PlayerState.Dashing] = false;
                }
            }
        }
        void Move()
        {
            playerStates.Clear();

            bool thereWasAYCollision = false;
            float horizontalMovement;

            HandleVerticalMovement(ref thereWasAYCollision);
            HandleHorizontalMovement(ref thereWasAYCollision, out horizontalMovement);
            if (!HandleJump(ref horizontalMovement)) HandleGlide();
            HandleDash();

            _spritePosition.X = _spriteBox.X;
            _spritePosition.Y = _spriteBox.Y;
        }
        #endregion

        #region Animation Functions
        void ManageAnimation()
        {
            switch (playerStates.Max().ToString())
            {
                case "Walking":
                    if (myAnimations.currentAnimation != "Walking")
                    {
                        SetAnimation("Walking");
                    }
                    break;

                case "Running":
                    if (myAnimations.currentAnimation != "Running")
                    {
                        SetAnimation("Running");
                    }
                    break;

                case "Standing":
                    if (myAnimations.currentAnimation != "Standing")
                    {
                        SetAnimation("Standing");
                    }
                    break;
            }
        }
        private void SetAnimation(string animationName)
        {
            myAnimations.SwitchAnimation(ref animationName);
            _spriteBox.Size = myAnimations.Size().ToVector2();
        }
        #endregion
    }
}