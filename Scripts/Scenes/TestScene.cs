using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Collections.Generic;
using KirbStomp;
using KirbStomp.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KirbStomp.Data;
using KirbStomp.Scripts.Projectiles;
using KirbStomp.Scripts.Classes.Platforms;
using KirbStomp.Scripts.Classes.GameObjects.Projectiles;
using KirbStomp.Scripts.Classes.Projectiles;
using KirbStomp.Scripts.Classes.GameObjects.Items;
public class TestScene : IScene
{
    public static Texture2D boxSheet = Game1.Get().Content.Load<Texture2D>("HitboxWire");

    private static int entityID = 0;
    public static int getNewID()
    {
        entityID++;
        return entityID;
    }

    private CollisionSystem _collisionSystem;

    private ArrayList _platformList;
    private ArrayList _characterList;
    private ArrayList _controllerList;
    private ProjectileManager _projectileManager;
    private ItemManager _itemManager;
    
    private Texture2D _platformSheet;
    private Texture2D _marioSheet;
    private Texture2D _linkSheet;
    
    private string marioSpriteSheetName = "MarioTransparentSpriteSheet";
    private string linkSpriteSheetName = "LinkTransparentSpriteSheet";

    private Camera2D _camera;
    public TestScene() {
        //this._projectileManager = new ProjectileManager(_coll);
    }
    public void Initialize()
    {

        _platformList = new();
        _characterList = new();
        _controllerList = new();

        _collisionSystem = new CollisionSystem();

        // Long term change this to
        // Load content and done
        LoadContent();
        ResetScene();
    }

