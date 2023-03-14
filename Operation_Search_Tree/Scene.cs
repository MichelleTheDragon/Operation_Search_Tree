using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    public class Scene //The base scene, that all scenes needs in order to run
    {
        public List<GameObject> MyGameObjects { get; protected set; } = new List<GameObject>(); //all gameobjects in the scene

        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObject myGameObject in MyGameObjects)
            {
                if (myGameObject.GetType().Equals(typeof(Node))) //if the gameobject is of the subclass Node
                {
                    ((Node)myGameObject).Update(gameTime); //Update Node
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
    }
}
