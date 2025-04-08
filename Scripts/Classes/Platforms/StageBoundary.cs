using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Carriers;
using KirbStomp.Scripts.Classes.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Platforms
{
    internal class StageBoundary : CollisionObject
    {
        private Rectangle _boundaryDimensions;
        private AllPurposeSprite _sprite;
        private int _ID;
        private BoundaryCarrier BoundaryCarrier;


        public Rectangle GetPosition()
        {
            return BoundaryCarrier.HitboxManager.GetApproximation();
        }

        public StageBoundary(Rectangle rectangle, Texture2D spriteSheet)
        {
            _ID = BattleScene.getNewID();
            _boundaryDimensions = rectangle;
            _sprite = new AllPurposeSprite(spriteSheet);

            BoundaryCarrier = new BoundaryCarrier() { Parent = this };
            Carriers.Add(BoundaryCarrier);

            BoundaryCarrier.HitboxManager.basicUpdateHitbox(rectangle);
        }

        public void Draw(SpriteBatch spriteBatch)

        {
            _sprite.DrawRectangle(spriteBatch, _boundaryDimensions);
        }

        public void DrawHitbox(SpriteBatch spriteBatch)
        {
            BoundaryCarrier.HitboxManager.Draw(spriteBatch);
        }
    }
}
