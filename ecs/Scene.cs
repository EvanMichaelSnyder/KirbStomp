using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ecs
{
    // a instance of this is pretty much a level, change scene to change level
    public class Scene
    {   
        private List<GameObject> gameObjects;
        public Scene() 
        {
            this.gameObjects = new List<GameObject>();
        }

        //updates scene
        public void Update(float dt)
        {
            foreach (GameObject obj in this.gameObjects)
            {
                obj.Update(dt);
            }
        }

        //add a game object to the scene
        public void AddGameObject(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }
        //closes up scene for when changing to new scene
        public void DestroyScene(GameObject gameObject)
        {
            foreach (GameObject child in this.gameObjects)
            {
                child.Destroy();
            }
            gameObjects.Clear();

        }





    }
}
