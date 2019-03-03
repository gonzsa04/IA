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

    class Astar
    {
        private List<DirectedEdge> edgeTo;
        private List<double> distTo;
        private int start;

        public void Init(EdgeWeightedDigraph G, ref IndexedPriorityQueue<double> pq, int s = 0)
        {
            start = s;
            edgeTo = new List<DirectedEdge>();
            distTo = new List<double>();
            DirectedEdge auxE = new DirectedEdge(0, 0, 0);

            for (int v = 0; v < G.V(); v++)
            {
                distTo.Insert(v, 1e9);
                edgeTo.Insert(v, auxE);
            }
            Debug.Log(start);
            distTo[start] = 0;
            pq.insert(start, distTo[start]);
            while (pq.Count != 0)
            {
                int v = (int)pq.Pop();
                Debug.Log(v);
                foreach (DirectedEdge e in G.Adj(v))
                {
                    relax(e, ref pq);
                }
            }
        }

        private void relax(DirectedEdge e, ref IndexedPriorityQueue<double> pq)
        {
            int v = e.From(), w = e.To();
            if (distTo[w] > distTo[v] + e.Weight())
            {
                distTo[w] = distTo[v] + e.Weight();
                edgeTo.Insert(w, e);
                pq.Set(w, distTo[w]);
            }
        }

        public List<int> GetPathTo(int end)
        {
            List<int> path = new List<int>();
            path.Insert(path.Count, end);
            while (edgeTo[end].From() != start)
            {
                end = edgeTo[end].From();
                path.Insert(path.Count, end);
            }
            return path;
        }

        public double GetDistTo(int end)
        {
            return distTo[end];
        }
    }
}
