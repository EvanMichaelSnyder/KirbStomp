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
public class TestScene : IScene
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


    private ArrayList _characterList;
    private ArrayList _controllerList;
    private ProjectileManager _projectileManager;
    public TestScene() {
        this._projectileManager = new ProjectileManager();
        _numFrames = 0;
    }
    public void Initialize()
    {
        //much of this should be moved to load content 
        string marioSpriteSheetName = "MarioTransparentSpriteSheet";
        Texture2D marioSheet = Game1.Get().Content.Load<Texture2D>(marioSpriteSheetName);
        //projectile stuff***
        Texture2D marioFireBallSheet = Game1.Get().Content.Load<Texture2D>("MarioProjectileTransparentSpriteSheet");
        AssetPool.LoadTexture(marioSheet, "MarioProjectile");
        AssetPool.LoadAnimationsFromXML(GetRelativeFilePathProjectile("MarioProjectile.XML"));


        ICharacter mario = new Character("Mario", marioSheet, marioSpriteSheetName);
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
        ICharacter mario2 = new Character("Mario", marioSheet, marioSpriteSheetName, new Vector2(300, 100));
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

        _characterList = new ArrayList();
        _characterList.Add(mario);
        _characterList.Add(mario2);

        _controllerList = new ArrayList();
        _controllerList.Add(controllerMario);
        _controllerList.Add(controllerMario2);

        CharacterXMLParser.LoadCharacter("Mario");
    }

    public void Update(GameTime gameTime)
    {

        // DebugFPS();
        //TODO REMOVE TEST
        if (Keyboard.GetState().IsKeyDown(Keys.D0))
        {
            this._projectileManager.AddProjectile(new MarioFireBall(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)));
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
        foreach (ICharacter chara in _characterList) { chara.CheckGroundCollision(); } 
        
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
    public ProjectileManager GetProjectileManager()
    {
        return this._projectileManager;
    }
}