    public void Update(GameTime gameTime)
    {
        //THESE FIRST TWO USED TO BE AT THE BOTTOM HOPE THIS DOESNT CAUSE ANY ISSUES
        foreach (ICharacter chara in _characterList) { chara.Animate(gameTime); }

        //if animate ends the current frame the event endOfState was added
        foreach (ICharacter chara in _characterList) { chara.UpdateState(); }


        //new keyboard inputs are taken
        foreach (IController controller in _controllerList) { controller.Update();} 

        //action list includes new events
        foreach (ICharacter chara in _characterList) { chara.ProcessButtons(); } 

        //State is actually changed
        foreach (ICharacter chara in _characterList) { chara.UpdateState(); } 

        foreach (ICharacter chara in _characterList) { chara.ApplyMovementBehavior(); }
        foreach (ICharacter chara in _characterList) { chara.Gravity(gameTime); }

        foreach (ICharacter chara in _characterList) { chara.MoveCharacter(gameTime); }

        //right now this is actually called under process buttons
        //foreach (ICharacter chara in _characterList) { chara.CheckGroundCollision(); } 
        _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Body, HitboxTypeEnum.Platform);
        _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Item, HitboxTypeEnum.Platform);
        _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Item, HitboxTypeEnum.Body);
        //State is actually changed
        foreach (ICharacter chara in _characterList) { chara.UpdateState(); } 
    
        //mario.checkHitCollision
        //mario.UpdateState(); //State is actually changed
        //mario.doSpecialBehaviors

        // is every action commented out above
        foreach (ICharacter chara in _characterList) { chara.DoBehavior(); }

        _camera.Update(gameTime);

        //nothing to do with mario, DEBUGGING 
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(transformMatrix: _camera.GetTranslationMatrix());

        foreach (ICharacter chara in _characterList)
        {
            chara.Draw(spriteBatch);
            chara.DrawHitbox(spriteBatch);
        }
        foreach(Platform platform in _platformList)
        {
            platform.Draw(spriteBatch);
            platform.DrawHitbox(spriteBatch);
        }

        spriteBatch.End();
    }
    private string GetRelativeFilePath(string file)
    {
        return Path.Combine(XMLData.GetDataFolder(), "CharacterData", file);
    }

    private string GetRelativeFilePathProjectile(string file)
    {
        return Path.Combine(XMLData.GetDataFolder(), "Projectiles", file);
    }
    public void LoadContent()
    {
        //projectile stuff***
        AssetPool.LoadTexture(_marioSheet, "MarioProjectile");
        AssetPool.LoadTexture(_linkSheet, "LinkProjectile");

        _platformSheet = Game1.Get().Content.Load<Texture2D>("Platforms");
        var windowSize = Game1.Get().GetScreenWindow().GetAdjustedWindowSize();
        _camera = new Camera2D(Game1.Get().GraphicsDevice, new Point(windowSize.width, windowSize.height));


        // Camera being loaded here allows for the option in the future where there's multiple cameras and viewports
        var windowSize = Game1.Get().GetScreenWindow().GetAdjustedWindowSize();
        _camera = new Camera2D(Game1.Get().GraphicsDevice, new Point(windowSize.width, windowSize.height));


        // TODO can be away to data, list of xmlPathsToCharacter, Get each file, and Load from animation repo
        string xmlPathLink = GetRelativeFilePath("Link.XML");
        string xmlPathMario = GetRelativeFilePath("Mario.XML");
        AnimationRepository.LoadAnimationsFromXml(xmlPathMario);
        AnimationRepository.LoadAnimationsFromXml(xmlPathLink);
        
        // TODO same for hitboxes as animations
        string xmlPathMarioHitbox = GetRelativeFilePath("MarioHitbox.XML");
        HitboxRepository.LoadHitboxesFromXml(xmlPathMarioHitbox);
    }
    public ProjectileManager GetProjectileManager()
    {
        //return this._projectileManager;
        return null;
    }

    // Eventually, the only thing ResetScene will do is call Initialize
    // This  isn't just resetting, it's loading everything (WE SHOULD MOVE AWAY FROM THIS)
    public void ResetScene() 
    {
        _platformList.Clear();
        _characterList.Clear();
        _controllerList.Clear();

        Character mario = new Character("Mario", _marioSheet, marioSpriteSheetName, new Vector2(100, 100));

        // NOTE, this is hard coded for the moment, this will be moved to an XML

        // Character 1
        IController controllerMario = new KeyboardController(mario.GetButtonDataManager, new Dictionary<Keys, ICommand>()
        {
            {Keys.W, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Up])},
            {Keys.A, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Left])},
            {Keys.S, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Down])},
            {Keys.D, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Right])},
            {Keys.Y, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Attack])},
            {Keys.T, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Special])},
            {Keys.Space, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Jump])},
            {Keys.V, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.GotHit])}
        });
        _characterList.Add(mario);
        _controllerList.Add(controllerMario);
        _collisionSystem.RegisterObject(mario);

        // Character 2
        Character mario2 = new Character("Mario", _marioSheet, marioSpriteSheetName, new Vector2(300, 100));
        IController controllerMario2 = new KeyboardController(mario2.GetButtonDataManager, new Dictionary<Keys, ICommand>()
        {
            {Keys.P, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Up])},
            {Keys.L, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Left])},
            {Keys.OemSemicolon, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Down])},
            {Keys.OemQuotes, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Right])},
            {Keys.Down, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Attack])},
            {Keys.Left, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Special])},
            {Keys.RightShift, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Jump])}
        });
        _characterList.Add(mario2);
        _controllerList.Add(controllerMario2);
        _collisionSystem.RegisterObject(mario2);

        // Should add the first instance
        CharacterXMLParser.LoadCharacter("Mario");





        // Platforms
        Platform platform = new Platform(PlatformTypeEnum.SideDirtPlatform,new Rectangle(10,420,780,20), _platformSheet);
        _platformList.Add(platform);
        _collisionSystem.RegisterObject(platform);
        
        Platform platform2 = new Platform(PlatformTypeEnum.SideDirtPlatform, new Rectangle(500, 250, 200, 20), _platformSheet);
        _platformList.Add(platform2);
        _collisionSystem.RegisterObject(platform2);

       // _characterList.Add(new Character("Link", linkSheet, linkSpriteSheetName, new Vector2(600, 700)));



        // Cameras
        var windowSize = Game1.Get().GetScreenWindow().GetAdjustedWindowSize();
        _camera = new Camera2D(Game1.Get().GraphicsDevice, new Point(windowSize.width, windowSize.height));

        
    }
}