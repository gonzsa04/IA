namespace UCM.IAV.IA.Search {

    using System;
    using System.Collections.Generic;
    using UCM.IAV.IA;

    public class NodeCool : IComparable<NodeCool> { 
        // n.PARENT: the NodeCool in the search tree that generated this NodeCool;
        private NodeCool parent_ = null;
        private bool closed_ = false;
        double gCost_ = 0.0, fCost_ = 0.0;
        int position_ = -1;
        

        /*// n.OPERATOR: the operator that was applied to the parent to generate the NodeCool;
        private Operator op = null;*/

        public NodeCool() { }

        public NodeCool GetParent()
        {
            return parent_;
        }

        public void SetParent(NodeCool parent)
        {
            parent_ = parent;
        }

        public double GetGCost()
        {
            return gCost_;
        }

        public void SetGCost(double gCost)
        {
            gCost_ = gCost;
        }

        public double GetFCost()
        {
            return fCost_;
        }

        public void SetFCost(double fCost)
        {
            fCost_ = fCost;
        }

        public bool GetClosed()
        {
            return closed_;
        }

        public void SetClosed(bool closed)
        {
            closed_ = closed;
        }

        public int GetPos()
        {
            return position_;
        }

        public void SetPos(int position)
        {
            position_ = position;
        }

        public bool IsRootNodeCool()
        {
            return parent_ == null;
        }

        public List<int> GetPathFromRoot()
        {
            List<int> path = new List<int>();
            NodeCool current = this;
            while (!current.IsRootNodeCool())
            {
                path.Insert(0, current.GetPos());
                current = current.GetParent();
            }
            // ensure the root NodeCool is added
            path.Insert(0, current.GetPos());
            return path;
        }

        public override string ToString() {
            return "[parent=" + parent_ + ", gCost=" + gCost_ + ", fCost=" + fCost_ + "]";
        }

        // A LO MEJOR TIENE SENTIDO HACER EL COMPARADOR DE NODOS COMO ALGO EXTERNO AL NODO... EL NODO SI ACASO QUE TENGAN EL COMPARARSE CON 'OTRO' NODO
        // Para poder comparar nodos (no tiene mucho sentido.. lo normal será comparar ESTE nodo, con el otro que te pasan :-)
        // PERO EL INTERFAZ EN C# ES ASÍ, SE COMPARAN DOS OBJETOS Y PUNTO
        public int CompareTo(NodeCool y) { 
            // AQUÍ COMPARAMOS NODOS, BÁSICAMENTE EL COSTE
            if (GetFCost() > y.GetFCost())
                return 1;
            if (GetFCost() < y.GetFCost())
                return -1;
            return 0; 
        }
    }
}