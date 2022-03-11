using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    class Node : GameObject
    {
        private int depth;
        public int Depth { get { return depth; } }
        private bool isHovered;
        private bool isClicked;
        private List<Edge> edges = new List<Edge>();
        public List<Edge> Edges { get { return edges; } }

        public Node(Texture2D sprite, Vector2 worldPos, float scale, int depth) : base(sprite, worldPos)
        {
            this.depth = depth;
            base.scale = scale;
            if (depth == 0)
            {
                colour = Color.LightGreen;
            }
            //RngColour();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            MouseState mouseState = Mouse.GetState();
            Point mousePoint = new Point(mouseState.X, mouseState.Y);
            Rectangle rectangle = new Rectangle((int)worldPos.X - 10, (int)worldPos.Y - 10, 20, 20);
            //if(Math.Sqrt(Math.Pow(mousePoint.X - rectangle.X, 2) + Math.Pow(mousePoint.Y - rectangle.Y, 2)) < 30.0f)
            //{

            //}

            if (rectangle.Contains(mousePoint) && depth > 0)
            {
                isHovered = true;
                colour = Color.LightPink;
                isClicked = mouseState.LeftButton == ButtonState.Pressed;
            }
            else if(isHovered == true)
            {
                colour = Color.White;
                isHovered = false;
                isClicked = false;
            }
        }
        public void AddEdge(Node other)
        {
            edges.Add(new Edge(this, other));
        }

        public void RngColour()
        {
            switch (depth)
            {
                case 0:
                    colour = Color.White;
                    break;
                case 1:
                    colour = Color.LightBlue;
                    break;
                case 2:
                    colour = Color.LightGreen;
                    break;
                case 3:
                    colour = Color.LightYellow;
                    break;
                case 4:
                    colour = Color.LightPink;
                    break;
                case 5:
                    colour = Color.Lavender;
                    break;
            }
        }
    }
}
