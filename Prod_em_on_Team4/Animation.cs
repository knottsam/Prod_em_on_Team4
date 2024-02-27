using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Prod_em_on_Team4
{
    internal class Animation
    {
        Texture2D sheet;
        List<Rectangle> frames = new();
        int currentFrame = 0;
        double FRAME_TIME;
        double currentFrameTime = 0;

        public int frameWidth { get; private set; }
        public int frameHeight { get; private set; }

        public Animation(Texture2D inSpriteSheet, int inNumberOfFrames, double frameRate) 
        {
            sheet = inSpriteSheet;
            frameWidth = inSpriteSheet.Width / inNumberOfFrames;
            frameHeight = inSpriteSheet.Height;
            FRAME_TIME = frameRate;

            for (int frameNumber = 0; frameNumber < inNumberOfFrames; frameNumber++)
            {
                frames.Add(new Rectangle(frameNumber * frameWidth, 0, frameWidth, inSpriteSheet.Height));
            }
        }

        public void Play()
        {
            currentFrameTime -= Globals.TotalMilliseconds;
            
            if (currentFrameTime < 0)
            {
                currentFrameTime = FRAME_TIME;
                currentFrame += (currentFrame != frames.Count - 1) ? 1 : -currentFrame;
            }
        }

        public void Stop()
        {
            currentFrame = 0;
            currentFrameTime = FRAME_TIME;
        }

        public void Draw(ref Vector2 spritePosition,ref SpriteEffects Flip)
        {
            Globals.spriteBatch.Draw(sheet, spritePosition, frames[currentFrame], Color.White, 0f, Vector2.Zero, 1f, Flip, 0f);
        }
    }
}
