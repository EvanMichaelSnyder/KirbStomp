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
public class BattleScene : IScene
{
    public static Texture2D boxSheet = Game1.Get().Content.Load<Texture2D>("HitboxWire");

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
    public BattleScene() {
        
        _numFrames = 0;
    }
    public void Initialize()
    {
        //much of this should be moved to load content 
        string marioSpriteSheetName = "MarioTransparentSpriteSheet";
        string linkSpriteSheetName = "LinkTransparentSpriteSheet";
        Texture2D marioSheet = Game1.Get().Content.Load<Texture2D>(marioSpriteSheetName);
        Texture2D linkSheet = Game1.Get().Content.Load<Texture2D>(linkSpriteSheetName);
        //projectile stuff***
        Texture2D marioFireBallSheet = Game1.Get().Content.Load<Texture2D>("MarioProjectileTransparentSpriteSheet");
        Texture2D linkProjectileSheet = Game1.Get().Content.Load<Texture2D>(linkSpriteSheetName);
        AssetPool.LoadTexture(marioSheet, "MarioProjectile");
        AssetPool.LoadTexture(linkSheet, "LinkProjectile");
        AssetPool.LoadAnimationsFromXML(GetRelativeFilePathProjectile("LinkProjectile.XML"));
        AssetPool.LoadAnimationsFromXML(GetRelativeFilePathProjectile("MarioProjectile.XML"));

        Texture2D PlatformSheet = Game1.Get().Content.Load<Texture2D>("Platforms");


        Character mario = new Character("Mario", marioSheet, marioSpriteSheetName);
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
        Character mario2 = new Character("Mario", marioSheet, marioSpriteSheetName, new Vector2(300, 100));
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
        //ICharacter link = new Character(linkSheet, LinkSpriteSheetName);
        //IController controllerLink = new KeyboardController(link.GetButtonDataManager);

        // mario.AssignLegitimateHitboxSheet(boxSheet);
        Platform platform = new Platform(PlatformTypeEnum.SideDirtPlatform,new Rectangle(10,420,780,20),PlatformSheet);
        Platform platform2 = new Platform(PlatformTypeEnum.SideDirtPlatform, new Rectangle(500, 250, 200, 20), PlatformSheet);
        _platformList = new ArrayList();
        _platformList.Add(platform);
        _platformList.Add(platform2);

        _characterList = new ArrayList();
        _characterList.Add(mario);
        _characterList.Add(mario2);
       // _characterList.Add(new Character("Link", linkSheet, linkSpriteSheetName, new Vector2(600, 700)));

        _controllerList = new ArrayList();
        _controllerList.Add(controllerMario);
        _controllerList.Add(controllerMario2);

        _collisionSystem = new CollisionSystem();
        _collisionSystem.RegisterObject(mario);
        _collisionSystem.RegisterObject(mario2);
        _collisionSystem.RegisterObject(platform);
        _collisionSystem.RegisterObject(platform2);

        this._projectileManager = new ProjectileManager(_collisionSystem);


        CharacterXMLParser.LoadCharacter("Mario");
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

        //State is actually changed
        foreach (ICharacter chara in _characterList) { chara.UpdateState(); } 
    
        //mario.checkHitCollision
        //mario.UpdateState(); //State is actually changed
        //mario.doSpecialBehaviors

        // is every action commented out above
        foreach (ICharacter chara in _characterList) { chara.DoBehavior(); } 

        //nothing to do with mario, DEBUGGING 
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        this._projectileManager.Draw(spriteBatch);
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
        string xmlPathLink = GetRelativeFilePath("Link.XML");
        string xmlPathMario = GetRelativeFilePath("Mario.XML");
        AnimationRepository.LoadAnimationsFromXml(xmlPathMario);
        AnimationRepository.LoadAnimationsFromXml(xmlPathLink);

        string xmlPathMarioHitbox = GetRelativeFilePath("MarioHitbox.XML");
        HitboxRepository.LoadHitboxesFromXml(xmlPathMarioHitbox);
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
}