namespace UCM.IAV.IA.Search.Informed
{
    using System.Collections.Generic;
    using UCM.IAV.IA.Search;
    using UCM.IAV.IA;
    using UCM.IAV.IA.Util;
    using UCM.IAV.Puzzles;

    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using System.Numerics;

    using UnityEngine;

    // usa un grafo dirigido valorado construido a partir de la matriz logica del juego y una lista para ir introduciendo
    // los nodos ordenados por prioridad (menor coste final)
    class Astar
    {
        private List<NodeCool> nodeGraph;   // los vertices del grafo dirigido valorado representados como nodos
        private List<int> path;             // almacena el camino a seguir desde el origen para llegar hasta el destino
        private int s_, f_;                 // origen desde el cual se consulta el camino a seguir y destino
        private int edgesExpanded = 0;      // numero de caminos expandidos
        private HeuristicFunctions heuristic = new HeuristicFunctions(); // elige las heuristicas

        public void Init(EdgeWeightedDigraph G, ref List<NodeCool> list, int s = 0, int f = 0, TipoHeuristicas H = TipoHeuristicas.SINH)
        {
            // inicializacion de variables
            s_ = s;
            f_ = f;
            nodeGraph = new List<NodeCool>();

            for (int i = 0; i < G.V(); i++)
            {
                NodeCool aux = new NodeCool();
                aux.SetPos(i);
                nodeGraph.Add(aux);
            }

            // nodo origen
            nodeGraph[s_].SetFCost(nodeGraph[s_].GetGCost() + heuristic.chooseHeuristic(H, nodeGraph[s_], f_));

            list.Add(nodeGraph[s_]); // introducimos el primer nodo en la lista

            // hasta que la lista no quede vacia de nodos vamos sacando el de menor coste
            // y actualizamos los caminos y distancias en caso de ser necesario
            while (list.Count != 0)
            {
                int current = list.First().GetPos();

                // si el nodo actual es el destino, devolvemos el camino y paramos
                if (current == f_)
                {
                    path = nodeGraph[current].GetPathFromRoot();
                    break;
                }

                list.Remove(nodeGraph[current]);
                nodeGraph[current].SetClosed(true); // marcamos como leido

                // para cada adyacente de la posicion del nodo actual en el grafo
                foreach (DirectedEdge e in G.Adj(nodeGraph[current].GetPos()))
                {
                    int neighbour = e.To();

                    // si no ha sido visitado ya
                    if (!nodeGraph[neighbour].GetClosed())
                    {
                        double gcost = nodeGraph[current].GetGCost() + e.Weight();

                        // si la lista no lo contiene ya, le actualizamos los costes, el padre y lo añadimos
                        if (!list.Contains(nodeGraph[neighbour]))
                        {
                            nodeGraph[neighbour].SetParent(nodeGraph[current]);
                            nodeGraph[neighbour].SetGCost(gcost);
                            nodeGraph[neighbour].SetFCost(gcost + heuristic.chooseHeuristic(H, nodeGraph[neighbour], f_));
                            list.Add(nodeGraph[neighbour]);
                            edgesExpanded++;
                        }
                        // si no, le modificamos los costes y el padre
                        else if (gcost < nodeGraph[neighbour].GetGCost())
                        {
                            list.Remove(nodeGraph[neighbour]);
                            nodeGraph[neighbour].SetParent(nodeGraph[current]);
                            nodeGraph[neighbour].SetGCost(gcost);
                            nodeGraph[neighbour].SetFCost(gcost + heuristic.chooseHeuristic(H, nodeGraph[neighbour], f_));
                            list.Add(nodeGraph[neighbour]);
                        }

                        list.Sort(); // ordenamos la lista de nodos segun su coste final (coste fisico + heuristica)
                    }
                }
            }
        }

        // devuelve una lista con las posiciones del tablero a seguir para llegar a "end"
        public List<int> GetPath()
        {
            return path;
        }

        // devuelve el numero de caminos expandidos
        public int GetEdgesEX()
        {
            return edgesExpanded;
        }
    }
}
