using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{
    internal class AnimatedStillSprite : ISprite
    {
        //public only so it can be passed to new implementations when switching
        private Texture2D _spriteSheet;
        private LuigiSpriteSheetMapping.LuigiState _state,_currentFrame,_maxFrame;

        public AnimatedStillSprite(Texture2D spriteSheet,LuigiSpriteSheetMapping.LuigiState state)
        {
            _spriteSheet = spriteSheet;
            _state = state;
            _currentFrame = state;
            _maxFrame = state + LuigiSpriteSheetMapping.StateToAnimationSize[state];
        }
        public void Update()
        {
            _currentFrame++;
            if (_currentFrame >= _maxFrame) { _currentFrame = _state; }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            Rectangle sourceRectangle;
            Rectangle destinationRectangle;

            sourceRectangle = LuigiSpriteSheetMapping.StateToSpriteMap[_currentFrame];
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, 64, 128);

            spriteBatch.Draw(_spriteSheet, destinationRectangle, sourceRectangle, Color.White);

        }
        public Texture2D GetSpriteSheet()
        {
            return _spriteSheet;
        }
    }
}