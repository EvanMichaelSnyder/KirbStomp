using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using KirbStomp.Scripts.Scenes;
public class MouseController {
    private MouseState _currentMouseState;
    private MouseState _previousMouseState;

    public Vector2 Position { get; private set; }
    public bool LeftClicked { get; private set; }
    public List<AreaUI2D> Areas { get; set; }
    internal Scene SceneObject { get; set; }

    public MouseController () {
        KirbStomp.SceneManager.Get().OnSceneChange += SceneManager_OnSceneChanged;
        Areas = new List<AreaUI2D>();
    }

    public void Update()
    {
        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();

        Position = new Vector2(_currentMouseState.X, _currentMouseState.Y);

        LeftClicked = _currentMouseState.LeftButton == ButtonState.Pressed &&
                      _previousMouseState.LeftButton == ButtonState.Released;
        
        if (LeftClicked) {
            foreach (var area in Areas) {
                if (area.Contains(Position)) {
                    Console.WriteLine("Mouse Clicked on Area: " + area.Name);
                }
            }
        }
    }
    private void SceneManager_OnSceneChanged(object sender, EventArgs e) {
        SceneObject = (Scene)KirbStomp.SceneManager.Get().GetCurrentScene();
        SetAreas(SceneObject.GetAreas());
    }
    public void SetAreas(List<AreaUI2D> areas) {
        this.Areas = areas;
    }
}