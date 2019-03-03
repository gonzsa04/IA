/*
* A representation of an edge from a source vertex to a target vertex
* with a weight(cost)
* */

using System.Collections.Generic;

namespace UCM.IAV.IA.Util
{

    public class DirectedEdge
    {
        private readonly int _v;//The source vertex
        private readonly int _w;//The target vertex
        private readonly double _weight;//The weight to go from _v to _w

        //Create a directed edge from v to w with weight 'weight'
        public DirectedEdge(int v, int w, double weight)
        {
            this._v = v;
            this._w = w;
            this._weight = weight;
        }

        //Return the weight
        public double Weight()
        {
            return _weight;
        }

        //Return the source vertex
        public int From()
        {
            return _v;
        }

        //Return the target vertex
        public int To()
        {
            return _w;
        }

        //Return a string representation of the edge
        public override string ToString()
        {
            return string.Format("{0:d}->{1:d} {2:f}", _v, _w, _weight);
        }
    }

    /*
    * Implementation of an edge weighted directed graph
    * */
    public class EdgeWeightedDigraph
    {
        private readonly int _v; //The number of vertices
        private int _e;//The number of edges
        private LinkedList<DirectedEdge>[] _adj;//A linked list representation of the adjacency lists

        /*
        * Create an edge weighted directed graph with V vertices
        * */
        public EdgeWeightedDigraph(int V)
        {
            this._v = V;
            this._e = 0;
            /*
            * create v linked lists, one for each vertex, which keeps track
            * of the edge from v to other vertices v 
            * */
            _adj = new LinkedList<DirectedEdge>[V];
            for (int v = 0; v < _v; v++)
            {
                _adj[v] = new LinkedList<DirectedEdge>();
            }
        }

        //Return the number of vertices
        public int V()
        {
            return _v;
        }

        //Return the number of edges    
        public int E()
        {
            return _e;
        }

        /*
        * Add an edge at the start of the linked list 
        * and increase the edge count
        * */
        public void AddEdge(DirectedEdge e)
        {
            _adj[e.From()].AddFirst(e);
            _e++;
        }

        //Iterate through the vertices linked lists
        public IEnumerable<DirectedEdge> Adj(int v)
        {
            return _adj[v];
        }

        //Iterate through all edges
        public IEnumerable<DirectedEdge> Edges()
        {
            LinkedList<DirectedEdge> linkedlist = new LinkedList<DirectedEdge>();
            for (int v = 0; v < _v; v++)
            {
                foreach (DirectedEdge e in _adj[v])
                    linkedlist.AddFirst(e);
            }
            return linkedlist;
        }
    }
}