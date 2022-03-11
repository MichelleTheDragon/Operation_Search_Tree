using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    public class GameObject
    {
        protected Texture2D sprite;
        protected Rectangle rect;
        protected Vector2 worldPos;
        public Vector2 WorldPos { get { return worldPos; } }
        protected float scale = 1.0f;
        protected float layer = 1.0f;
        protected Color colour = Color.White;
        protected Vector2 origin = Vector2.Zero;
        protected SpriteEffects effects;
        protected float rotation = 0;

        public GameObject()
        {

        }

        public GameObject(Texture2D sprite, Vector2 worldPos)
        {
            this.sprite = sprite;
            this.worldPos = worldPos;
            rect = new Rectangle(0, 0, sprite.Width, sprite.Height);
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            if (sprite != null)
            {
                _spriteBatch.Draw(sprite, worldPos, rect, colour, rotation, origin, scale, effects, layer);
            }
        }
    }
}
