namespace UCM.IAV.IA.Search.Informed
{
    using System.Collections.Generic;
    using UCM.IAV.IA.Search;
    using UCM.IAV.IA;
    using UCM.IAV.IA.Util;

    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using System.Numerics;
    
    using UnityEngine;

    // usa un grafo dirigido valorado construido a partir de la matriz logica del juego y una indexpq para ir introduciendo
    // las aristas ordenadas por prioridad (menor coste)
    class Astar
    {
        private List<DirectedEdge> edgeTo; // lista de aristas a seguir para llegar desde un vertice del grafo al origen consultado
        private List<int> distTo;          // lista de distancias desde cada vertice del grafo al origen consultado
        private int start;                 // origen desde el cual se consulta el camino a seguir

        public void Init(EdgeWeightedDigraph G, ref IndexedPriorityQueue<int> pq, int s = 0)
        {
            // inicializacion de variables
            start = s;
            edgeTo = new List<DirectedEdge>();
            distTo = new List<int>();
            DirectedEdge auxE = new DirectedEdge(-1, -1, 1e9);
            for (int v = 0; v < G.V(); v++)
            {
                distTo.Insert(v, (int)1e9);
                edgeTo.Insert(v, auxE);
            }

            // la distancia al origen sera 0, y el valor de su arista tambien
            distTo[start] = 0;
            edgeTo[start] = new DirectedEdge(start, start, 0);
            pq.insert(start, distTo[start]); // introducimos la primera arista en la indexpq

            // hasta que la indexpq no quede vacia de aristas vamos sacando la de menor coste
            // y actualizamos los caminos y distancias en caso de ser necesario
            while (pq.Count != 0)
            {
                int v = (int)pq.Pop();
                foreach (DirectedEdge e in G.Adj(v))
                {
                    relax(e, ref pq);
                }
            }
        }

        private void relax(DirectedEdge e, ref IndexedPriorityQueue<int> pq)
        {
            int v = e.From(), w = e.To();
            if (distTo[w] > distTo[v] + e.Weight())
            {
                distTo[w] = distTo[v] + (int)e.Weight();
                edgeTo[w] = e;
                pq.Set(w, distTo[w]);
            }
        }

        // devuelve una lista con las casillas del tablero a seguir para llegar a "end"
        public List<int> GetPathTo(int end)
        {
            List<int> path = new List<int>();
            path.Insert(path.Count, end);
            while (edgeTo[end].From() != start)
            {
                end = edgeTo[end].From();
                path.Insert(path.Count, end);
                if (end < 0) break;
            }
            return path;
        }

        // devuelve la distancia desde el origen a "end"
        public double GetDistTo(int end)
        {
            return distTo[end];
        }
    }
}
