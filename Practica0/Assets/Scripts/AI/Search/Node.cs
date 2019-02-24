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

    using System;
    using System.Collections.Generic;
    using UCM.IAV.IA;

    /**
     * Estructura de datos con la que se construye el árbol de búsqueda. 
     * Todos los nodos tienen una configuración (SETUP), un padre (PARENT, porque estas referencias van de hijos a padres), 
     * el operador (OPERATOR) que fue aplicado al padre para generar este nodo y el coste de la ruta (la famosa g(n)) 
     * desde la configuración inicial hasta la de este nodo, que puede ser recorrida siguiendo las referencias al padre.
     * 
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public class Node : IComparable<Node> { // HE PUESTO QUE EXTIENDA ICOMPARER PORQUE SE NECESITA PARA PODER HACER COLAS DE PRIORIDAD CON ELLOS

        // n.SETUP: the setup in the setup space to which the node corresponds;
        private object setup;

        // n.PARENT: the node in the search tree that generated this node;
        private Node parent;

        // n.OPERATOR: the operator that was applied to the parent to generate the node;
        private Operator op;

        // n.PATH-COST: the cost, traditionally denoted by g(n), of the path from
        // the initial setup to the node, as indicated by the parent pointers.
        private double pathCost;

        public Node(object setup) {
            this.setup = setup;
            this.pathCost = 0.0;
        }

        public Node(object setup, Node parent, Operator op, double stepCost) : this(setup)
        {
            this.parent = parent;
            this.op = op;
            this.pathCost = parent.pathCost + stepCost;
        }

        public object GetSetup()
        {
            return setup;
        }

        public Node GetParent()
        {
            return parent;
        }

        public Operator GetOperator()
        {
            return op;
        }

        public double GetPathCost()
        {
            return pathCost;
        }

        public bool IsRootNode()
        {
            return parent == null;
        }

        public List<Node> GetPathFromRoot()
        {
            List<Node> path = new List<Node>();
            Node current = this;
            while (!current.IsRootNode())
            {
                path.Insert(0, current);
                current = current.GetParent();
            }
            // ensure the root node is added
            path.Insert(0, current);
            return path;
        }

        public override string ToString() {
            return "[parent=" + parent + ", op=" + op + ", setup="
                    + GetSetup() + ", pathCost=" + pathCost + "]";
        }

        // A LO MEJOR TIENE SENTIDO HACER EL COMPARADOR DE NODOS COMO ALGO EXTERNO AL NODO... EL NODO SI ACASO QUE TENGAN EL COMPARARSE CON 'OTRO' NODO
        // Para poder comparar nodos (no tiene mucho sentido.. lo normal será comparar ESTE nodo, con el otro que te pasan :-)
        // PERO EL INTERFAZ EN C# ES ASÍ, SE COMPARAN DOS OBJETOS Y PUNTO
        public int CompareTo(Node y) { 
            // AQUÍ COMPARAMOS NODOS, BÁSICAMENTE EL COSTE
            if (GetPathCost() > y.GetPathCost())
                return 1;
            if (GetPathCost() < y.GetPathCost())
                return -1;
            return 0; 
        }
    }
}