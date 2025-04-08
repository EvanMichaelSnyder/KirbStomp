using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Data;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KirbStomp.Scripts.Projectiles
{
    public static class AssetPool
    {
        private static Dictionary<String, Texture2D> textures = new Dictionary<String, Texture2D>();
        private static Dictionary<String, Dictionary<String,Animation>> Animations = new Dictionary<string, Dictionary<string, Animation>>();

        public static void LoadTexture(Texture2D texture, String textureName)
        {
            if (textures.ContainsKey(textureName))
            {
                textures[textureName] = texture;
            }
            else
            {
                textures.Add(textureName, texture);
            }
        }

        public static Texture2D GetTexture(String name)
        {
            if (!textures.ContainsKey(name))
            {
                throw new Exception("texture " + name + " does not exist in AssetPool");
            }

            return textures[name];
        }

        public static Animation GetAnimation(String xmlName, String animName)
        {
            if (!Animations.ContainsKey(xmlName))
            {
                throw new Exception("ASSET POOL DOES NO CONTAIN animation WITH xmlName: " + xmlName);
            }else if (!Animations[xmlName].ContainsKey(animName))
            {
                throw new Exception("ASSET POOL DOES NO CONTAIN animation WITH animName: " + animName);
            }
            return Animations[xmlName][animName];
        }

        public static void LoadAnimationsFromXML(String xml)
        {
            XDocument doc = XDocument.Load(xml);
            XElement root = doc.Root;

            // Parse Texture element
            XElement texture = root.Element("Texture");
            string sheetName = texture.Attribute("spriteSheet").Value;
            float scale = float.Parse(texture.Attribute("scale").Value);
            float boundX = float.Parse(texture.Attribute("boundX").Value);
            float offsetDirectional = float.Parse(texture.Attribute("offsetDirectional").Value);


            Dictionary<String, Animation> animations = new Dictionary<string, Animation>();
            // Parse Animation elements
            foreach (XElement animElement in root.Elements("Animation"))
            {
                float frameTime = float.Parse(animElement.Attribute("duration").Value);
                bool doesLoop = bool.Parse(animElement.Attribute("loop").Value);
                Vector2 animationOffset = ParseVector(animElement.Attribute("animationOffset").Value);
                string animName = animElement.Attribute("name").Value;

                Animation animation = new Animation(animName, doesLoop);

                // Parse Frame elements
                foreach (XElement frameElement in animElement.Elements("Frame"))
                {

                    Vector2 position = ParseVector(frameElement.Attribute("position").Value);
                    Vector2 size = ParseVector(frameElement.Attribute("size").Value);
                    Vector2 PerFrameOffset = ParseVector(frameElement.Attribute("perFrameOffset").Value);

                    Rectangle srcRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
                    Animation.Frame frame = new Animation.Frame(srcRectangle, frameTime, PerFrameOffset);
                    animation.AddFrame(frame);
                }
                animations.Add(animName, animation);
            }
            if(!Animations.ContainsKey(sheetName))
            {
                Animations.Add(sheetName, animations);
            }        }


        //HELPERS
        private static Vector2 ParseVector(string vectorString)
        {
            string[] parts = vectorString.Trim('(', ')').Split(',');
            return new Vector2(
                float.Parse(parts[0].Trim()),
                float.Parse(parts[1].Trim())
            );
        }
    }
}
