using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Prod_em_on_Team4
{
    internal class AnimationManager
    {
        Dictionary<string, Animation> AnimationSet;
        public string currentAnimation;

        public AnimationManager(Dictionary<string, Animation> inAnimationSet) 
        {
            AnimationSet = inAnimationSet;
        }
        
        public Point Size()
        {
            return new Point(AnimationSet[currentAnimation].frameWidth, AnimationSet[currentAnimation].frameHeight);
        }

        public void SwitchAnimation(ref string animationName)
        {
            if (currentAnimation != null)
            {
                AnimationSet[currentAnimation].Stop();
            }
            currentAnimation = animationName;
        }

        public void UpdateAnimation()
        {
            AnimationSet[currentAnimation].Play();
        }

        public void Draw(ref Vector2 spritePosition, SpriteEffects Flip)
        {
            AnimationSet[currentAnimation].Draw(ref spritePosition, ref Flip);
        }
    }
}