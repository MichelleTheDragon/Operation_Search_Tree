using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    class Button : GameObject
    {
        private string text;
        private SpriteFont font;
        private float textScale = 1.0f;
        private Vector2 textOrigin = Vector2.Zero;

        public Button(Texture2D sprite, Vector2 worldPos, SpriteFont font) : base(sprite, worldPos)
        {
            base.WorldPos = worldPos;
            this.font = font;
            //vectorScale = new Vector2(200.0f, 40.0f);
        }

        public Button(Texture2D sprite, Vector2 worldPos, SpriteFont font, string text) : base(sprite, worldPos)
        {
            base.WorldPos = worldPos;
            this.font = font;
            this.text = text;
            rect = new Rectangle(0,0,200, 40);
            origin = new Vector2(rect.Size.X / 2, rect.Size.Y / 2);
            textOrigin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
            _spriteBatch.DrawString(font, text, WorldPos, Color.Black, rotation, textOrigin, textScale, effects, layer);
        }
    }
}
