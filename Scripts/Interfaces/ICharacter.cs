using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Interfaces
{
    internal interface ICharacter
    {
        public void UpdateState();
        public void DoBehavior();
        ButtonDataManager GetButtonDataManager { get; }
        public void Animate(GameTime gameTime);
        public void DebugState();
        public void Draw(SpriteBatch spriteBatch);
        public void DrawHitbox(SpriteBatch spriteBatch);
        public void ProcessButtons();
        public void ApplyMovementBehavior();
        public void Gravity(GameTime gameTime);
        public void MoveCharacter(GameTime gameTime);
        public void CheckGroundCollision();


        public void AssignLegitimateHitboxSheet(Texture2D spriteSheet);
    }
}