/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search {
    
    using System.Collections.Generic; 
    using UCM.IAV.IA;
    using UCM.IAV.IA.Util;

    /**
     * Realiza el corazón de toda búsqueda general, utilizando algún tipo de cola (FIFO, LIFO o de prioridad) como estructura para gestionar la frontera. 
     * 
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public abstract class QueueSearch : NodeExpander {

        public static readonly string METRIC_QUEUE_SIZE = "queueSize";

        public static readonly string METRIC_MAX_QUEUE_SIZE = "maxQueueSize";

        public static readonly string METRIC_PATH_COST = "pathCost";

        private IQueue<Node> frontier = null; // IQueue puede ser cola FIFO, LIFO o de prioriad
        private bool checkGoalBeforeAddingToFrontier = false;

        public virtual bool IsFailure(List<Operator> result) {
            return 0 == result.Count;
        }

        // Si se ha encontrado el objetivo, devuelve la lista operadores hasta llegar hasta él.
        // Si ya estás en el objetivo, devuelve una lista con un sólo operador NoOp dentro.
        // Si falla en encontrar el objetivo se devuelve una lista vacía y eso significa que la búsqueda ha fallado. 
        public virtual List<Operator> Search(Problem problem, IQueue<Node> frontier) {

            this.frontier = frontier;

            ClearInstrumentation();
            // initialize the frontier using the initial setup of the problem
            Node root = new Node(problem.GetInitialSetup());
            if (IsCheckGoalBeforeAddingToFrontier())
            {
                if (SearchUtils.IsGoalSetup(problem, root))
                {
                    return SearchUtils.GetOperatorsFromNodes(root.GetPathFromRoot());
                }
            }
            frontier.Enqueue(root);
            setQueueSize(frontier.Count);
            while (!(frontier.Count==0))
            {
                // choose a leaf node and remove it from the frontier
                Node nodeToExpand = PopNodeFromFrontier();
                setQueueSize(frontier.Count);
                // Only need to check the nodeToExpand if have not already
                // checked before adding to the frontier
                if (!IsCheckGoalBeforeAddingToFrontier())
                {
                    // if the node contains a goal setup then return the corresponding solution
                    if (SearchUtils.IsGoalSetup(problem, nodeToExpand))
                    {
                        setPathCost(nodeToExpand.GetPathCost());
                        return SearchUtils.GetOperatorsFromNodes(nodeToExpand
                                .GetPathFromRoot());
                    }
                }
                // expand the chosen node, adding the resulting nodes to the
                // frontier
                foreach (Node fn in GetResultingNodesToAddToFrontier(nodeToExpand,
                        problem))
                {
                    if (IsCheckGoalBeforeAddingToFrontier())
                    {
                        if (SearchUtils.IsGoalSetup(problem, fn))
                        {
                            setPathCost(fn.GetPathCost());
                            return SearchUtils.GetOperatorsFromNodes(fn
                                    .GetPathFromRoot());
                        }
                    }
                    frontier.Enqueue(fn);
                }
                setQueueSize(frontier.Count);
            }
            // if the frontier is empty then return failure
            return Failure();
        }

        public virtual bool IsCheckGoalBeforeAddingToFrontier()
        {
            return checkGoalBeforeAddingToFrontier;
        }

        public virtual void SetCheckGoalBeforeAddingToFrontier(
                bool checkGoalBeforeAddingToFrontier)
        {
            this.checkGoalBeforeAddingToFrontier = checkGoalBeforeAddingToFrontier;
        }

        public virtual Node PopNodeFromFrontier()
        {
            return frontier.Dequeue();
        }
         
        /* 
        // Elimina un nodo de la frontera
        public virtual bool RemoveNodeFromFrontier(Node toRemove)
        {
            if (frontier.Contains(toRemove)) {
                // Como estoy trabajando con COLAS y no con listas, no hay forma eficiente de borrar un nodo al azar, salvo recorriéndolo todo
                // Sería mejor borrar al extraer el nodo o algo así, que es lo normal al trabajar con colas
                // BORRARLO DE ALGUNA FORMA, AHORA MISMO NO ES POSIBLE: frontier.Remove(toRemove);
                return true;
            }
            return false; 
        } 
        */

        public abstract List<Node> GetResultingNodesToAddToFrontier(Node nodeToExpand, Problem p);

        // Sobreescribimos el método del padre
        public override void ClearInstrumentation() {
            base.ClearInstrumentation();
            metrics.set(METRIC_QUEUE_SIZE, 0);
            metrics.set(METRIC_MAX_QUEUE_SIZE, 0);
            metrics.set(METRIC_PATH_COST, 0);
        }

        public int getQueueSize()
        {
            return metrics.getInt("queueSize");
        }

        public void setQueueSize(int queueSize)
        {

            metrics.set(METRIC_QUEUE_SIZE, queueSize);
            int maxQSize = metrics.getInt(METRIC_MAX_QUEUE_SIZE);
            if (queueSize > maxQSize)
            {
                metrics.set(METRIC_MAX_QUEUE_SIZE, queueSize);
            }
        }

        public int getMaxQueueSize()
        {
            return metrics.getInt(METRIC_MAX_QUEUE_SIZE);
        }

        public double getPathCost()
        {
            return metrics.getDouble(METRIC_PATH_COST);
        }

        public void setPathCost(double pathCost)
        {
            metrics.set(METRIC_PATH_COST, pathCost);
        }

        //
        // PRIVATE METHODS
        //
        private List<Operator> Failure()
        {
            return new List<Operator>();
        }
    }
}