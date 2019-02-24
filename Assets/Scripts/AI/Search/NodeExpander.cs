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

    /**
     * Herramienta que se utiliza para expandir un nodo y obtener todos sus hijos a la vez. 
     * 
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public class NodeExpander {

        public static readonly string METRIC_NODES_EXPANDED = "nodesExpanded";

        protected Metrics metrics;

        public NodeExpander() {
            metrics = new Metrics();
        }

        // Es virtual porque podría interesar sobreescribirlo en una clase que herede de esta
        public virtual void ClearInstrumentation() {
            metrics.set(METRIC_NODES_EXPANDED, 0);
        }

        public int GetNodesExpanded() // o get expanded nodes
        {
            return metrics.getInt(METRIC_NODES_EXPANDED);
        }

        public Metrics GetMetrics()
        {
            return metrics;
        }

        // Devuelve la lista de todos los nodos hijos del nodo que se desea expandir
        public List<Node> ExpandNode(Node node, Problem problem) {
            List<Node> childNodes = new List<Node>();

            OperatorsFunction operatorsFunction = problem.GetOperatorsFunction();
            ResultFunction resultFunction = problem.GetResultFunction();
            StepCostFunction stepCostFunction = problem.GetStepCostFunction();

            foreach (Operator op in operatorsFunction.Operators(node.GetSetup()))
            {
                object successorSetup = resultFunction.GetResult(node.GetSetup(), op);
                double stepCost = stepCostFunction.GetCost(node.GetSetup(), op, successorSetup);
                childNodes.Add(new Node(successorSetup, node, op, stepCost));
            }
            metrics.set(METRIC_NODES_EXPANDED, metrics.getInt(METRIC_NODES_EXPANDED) + 1);

            return childNodes;
        }
    }
}