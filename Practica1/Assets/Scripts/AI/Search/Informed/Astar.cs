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
        private List<int> distTo;
        private int start;

        public void Init(EdgeWeightedDigraph G, ref IndexedPriorityQueue<int> pq, int s = 0)
        {
            start = s;
            edgeTo = new List<DirectedEdge>();
            distTo = new List<int>();
            DirectedEdge auxE = new DirectedEdge(-1, -1, 1e9);

            for (int v = 0; v < G.V(); v++)
            {
                distTo.Insert(v, (int)1e9);
                edgeTo.Insert(v, auxE);
            }

            distTo[start] = 0;
            edgeTo[start] = new DirectedEdge(start, start, 0);
            pq.insert(start, distTo[start]);
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

        public double GetDistTo(int end)
        {
            return distTo[end];
        }
    }
}
