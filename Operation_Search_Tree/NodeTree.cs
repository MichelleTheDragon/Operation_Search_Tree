using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    public class NodeTree : Scene
    {
        private Texture2D nodeSprite;
        private Node startNode;
        private Random rngAmount = new Random();
        private Vector2 startNodePos;
        private bool keyDownR;
        private bool keyDownSpace;
        private bool keyDownRight;
        private bool keyDownLeft;
        private static List<Node> nodeset = new List<Node>();
        public List<Node> Nodeset { get { return nodeset; } }
        private List<Node[]> nodesConnected = new List<Node[]>();
        private List<Node[]> pathChosen = new List<Node[]>();
        public List<SlowColours> VisualPath = new List<SlowColours>();

        public static Node goal;
        private SearchTrees mySearchTree;
        private bool visualizeColours;
        private int colourShown = -1;
        private float colourTimer = 0.0f;
        private static bool drawFastestPath;
        private int searchMethod = 0;
        public static bool IsRunning { get; protected set; }
        public float Zoom { get; set; } = 1.0f;
        private float currentMouseWheelValue, previousMouseWheelValue, zoom, previousZoom;

        public static float DistanceBetweenDepth { get; protected set; } = 80.0f;
        private bool resetZoom;
        private bool drawNodesOkay;
        public static bool goalFound { get; set; }
        private bool autoRun = false;
        private bool stepByStep = false;
        private bool randomSearchTree = true;
        private bool buttonHeldLeft;
        private bool speedLeft;
        private float speedLeftTimer;
        private bool buttonHeldRight;
        private bool speedRight;
        private float speedRightTimer;
        private float pathShowTimer;
        public int nodeDepth { get; set; } = 5;

        public NodeTree(ContentManager Content, Vector2 startNodePos)
        {
            this.startNodePos = startNodePos;
            nodeSprite = Content.Load<Texture2D>("Sprites/Node");
            GenerateNodes(15, true);
            mySearchTree = new SearchTrees();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateZoom();
            KeyboardState state = Keyboard.GetState();

            if (visualizeColours)
            {
                if (stepByStep)
                {
                    if (((state.IsKeyDown(Keys.Right) && !keyDownRight) || speedRight) && colourShown < VisualPath.Count - 1)
                    {
                        colourShown++;
                        VisualPath[colourShown].NodetoColour.ChangeColour(VisualPath[colourShown].Colour);
                        keyDownRight = true;
                        speedRight = false;
                        buttonHeldRight = true;
                    }
                    else if (state.IsKeyUp(Keys.Right) && keyDownRight)
                    {
                        keyDownRight = false;
                        buttonHeldRight = false;
                        speedRight = false;
                        speedRightTimer = 0.0f;
                    }


                    if (((state.IsKeyDown(Keys.Left) && !keyDownLeft) || speedLeft) && colourShown > 1)
                    {
                        colourShown--;
                        if (colourShown + 1 < VisualPath.Count)
                        {
                            if (VisualPath[colourShown].NodetoColour != VisualPath[colourShown + 1].NodetoColour)
                            {
                                VisualPath[colourShown].NodetoColour.ChangeColour(VisualPath[colourShown].Colour);
                                VisualPath[colourShown + 1].NodetoColour.ResetColour();
                            } else
                            {
                                VisualPath[colourShown].NodetoColour.ChangeColour(VisualPath[colourShown].Colour);
                            }
                        }
                        else
                        {
                            VisualPath[colourShown].NodetoColour.ChangeColour(VisualPath[colourShown].Colour);
                        }

                        keyDownLeft = true;
                        speedLeft = false;
                        buttonHeldLeft = true;
                    }
                    else if (state.IsKeyUp(Keys.Left) && keyDownLeft)
                    {
                        keyDownLeft = false;
                        buttonHeldLeft = false;
                        speedLeft = false;
                        speedLeftTimer = 0.0f;
                    }

                    if (buttonHeldLeft)
                    {
                        if (speedLeftTimer > 0.5f)
                        {
                            speedLeft = true;
                            speedLeftTimer = 0.45f;
                        }
                        speedLeftTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (buttonHeldRight)
                    {
                        if (speedRightTimer > 0.5f)
                        {
                            speedRight = true;
                            speedRightTimer = 0.45f;
                        }
                        speedRightTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
                else
                {
                    if (colourTimer > 0.01f)
                    {
                        colourShown++;
                        VisualPath[colourShown].NodetoColour.ChangeColour(VisualPath[colourShown].Colour);
                        colourTimer = 0.0f;
                    }
                    else
                    {
                        colourTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }

                if (colourShown == VisualPath.Count - 1)
                {
                    drawFastestPath = true;
                    IsRunning = false;
                    visualizeColours = false;
                }
            }
            if (drawFastestPath)
            {
                pathShowTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (state.IsKeyDown(Keys.R) && !keyDownR)
            {
                GenerateNodes(nodeDepth, true);
                keyDownR = true;
            } else if (state.IsKeyUp(Keys.R) && keyDownR)
            {
                keyDownR = false;
            }

            if (state.IsKeyDown(Keys.Space) && !keyDownSpace && goal != null && !IsRunning)
            {
                RunSearch();
                keyDownSpace = true;
            }
            else if (state.IsKeyUp(Keys.Space) && keyDownSpace)
            {
                keyDownSpace = false;
            }
        }

        public void RunSearch()
        {
            CleanNodes(Color.Blue);
            VisualPath = new List<SlowColours>();
            List<Node> newPath = new List<Node>();
            switch (searchMethod)
            {
                case 0:
                    newPath = mySearchTree.BreadthFirstSearch(Nodeset, goal, VisualPath);
                    break;
                case 1:
                    newPath = mySearchTree.DepthFirstSearch(Nodeset, Nodeset[0], goal, VisualPath);
                    break;
                default:
                    newPath = mySearchTree.BreadthFirstSearch(Nodeset, goal, VisualPath);
                    break;
            }
            pathChosen = new List<Node[]>();
            for (int i = 1; i < newPath.Count; i++)
            {
                pathChosen.Add(new Node[] { newPath[i - 1], newPath[i] });
                VisualPath.Add(new SlowColours(newPath[i - 1], Color.Blue));
            }
            visualizeColours = true;
            colourShown = -1;
            IsRunning = true;
        }

        public void AddEdge(Node from, Node to)
        {
            from.AddEdge(to);
            to.AddEdge(from);
            nodesConnected.Add(new Node[] { from, to });
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
            _spriteBatch.Begin();// SpriteSortMode.Immediate, null, null, null, null, null, GameWorld.MyCamera.Transform);
            if (drawNodesOkay)
            {
                foreach (Node[] nodeConnected in nodesConnected)
                {
                    GameWorld.DrawLine(_spriteBatch, nodeConnected[0].WorldPos, nodeConnected[1].WorldPos, Color.White, 2);
                }
            }
            if (drawFastestPath)
            {
                foreach (Node[] pathLine in pathChosen)
                {
                    GameWorld.DrawLine(_spriteBatch, pathLine[1].WorldPos, pathLine[0].WorldPos, Color.Blue, 2);
                }
                if (autoRun && pathShowTimer >= 3.0f)
                {
                    pathShowTimer = 0.0f;
                    if (randomSearchTree)
                    {
                        searchMethod = rngAmount.Next(0, 2);
                    }
                    GenerateNodes(rngAmount.Next(5, 16), true);
                    ChangeGoal(nodeset[rngAmount.Next(1, nodeset.Count)]);
                    RunSearch();
                }
            }
            _spriteBatch.End(); 
            if (drawNodesOkay)
            {
                base.Draw(_spriteBatch); //, GameWorld.MyCamera.Transform);
            }
        }

        public void GenerateNodes(int maxDepth, bool isBothWay)
        {
            MyGameObjects = new List<GameObject>();
            nodeset = new List<Node>(); 
            nodesConnected = new List<Node[]>();
            drawFastestPath = false;
            goal = null;
            Zoom = 5.0f / (float)maxDepth;
            resetZoom = true;
            visualizeColours = false;
            drawNodesOkay = false;
            IsRunning = false;
            goalFound = false;

            startNode = new Node(nodeSprite, startNodePos, 0.5f, 0, startNodePos, 0);
            MyGameObjects.Add(startNode);
            nodeset.Add(startNode);
            for (int i = 0; i < maxDepth; i++)
            {
                int randomAmountofNodes = rngAmount.Next((3 * i) + 3, 10 + i * 3);
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
                        newNodePos = new Vector2(startNodePos.X + (DistanceBetweenDepth * (i + 1) * myAngleX), startNodePos.Y + (DistanceBetweenDepth * (i + 1) * myAngleY));
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
                        Node newNode = new Node(nodeSprite, newNodePos, rngAmount.Next(25, 35) * 0.01f, i + 1, startNode.WorldPos, randomAngle);
                        myNodePositions.Add(newNode);
                        MyGameObjects.Add(newNode); //temp
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
                CleanNodes(Color.White);
            }

            goal = myNode;
        }

        public static void CleanNodes(Color color)
        {
            foreach (Node newNode in nodeset)
            {
                newNode.ResetColour();
            }
            if (goal != null)
            {
                goal.ChangeColour(color);
            }
            drawFastestPath = false;
        }

        public void UpdateZoom()
        {
            previousMouseWheelValue = currentMouseWheelValue;
            currentMouseWheelValue = Mouse.GetState().ScrollWheelValue;

            if (currentMouseWheelValue > previousMouseWheelValue)
            {
                AdjustZoom(.05f);
            }

            if (currentMouseWheelValue < previousMouseWheelValue)
            {
                AdjustZoom(-.05f);
            }

            previousZoom = zoom;
            zoom = Zoom;
            if (previousZoom != zoom || resetZoom)
            {
                foreach (Node myNode in nodeset)
                {
                    myNode.ChangeDistanceToCenter(zoom);
                }
                if (resetZoom)
                {
                    drawNodesOkay = true;
                    resetZoom = false;
                }
            }
        }
        public void AdjustZoom(float zoomAmount)
        {
            Zoom += zoomAmount;
            if (Zoom < .20f)
            {
                Zoom = .20f;
            }
            if (Zoom > 2f)
            {
                Zoom = 2f;
            }
        }

        public int GenerateNew()
        {
            autoRun = false;
            GenerateNodes(nodeDepth, true);
            return 1;
        }

        public int DepthLower()
        {
            if (nodeDepth > 3)
            {
                nodeDepth--;
            }
            return 1;
        }
        public int DepthHigher()
        {
            if (nodeDepth < 30)
            {
                nodeDepth++;
            }
            return 1;
        }

        public int SetDFS()
        {
            searchMethod = 1;
            return 1;
        }

        public int SetBFS()
        {
            searchMethod = 0;
            return 1;
        }


        public int RunSearchButton()
        {
            if (goal != null && !IsRunning)
            {
                RunSearch();
            }
            return 1;
        }

        public int SetStepByStep()
        {
            stepByStep = !stepByStep;
            if (stepByStep)
            {
                return 1;
            }
            return 0;
        }

        public int StepDown()
        {
            speedLeft = true;
            return 1;
        }

        public int StepUp()
        {
            speedRight = true;
            return 1;
        }

        public int AutoRun()
        {
            GenerateNodes(rngAmount.Next(5, 16), true);
            ChangeGoal(nodeset[rngAmount.Next(1, nodeset.Count)]);
            RunSearch();
            autoRun = true;
            return 1;
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
