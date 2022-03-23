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
        private NodeTree nodeTreeScene;
        private List<Button> searchButtons = new List<Button>();
        //private Texture2D baseButton;

        public UI(SpriteFont baseFont, Texture2D baseButton, NodeTree nodeTreeScene, Viewport myScreen)
        {
            this.baseFont = baseFont;
            this.nodeTreeScene = nodeTreeScene;
            myButtons.Add(new Button(baseButton, new Vector2(50, 50), baseFont, "Generate Nodes", 250, 40, nodeTreeScene.GenerateNew, false));
            myTexts.Add(new UIText(baseFont, new Vector2(50, 105), "Depth: ", nodeTreeScene));
            myButtons.Add(new Button(baseButton, new Vector2(210, 100), baseFont, "-", 40, 40, nodeTreeScene.DepthLower, false));
            myButtons.Add(new Button(baseButton, new Vector2(260, 100), baseFont, "+", 40, 40, nodeTreeScene.DepthHigher, false));

            Button newButton1 = new Button(baseButton, new Vector2(50, 150), baseFont, "BFS", 125, 40, nodeTreeScene.SetBFS, true, this);
            Button newButton2 = new Button(baseButton, new Vector2(175, 150), baseFont, "DFS", 125, 40, nodeTreeScene.SetDFS, true, this);
            myButtons.Add(newButton1);
            myButtons.Add(newButton2);
            searchButtons.Add(newButton1);
            searchButtons.Add(newButton2);

            myButtons.Add(new Button(baseButton, new Vector2(50, 200), baseFont, "Start Search", 250, 40, nodeTreeScene.RunSearchButton, false));

            myButtons.Add(new Button(baseButton, new Vector2(myScreen.Width - 300, 50), baseFont, "Autorun", 250, 40, nodeTreeScene.AutoRun, false));
        }

        public void CleanSearchColours()
        {
            foreach (Button searchButton in searchButtons)
            {
                searchButton.CleanColour();
            }
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);

            foreach (Button myButton in myButtons)
            {
                myButton.Update(gameTime);
            }
            foreach (UIText myText in myTexts)
            {
                myText.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            foreach (UIText myText in myTexts)
            {
                if (myText.dynamicText)
                {
                    _spriteBatch.DrawString(myText.Font, myText.FullText, myText.Pos, myText.Colour);
                }
                else {
                    _spriteBatch.DrawString(myText.Font, myText.Text, myText.Pos, myText.Colour);
                }
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
        private NodeTree nodeTree;
        public string FullText { get; private set; } = "";
        public  bool dynamicText { get; private set; }

        public UIText(SpriteFont font, Vector2 pos, string text)
        {
            Font = font;
            Pos = pos;
            FullText = text;
        }
        public UIText(SpriteFont font, Vector2 pos, string text, NodeTree nodeTree)
        {
            Font = font;
            Pos = pos;
            Text = text;
            this.nodeTree = nodeTree;
            dynamicText = true;
        }

        public UIText(SpriteFont font, Vector2 pos, string text, Color colour)
        {
            Font = font;
            Pos = pos;
            FullText = text;
            Colour = colour;
        }

        public override void Update(GameTime gameTime)
        {
            if (dynamicText)
            {
                FullText = Text + nodeTree.nodeDepth;
            }
        }
    }
}
