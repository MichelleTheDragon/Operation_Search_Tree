using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    class NodeTree : Scene
    {
        private Texture2D nodeSprite;
        private Node startNode;
        private Random rngAmount = new Random();
        private Vector2 startNodePos;
        private bool keyDownR;
        private List<Node> nodeset = new List<Node>();
        public List<Node> Nodeset { get { return nodeset; } }
        private List<Vector2[]> nodesConnected = new List<Vector2[]>();

        public NodeTree(ContentManager Content, Vector2 startNodePos)
        {
            this.startNodePos = startNodePos;
            nodeSprite = Content.Load<Texture2D>("Sprites/Node");
            GenerateNodes(5, true);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.R) && !keyDownR)
            {
                GenerateNodes(5, true);
                keyDownR = true;
            } else if (state.IsKeyUp(Keys.R) && keyDownR)
            {
                keyDownR = false;
            }
        }

        public void AddEdge(Node from, Node to)
        {
            from.AddEdge(to);
            to.AddEdge(from);
            nodesConnected.Add(new Vector2[] { from.WorldPos, to.WorldPos });
        }

        public void ConnectNodes()
        {

            foreach (Node myNode in nodeset)
            {
                if (myNode.Depth > 0)
                {
                    if (myNode.Depth == 1)
                    {
                        AddEdge(myNode, nodeset[0]);
                    } else
                    {
                        Node closestNode = null;
                        double closestValue = 0;
                        foreach(Node myNode2 in nodeset)
                        {
                            if(myNode2.Depth == myNode.Depth - 1)
                            {
                                if (closestNode == null)
                                {
                                    closestNode = myNode2;
                                    closestValue = Math.Sqrt(Math.Pow(myNode2.WorldPos.X - myNode.WorldPos.X, 2) + Math.Pow(myNode2.WorldPos.Y - myNode.WorldPos.Y, 2));
                                }
                                else
                                {
                                    double tempDistance = Math.Sqrt(Math.Pow(myNode2.WorldPos.X - myNode.WorldPos.X, 2) + Math.Pow(myNode2.WorldPos.Y - myNode.WorldPos.Y, 2));
                                    if (tempDistance < closestValue)
                                    {
                                        closestNode = myNode2;
                                        closestValue = tempDistance;
                                    }
                                }
                            }
                        }

                        AddEdge(myNode, closestNode);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin();
            foreach (Vector2[] nodeConnected in nodesConnected)
            {
                GameWorld.DrawLine(_spriteBatch, nodeConnected[0], nodeConnected[1], Color.White, 3);
            }
            _spriteBatch.End();
            base.Draw(_spriteBatch);
        }

        public void GenerateNodes(int maxDepth, bool isBothWay)
        {
            myGameObjects = new List<GameObject>();
            nodeset = new List<Node>(); 
            nodesConnected = new List<Vector2[]>();

            startNode = new Node(nodeSprite, startNodePos, 0.5f, 0);
            myGameObjects.Add(startNode);
            nodeset.Add(startNode);
            for (int i = 0; i < maxDepth; i++)
            {
                int randomAmountofNodes = rngAmount.Next((maxDepth - i) + 1, 10 + i * 3);
                List<Vector2> myNodePositions = new List<Vector2>();
                for (int j = 0; j < randomAmountofNodes; j++)
                {
                    float randomAngle = 0;
                    bool notOverlapping = true;
                    int failCounter = 0;
                    bool foundPlacement = true;
                    Vector2 newNodePos = Vector2.Zero;
                    while (notOverlapping)
                    {
                        notOverlapping = false;
                        //randomAngle = 0.3f + (float)rngAmount.NextDouble() * 2.55f;
                        randomAngle = (float)(rngAmount.NextDouble() * Math.PI * 2);
                        float myAngleX = (float)Math.Cos(randomAngle);
                        float myAngleY = (float)Math.Sin(randomAngle);
                        newNodePos = new Vector2(startNodePos.X + (80 * (i + 1) * myAngleX), startNodePos.Y + (80 * (i + 1) * myAngleY));
                        foreach (Vector2 myNodePosition in myNodePositions)
                        {
                            if (Math.Sqrt(Math.Pow(newNodePos.X - myNodePosition.X, 2) + Math.Pow(newNodePos.Y - myNodePosition.Y, 2)) < 30.0f)
                            {
                                notOverlapping = true;
                            }
                        }
                        if (notOverlapping)
                        {
                            failCounter++;
                            if (failCounter > 5 * (i + 1))
                            {
                                foundPlacement = false;
                                break;
                            }
                        }
                    }
                    if (foundPlacement)
                    {
                        Node newNode = new Node(nodeSprite, newNodePos, 0.4f - 0.02f * (i + 1), i + 1);
                        myNodePositions.Add(newNode.WorldPos);
                        myGameObjects.Add(newNode); //temp
                        nodeset.Add(newNode);
                    }
                }
            }
            ConnectNodes();
        }
    }
}
