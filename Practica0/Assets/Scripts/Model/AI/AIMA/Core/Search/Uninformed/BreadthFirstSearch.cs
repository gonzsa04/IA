
namespace AIMA.Core.Search.Uninformed
{
    using System.Collections.Generic;
    using AIMA.Core.Agent;
    using AIMA.Core.Search.Framework;
    using AIMA.Core.Util.DataStructure;

    public class BreadthFirstSearch : Search
    {
        private QueueSearch s;

        // constructora por defecto, nos basamos en GraphSearch
        public BreadthFirstSearch():this(new GraphSearch())
        {

        }

        // Constructor de busqueda primero en anchura, partiendo de una busqueda dada
        public BreadthFirstSearch(QueueSearch search)
        {
            // 
            search.setCheckGoalBeforeAddingToFrontier(true);
            this.s = search();
        }

        // el metodo mas importante de toda busqueda
        public List<Operator> Search(Problem p)
        {
            return this.s.search(p, new FIFOQueue<Node>());
        }

        public Metrics getMetrics()
        {
            return this.s.getMetrics();
        }
    }
}
