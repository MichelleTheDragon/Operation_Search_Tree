﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    public class PathInfo
    {
        private List<Node> myPath = new List<Node>();
        public List<Node> MyPath { get { return myPath; } }
        private Node myNode;
        public Node MyNode { get { return myNode; } }

        public PathInfo(Node myNode, List<Node> myPath)
        {
            this.myPath = myPath;
            this.myNode = myNode;
            myPath.Add(myNode);
        }
    }
}