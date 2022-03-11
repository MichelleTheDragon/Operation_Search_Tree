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
        private bool keyDownSpace;
        private static List<Node> nodeset = new List<Node>();
        public List<Node> Nodeset { get { return nodeset; } }
        private List<Vector2[]> nodesConnected = new List<Vector2[]>();
        private List<Vector2[]> pathChosen = new List<Vector2[]>();
        public List<SlowColours> VisualPath = new List<SlowColours>();

        public static Node goal;
        private SearchTrees mySearchTree;
        private bool visualizeColours;
        private int colourShown = 0;
        private float colourTimer = 0.0f;
        private bool drawFastestPath;
        private int searchMethod = 0;
        public static bool isRunning { get; protected set; }

        public NodeTree(ContentManager Content, Vector2 startNodePos)
        {
            this.startNodePos = startNodePos;
            nodeSprite = Content.Load<Texture2D>("Sprites/Node");
            GenerateNodes(5, true);
            mySearchTree = new SearchTrees();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (visualizeColours)
            {
                if (colourTimer > 0.1f)
                {
                    VisualPath[colourShown].NodetoColour.ChangeColour(VisualPath[colourShown].Colour);
                    colourShown++;
                    colourTimer = 0.0f;
                }
                else
                {
                    colourTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (colourShown == VisualPath.Count)
                {
                    drawFastestPath = true;
                    isRunning = false;
                    visualizeColours = false;
                }
            }

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.R) && !keyDownR)
            {
                GenerateNodes(5, true);
                keyDownR = true;
            } else if (state.IsKeyUp(Keys.R) && keyDownR)
            {
                keyDownR = false;
            }

            if (state.IsKeyDown(Keys.Space) && !keyDownSpace && goal != null && !isRunning)
            {
                VisualPath = new List<SlowColours>();
                List<Node> newPath = new List<Node>();
                switch (searchMethod)
                {
                    case 0:
                        newPath = mySearchTree.BreadthFirstSearch(Nodeset, goal, VisualPath);
                        break;
                    default:
                        newPath = mySearchTree.BreadthFirstSearch(Nodeset, goal, VisualPath);
                        break;
                }
                pathChosen = new List<Vector2[]>();
                foreach (Node pathStep in newPath)
                {
                    if (pathStep.Depth > 0)
                    {
                        pathChosen.Add(new Vector2[] { pathStep.Edges[0].To.WorldPos, pathStep.Edges[0].From.WorldPos });
                        VisualPath.Add(new SlowColours(pathStep.Edges[0].To, Color.Blue));
                    }
                }
                visualizeColours = true;
                colourShown = 0;
                keyDownSpace = true;
                isRunning = true;
            }
            else if (state.IsKeyUp(Keys.Space) && keyDownSpace)
            {
                keyDownSpace = false;
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
            List<Node[]> siblings = new List<Node[]>();
            List<Node> hasSibling = new List<Node>();
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
                        Node closestSiblingNode = null;
                        double closestValue = 0;
                        double closestSiblingValue = 0;
                        foreach (Node myNode2 in nodeset)
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
                            if (myNode2.Depth == myNode.Depth && myNode2 != myNode)
                            {
                                double tempSiblingValue = Math.Sqrt(Math.Pow(myNode2.WorldPos.X - myNode.WorldPos.X, 2) + Math.Pow(myNode2.WorldPos.Y - myNode.WorldPos.Y, 2));
                                if (tempSiblingValue < 90)
                                {
                                    if (!hasSibling.Contains(myNode2))
                                    {
                                        if (closestSiblingNode == null)
                                        {
                                            closestSiblingNode = myNode2;
                                            closestSiblingValue = tempSiblingValue;
                                        } else if(tempSiblingValue < closestSiblingValue)
                                        {
                                            closestSiblingNode = myNode2;
                                            closestSiblingValue = tempSiblingValue;
                                        }
                                    }
                                }
                            }
                        }
                        AddEdge(myNode, closestNode);

                        if (closestSiblingNode != null && rngAmount.Next(1, 100) < 10)
                        {
                            hasSibling.Add(myNode);
                            hasSibling.Add(closestSiblingNode);
                            siblings.Add(new Node[] { myNode , closestSiblingNode });
                        }
                    }
                }
            }
            foreach (Node[] sibling in siblings)
            {
                AddEdge(sibling[0], sibling[1]);
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Begin();
            foreach (Vector2[] nodeConnected in nodesConnected)
            {
                GameWorld.DrawLine(_spriteBatch, nodeConnected[0], nodeConnected[1], Color.White, 3);
            }
            if (drawFastestPath)
            {
                foreach (Vector2[] pathLine in pathChosen)
                {
                    GameWorld.DrawLine(_spriteBatch, pathLine[1], pathLine[0], Color.Blue, 3);
                }
            }
            _spriteBatch.End();
            base.Draw(_spriteBatch);
        }

        public void GenerateNodes(int maxDepth, bool isBothWay)
        {
            myGameObjects = new List<GameObject>();
            nodeset = new List<Node>(); 
            nodesConnected = new List<Vector2[]>();
            drawFastestPath = false;
            goal = null;

            startNode = new Node(nodeSprite, startNodePos, 0.5f, 0);
            myGameObjects.Add(startNode);
            nodeset.Add(startNode);
            for (int i = 0; i < maxDepth; i++)
            {
                int randomAmountofNodes = rngAmount.Next((3 * i) + 1, 10 + i * 3);
                List<Node> myNodePositions = new List<Node>();
                for (int j = 0; j < randomAmountofNodes; j++)
                {
                    float randomAngle = 0;
                    bool notOverlapping = true;
                    bool parentTooFarAway = true;
                    int failCounter = 0;
                    bool foundPlacement = true;
                    Vector2 newNodePos = Vector2.Zero;
                    while (notOverlapping || parentTooFarAway)
                    {
                        notOverlapping = false;
                        parentTooFarAway = true;
                        //randomAngle = 0.3f + (float)rngAmount.NextDouble() * 2.55f;
                        randomAngle = (float)(rngAmount.NextDouble() * Math.PI * 2);
                        float myAngleX = (float)Math.Cos(randomAngle);
                        float myAngleY = (float)Math.Sin(randomAngle);
                        newNodePos = new Vector2(startNodePos.X + (80 * (i + 1) * myAngleX), startNodePos.Y + (80 * (i + 1) * myAngleY));
                        foreach (Node myNodePosition in myNodePositions)
                        {
                            if (Math.Sqrt(Math.Pow(newNodePos.X - myNodePosition.WorldPos.X, 2) + Math.Pow(newNodePos.Y - myNodePosition.WorldPos.Y, 2)) < 30.0f)
                            {
                                notOverlapping = true;
                            }
                        }
                        foreach (Node test in nodeset)
                        {
                            if (test.Depth == i)
                            {
                                if (Math.Sqrt(Math.Pow(newNodePos.X - test.WorldPos.X, 2) + Math.Pow(newNodePos.Y - test.WorldPos.Y, 2)) < 130.0f)
                                {
                                    parentTooFarAway = false;
                                }
                            }
                        }
                        if (notOverlapping || parentTooFarAway)
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
                        Node newNode = new Node(nodeSprite, newNodePos, rngAmount.Next(25, 35) * 0.01f, i + 1);
                        myNodePositions.Add(newNode);
                        myGameObjects.Add(newNode); //temp
                        nodeset.Add(newNode);
                    }
                }
            }
            ConnectNodes();
        }
        
        public static void ChangeGoal(Node myNode)
        {
            if (goal != null)
            {
                goal.ChangeColour(Color.White);
                foreach (Node newNode in nodeset)
                {
                    newNode.ResetColour();
                }
            }

            goal = myNode;
        }
    }

    public class SlowColours
    {
        public Node NodetoColour { get; set; }
        public Color Colour { get; set; }
        public SlowColours(Node myNode, Color myColour)
        {
            NodetoColour = myNode;
            Colour = myColour;
        }
    }
}
