using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    public class Node : GameObject
    {
        private int depth;
        public int Depth { get { return depth; } }
        private bool isHovered;
        private List<Edge> edges = new List<Edge>();
        public List<Edge> Edges { get { return edges; } }
        private bool clickRegistered;
        public Color LastColour { get; protected set; } = Color.White;
        private Color resetColour = Color.White;

        public Node(Texture2D sprite, Vector2 worldPos, float scale, int depth) : base(sprite, worldPos)
        {
            this.depth = depth;
            base.scale = scale;
            if (depth == 0)
            {
                colour = Color.LightGreen;
                resetColour = colour;
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

            if (rectangle.Contains(mousePoint) && depth > 0 && isHovered != true)
            {
                isHovered = true;
                LastColour = colour;
                colour = Color.LightBlue;
            }
            else if(!rectangle.Contains(mousePoint) && isHovered == true)
            {
                colour = LastColour;
                isHovered = false;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && isHovered && !clickRegistered && !NodeTree.isRunning)
            {
                NodeTree.ChangeGoal(this);

                clickRegistered = true;
                colour = Color.Blue;
                LastColour = colour;
            } else if (mouseState.LeftButton == ButtonState.Released && clickRegistered)
            {
                clickRegistered = false;
            }
        }
        public void AddEdge(Node other)
        {
            edges.Add(new Edge(this, other));
        }

        public void ChangeColour(Color newColour)
        {
            colour = newColour;
        }

        public void ResetColour()
        {
            colour = resetColour;
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
