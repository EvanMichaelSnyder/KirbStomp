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
using static System.Net.Mime.MediaTypeNames;
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
    private ArrayList _boundaryList;
    private ProjectileManager _projectileManager;

    private ItemManager _itemManager;
    
    private Texture2D _platformSheet;
    private Texture2D _marioSheet;
    private Texture2D _linkSheet;
    private Texture2D _marioFireBallSheet;
    private Texture2D _linkArrowSheet;
    private Texture2D _btUISheet;

    //debounce purely for demenstration
    private float _debounce = 0f;

    private string marioSpriteSheetName = "MarioTransparentSpriteSheet";
    private string linkSpriteSheetName = "LinkTransparentSpriteSheet";

    private Camera2D _camera;
    private SpriteFont impactFont;
    private Sprite _background;
    // private List<IUI> _uiList;
    // private List<IUI> _uiList;

    public BattleScene() {
        
        _numFrames = 0;
    }
    public void Initialize()
    {
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
        _debounce += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if(_debounce > .2f)
        {
            

            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                this._projectileManager.SpawnProjectile("MarioFireBall", new Vector2(Mouse.GetState().X / (float)Game1.Get().GetScreenWindow().globalScaleX, Mouse.GetState().Y / (float)Game1.Get().GetScreenWindow().globalScaleY), true);
                _debounce = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D9))
            {
                this._projectileManager.AddProjectile(new LinkArrow(new Vector2(Mouse.GetState().X / (float)Game1.Get().GetScreenWindow().globalScaleX, Mouse.GetState().Y / (float)Game1.Get().GetScreenWindow().globalScaleY), false));
                _debounce = 0;
            }else if (Keyboard.GetState().IsKeyDown(Keys.D8))
            {
                this._projectileManager.SpawnProjectile("Boomerang", new Vector2(Mouse.GetState().X / (float)Game1.Get().GetScreenWindow().globalScaleX, Mouse.GetState().Y / (float)Game1.Get().GetScreenWindow().globalScaleY), true);
                _debounce = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D7))
            {
                this._projectileManager.SpawnProjectile("Bomb", new Vector2(Mouse.GetState().X / (float)Game1.Get().GetScreenWindow().globalScaleX, Mouse.GetState().Y / (float)Game1.Get().GetScreenWindow().globalScaleY), true);
                _debounce = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D6))
            {
                this._projectileManager.SpawnProjectile("LinkThrustSide", new Vector2(Mouse.GetState().X / (float)Game1.Get().GetScreenWindow().globalScaleX, Mouse.GetState().Y / (float)Game1.Get().GetScreenWindow().globalScaleY), true);
                _debounce = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.OemCloseBrackets))
            {
                this._projectileManager.SpawnProjectile("LinkUpThrust", new Vector2(Mouse.GetState().X / (float)Game1.Get().GetScreenWindow().globalScaleX, Mouse.GetState().Y / (float)Game1.Get().GetScreenWindow().globalScaleY), true,(Character) _characterList[1]);
                _debounce = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                this._projectileManager.SpawnProjectile("LinkSideSlash", new Vector2(Mouse.GetState().X / (float)Game1.Get().GetScreenWindow().globalScaleX, Mouse.GetState().Y / (float)Game1.Get().GetScreenWindow().globalScaleY), true, (Character)_characterList[1]);
                _debounce = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                this._projectileManager.SpawnProjectile("LinkDownThrust", new Vector2(Mouse.GetState().X / (float)Game1.Get().GetScreenWindow().globalScaleX, Mouse.GetState().Y / (float)Game1.Get().GetScreenWindow().globalScaleY), true, (Character)_characterList[1]);
                _debounce = 0;
            }


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
        _collisionSystem.CheckCollisionPairGround(HitboxTypeEnum.Body, HitboxTypeEnum.Platform, gameTime);
        _collisionSystem.CheckCollisionPairGround(HitboxTypeEnum.Body, HitboxTypeEnum.Boundary, gameTime);
        _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Item, HitboxTypeEnum.Platform, gameTime);
        _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Item, HitboxTypeEnum.Body, gameTime);
        //State is actually changed
        foreach (ICharacter chara in _characterList) { chara.UpdateState(); }

        //mario.checkHitCollision
        _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Body, HitboxTypeEnum.Attack, gameTime);
        //mario.UpdateState(); //State is actually changed
        foreach (ICharacter chara in _characterList) { chara.UpdateState(); }
        //mario.doSpecialBehaviors

        // is every action commented out above
        foreach (ICharacter chara in _characterList) { chara.DoBehavior(); }

        _camera.Update(gameTime);


        //nothing to do with mario, DEBUGGING 
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(transformMatrix: _camera.GetTranslationMatrix());
        _background.Draw(spriteBatch, new Vector2(0, 0));
        this._projectileManager.Draw(spriteBatch);
        this._itemManager.Draw(spriteBatch);
        foreach(Platform platform in _platformList)
        {
            platform.Draw(spriteBatch);
            platform.DrawHitbox(spriteBatch);
        }
        // foreach (IUI ui in _uiList)
        // {
        //     ui.Draw(spriteBatch);
        // }
        foreach (ICharacter chara in _characterList)
        {
            chara.Draw(spriteBatch);
            chara.DrawHitbox(spriteBatch);
        }
        // spriteBatch.DrawString(impactFont, "MARIO", new Vector2(220, 770), Color.White);
        // spriteBatch.DrawString(impactFont, "LINK", new Vector2(575, 770), Color.White);
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
        impactFont = Game1.Get().Content.Load<SpriteFont>("impact");
        _btUISheet = Game1.Get().Content.Load<Texture2D>("BattleUISpriteSheet");
        _background = new Sprite(Game1.Get().Content.Load<Texture2D>("SpaceBackground"), new Rectangle(0, 0, 3000, 2000), .27f);

        //projectile stuff***
        _marioFireBallSheet = Game1.Get().Content.Load<Texture2D>("MarioProjectileTransparentSpriteSheet");
        _linkArrowSheet = Game1.Get().Content.Load<Texture2D>(linkSpriteSheetName);
        Texture2D itemSheet = Game1.Get().Content.Load<Texture2D>("Items");
        _marioSheet = Game1.Get().Content.Load<Texture2D>(marioSpriteSheetName);
        _linkSheet = Game1.Get().Content.Load<Texture2D>(linkSpriteSheetName);
        AssetPool.LoadTexture(itemSheet, "Items");
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
        string xmlPathLinkHitbox = GetRelativeFilePath("LinkHitbox.XML");
        HitboxRepository.LoadHitboxesFromXml(xmlPathLinkHitbox);
        string xmlPathLinkAttackHitbox = GetRelativeFilePath("LinkAttackHitbox.XML");
        AttackHitboxRepository.LoadHitboxesFromXml(xmlPathLinkAttackHitbox);
    }

    private void DebugFPS()
    {
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
        float scale = .2f;
        CharacterUIData marioUIData = new CharacterUIData(
            new Sprite(_btUISheet, new Rectangle(0, 524, 99, 60), scale), // stock icon
            new Sprite(_btUISheet, new Rectangle(0, 621, 240, 299), scale), // portrait icon
            "MARIO"
        );
        CharacterUIData linkUIData = new CharacterUIData(
            new Sprite(_btUISheet, new Rectangle(0, 0, 104, 85), scale), // stock icon
            new Sprite(_btUISheet, new Rectangle(0, 105, 424, 307), scale), // portrait icon
            "LINK"
        );

        Character mario = new Character("Mario", _marioSheet, new Vector2(100, 100), marioUIData);
        IController controllerMario = new KeyboardController(mario.GetButtonDataManager, new Dictionary<Keys, ICommand>()
        {
            {Keys.W, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Up])},
            {Keys.A, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Left])},
            {Keys.S, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Down])},
            {Keys.D, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Right])},
            {Keys.Y, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Attack])},
            {Keys.T, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Special])},
            {Keys.Space, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Jump])},
        });
        Character link = new Character("Link", _linkSheet, new Vector2(300, 70), linkUIData);
        IController controllerLink = new KeyboardController(link.GetButtonDataManager, new Dictionary<Keys, ICommand>()
        {
            {Keys.P, new UpdateButtonCommand(link.GetButtonDataManager.ButtonDataSheet[GameButtons.Up])},
            {Keys.L, new UpdateButtonCommand(link.GetButtonDataManager.ButtonDataSheet[GameButtons.Left])},
            {Keys.OemSemicolon, new UpdateButtonCommand(link.GetButtonDataManager.ButtonDataSheet[GameButtons.Down])},
            {Keys.OemQuotes, new UpdateButtonCommand(link.GetButtonDataManager.ButtonDataSheet[GameButtons.Right])},
            {Keys.Down, new UpdateButtonCommand(link.GetButtonDataManager.ButtonDataSheet[GameButtons.Attack])},
            {Keys.Left, new UpdateButtonCommand(link.GetButtonDataManager.ButtonDataSheet[GameButtons.Special])},
            {Keys.RightShift, new UpdateButtonCommand(link.GetButtonDataManager.ButtonDataSheet[GameButtons.Jump])}
        });
        //ICharacter link = new Character(linkSheet, LinkSpriteSheetName);
        //IController controllerLink = new KeyboardController(link.GetButtonDataManager);

        // mario.AssignLegitimateHitboxSheet(boxSheet);

        // PlayerBattleUI _playerOneBTUI = new PlayerBattleUI(impactFont, 
        //     mario,
        //     new Vector2(100, 380),
        //     new Sprite(_btUISheet, new Rectangle(515, 561, 508, 339), scale), // portrait background
        //     new Sprite(_btUISheet, new Rectangle(0, 956, 874, 49), scale)  // name holder
        // );
        // PlayerBattleUI _playerTwoBTUI = new PlayerBattleUI(impactFont, 
        //     link,
        //     new Vector2(440, 380),
        //     new Sprite(_btUISheet, new Rectangle(515, 0, 508, 339), scale), // portrait background
        //     new Sprite(_btUISheet, new Rectangle(1, 437, 874, 49), scale)  // name holder
        // );

        // _uiList = new List<IUI>();
        // _uiList.Add(_playerOneBTUI);
        // _uiList.Add(_playerTwoBTUI);


        Platform platformMain = new Platform(PlatformTypeEnum.SideDirtPlatform,new Rectangle(10,370,780,20), _platformSheet);
        Platform platformSideR = new Platform(PlatformTypeEnum.SideDirtPlatform, new Rectangle(500, 200, 200, 20), _platformSheet);
        Platform platformSideL = new Platform(PlatformTypeEnum.SideDirtPlatform, new Rectangle(100, 200, 200, 20), _platformSheet);
        _platformList = new ArrayList();
        _platformList.Add(platformMain);
        _platformList.Add(platformSideR);
        _platformList.Add(platformSideL);

        StageBoundary BoundaryBottom = new StageBoundary(new Rectangle(-100, 700, 1000, 50), _platformSheet);
        StageBoundary BoundaryTop = new StageBoundary(new Rectangle(-100, -500, 1000, 50), _platformSheet);
        StageBoundary BoundaryLeft = new StageBoundary(new Rectangle(-150, -500, 50, 1250), _platformSheet);
        StageBoundary BoundaryRight = new StageBoundary(new Rectangle(900, -500, 50, 1250), _platformSheet);

        _boundaryList = new ArrayList();
        _boundaryList.Add(BoundaryBottom);
        _boundaryList.Add(BoundaryTop);
        _boundaryList.Add(BoundaryLeft);
        _boundaryList.Add(BoundaryRight);

        _characterList = new ArrayList();
        _characterList.Add(mario);
        _characterList.Add(link);
       // _characterList.Add(new Character("Link", linkSheet, linkSpriteSheetName, new Vector2(600, 700)));

        _controllerList = new ArrayList();
        _controllerList.Add(controllerMario);
        _controllerList.Add(controllerLink);

        _collisionSystem = new CollisionSystem();
        _collisionSystem.RegisterObject(mario);
        _collisionSystem.RegisterObject(link);
        _collisionSystem.RegisterObject(platformMain);
        _collisionSystem.RegisterObject(platformSideR);
        _collisionSystem.RegisterObject(platformSideL);
        _collisionSystem.RegisterObject(BoundaryTop);
        _collisionSystem.RegisterObject(BoundaryLeft);
        _collisionSystem.RegisterObject(BoundaryRight);
        _collisionSystem.RegisterObject(BoundaryBottom);

        this._projectileManager = new ProjectileManager(_collisionSystem);
        this._itemManager = new ItemManager(_collisionSystem);

        var windowSize = Game1.Get().GetScreenWindow().GetAdjustedWindowSize();
        _camera = new Camera2D(Game1.Get().GraphicsDevice, new Point(windowSize.width, windowSize.height));

        this._itemManager.AddItem(new HamburgerItem(new Vector2(500, 200)));
        this._itemManager.AddItem(new ArrowStormItem(new Vector2(200, 200)));
        this._itemManager.AddItem(new BombItem(new Vector2(400, 200)));
        CharacterXMLParser.LoadCharacter("Mario");
        CharacterXMLParser.LoadCharacter("Link");
    }
}