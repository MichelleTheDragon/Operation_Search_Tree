using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    public class SearchTrees
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
    }
}
