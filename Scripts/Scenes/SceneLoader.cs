using KirbStomp.Data;
using KirbStomp.Interfaces;
using KirbStomp.Scripts.Classes.Platforms;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KirbStomp.Scripts.Scenes
{

    internal class SceneLoader
    {
        // Should load
        // Character
        // Platforms
        // UIs
        // Projectiles
        // Items

        private static string loadFile;
        private static string name;
        private static XElement sceneElement;
        private static XElement fileElementsToLoad;

        /*
        Dictionary<string, GameButtons> stringToGameButtonDict = new()
            {
                { "up", GameButtons.Up},
                { "down", GameButtons.Down },
                {"left", GameButtons.Left },
                {"right", GameButtons.Right },
                {"jump", GameButtons.Jump },
                {"neutral", GameButtons.Down },
                {"special", GameButtons.Special }
            };

         */
        

        public static void SetLoadFile(string file)
        {
            loadFile = file;
            sceneElement = GetSceneXElement(loadFile);
            fileElementsToLoad = GetXElementOrAssert("FilesToLoad", sceneElement);
            name = GetXElementOrAssert("Name", sceneElement).Value;
        }
        public static void LoadScene(List<ICharacter> characters, List<IController> controllerList, List<Platform> platformList, List<IUINew> UIList, List<StageBoundary> boundaryList)
        {
            if (loadFile == "")
                throw new Exception("Error load file was never set. Load file is \"\"");
            LoadCharacters(characters, controllerList);
            LoadPlatforms(platformList);    // Only thing hard coded now
            LoadUI(UIList, characters);
            LoadBoundaries(boundaryList);  // Also hardcoded but it exactly like platforms
            LoadAssetPool();
        }

        public static void LoadCharacters(List<ICharacter> characterList, List<IController> controllerList)
        {
            string[] characterFiles;
            XElement characters = GetXElementOrAssert("Characters", fileElementsToLoad);
            if (characters.Elements().Count() == 0)
            {
                return;
            }
            Character loadedCharacter;
            IController controller;
            foreach (XElement character in characters.Elements("Character"))
            {
                characterFiles = (character.Value).Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (characterFiles.Count() != 3)
                {
                    throw new Exception($"Error: character file count != 3 in Scene Name: {name}");
                }
                loadedCharacter = GenerateCharacter(characterFiles[0], characterFiles[1]);
                controller = GenerateController(characterFiles[2], loadedCharacter, characterFiles[0]);


                characterList.Add(loadedCharacter);
                controllerList.Add(controller);
            }
        }


        public static void LoadPlatforms(List<Platform> platformList)
        {
            string platformFile = "";
            XElement platforms = GetXElementOrAssert("Platforms", fileElementsToLoad);
            if (platforms.Elements().Count() == 0)
            {
                return;
            }
            Platform platform;
            var a = platforms.Elements("Platform");
            foreach (XElement platformItem in platforms.Elements("Platform"))
            {
                platformFile = platformItem.Value.Replace(" ", string.Empty);
                ParsePlatforms(platformFile, platformList);

            }
        }

        public static void LoadBoundaries(List<StageBoundary> boundaryList)
        {
 
            string[] platformFiles = default;
            XElement platforms = GetXElementOrAssert("Platforms", fileElementsToLoad);
            if (platforms.Elements().Count() == 0)
            {
                return;
            }
            Platform platform;
            foreach (XElement platformItem in platforms.Elements("Platform"))
            {
                platformFiles = (platformItem.Value).Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                //platform = GeneratePlatform(platformFiles[0], platformFiles[1]);

            }

            Texture2D tex = Game1.Get().Content.Load<Texture2D>("Platforms");

            StageBoundary BoundaryBottom = new StageBoundary(new Rectangle(-400, 700, 1600, 50), tex);
            StageBoundary BoundaryTop = new StageBoundary(new Rectangle(-400, -500, 1600, 50), tex);
            StageBoundary BoundaryLeft = new StageBoundary(new Rectangle(-450, -500, 50, 1250), tex);
            StageBoundary BoundaryRight = new StageBoundary(new Rectangle(1200, -500, 50, 1250), tex);

    
            boundaryList.Add(BoundaryBottom);
            boundaryList.Add(BoundaryTop);
            boundaryList.Add(BoundaryLeft);
            boundaryList.Add(BoundaryRight);
        }

        public static void LoadUI(List<IUINew> UIList, List<ICharacter> characters)
        {
            string UIToLoad = default;
            XElement UIs = GetXElementOrAssert("UIs", fileElementsToLoad);
            if (UIs.Elements().Count() == 0)
            {
                return;
            }
            // IUI ui;
            // int charaIndex = 0;
            // foreach (XElement UIElement in UIs.Elements("PlayerUI"))
            // {
            //     UIToLoad = UIElement.Value.Replace(" ", string.Empty);
                
            //     ui = GeneratePlayerUI(UIToLoad, characters[charaIndex]);
            //     UIList.Add(ui);
            //     charaIndex++;
            // }
            Texture2D gameButtonsUISheet = Game1.Get().Content.Load<Texture2D>("GameButtons");
            Texture2D gameNameSheet = Game1.Get().Content.Load<Texture2D>("GameName");
            SpriteFont impactFont = Game1.Get().Content.Load<SpriteFont>("impact");

            if(name.Contains("StartScreen")){
                ButtonUI startButton = new ButtonUI(new Sprite("StartButton", gameButtonsUISheet, new Rectangle(28, 240, 142, 89), 1f), new Vector2(330, 200));
                ButtonUI exitButton = new ButtonUI(new Sprite("ExitButton", gameButtonsUISheet, new Rectangle(186, 240, 154, 89), 1f), new Vector2(325, 300));
                ButtonUI titleButton = new ButtonUI(new Sprite("TitleButton", gameNameSheet, new Rectangle(13, 119, 273, 59), 1.2f), new Vector2(240, 70));
              
                UIList.Add(startButton);
                UIList.Add(exitButton);
                UIList.Add(titleButton);
            }
            else if (name.Contains("EndScreen"))
            {
                ButtonUI menuButton = new ButtonUI(new Sprite("MenuButton", gameButtonsUISheet, new Rectangle(186, 134, 154, 89), 1f), new Vector2(330, 200));
                ButtonUI exitButton = new ButtonUI(new Sprite("ExitButton", gameButtonsUISheet, new Rectangle(186, 240, 154, 89), 1f), new Vector2(325, 300));
                
                UIList.Add(menuButton);
                UIList.Add(exitButton);
            }
            else if (name.Contains("SceneMain"))
            {
                Console.WriteLine("Loading GeneralSceneTemplate UI");
                float scale = .2f;
                Texture2D _btUISheet = Game1.Get().Content.Load<Texture2D>("BattleUISpriteSheet");
                const string PBACKGROUND = "PortraitBackground";
                const string NAMEHOLDER = "NameHolder";
                PlayerBattleUINew playerOneUI = new PlayerBattleUINew (
                    "PlayerOneUI", //name
                    new Vector2(100, 380), //position
                    (Character)characters[0], //first character
                    impactFont, //font
                    new Sprite(PBACKGROUND, _btUISheet, new Rectangle(515, 561, 508, 339), scale), // portrait background
                    new Sprite(NAMEHOLDER, _btUISheet, new Rectangle(0, 956, 874, 49), scale)  // name holder
                );
                PlayerBattleUINew playerTwoUI = new PlayerBattleUINew (
                    "PlayerTwoUI", //name
                    new Vector2(440, 380), //position
                    (Character)characters[1], //first character
                    impactFont, //font
                    new Sprite(PBACKGROUND, _btUISheet, new Rectangle(515, 0, 508, 339), scale), // portrait background
                    new Sprite(NAMEHOLDER, _btUISheet, new Rectangle(1, 437, 874, 49), scale)  // name holder
                );
                UIList.Add(playerOneUI);
                UIList.Add(playerTwoUI);
            }
            // foreach(XElement UIElement in UIs.Elements("ButtonUI")) {
            //     UIToLoad = UIElement.Value.Replace(" ", string.Empty);
                // ui = GenerateButtonUI(UIToLoad);
                // UIList.Add(ui);
            // }
        }

        public static void LoadAssetPool()
        {
            //Load textures
            // Load Animations
            XElement assetPool = GetXElementOrAssert("AssetPool", fileElementsToLoad);
            Texture2D itemSheet = Game1.Get().Content.Load<Texture2D>("Items");
            Texture2D _marioSheet = Game1.Get().Content.Load<Texture2D>("MarioTransparentSpriteSheet");
            Texture2D _linkSheet = Game1.Get().Content.Load<Texture2D>("LinkTransparentSpriteSheet");
            AssetPool.LoadTexture(itemSheet, "Items");
            AssetPool.LoadTexture(_marioSheet, "MarioProjectile");
            AssetPool.LoadTexture(_linkSheet, "LinkProjectile");

            LoadAllTexturesFromXElement(GetXElementOrAssert("Textures", assetPool));
            LoadAllAnimationsFromXElement(GetXElementOrAssert("Animations", assetPool));
        }
        private static void LoadAllTexturesFromXElement(XElement textures)
        {
            foreach (XElement texture in textures.Elements("Texture"))
            {
                LoadTextureFromXElement(texture);
            }
        }
        private static void LoadTextureFromXElement(XElement texture)
        {
            string[] textureSplit = texture.Value.Split(new char[] {' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Texture2D spriteSheet = Game1.Get().Content.Load<Texture2D>(textureSplit[1]);
            AssetPool.LoadTexture(spriteSheet, textureSplit[0]);
        }
        private static void LoadAllAnimationsFromXElement(XElement animations)
        {
            string path = Path.Combine(XMLData.GetDataFolder(), "Projectiles");
            foreach(XElement animation in animations.Elements("Animation"))
            {
                AssetPool.LoadAnimationsFromXML(Path.Combine(path, animation.Value.Replace(" ", string.Empty) + ".XML"));
            }
        }

        private static XElement GetSceneXElement(string fileName)
        {
            string dataFolder = XMLData.GetDataFolder();
            string sceneDataFile = Path.Combine(dataFolder, "SceneData", fileName + ".XML");

            XDocument sceneDoc = XDocument.Load(sceneDataFile);
            XElement root = sceneDoc.Root;
            if (root.Name != "Scene")
            {
                throw new ArgumentException($"File at {sceneDataFile} has wrong root of {root.Name} and not Character");
            }

            return root;
        }
        private static XElement GetXElementOrAssert(string name, XElement parent)
        {
            XElement output = parent.Element(name);
            if (output == null)
            {
                throw new Exception($"XElement did not contain {name} element in Parent: {parent.Name}");
            }
            return output;
        }


        private static Character GenerateCharacter(string statsFile, string spriteSheetFile)
        {
            CharacterStats stats = CharacterXMLParser.LoadCharacterStatsFile(statsFile);
            // Load Texture, Animations, Hitboxes, and Attack Hitboxes
            string fileLocation = Path.Combine(XMLData.GetDataFolder(), "CharacterData", statsFile);
            Texture2D tex = Game1.Get().Content.Load<Texture2D>(spriteSheetFile);
            AnimationRepository.LoadAnimationsFromXml(fileLocation + ".XML");
            HitboxRepository.LoadHitboxesFromXml(fileLocation + "Hitbox.XML");
            AttackHitboxRepository.LoadHitboxesFromXml(fileLocation + "AttackHitbox.XML");

            Character output;
            Texture2D stockIconSheet = Game1.Get().Content.Load<Texture2D>(stats.stockIcon.spriteSheet);
            Texture2D portraitIconSheet = Game1.Get().Content.Load<Texture2D>(stats.portraitIcon.spriteSheet);

            const string stockIconName = "StockIcon";
            const string portraitIconName = "PortraitIcon";
            output = new Character(stats.name, tex, stats.position, new CharacterUIData(new Sprite(stockIconName, stockIconSheet, stats.stockIcon.sourceRectangle, stats.stockIcon.scale), new Sprite(portraitIconName, portraitIconSheet, stats.portraitIcon.sourceRectangle, stats.stockIcon.scale), stats.name));

            /*
            Texture2D _btUISheet = Game1.Get().Content.Load<Texture2D>("BattleUISpriteSheet");
            switch(stats.name) {
                case "Mario":
                    Console.WriteLine("Mario UIData");
                    output = new Character(stats.name, tex, new Vector2(100, 100), new CharacterUIData(
                        new Sprite(_btUISheet, new Rectangle(0, 524, 99, 60), scale), // stock icon
                        new Sprite(_btUISheet, new Rectangle(0, 621, 240, 299), scale), // portrait icon
                        // "MARIO"
                        stats.name.ToUpper()
                    ));
                    break;
                case "Link":
                    Console.WriteLine("Link UIData");
                    output = new Character(stats.name, tex, new Vector2(100, 100), new CharacterUIData(
                    new Sprite(_btUISheet, new Rectangle(0, 0, 104, 85), scale), // stock icon
                    new Sprite(_btUISheet, new Rectangle(0, 105, 424, 307), scale), // portrait icon
                    // "LINK"
                    stats.name.ToUpper() // character name
                ));
                    break;
                case "MegaMan":
                    Console.WriteLine("Megaman UIData");
                    output = new Character(stats.name, tex, new Vector2(100, 100), new CharacterUIData(
                        new Sprite(_btUISheet, new Rectangle(0, 524, 99, 60), scale), // stock icon
                        new Sprite(_btUISheet, new Rectangle(0, 621, 240, 299), scale), // portrait icon
                        // "MEGAMAN"
                        stats.name.ToUpper() // character name
                    ));
                    break;
                default:
                    throw new Exception($"Error: character name {stats.name} not recognized");
            }
            

             */

            // Character output = new Character(stats.name, tex, new Vector2(100, 100), uiData);
            return output;
        }

        private static IController GenerateController(string controllerFile, Character character, string temp = "")
        {
            const int MAX_NUMBER_OF_CONTROLS = 8;
            IController controllerOutput = default;
            Dictionary<Keys, ICommand> controls = new();
            bool gotHitKey = false;

            XElement controlsElement = GetXMLRootElement("PlayerControls", controllerFile);
            List<Keys> controlKeys = ParseControlsFromXML(controlsElement, out gotHitKey);
            if (controlKeys.Count !=  MAX_NUMBER_OF_CONTROLS- (gotHitKey ? 0 : 1))
            {
                throw new Exception($"controls file {controllerFile} was not able to parse {MAX_NUMBER_OF_CONTROLS - (gotHitKey ? 0 : 1)} controls, instead: {controlKeys.Count}");
            }
            ButtonDataManager charButtonManager = character.GetButtonDataManager;

            if(gotHitKey)
            {
                controls.Add(controlKeys.Last(), new UpdateButtonCommand(charButtonManager.ButtonDataSheet[GameButtons.GotHit]));
            }

            controls.Add(controlKeys[0], new UpdateButtonCommand(charButtonManager.ButtonDataSheet[GameButtons.Up]));
            controls.Add(controlKeys[1], new UpdateButtonCommand(charButtonManager.ButtonDataSheet[GameButtons.Down]));
            controls.Add(controlKeys[2], new UpdateButtonCommand(charButtonManager.ButtonDataSheet[GameButtons.Left]));
            controls.Add(controlKeys[3], new UpdateButtonCommand(charButtonManager.ButtonDataSheet[GameButtons.Right]));
            controls.Add(controlKeys[4], new UpdateButtonCommand(charButtonManager.ButtonDataSheet[GameButtons.Jump]));
            controls.Add(controlKeys[5], new UpdateButtonCommand(charButtonManager.ButtonDataSheet[GameButtons.Attack]));
            controls.Add(controlKeys[6], new UpdateButtonCommand(charButtonManager.ButtonDataSheet[GameButtons.Special]));

            controllerOutput = new KeyboardController(charButtonManager, controls);

            return controllerOutput;
        }

        private static void ParsePlatforms(string platformFile, List<Platform> platformList)
        {
            XElement platformsRoot = GetXMLRootElement("Platforms", platformFile);
            Platform platform;
            foreach (XElement platformElement in platformsRoot.Elements("Platform"))
            {
                platform = GeneratePlatform(platformElement);
                if(platform == null)
                {
                    throw new Exception($"Generate Platform failed in file {platformFile}");
                }
                platformList.Add(platform);
            }

        }
        private static Platform GeneratePlatform(XElement platformXelement)
        {
            string type = GetXElementStringNoSpaces("Type", platformXelement);
            PlatformTypeEnum typeEnum= Enum.Parse<PlatformTypeEnum>(type);

            string spriteSheet = GetXElementStringNoSpaces("SpriteSheet", platformXelement);
            Texture2D tex = Game1.Get().Content.Load<Texture2D>(spriteSheet);

            string positionStr = GetXElementOrAssert("SourcePos", platformXelement).Value;
            int[] positionInts = Array.ConvertAll(positionStr.Split(new char[] {' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);
            

            string sizeStr = GetXElementOrAssert("SourceSize", platformXelement).Value;
            int[] sizeInts = Array.ConvertAll(sizeStr.Split(new char[] {' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);
            Rectangle sourceRect = new(positionInts[0], positionInts[1], sizeInts[0], sizeInts[1]);
            
            return new Platform(typeEnum, sourceRect, tex);
        }

        private static PlayerBattleUI GeneratePlayerUI(string UIFile, ICharacter character)
        {
            var (font, requiredIcons, position) = ParseUIFile(UIFile);
            return new PlayerBattleUI(font, (Character) character, position, requiredIcons[0], requiredIcons[1]);;
        }
    
        private static (SpriteFont, List<Sprite>, Vector2) ParseUIFile(string fileName)
        {
            // CharacterName
            // Font
            // 2 required sprites
            List<Sprite> icons = new();
            XElement root = GetXMLRootElement("UI", fileName);

            // string characterName = GetXElementOrAssert("CharacterName", root).Value.Replace(" ", string.Empty);
            string font = GetXElementOrAssert("Font", root).Value.Replace(" ", string.Empty);
            SpriteFont spriteFont = Game1.Get().Content.Load<SpriteFont>(font);

            string positionStr = GetXElementOrAssert("Position", root).Value;
            int[] positionInts = Array.ConvertAll(positionStr.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);
            Point position = new(positionInts[0], positionInts[1]);


            // XElement stockIcon = GetXElementOrAssert("StockIcon", root);
            // XElement portraitIcon = GetXElementOrAssert("PortraitIcon", root);
            XElement portraitBackground=  GetXElementOrAssert("PortraitBackground", root);
            XElement nameHolder = GetXElementOrAssert("NameHolder", root);

            // icons.Add(ParseIcon(stockIcon));
            // icons.Add(ParseIcon(portraitIcon));
            icons.Add(ParseIcon(portraitBackground));
            icons.Add(ParseIcon(nameHolder));

            return (spriteFont, icons, position.ToVector2());
        }


        private static XElement GetXMLRootElement(string rootName, string fileName)
        {
            string dataFolder = XMLData.GetDataFolder();
            string sceneDataFile = Path.Combine(dataFolder, rootName + "Data", fileName + ".XML");

            XDocument sceneDoc = XDocument.Load(sceneDataFile);
            XElement root = sceneDoc.Root;
            if (root.Name != rootName)
            {
                throw new ArgumentException($"File at {sceneDataFile} has wrong root of {root.Name} and not {rootName}");
            }

            return root;
        }

        private static Sprite ParseIcon(XElement iconElement)
        {
            // SpriteSheet
            // Source Rectangle
            // Scale

            string spriteSheet = GetXElementOrAssert("SpriteSheet", iconElement).Value.Replace(" ", string.Empty);

            string rectangleStr = GetXElementOrAssert("Source", iconElement).Value;
            int[] rectInts = Array.ConvertAll(rectangleStr.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);
            Rectangle source = new(rectInts[0], rectInts[1], rectInts[2], rectInts[3]);

            float scale = float.Parse(GetXElementOrAssert("Scale", iconElement).Value);

            Texture2D sheet = Game1.Get().Content.Load<Texture2D>(spriteSheet);

            return new Sprite(sheet, source, scale);
        }

        
        private static List<Keys> ParseControlsFromXML(XElement playerControlsXML, out bool containsMisc)
        {
            List<string> stringKeys = new();
            List<Keys> keyOutput = new();
            containsMisc = false;
            XElement temp = null;
            // 4 from Movement
            // 2 Attack
            // 1 optional Misc
            XElement movement = GetXElementOrAssert("Movement", playerControlsXML);
            stringKeys.Add(GetXElementStringNoSpaces("Up", movement));
            stringKeys.Add(GetXElementStringNoSpaces("Down", movement));
            stringKeys.Add(GetXElementStringNoSpaces("Left", movement));
            stringKeys.Add(GetXElementStringNoSpaces("Right", movement));
            stringKeys.Add(GetXElementStringNoSpaces("Jump", movement));

            XElement attack = GetXElementOrAssert("Attack", playerControlsXML);
            stringKeys.Add(GetXElementStringNoSpaces("Neutral", attack));
            stringKeys.Add(GetXElementStringNoSpaces("Special", attack));
            
            if((temp = playerControlsXML.Element("Misc")) != null)
            {
                XElement misc = GetXElementOrAssert("Misc", playerControlsXML);
                stringKeys.Add(GetXElementStringNoSpaces("GotHit", misc));
                containsMisc = true;
            }
            foreach(string key in stringKeys)
            {
                Keys parsedKey = StringToKey(key);
                if(parsedKey != default) keyOutput.Add(parsedKey);
            }
            return keyOutput;
        }
        
        private static string GetXElementStringNoSpaces(string name, XElement xElement)
        {
            return GetXElementOrAssert(name, xElement).Value.Replace(" ", string.Empty);
        }


        private static Keys StringToKey(string stringKey)
        {
            return Enum.Parse<Keys>(stringKey);
        }
    }
}
