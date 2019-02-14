namespace AIMA.Core.Search.Framework
{
    using System.Collections.Generic;
    using AIMA.Core.Agent;

    /**
     * @author Ravi Mohan
     * 
     */
    public interface Search
    {
        List<Operator> Search(Problem p);

        Metrics getMetrics();
    }
}