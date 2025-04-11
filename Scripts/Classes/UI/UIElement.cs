using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp.Interfaces;
using KirbStomp.Scripts.Projectiles;
public class UIElement : IUINew {
    
    public string Name { get; set; }
    public Vector2 Position { get; set; }
    public bool IsVisible { get; set; }
    public List<Sprite> Sprites { get; set; }
    public List<SpriteString> TextSprites { get; set; }

    public UIElement(string name, Vector2 position) {
        this.Name = name;
        this.Position = position;
        this.IsVisible = true;
        this.Sprites = new List<Sprite>();
        this.TextSprites = new List<SpriteString>();
    }

    public UIElement(string name, Vector2 position, List<Sprite> sprites, List<SpriteString> textSprites) {
        this.Name = name;
        this.Position = position;
        this.IsVisible = true;
        this.Sprites = sprites;
        this.TextSprites = textSprites;
    }

    public void Initialize() {
        // Initialization logic here
    }

    public void Draw(SpriteBatch spriteBatch) {
        
        if (this.IsVisible) {
            foreach (var sprite in this.Sprites) {
                sprite.Draw(spriteBatch);
            }
            foreach (var textSprite in this.TextSprites) {
                textSprite.Draw(spriteBatch);
            }
        }
    }
    public void AddSprite(Sprite sprite) {
        this.Sprites.Add(sprite);
    }
    public void AddTextSprite(SpriteString textSprite) {
        this.TextSprites.Add(textSprite);
    }
    public void SetVisibility(bool isVisible) {
        this.IsVisible = isVisible;
    }
}