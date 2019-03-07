namespace UCM.IAV.IA.Search.Informed
{
    using System.Collections.Generic;
    using UCM.IAV.IA.Search;
    using UCM.IAV.IA;
    using UCM.IAV.IA.Util;
    using UCM.IAV.Puzzles; //QUITAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAR

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
        private List<double> distTo;          // lista de distancias desde cada vertice del grafo al origen consultado
        private int start, fin;                 // origen desde el cual se consulta el camino a seguir
        private int edgesExpanded = 0;

        public void Init(EdgeWeightedDigraph G, ref IndexedPriorityQueue<int> pq, int s = 0, int f = 0, TipoHeuristicas H = TipoHeuristicas.SINH)
        {
            // inicializacion de variables
            start = s;
            fin = f;
            edgeTo = new List<DirectedEdge>();
            distTo = new List<double>();
            DirectedEdge auxE = new DirectedEdge(-1, -1, 1e9);
            for (int v = 0; v < G.V(); v++)
            {
                distTo.Insert(v, (int)1e9);
                edgeTo.Insert(v, auxE);
            }

            // la distancia al origen sera 0, y el valor de su arista tambien
            distTo[start] = 0;
            edgeTo[start] = new DirectedEdge(start, start, 0);
            pq.insert(start, (int)Math.Ceiling(distTo[start])); // introducimos la primera arista en la indexpq

            // hasta que la indexpq no quede vacia de aristas vamos sacando la de menor coste
            // y actualizamos los caminos y distancias en caso de ser necesario
            while (pq.Count != 0)
            {
                int v = (int)pq.Pop();
                foreach (DirectedEdge e in G.Adj(v))
                {
                    DirectedEdge aux = e;
                    heuristic(H, ref aux);
                    relax(aux, ref pq);
                }
                    edgesExpanded++;
            }
        }

        private void relax(DirectedEdge e, ref IndexedPriorityQueue<int> pq)
        {
            int v = e.From(), w = e.To();

            double sinh = distTo[v] + e.Weight();
            double conh = distTo[v] + e.Weight() + e.Heuristic();
            double W = distTo[w];
            if (distTo[w] > distTo[v] + e.Weight() + e.Heuristic())
            {
                distTo[w] = distTo[v] + e.Weight() + e.Heuristic();
                edgeTo[w] = e;
                pq.Set(w, (int)Math.Ceiling(distTo[w]));
            }
        }

        // devuelve una lista con las casillas del tablero a seguir para llegar a "end"
        public List<int> GetPath()
        {
            List<int> path = new List<int>();
            path.Insert(path.Count, fin);
            while (fin >= 0 && edgeTo[fin].From() != start)
            {
                fin = edgeTo[fin].From();
                path.Insert(path.Count, fin);
            }
            return path;
        }

        public int GetEdgesEX() {
            return edgesExpanded;
        }


        private void heuristic(TipoHeuristicas t, ref DirectedEdge e)
        {
            switch (t)
            {
                case TipoHeuristicas.H1:
                    heuristic1(ref e);
                    break;
                case TipoHeuristicas.H2:
                    heuristic2(ref e);
                    break;
                case TipoHeuristicas.H3:
                    heuristic3(ref e);
                    break;
            }
        }

        private void heuristic1(ref DirectedEdge e)
        {
        }

        private void heuristic2(ref DirectedEdge e)
        {
        }

        private void heuristic3(ref DirectedEdge e)
        {
            int inix = (int)(e.From() / GameManager.instance.columns);
            int iniy = (int)(e.From() - inix * GameManager.instance.columns);
            int destx = (int)(fin / GameManager.instance.columns);
            int desty = (int)(fin - destx * GameManager.instance.columns);

            double catA = destx - inix;
            double catB = desty - iniy;

            double h = Math.Sqrt(catA * catA + catB * catB);

            e = new DirectedEdge(e.From(), e.To(), e.Weight(), h);
        }
    }
}
