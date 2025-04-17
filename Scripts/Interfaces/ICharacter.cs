
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KirbStomp.Interfaces
{
    internal interface ICharacter
    {
        public string GetName();
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
        public Rectangle GetPosition();
        public event EventHandler OnDeath;
        //public void CheckGroundCollision();


        //public void AssignLegitimateHitboxSheet(Texture2D spriteSheet);
    }
}