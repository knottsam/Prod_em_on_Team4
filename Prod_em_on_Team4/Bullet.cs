using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Prod_em_on_Team4
{
    public class Bullet : Sprite
    {
        int _bulletSpeed = 30;
        bool _IHitSomething = false;
        Vector2 dist = new Vector2(0, 0);
        public bool IHitSomething { get => _IHitSomething; }

        Vector2 _direction;

        static Texture2D _bulletTexture;

        public Bullet(Vector2 spritePosition, Color spriteColour, ref Vector2 direction) : base(ref spritePosition, ref spriteColour)
        {
            _direction = direction;
            direction.Normalize();
            _spritePosition.Y -= 0.5f * _bulletTexture.Height;
            _spriteBox = new RectangleF(ref _spritePosition, ref _bulletTexture);

            foreach (Tile t in TileMap.GetTilesAround(_spritePosition.ToPoint()))
            {
                if (_spriteBox.Intersects(t.SpriteBox))
                {
                    _IHitSomething = true;
                    break;
                }
            }
        }

        public static void LoadContent(string fileName)
        {
            _bulletTexture = Globals.Content.Load<Texture2D>(fileName);
        }

        public void Update()
        {
            float xMove = (_direction.X * _bulletSpeed);
            float yMove = (_direction.Y * _bulletSpeed);

            _spriteBox.X += xMove;
            dist.X += xMove;
            foreach (Tile t in TileMap.GetTilesAround(_spritePosition.ToPoint()))
            {
                if (_spriteBox.Intersects(t.SpriteBox))
                {
                    _spriteBox.X = (xMove > 0) ? t.SpriteBox.Left - _spriteBox.Width : _spriteBox.X = t.SpriteBox.Right;
                    _IHitSomething = true;
                    break;
                }
            }

            foreach (Enemy enemy in Game1.enemylist)
            {
                if (_spriteBox.Intersects(enemy.SpriteBox))
                {
                    _spriteBox.X = (xMove > 0) ? enemy.SpriteBox.Left - _spriteBox.Width : _spriteBox.X = enemy.SpriteBox.Right;
                    _IHitSomething = true;

                    enemy.HP--;
                    enemy.Colour = Color.IndianRed;
                    break;
                }
            }

            _spriteBox.Y += yMove;
            dist.Y += yMove;
            foreach (Tile t in TileMap.GetTilesAround(_spritePosition.ToPoint()))
            {
                if (_spriteBox.Intersects(t.SpriteBox))
                {
                    _spriteBox.X = (yMove > 0) ? t.SpriteBox.Top - _spriteBox.Height : t.SpriteBox.Bottom;
                    _IHitSomething = true;
                    break;
                }
            }

            if (dist.Length() > 1000)
            {
                _IHitSomething = true;
            }

            _spritePosition = _spriteBox.Location;
        }

        public override void Draw()
        {
            Globals.spriteBatch.Draw(_bulletTexture, _spritePosition, _spriteColour);
        }
    }
}