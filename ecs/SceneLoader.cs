using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.ecs.Components;
using Microsoft.Xna.Framework.Input;

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
            Component playerMovement = new PlayerMovement(200,.8f,150, Keys.W, Keys.A, Keys.D);
            test.AddComponent(playerMovement);
            scene.AddGameObject(test);

            GameObject test2 = new GameObject();
            test2.AddComponent(new TestSprite());
            test2.setPosition(new Vector2(300, 200));
            Component playerMove2 = new PlayerMovement(200, .8f, 150, Keys.Up, Keys.Left, Keys.Right);
            test2.AddComponent(playerMove2);
            scene.AddGameObject(test2);
            //***
            return scene;

        }

    }


}
