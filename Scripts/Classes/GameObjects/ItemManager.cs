using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Projectiles
{
    public class ItemManager
    {
        private List<AItem> _items;
        private List<AItem> _removedPool;
        private CollisionSystem _collisionSystem;
        //this can be something loaded in by level loader maybe, so that the items of a certain level can be different
        private String[] LEVEL_RANDOM_ITEMS_TO_SPAWN = { "HamburgerItem", "ArrowStormItem", "BombItem" };
        private Vector2[] POSSIBLE_RANDOM_SPAWN_LOCATIONS = {new Vector2(200,300),new Vector2(200,100), new Vector2(300,0), new Vector2(600,200)};
        private float TIME_BETWEEN_SPAWNS = 8f;
        //*****************************
        private float spawnDebounce = 0;

        public ItemManager(CollisionSystem collisionSystem)
        {
            this._removedPool = new List<AItem>();
            this._items = new List<AItem>();
            this._collisionSystem = collisionSystem;
        }

        public void Reset(CollisionSystem collisionSystem)
        {
            this._collisionSystem = collisionSystem;
            this._items.Clear();
            this._removedPool.Clear();
        }

        public void Update(float dt)
        {
            spawnDebounce += dt;

            foreach (AItem go in this._items)
            {
                go.UpdateCollider();
                go.Update(dt);
            }

            foreach (AItem go in this._removedPool)
            {
                this._items.Remove(go);
                this._collisionSystem.RemoveObject(go);

            }
            this._removedPool.Clear();

            if (this.spawnDebounce >= TIME_BETWEEN_SPAWNS)
            {
                this.spawnDebounce = 0;
                this.SpawnRandomItem();
            }





        }

        public void SpawnRandomItem()
        {
           
            Random random = new Random();
            int indexOfRandItem = random.Next(0,this.LEVEL_RANDOM_ITEMS_TO_SPAWN.Length);
            int indexOfRandPos = random.Next(0, this.POSSIBLE_RANDOM_SPAWN_LOCATIONS.Length);

            Vector2 pos = this.POSSIBLE_RANDOM_SPAWN_LOCATIONS[indexOfRandPos];
            String itemName = this.LEVEL_RANDOM_ITEMS_TO_SPAWN[indexOfRandItem];

            this.SpawnItem(itemName, pos);

        }



        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AItem go in this._items)
            {
                go.Draw(spriteBatch);
            }
        }

        public void RemoveItem(AItem item)
        {
            if (!this._items.Contains(item))
            {
                throw new Exception("projectile cannot be removed, dne: ");
            }
            this._removedPool.Add(item);
        }


        public void AddItem(AItem item)
        {

            this._items.Add(item);
            item.SetItemManager(this);
            
            item.ProvideItemCarriers();
            item.RegisterCollider();
            this._collisionSystem.RegisterObject(item);
        }

        public void SpawnItem(String itemName, Vector2 pos)
        {

            Type type = Type.GetType(itemName);
            //try to find w/o extra
            if (type == null)
            {
                type = Type.GetType("KirbStomp.Scripts.Classes.GameObjects.Items." + itemName);
            }
            if (type == null)
            {
                type = Type.GetType("KirbStomp.Scripts.Classes.GameObjects.Items." + itemName + ".cs");
            }
            if (type == null)
            {
                throw new Exception("itemname: " + itemName + " is not a valid class or item");
            }
            else if (!type.IsSubclassOf(typeof(AItem)))
            {
                throw new Exception("itemName: " + itemName + " is not a item");
            }
            AItem item = (AItem)Activator.CreateInstance(type, pos);
            this.AddItem(item);
        }

        public void LoadSpawnFunctionality(String[] listOfItemsToBeSpawned, Vector2[] possibleSpawnLocations,  float timeBetweenSpawns)
        {
            if(listOfItemsToBeSpawned == null ||listOfItemsToBeSpawned.Length < 1)
            {
                throw new Exception("invalid list of items");

            }
            if(possibleSpawnLocations == null || possibleSpawnLocations.Length < 1)
            {
                throw new Exception("invalid list of spawn pos");
            }
            this.LEVEL_RANDOM_ITEMS_TO_SPAWN = listOfItemsToBeSpawned;
            this.TIME_BETWEEN_SPAWNS = timeBetweenSpawns;
            this.POSSIBLE_RANDOM_SPAWN_LOCATIONS = possibleSpawnLocations;
        }
    }
}
