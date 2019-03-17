/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search.Uninformed {

    using System.Collections.Generic;
    using UCM.IAV.IA.Search;
    using UCM.IAV.IA;
    using UCM.IAV.IA.Util;

    /**
     * Realiza la búsqueda primero en anchura.
     * 
     * <code>
     * function BREADTH-FIRST-SEARCH(problem) returns a solution, or failure
     *   node <- a node with SETUP = problem.INITIAL-SETUP, PATH-COST=0
     *   if problem.GOAL-TEST(node.SETUP) then return SOLUTION(node)
     *   frontier <- a FIFO queue with node as the only element
     *   explored <- an empty set
     *   loop do
     *      if EMPTY?(frontier) then return failure
     *      node <- POP(frontier) // chooses the shallowest node in frontier
     *      add node.SETUP to explored
     *      for each operator in problem.OPERATORS(node.SETUP) do
     *          child <- CHILD-NODE(problem, node, operator)
     *          if child.SETUP is not in explored or frontier then
     *              if problem.GOAL-TEST(child.SETUP) then return SOLUTION(child)
     *              frontier <- INSERT(child, frontier)
     * </code> 
     * 
     * Soporta tamto la versión para grafos como para árboles simplemente asignándole un ejemplar de TreeSearch o GraphSearch en el constructor
     */
    public class BreadthFirstSearch : Search
    {

        private QueueSearch search;

        public BreadthFirstSearch() : this(new GraphSearch())
        {
      
        }

        public BreadthFirstSearch(QueueSearch search)
        {
            // Goal test is to be applied to each node when it is generated
            // rather than when it is selected for expansion.
            search.SetCheckGoalBeforeAddingToFrontier(true);
            this.search = search;
        }

        public List<Operator> Search(Problem p)
        {
            return this.search.Search(p, new FIFOQueue<Node>());  
        }

        public Metrics GetMetrics()
        {
            return this.search.GetMetrics();
        }
    }
}