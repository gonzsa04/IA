namespace AIMA.Core.Search.Framework
{ 
    using System.Collections.Generic;
using AIMA.Core.Agent;
using AIMA.Core.Util.DataStructure;

/**
 * @author Ravi Mohan
 * 
 */
public abstract class PrioritySearch : Search {
	protected QueueSearch search;

	public List<Operator> Search(Problem p) {
		return search.search(p, new PriorityQueue<Node>(5, getComparator()));
	}

	public Metrics getMetrics() {
		return search.getMetrics();
	}

        //
        // PROTECTED METHODS
        // COMPARATOR lo he cambiado por Comparer
        protected abstract Comparer<Node> getComparator();
}
}