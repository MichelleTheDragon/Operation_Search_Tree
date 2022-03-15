using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    class UI : GameObject
    {
        private SpriteFont baseFont;
        private List<UIText> myTexts = new List<UIText>();
        private List<Button> myButtons = new List<Button>();
        //private Texture2D baseButton;

        public UI(SpriteFont baseFont, Texture2D baseButton)
        {
            this.baseFont = baseFont;
            //myTexts.Add(new UIText(baseFont, new Vector2(20, 20), "Hello!"));
            myButtons.Add(new Button(baseButton, new Vector2(150, 70), baseFont, "Testing..."));
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            foreach (UIText myText in myTexts)
            {
                _spriteBatch.DrawString(myText.Font, myText.Text, myText.Pos, myText.Colour);
            }
            foreach (Button myButton in myButtons)
            {
                myButton.Draw(_spriteBatch);
            }
        }
    }

    class UIText : GameObject
    {
        public SpriteFont Font { get; private set; }
        public Vector2 Pos { get; private set; } = Vector2.Zero;
        public string Text { get; private set; } = "";
        public Color Colour { get; private set; } = Color.White;

        public UIText(SpriteFont font, Vector2 pos, string text)
        {
            Font = font;
            Pos = pos;
            Text = text;
        }

        public UIText(SpriteFont font, Vector2 pos, string text, Color colour)
        {
            Font = font;
            Pos = pos;
            Text = text;
            Colour = colour;
        }
    }
}
