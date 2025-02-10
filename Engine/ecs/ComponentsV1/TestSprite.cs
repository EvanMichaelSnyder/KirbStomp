
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ecs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Engine.ecs.ComponentsV1
{
    //for test purpose
    public class TestSprite : Component
    {
        private Texture2D tex;
        private Rectangle rect;
        private SpriteBatch spriteBatch;
        public TestSprite()
        {
            spriteBatch = Game1.get().GetSpriteBatch();
            tex = Game1.get().Content.Load<Texture2D>("Smile");
            rect = new Rectangle(0, 0, 128, 128);

        }

        public override void Update(float dt)
        {
            spriteBatch.Draw(tex, gameObject.getPosition(), rect, Color.White);
        }
    }
}
