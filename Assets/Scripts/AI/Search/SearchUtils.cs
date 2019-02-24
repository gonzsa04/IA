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
     * Utilidades para realizar la búsqueda.
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public class SearchUtils {

        // Devuelve la lista de operadores aplicables a toda una lista de nodos
        public static List<Operator> GetOperatorsFromNodes(List<Node> nodeList) {
            List<Operator> operators = new List<Operator>();
            if (nodeList.Count == 1)
            {
                // I'm at the root node, this indicates I started at the
                // Goal node, therefore just return a NoOp
                operators.Add(NoOperator.NO_OP);
            }
            else
            {
                // ignore the root node this has no operator
                // hence index starts from 1 not zero
                for (int i = 1; i < nodeList.Count; i++)
                {
                    Node node = nodeList[i];
                    operators.Add(node.GetOperator());
                }
            }
            return operators;
        }

        // Devuelve si es cierto o no que un nodo contiene una configuración objetivo
        public static bool IsGoalSetup(Problem p, Node n) {

            bool isGoal = false;
            GoalTest gt = p.GetGoalTest();
            if (gt.IsGoalSetup(n.GetSetup()))
            {
                if (gt is SolutionChecker) {
                    isGoal = ((SolutionChecker)gt).IsAcceptableSolution(
                            SearchUtils.GetOperatorsFromNodes(n.GetPathFromRoot()), n
                                    .GetSetup());
                }
                else
                {
                    isGoal = true;
                }
            }
            return isGoal;
        }
    }
}