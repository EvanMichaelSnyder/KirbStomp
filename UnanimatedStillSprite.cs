using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0
{
    internal class UnanimatedStillSprite : ISprite
    {
        private Texture2D _spriteSheet;
        private LuigiSpriteSheetMapping.LuigiState _state;
        public UnanimatedStillSprite(Texture2D spriteSheet, LuigiSpriteSheetMapping.LuigiState state)
        {
            _spriteSheet = spriteSheet;
            _state = state;
        }
        public void Update()
        {
            //nothing
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            Rectangle sourceRectangle;
            Rectangle destinationRectangle;

            sourceRectangle = LuigiSpriteSheetMapping.StateToSpriteMap[_state];
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, 64, 128);

            spriteBatch.Draw(_spriteSheet, destinationRectangle, sourceRectangle, Color.White);

        }
        public Texture2D GetSpriteSheet()
        {
            return _spriteSheet;
        }
    }
}
