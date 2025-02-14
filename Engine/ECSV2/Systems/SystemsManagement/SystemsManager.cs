using KirbStomp.Engine.ECSV2.Systems.ISystems;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Systems.SystemsManagement
{
    internal class SystemsManager
    {
        private List<ISystem> updatableSystems;
        private List<ISystem> loadableSystems;
        private List<ISystem> drawableSystems;

        public SystemsManager()
        {
            updatableSystems = new();
            loadableSystems = new();
            drawableSystems = new();
            SystemsLoader.LoadAllSystem(this);
        }

        public void AddSystem(ISystem system)
        {
            if (system is IUpdatableSystem) updatableSystems.Add(system);
            if (system is IDrawableSystem) drawableSystems.Add(system);
            if (system is ILoadableSystem) loadableSystems.Add(system);
        }

        public void UpdateAllSystem(float deltaTime)
        {
            foreach (ISystem system in updatableSystems)
            {
                ((IUpdatableSystem)system).Update(deltaTime);
            }
        }

        public void DrawAllSystem(SpriteBatch spriteBatch)
        {
            foreach (ISystem system in drawableSystems)
            {
                ((IDrawableSystem)system).Draw(spriteBatch);
            }
        }

        public void LoadAlLSystem(ContentManager content)
        {
            foreach (ISystem system in loadableSystems)
            {
                ((ILoadableSystem)system).Load(content);
            }
        }
		public void ResetAllSystems()
		{
            updatableSystems = new();
            loadableSystems = new();
            drawableSystems = new();
            SystemsLoader.LoadAllSystem(this);
		}


    }
}
