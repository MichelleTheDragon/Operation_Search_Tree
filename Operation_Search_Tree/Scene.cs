using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    public class Scene
    {
        //protected List<GameObject> myGameObjects = new List<GameObject>();
        public List<GameObject> MyGameObjects { get; protected set; } = new List<GameObject>();

        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObject myGameObject in MyGameObjects)
            {
                if (myGameObject.GetType().Equals(typeof(Node)))
                {
                    ((Node)myGameObject).Update(gameTime);//, Mouse.GetState());
                }
                myGameObject.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin();

            foreach (GameObject myGameObject in MyGameObjects)
            {
                myGameObject.Draw(_spriteBatch);
            }

            _spriteBatch.End();
        }
        //public virtual void Draw(SpriteBatch _spriteBatch, Matrix cameraTransform)
        //{
        //    _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, cameraTransform);

        //    foreach (GameObject myGameObject in MyGameObjects)
        //    {
        //        myGameObject.Draw(_spriteBatch);
        //    }

        //    _spriteBatch.End();
        //}
    }
}
