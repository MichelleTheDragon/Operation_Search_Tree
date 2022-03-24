using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    public class SearchTrees : GameObject
    {
        public List<Node> BreadthFirstSearch(List<Node> myNodes, Node goal, List<SlowColours> visualPath)
        {
            if (myNodes.Count == 0)
            {
                return null;
            }
            List<PathInfo> someNodes = new List<PathInfo>();
            List<PathInfo> someNodes2 = new List<PathInfo>();
            List<Node> alreadyChecked = new List<Node>();

            someNodes.Add(new PathInfo(myNodes[0], new List<Node>()));
            while (someNodes.Count > 0)
            {
                for (int i = 0; i < someNodes.Count; i++)
                {
                    visualPath.Add(new SlowColours(someNodes[i].MyNode, Color.Red));
                    foreach (Edge neighbor in someNodes[i].MyNode.Edges)
                    {
                        if (!alreadyChecked.Contains(neighbor.To))
                        {
                            PathInfo newPath = new PathInfo(neighbor.To, new List<Node>(someNodes[i].MyPath));
                            someNodes2.Add(newPath);
                            visualPath.Add(new SlowColours(neighbor.To, Color.Yellow));
                            if (neighbor.To == goal)
                            {
                                visualPath.Add(new SlowColours(neighbor.To, Color.Blue));
                                return newPath.MyPath;
                            }
                        }
                    }
                    alreadyChecked.Add(someNodes[i].MyNode);
                }
                someNodes = new List<PathInfo>(someNodes2);
                someNodes2.Clear();
            }
            return null;
        }

        //public List<Node> DepthFirstSearch(List<Node> myNodes, Node goal, List<SlowColours> visualPath)
        //{
        //    if (myNodes.Count == 0)
        //    {
        //        return null;
        //    }
        //    List<PathInfo> someNodes = new List<PathInfo>();
        //    List<PathInfo> someNodes2 = new List<PathInfo>();
        //    List<Node> alreadyChecked = new List<Node>();

        //    someNodes.Add(new PathInfo(myNodes[0], new List<Node>()));
        //    while (someNodes.Count > 0)
        //    {
                
        //        someNodes[someNodes.Count - 1]
        //    }

        //    return null;
        //}

        /// <summary>
        ///     Input a list of connected Nodes, and recieve a found path to the goal node from the start node.
        /// </summary>
        /// <param name="myList">All nodes</param>
        /// <param name="myNode">Start Node</param>
        /// <param name="goal">Goal Node</param>
        /// <param name="visualPath">List of visual process</param>
        /// <returns>The path to the goal</returns>
        public List<Node> DepthFirstSearch(List<Node> myList, Node myNode, Node goal, List<SlowColours> visualPath)
        {
            if (myList.Count == 0) //In case of the list being empty
            {
                return null;
            }
            List<Node> visited = new List<Node>(); 
            List<PathInfo> nodeStack = new List<PathInfo>();

            nodeStack.Add(new PathInfo(myNode, new List<Node>())); //add the start Node
            while (nodeStack.Count > 0)
            {
                PathInfo newNode = nodeStack[nodeStack.Count - 1]; //set last Node in the list to the current node to check
                visited.Add(newNode.MyNode); //Mark as checked
                visualPath.Add(new SlowColours(newNode.MyNode, Color.Red)); //Mark (visually) as checked

                if (newNode.MyNode == goal) //check if the current node is the goal node
                {
                    visualPath.Add(new SlowColours(newNode.MyNode, Color.Blue)); //Mark it (visually) as goal 
                    return newNode.MyPath; //return the path it took to get there
                }

                foreach (Edge neighbor in newNode.MyNode.Edges) //look at all neighbors
                {
                    if (!visited.Contains(neighbor.To)) //if it hasn't been checked before
                    {
                        visualPath.Add(new SlowColours(neighbor.To, Color.Yellow)); //Mark it (visually) to be checked later
                        nodeStack.Add(new PathInfo(neighbor.To, new List<Node>(newNode.MyPath))); //Add the Node to the list to check
                    }
                }
                nodeStack.Remove(newNode); //remove the current Node from the list 
            }
            return myList; //in case of emergence spit out random stuff. should currently be unreachable
        }
    }
}
