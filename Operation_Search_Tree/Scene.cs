using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    class Scene
    {
        protected List<GameObject> myGameObjects = new List<GameObject>();

        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObject myGameObject in myGameObjects)
            {
                myGameObject.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin();

            foreach (GameObject myGameObject in myGameObjects)
            {
                myGameObject.Draw(_spriteBatch);
            }

            _spriteBatch.End();
        }
    }
}
