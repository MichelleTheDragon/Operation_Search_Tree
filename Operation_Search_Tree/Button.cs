using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private bool isHovered;
        private Color LastColour;
        private bool clickRegistered;

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = new Point(mouseState.X, mouseState.Y);
            Rectangle rectangle = new Rectangle((int)WorldPos.X - 10, (int)WorldPos.Y - 10, 20, 20);

            if (rectangle.Contains(mousePoint) && isHovered != true && mouseState.LeftButton == ButtonState.Released)
            {
                isHovered = true;
                LastColour = colour;
                colour = Color.LightBlue;
            }
            else if (!rectangle.Contains(mousePoint) && isHovered == true)
            {
                colour = LastColour;
                isHovered = false;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && isHovered && !clickRegistered)
            {
                clickRegistered = true;
                colour = Color.Blue;
                LastColour = colour;
            }
            else if (mouseState.LeftButton == ButtonState.Released && clickRegistered)
            {
                clickRegistered = false;
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
            _spriteBatch.DrawString(font, text, WorldPos, Color.Black, rotation, textOrigin, textScale, effects, layer);
        }
    }
}
