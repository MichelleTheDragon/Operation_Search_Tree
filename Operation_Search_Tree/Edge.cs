﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Operation_Search_Tree
{
    class Edge
    {
        private Node from;
        public Node From { get { return from; } }
        private Node to;
        public Node To { get { return to; } }

        public Edge(Node from, Node to)
        {
            this.from = from;
            this.to = to;
        }


    }
}
