using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.ecs.Components;

namespace KirbStomp.ecs
{
    //TODO Eventually, we want to have the scene be created, then load the scene data from a file. FOR NOW, JUST HARD CODE
    public class SceneLoader
    {
        // takes file path, and returns re-created scene from file
        public static Scene LoadScene(String filePath)
        {
            Scene scene = new Scene();
            //TODO implement scene loading stuff... For now just hard coded

            //REMOVE*********
            GameObject test = new GameObject();
            test.AddComponent(new TestSprite());
            test.setPosition(new Vector2(100, 200));
            test.AddComponent(new MoveRightTest());
            scene.AddGameObject(test);

            //
            return scene;

        }

    }


}
