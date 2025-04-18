using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using KirbStomp;
using KirbStomp.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp.Scripts.Scenes;
using KirbStomp.Scripts.Projectiles;
public class GameEvents {
    public GameEvents() {
        
    }
    public void SwitchScene(string sceneName) {
        // Logic to switch to a different scene
        SceneManager.Get().SwitchScene(sceneName);
    }
    public void StartGame() {
        SceneManager.Get().SwitchScene("GeneralSceneTemplate");
        SceneManager.Get().ResetCurrentScene();
        if(SceneManager.Get().IsPaused())
        {
            SceneManager.Get().UpdateSceneByCall(SceneCalls.Pause);
        }
        
    }
    public void ResumeGame() {
        SceneManager.Get().SwitchScene("GeneralSceneTemplate");
        SceneManager.Get().UpdateSceneByCall(SceneCalls.Pause);
    }
    public void ExitGame(){
        // Logic to exit the game
        Game1.Get().Exit();
    }
    public void EndGame(object sender, IScene.OnGameEndEventArgs args) {
        // Logic to end the game
        SwitchScene("EndScreen");
        Scene endScreen = (Scene)SceneManager.Get().GetCurrentScene();
        endScreen.RemoveScreenIUI("EndTextUI");
        SpriteFont impactFont = Game1.Get().Content.Load<SpriteFont>("impact");

        string winnerText = "Winner: " + args.character.GetName();
        float scale = 2f;
        SpriteString endGameText = new SpriteString("WinnerText", impactFont, winnerText, scale, new Vector2(400 - (impactFont.MeasureString(winnerText).X / 2) * scale, 110));
        UIElement endTextUI = new UIElement("EndTextUI", new Vector2(0, 0));
        endTextUI.AddTextSprite(endGameText);
        if (SceneManager.Get().GetCurrentScene() is Scene scene) {scene.AddScreenIUI(endTextUI);};
    }
    
}