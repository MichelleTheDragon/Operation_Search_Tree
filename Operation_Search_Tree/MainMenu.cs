using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    class MainMenu : Scene
    {
        GameWorld myWorld;

        public MainMenu(GameWorld myWorld)
        {
            this.myWorld = myWorld;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch _spriteBatch)
        {

        }

        public int Start()
        {
            GameWorld.SceneNumber++;
            return 1;
        }
        public int Quit()
        {
            myWorld.Exit();
            return 1;
        }
    }
}
