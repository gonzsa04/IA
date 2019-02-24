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
     * Realiza la búsqueda en grafos de manera general.
     * 
     * <code>
     * function GRAPH-SEARCH(problem) returns a solution, or failure
     *   initialize the frontier using the initial setup of problem
     *   initialize the explored set to be empty
     *   loop do
     *     if the frontier is empty then return failure
     *     choose a leaf node and remove it from the frontier
     *     if the node contains a goal setup then return the corresponding solution
     *     add the node to the explored set
     *     expand the chosen node, adding the resulting nodes to the frontier
     *       only if not in the frontier or explored set
     * </code> 
     * 
     * Matiz importante:
     * In contrast to the code above, here, nodes resulting from node expansion are added to the
     * frontier even if nodes with equal setups already exist there. This makes it
     * possible to use the implementation also in combination with priority queue frontiers.
     * 
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public class GraphSearch : QueueSearch {

        private HashSet<object> explored = new HashSet<object>();
        // EN VEZ DE MAP DICTIONARY
        private Dictionary<object, Node> frontierSetup = new Dictionary<object, Node>();
        private IComparer<Node> replaceFrontierNodeAtSetupCostFunction = null;
        private List<Node> addToFrontier = new List<Node>();

        public IComparer<Node> GetReplaceFrontierNodeAtSetupCostFunction() {
            return replaceFrontierNodeAtSetupCostFunction;
        }

        public void SetReplaceFrontierNodeAtSetupCostFunction(
                IComparer<Node> replaceFrontierNodeAtSetupCostFunction) {
            this.replaceFrontierNodeAtSetupCostFunction = replaceFrontierNodeAtSetupCostFunction;
        }

        // Need to override search() method so that I can re-initialize
        // the explored set should multiple calls to search be made.
        public override List<Operator> Search(Problem problem, IQueue<Node> frontier) //IQueue (una cola FIFO normal, o una LIFO, o de prioridad)
        {
            // initialize the explored set to be empty
            explored.Clear();
            frontierSetup.Clear();
            return base.Search(problem, frontier);
        }

        public override Node PopNodeFromFrontier()
        {
            Node toRemove = base.PopNodeFromFrontier();
            frontierSetup.Remove(toRemove.GetSetup());
            return toRemove;
        }

        /* Ahora mismo no tiene sentido hacer un borrado de un nodo que no sea el primeroque ofrece la cola
        public override bool RemoveNodeFromFrontier(Node toRemove)
        {
            bool removed = base.RemoveNodeFromFrontier(toRemove);
            if (removed)
            {
                frontierSetup.Remove(toRemove.GetSetup());
            }
            return removed;
        } */

        public override List<Node> GetResultingNodesToAddToFrontier(Node nodeToExpand, Problem problem) {

            addToFrontier.Clear();
            // add the node to the explored set
            explored.Add(nodeToExpand.GetSetup());
            // expand the chosen node, adding the resulting nodes to the frontier
            foreach (Node cfn in ExpandNode(nodeToExpand, problem)) {

                Node frontierNode;
                frontierSetup.TryGetValue(cfn.GetSetup(), out frontierNode); // El funcionamiento de este Diccionario parece estar fallando (antes es que tenía notación de array y seguramente no era válido)
                bool yesAddToFrontier = false;
                // only if not in the frontier or explored set
                if (null == frontierNode && !explored.Contains(cfn.GetSetup()))
                {
                    yesAddToFrontier = true;
                }
                else if (null != frontierNode
                      && null != replaceFrontierNodeAtSetupCostFunction
                      && replaceFrontierNodeAtSetupCostFunction.Compare(cfn, frontierNode) < 0)
                {
                    // child.SETUP is in frontier with higher cost
                    // replace that frontier node with child
                    yesAddToFrontier = true;
                    // Want to replace the current frontier node with the child
                    // node therefore mark the child to be added and remove the
                    // current fontierNode

                    /**************************************************************
                     * POR AHORA HE ELIMINADO ESTA PARTE, HAY COMENTARIOS DE QUE USANDO COLAS DE PRIORIDAD NO SE NECESITA ESTO
                     * ADEMÁS TÉCNICAMENTE NO SE PUEDE ELIMINAR UN NODO DE FORMA TAN DIRECTAN DE UNA COLA...
                     * 
                    RemoveNodeFromFrontier(frontierNode);
                    **********************************************/


                    // Ensure removed from add to frontier as well
                    // as 1 or more may reach the same setup at the same time
                    addToFrontier.Remove(frontierNode);
                }

                if (yesAddToFrontier)
                {
                    addToFrontier.Add(cfn);
                    frontierSetup.Add(cfn.GetSetup(), cfn);
                }
            }

            return addToFrontier;
        }
    }
}