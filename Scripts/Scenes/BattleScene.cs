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
public class BattleScene : IScene
{
    public static Texture2D boxSheet = Game1.Get().Content.Load<Texture2D>("HitboxWire");
    public static Texture2D attackBoxSheet = Game1.Get().Content.Load<Texture2D>("AttackWire");

    private static int entityID = 0;
    public static int getNewID()
    {
        entityID++;
        return entityID;
    }
    //FPS Debugging
    int _numFrames = 0;
    private float _fps;
    private int _framesRendered;
    private DateTime _lastTime;

    private CollisionSystem _collisionSystem;

    private ArrayList _characterList;
    private ArrayList _platformList;
    private ArrayList _controllerList;
    private ProjectileManager _projectileManager;
    private ItemManager _itemManager;
    
    private Texture2D _platformSheet;
    private Texture2D _marioSheet;
    private Texture2D _linkSheet;
    private Texture2D _marioFireBallSheet;
    private Texture2D _linkArrowSheet;
    private string marioSpriteSheetName = "MarioTransparentSpriteSheet";
    private string linkSpriteSheetName = "LinkTransparentSpriteSheet";

    private Camera2D _camera;
    public BattleScene() {
        
        _numFrames = 0;
    }
    public void Initialize()
    {
        _platformList = new();
        _characterList = new();
        _controllerList = new();

        LoadContent();
        ResetScene();
    }

    public void Update(GameTime gameTime)
    {
        /*
        order of events

        update key registers
        turn key registers into stateChangingEvents
        doSCE
        alter movement based off of state behavior
        do movement
        check ground collision
        doSCE
        check hit collision
        doSCE
        do Behavior special (spawn fireball)
        Draw
        current frame increment and if endOfState add it to the events
        do ECS
        */

        // DebugFPS();
        //TODO REMOVE TEST
        if (Keyboard.GetState().IsKeyDown(Keys.D0))
        {
            this._projectileManager.AddProjectile(new MarioFireBall(new Vector2(Mouse.GetState().X/(float)Game1.Get().GetScreenWindow().globalScaleX, Mouse.GetState().Y/ (float)Game1.Get().GetScreenWindow().globalScaleY), true));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D9))
        {
            this._projectileManager.AddProjectile(new LinkArrow(new Vector2(Mouse.GetState().X / (float)Game1.Get().GetScreenWindow().globalScaleX, Mouse.GetState().Y / (float)Game1.Get().GetScreenWindow().globalScaleY), false));
        }

        this._projectileManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        this._itemManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
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
        _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Body, HitboxTypeEnum.Platform);
        _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Item, HitboxTypeEnum.Platform);
        _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Item, HitboxTypeEnum.Body);
        //State is actually changed
        foreach (ICharacter chara in _characterList) { chara.UpdateState(); }

        //mario.checkHitCollision
        _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Body, HitboxTypeEnum.Attack);
        //mario.UpdateState(); //State is actually changed
        foreach (ICharacter chara in _characterList) { chara.UpdateState(); }
        //mario.doSpecialBehaviors

        // is every action commented out above
        foreach (ICharacter chara in _characterList) { chara.DoBehavior(); }

        _camera.Update(gameTime);


        //nothing to do with mario, DEBUGGING 
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(transformMatrix: _camera.GetTranslationMatrix());

        this._projectileManager.Draw(spriteBatch);
        this._itemManager.Draw(spriteBatch);
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
        //much of this should be moved to load content 
        
        //projectile stuff***
        AssetPool.LoadTexture(_marioSheet, "MarioProjectile");
        AssetPool.LoadTexture(_linkSheet, "LinkProjectile");
        AssetPool.LoadAnimationsFromXML(GetRelativeFilePathProjectile("LinkProjectile.XML"));
        AssetPool.LoadAnimationsFromXML(GetRelativeFilePathProjectile("MarioProjectile.XML"));

        _platformSheet = Game1.Get().Content.Load<Texture2D>("Platforms");
        var windowSize = Game1.Get().GetScreenWindow().GetAdjustedWindowSize();
        _camera = new Camera2D(Game1.Get().GraphicsDevice, new Point(windowSize.width, windowSize.height));

        string xmlPathLink = GetRelativeFilePath("Link.XML");
        string xmlPathMario = GetRelativeFilePath("Mario.XML");
        AnimationRepository.LoadAnimationsFromXml(xmlPathMario);
        AnimationRepository.LoadAnimationsFromXml(xmlPathLink);

        string xmlPathMarioHitbox = GetRelativeFilePath("MarioHitbox.XML");
        HitboxRepository.LoadHitboxesFromXml(xmlPathMarioHitbox);
        string xmlPathMarioAttackHitbox = GetRelativeFilePath("MarioAttackHitbox.XML");
        AttackHitboxRepository.LoadHitboxesFromXml(xmlPathMarioAttackHitbox);
    }

    private void DebugFPS() {
        //fps
        _numFrames++;
        _framesRendered++;
        if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
        {
            // one second has elapsed 
            _fps = _framesRendered;
            _framesRendered = 0;
            _lastTime = DateTime.Now;
        }
        Console.WriteLine(_numFrames + " FPS: " + _fps);
    }
    public ProjectileManager GetProjectileManager()
    {
        return this._projectileManager;
    }



    public void ResetScene() {

        _platformList.Clear();
        _characterList.Clear();
        _controllerList.Clear();

        Character mario = new Character("Mario", _marioSheet, marioSpriteSheetName, new Vector2(100, 100));
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

        //ICharacter link = new Character(linkSheet, LinkSpriteSheetName);
        //IController controllerLink = new KeyboardController(link.GetButtonDataManager);

        // mario.AssignLegitimateHitboxSheet(boxSheet);
        
        Platform platform = new Platform(PlatformTypeEnum.SideDirtPlatform,new Rectangle(10,420,780,20), _platformSheet);
        Platform platform2 = new Platform(PlatformTypeEnum.SideDirtPlatform, new Rectangle(500, 250, 200, 20), _platformSheet);
        _platformList.Add(platform);
        _platformList.Add(platform2);

       // _characterList.Add(new Character("Link", linkSheet, linkSpriteSheetName, new Vector2(600, 700)));


        _collisionSystem = new CollisionSystem();
        _collisionSystem.RegisterObject(platform);
        _collisionSystem.RegisterObject(platform2);

        this._projectileManager = new ProjectileManager(_collisionSystem);
        this._itemManager = new ItemManager(_collisionSystem);

        var windowSize = Game1.Get().GetScreenWindow().GetAdjustedWindowSize();
        _camera = new Camera2D(Game1.Get().GraphicsDevice, new Point(windowSize.width, windowSize.height));

        this._itemManager.AddItem(new HamburgerItem(new Vector2(200, 200)));
        this._itemManager.AddItem(new ArrowStormItem(new Vector2(200, 200)));
        CharacterXMLParser.LoadCharacter("Mario");
    }
}