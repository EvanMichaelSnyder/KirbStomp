using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp
{
    internal interface ICharacter
    {
        public void UpdateState();
        public void doBehavior();
        ButtonDataManager GetButtonDataManager { get; }
        public void Animate(GameTime gameTime);
        public void debugState();
        public void draw(SpriteBatch spriteBatch);
        public void ProcessButtons();
        public void ApplyMovementBehavior();
        public void gravity(GameTime gameTime);
        public void MoveCharacter(GameTime gameTime);
        public void checkGroundCollision();
    }
}