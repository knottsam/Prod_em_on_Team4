using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Prod_em_on_Team4
{
    public class Enemy : Sprite
    {
        Vector2 Velocity = new(0, 1);

        private readonly int maxHP = 10;
        public int HP;

        private Texture2D HealthBar;

        public Enemy(Vector2 spritePosition, Color spriteColour) : base(ref spritePosition, ref spriteColour)
        {
            HP = maxHP;
            _spriteBox = new RectangleF(ref _spritePosition);
            string enemyTexture = "player2.0";
            LoadContent(ref enemyTexture);

            HealthBar = Globals.Content.Load<Texture2D>("HealthBar");
        }

        RectangleF shadowYCollision()
        {
            float Y1 = (Velocity.Y > 0) ? _spriteBox.Bottom : _spriteBox.Top;
            return new RectangleF(new Vector2(_spriteBox.Left, Y1), new Vector2(_spriteBox.Right, Y1 + Velocity.Y));
        }

        private void Gravity()
        {
            RectangleF collisionArea = shadowYCollision();
            Point collisionAreaLocation = new Point((int)collisionArea.X, (int)collisionArea.Y);
            int signOfYVelocity = Math.Sign(Velocity.Y);
            bool yCollision = false;

            for (int l = -1; (l + 1) < collisionArea.Height / 64; l++)
            {
                foreach (Tile t in TileMap.GetTilesAround(new Point(collisionAreaLocation.X, collisionAreaLocation.Y + (64 * l * signOfYVelocity)), "Only-Y", signOfYVelocity))
                {
                    if (t.SpriteBox.Intersects(ref collisionArea))
                    {
                        yCollision = true;
                        if (Velocity.Y > 0)
                        {
                            _spriteBox.Y = (t.SpriteBox.Top - _spriteBox.Height);
                        }
                        else
                        {
                            _spriteBox.Y = t.SpriteBox.Bottom;
                        }

                        Velocity.Y = 1;
                        break;
                    }
                }
                if (yCollision) { break; }
            }
            if (!yCollision)
            {
                _spriteBox.Y += Velocity.Y;
                Velocity.Y += Globals.gravityAmount * Globals.TotalSeconds;
            }
        }

        private float walkSpeed = 3;

        private void Walk()
        {
            foreach (Tile t in TileMap.GetTilesAround(_spritePosition.ToPoint() + new Point(16, 0), "Only-X", Math.Sign(walkSpeed), true))
            {
                if (_spriteBox.Intersects(t.SpriteBox)) // Collision!
                {
                    _spriteBox.X = ((walkSpeed) > 0) ? t.SpriteBox.Left - _spriteBox.Width : t.SpriteBox.Right;
                    walkSpeed *= -1;
                    break;
                }
            }
            _spriteBox.X += walkSpeed;
        }

        public void Update()
        {
            Gravity();
            Walk();

            if (_spriteBox.Intersects(PlayerManager.Spy.SpriteBox) && !PlayerManager.Spy.playerUseState[Player.PlayerState.Dashing])
            {
                if (MathF.Sign(PlayerManager.Spy.Position.X) == MathF.Sign(walkSpeed))
                {
                    PlayerManager.Spy.Velocity = new Vector2(20 * MathF.Sign(walkSpeed), -10);
                }
                else
                {
                    PlayerManager.Spy.Velocity = new Vector2(20 * -MathF.Sign(-walkSpeed), -10);
                }

                PlayerManager.Spy.HP--;
            }

            _spritePosition.X = _spriteBox.X;
            _spritePosition.Y = _spriteBox.Y;
        }


        public override void Draw()
        {
            base.Draw();

            Globals.spriteBatch.Draw(HealthBar, Position - new Vector2(0, 16), Color.Red);
            Globals.spriteBatch.Draw(HealthBar, Position - new Vector2(0, 16), new Rectangle(0, 0, HealthBar.Width * HP / maxHP, HealthBar.Height), Color.Green);
 
            _spriteColour = Color.White;

        }
    }
}