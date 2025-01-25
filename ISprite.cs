using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//an interface for any and all sprites
public interface ISprite {
    void Draw(SpriteBatch spriteBatch);         // draw sprite
    void Update(GameTime gameTime);             // update sprite
}