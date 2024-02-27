using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace Prod_em_on_Team4
{
    public class RectangleF
    {
        public float X, Y, Width, Height;

        public float Left { get => X; set => X = value; }
        public float Right { get => X + Width; set => X = value - Width; }
        public float Top { get => Y; set => Y = value; }
        public float Bottom { get => Y + Height; set => Y = value - Height; }

        public Vector2 Location { get => new Vector2(X, Y); set { X = value.X; Y = value.Y; } }
        public Vector2 Size { set { Width = value.X; Height = value.Y; } }

        public RectangleF(ref Vector2 Position) 
        {
            X = Position.X;
            Y = Position.Y;
        }

        public RectangleF(ref Vector2 Position, ref float width, ref float height)
        {
            X = Position.X;
            Y = Position.Y;
            Width = width;
            Height = height;
        }

        public RectangleF(ref float x, ref float y, ref float width, ref float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF(ref Vector2 Position, ref Texture2D texture)
        {
            X = Position.X;
            Y = Position.Y;
            Width = texture.Width;
            Height = texture.Height;
        }

        public RectangleF(ref Vector2 Pos1, ref Vector2 Pos2)
        {
            X = Pos1.X;
            Y = Pos1.Y;
            Width = Math.Abs(Pos2.X - Pos1.X);
            Height = Math.Abs(Pos2.Y - Pos1.Y);

            if (Pos2.Y - Pos1.Y < 0)
            {
                Y -= Height;
            }
            if (Pos2.X - Pos1.X < 0)
            {
                X -= Width;
            }
        }


        public RectangleF(Vector2 Pos1, Vector2 Pos2)
        {
            X = Pos1.X;
            Y = Pos1.Y;
            Width = Math.Abs(Pos2.X - Pos1.X);
            Height = Math.Abs(Pos2.Y - Pos1.Y);

            if (Pos2.Y - Pos1.Y < 0)
            {
                Y -= Height;
            }
            if (Pos2.X - Pos1.X < 0)
            {
                X -= Width;
            }
        }

        public Rectangle ToRectange()
        {
            return new Rectangle((int)X,  (int)Y, (int)Width, (int)Height);
        }

        public bool Intersects(RectangleF rect)
        {
            if (rect.Left < Right && Left < rect.Right && rect.Top < Bottom)
            {
                return Top < rect.Bottom;
            }

            return false;
        }
        public bool Intersects(ref RectangleF rect)
        {
            if (rect.Left < Right && Left < rect.Right && rect.Top < Bottom)
            {
                return Top < rect.Bottom;
            }

            return false;
        }
    }
}