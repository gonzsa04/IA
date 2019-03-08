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
        private List<NodeCool> edgeTo; // lista de nodos a seguir para llegar desde un vertice del grafo al origen consultado
        private List<int> path;
        private int s_, f_;                 // origen desde el cual se consulta el camino a seguir
        private int edgesExpanded = 0;

        public void Init(EdgeWeightedDigraph G, ref List<NodeCool> pq, int s = 0, int f = 0, TipoHeuristicas H = TipoHeuristicas.SINH)
        {
            // inicializacion de variables
            s_ = s;
            f_ = f;

            edgeTo = new List<NodeCool>();

            for (int i = 0; i < G.V(); i++)
            {
                NodeCool aux = new NodeCool();
                aux.SetPos(i);
                edgeTo.Add(aux);
            }

            edgeTo[s_] = new NodeCool();
            edgeTo[s_].SetPos(s_);
            edgeTo[s_].SetGCost(0.0);
            edgeTo[s_].SetFCost(edgeTo[s_].GetGCost() + heuristic(H, edgeTo[s_]));

            pq.Add(edgeTo[s_]); // introducimos la primera arista en la indexpq

            // hasta que la indexpq no quede vacia de aristas vamos sacando la de menor coste
            // y actualizamos los caminos y distancias en caso de ser necesario
            while (pq.Count != 0)
            {
                int current = pq.First().GetPos();

                if (current == f_)
                {
                    path = edgeTo[current].GetPathFromRoot();
                    break;
                }

                pq.Remove(edgeTo[current]);

                edgeTo[current].SetClosed(true);

                foreach (DirectedEdge e in G.Adj(edgeTo[current].GetPos()))
                {
                    int neighbour = e.To();

                    if (!edgeTo[neighbour].GetClosed())
                    {
                        double gcost = edgeTo[current].GetGCost() + e.Weight();

                        if (!pq.Contains(edgeTo[neighbour]))
                        {
                            edgeTo[neighbour].SetParent(edgeTo[current]);
                            edgeTo[neighbour].SetGCost(gcost);
                            edgeTo[neighbour].SetFCost(gcost + heuristic(H, edgeTo[neighbour]));
                            pq.Add(edgeTo[neighbour]);
                        }
                        else if (gcost < edgeTo[neighbour].GetGCost())
                        {
                            pq.Remove(edgeTo[neighbour]);
                            edgeTo[neighbour].SetParent(edgeTo[current]);
                            edgeTo[neighbour].SetGCost(gcost);
                            edgeTo[neighbour].SetFCost(gcost + heuristic(H, edgeTo[neighbour]));
                            pq.Add(edgeTo[neighbour]);
                        }
                        pq.Sort();
                    }
                }
            }
        }

        /*private void relax(DirectedEdge e, ref IndexedPriorityQueue<double> pq)
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
        }*/

        // devuelve una lista con las casillas del tablero a seguir para llegar a "end"
        public List<int> GetPath()
        {
            return path;
        }

        public int GetEdgesEX() {
            return edgesExpanded;
        }


        private double heuristic(TipoHeuristicas t, NodeCool e)
        {
            switch (t)
            {
                case TipoHeuristicas.H1:
                    return heuristic1(e);

                case TipoHeuristicas.H2:
                    return heuristic2(e);

                case TipoHeuristicas.H3:
                    return heuristic3(e);

                default:
                    return 0.0;
            }
        }

        private double heuristic1(NodeCool e)
        {
            return 0.0;
        }

        private double heuristic2(NodeCool e)
        {
            return 0.0;
        }

        private double heuristic3(NodeCool e)
        {
            int inix = (int)(e.GetPos() / GameManager.instance.columns);
            int iniy = (int)(e.GetPos() - inix * GameManager.instance.columns);
            int destx = (int)(f_ / GameManager.instance.columns);
            int desty = (int)(f_ - destx * GameManager.instance.columns);

            double catA = destx - inix;
            double catB = desty - iniy;

             return Math.Sqrt(catA * catA + catB * catB);
        }
    }
}
