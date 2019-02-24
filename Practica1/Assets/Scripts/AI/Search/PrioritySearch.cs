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

    // Esto pertenece a la infraestructura (framework) de la búsqueda

    using System.Collections.Generic;
    using UCM.IAV.IA.Util;
    using UCM.IAV.IA.PriorityQueue;
    using UCM.IAV.IA;

    public abstract class PrioritySearch : Search {
	protected QueueSearch search;

	public List<Operator> Search(Problem p) {
            // AL CONSTRUCTOR LE METÍAN UN 5 A CAPÓN COMO PRIMER PARÁMETRO (INITIALCAPACITY)... ESO CREO QUE YA NO HACE FALTA, QUE ESTA LISTA DE PREORIDAD SE REDIMENSIONA SOLA
		return search.Search(p, new PriorityQueueWrapper()); // He puesto Node, Node... tanto en los Item como en la prioridad...
            // NO LE METO COMPARER NI NADA PORQUE LA IDEA NO VA A SER AQUÍ COPARAR NODOS SINO QUE INTERNAMENTE SE COMPAREN FLOATS, SOLAMENTE
	}

	public Metrics GetMetrics() {
		return search.GetMetrics();
	}

        //
        // PROTECTED METHODS
        // COMPARATOR lo he cambiado por Comparer 
        protected abstract IComparer<Node> GetComparer();
        // LO QUE VOY A HACER ES DAR EL COSTE, QUE ES UN DOUBLE, Y ESA SERÁ LA PRIORIDAD
}
}