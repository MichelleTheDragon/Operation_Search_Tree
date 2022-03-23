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
        private Func<int> myFunc;
        private bool buttonColoured;
        private UI myUI;
        private Color textColour = Color.Black;

        public Button(Texture2D sprite, Vector2 worldPos, SpriteFont font, Func<int> myFunc) : base(sprite, worldPos)
        {
            base.WorldPos = worldPos;
            this.font = font;
            //vectorScale = new Vector2(200.0f, 40.0f);
            this.myFunc = myFunc;
        }

        public Button(Texture2D sprite, Vector2 worldPos, SpriteFont font, string text, int rectWidth, int rectHeight, Func<int> myFunc, bool canBeHeld) : base(sprite, worldPos)
        {
            base.WorldPos = new Vector2(worldPos.X + rectWidth / 2, worldPos.Y + rectHeight / 2);
            this.font = font;
            this.text = text;
            this.myFunc = myFunc;
            buttonColoured = canBeHeld;
            rect = new Rectangle(0, 0, rectWidth, rectHeight);
            origin = new Vector2(rect.Size.X / 2, rect.Size.Y / 2);
            textOrigin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
        }

        public Button(Texture2D sprite, Vector2 worldPos, SpriteFont font, string text, int rectWidth, int rectHeight, Func<int> myFunc, bool canBeHeld, UI myUI) : base(sprite, worldPos)
        {
            base.WorldPos = new Vector2(worldPos.X + rectWidth / 2, worldPos.Y + rectHeight / 2);
            this.font = font;
            this.text = text;
            this.myFunc = myFunc;
            this.myUI = myUI;
            buttonColoured = canBeHeld;
            rect = new Rectangle(0, 0, rectWidth, rectHeight);
            origin = new Vector2(rect.Size.X / 2, rect.Size.Y / 2);
            textOrigin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = new Point(mouseState.X, mouseState.Y);
            Rectangle rectangle = new Rectangle((int)WorldPos.X - rect.Width/2, (int)WorldPos.Y - rect.Height/2, rect.Width, rect.Height);

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
                myFunc();
                if (buttonColoured)
                {
                    myUI.CleanSearchColours();
                    colour = Color.Purple;
                    textColour = Color.White;
                    LastColour = colour;
                }
                clickRegistered = true;
            }
            else if (mouseState.LeftButton == ButtonState.Released && clickRegistered)
            {
                clickRegistered = false;
            }
        }

        public void CleanColour()
        {
            colour = Color.White;
            LastColour = colour;
            textColour = Color.Black;
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
            _spriteBatch.DrawString(font, text, WorldPos, textColour, rotation, textOrigin, textScale, effects, layer);
        }
    }
}
