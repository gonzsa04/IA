
namespace AIMA.Core.Search.Framework
{
    using System.Collections.Generic;
    using AIMA.Core.Agent;

    public class Node{
        private object config; //"auto" 

        private Node parent;

        private Operator op; // operador desde el cual el padre ha accedido al hijo (nodo actual)

        private double pathCost; //coste desde la raiz hasta este nodo

        //Constructor de un nodo raiz
        public Node(object config)
        {
            this.config = config;
            this.pathCost = 0.0d;
        }

        //Constructor de un nodo hijo
        public Node(object config, Node parent, Operator op, double stepCost) : this(config)
        {
            this.parent = parent;
            this.op = op;
            this.pathCost = parent.pathCost + stepCost;
        }

        public object getState() { return config; }
        public Node getParent() { return parent; }
        public Operator getAction() { return op; }
        public double getPathCost() { return pathCost; }

        public bool isRootNode() { return parent == null; }

        public List<Node> getPathFromRoot()
        {
            List<Node> path = new List<Node>();
            Node current = this;
            while (!current.isRootNode())
            {
                path.Insert(0, current);
                current = current.getParent();

            }
            path.Insert(0, current);
            return path;
        }

        public override string ToString() //muy util para debuggear
        {
            return "[parent =" + parent + ", operator= " + op + ",config= " + config + ",pathCost= " + pathCost + "]";
        }
    }
}
