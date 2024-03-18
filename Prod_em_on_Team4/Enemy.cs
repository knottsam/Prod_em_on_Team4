using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Prod_em_on_Team4
{
    internal class Enemy : Sprite
    {
        Vector2 Velocity = new(0, 1);

        public Enemy(Vector2 spritePosition, Color spriteColour) : base(ref spritePosition, ref spriteColour)
        {
            _spriteBox = new RectangleF(ref _spritePosition);
            string enemyTexture = "player2.0";
            LoadContent(ref enemyTexture);
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

            _spritePosition.X = _spriteBox.X;
            _spritePosition.Y = _spriteBox.Y;

        }

        public void Update()
        {
            Gravity();
        }
    }
}