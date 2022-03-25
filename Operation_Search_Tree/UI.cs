﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    class UI : GameObject
    {
        private List<UIText> myTexts = new List<UIText>();
        private List<Button> myButtons = new List<Button>();
        private List<Button> searchButtons = new List<Button>(); //the DFS and BFS button

        public UI(SpriteFont baseFont, Texture2D baseButton, NodeTree nodeTreeScene, Viewport myScreen)
        {
            //Top Left buttons
            myButtons.Add(new Button(baseButton, new Vector2(50, 50), baseFont, "Generate Nodes", 250, 40, nodeTreeScene.GenerateNew, 0)); 
            myTexts.Add(new UIText(baseFont, new Vector2(50, 105), "Depth: ", nodeTreeScene));
            myButtons.Add(new Button(baseButton, new Vector2(210, 100), baseFont, "-", 40, 40, nodeTreeScene.DepthLower, 0));
            myButtons.Add(new Button(baseButton, new Vector2(260, 100), baseFont, "+", 40, 40, nodeTreeScene.DepthHigher, 0));

            Button newButton1 = new Button(baseButton, new Vector2(50, 150), baseFont, "BFS", 125, 40, nodeTreeScene.SetBFS, 1, this);
            Button newButton2 = new Button(baseButton, new Vector2(175, 150), baseFont, "DFS", 125, 40, nodeTreeScene.SetDFS, 1, this);
            myButtons.Add(newButton1);
            myButtons.Add(newButton2);
            searchButtons.Add(newButton1);
            searchButtons.Add(newButton2);

            myButtons.Add(new Button(baseButton, new Vector2(50, 200), baseFont, "Start Search", 250, 40, nodeTreeScene.RunSearchButton, 0));

            //Bottom buttons
            myButtons.Add(new Button(baseButton, new Vector2(myScreen.Width/2 - 100, myScreen.Height - 80), baseFont, "Step by Step", 200, 40, nodeTreeScene.SetStepByStep, 2));
            myButtons.Add(new Button(baseButton, new Vector2(myScreen.Width / 2 - 150, myScreen.Height - 80), baseFont, "-", 40, 40, nodeTreeScene.StepDown, 0));
            myButtons.Add(new Button(baseButton, new Vector2(myScreen.Width / 2 + 110, myScreen.Height - 80), baseFont, "+", 40, 40, nodeTreeScene.StepUp, 0));

            //Top right buttons
            myButtons.Add(new Button(baseButton, new Vector2(myScreen.Width - 300, 50), baseFont, "Autorun", 250, 40, nodeTreeScene.AutoRun, 0));
        }

        public void CleanSearchColours() //Makes sure the DFS and BFS buttons are white when not active
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
                if (myText.dynamicText) //if the text contains values that can change at runtime
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

    class UIText : GameObject //A basic class that contains all the information needed for text visualization 
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
