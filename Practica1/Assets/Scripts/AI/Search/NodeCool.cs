namespace UCM.IAV.IA.Search {

    using System;
    using System.Collections.Generic;
    using UCM.IAV.IA;

    // clase nodo nuestra
    public class NodeCool : IComparable<NodeCool> { 
        private NodeCool parent_ = null;   // padre
        private bool closed_ = false;      // visitado
        double gCost_ = 0.0, fCost_ = 0.0; // coste fisico / coste final
        int position_ = -1;                // posicion dentro de la matriz logica

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

        // devuelve la lista de posiciones de la matriz logica que habra que
        // seguir para llegar desde este nodo hasta la raiz
        public List<int> GetPathFromRoot()
        {
            List<int> path = new List<int>();
            NodeCool current = this;
            while (!current.IsRootNodeCool())
            {
                path.Insert(0, current.GetPos());
                current = current.GetParent();
            }
            
            path.Insert(0, current.GetPos());
            return path;
        }

        public override string ToString() {
            return "[parent=" + parent_ + ", gCost=" + gCost_ + ", fCost=" + fCost_ + "]";
        }

        public int CompareTo(NodeCool y) { 
            if (GetFCost() > y.GetFCost())
                return 1;
            if (GetFCost() < y.GetFCost())
                return -1;
            return 0; 
        }
    }
}