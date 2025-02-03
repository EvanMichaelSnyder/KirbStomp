
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.ecs.Components
{
    //for test purpose
    public class TestSprite : Component
    {
        private Texture2D tex;
        private Rectangle rect;
        private SpriteBatch spriteBatch;
        public TestSprite() {
            this.spriteBatch = Game1.get().GetSpriteBatch();
            this.tex = Game1.get().Content.Load<Texture2D>("Smile");
            this.rect = new Rectangle(0,0,128,128);

        }

        public override void Update(float dt)
        {
            this.spriteBatch.Draw(this.tex, this.gameObject.getPosition(), this.rect, Color.White);
        }
    }
}
