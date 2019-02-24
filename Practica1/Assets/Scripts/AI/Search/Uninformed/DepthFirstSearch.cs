/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search.Uninformed {

    using System;
    using System.Collections.Generic;
    using UCM.IAV.IA.Search;
    using UCM.IAV.IA;
    using UCM.IAV.IA.Util;

    /**
     * Realiza la b�squeda primero en profundidad.
     * 
     * Siempre expande el nodo m�s profundo que haya en la frontera actual del �rbol de b�squeda.
     * 
     * Soporta tamto la versi�n para grafos como para �rboles simplemente asign�ndole un ejemplar de TreeSearch o GraphSearch en el constructor
     */
    public class DepthFirstSearch : Search {

        QueueSearch search;

        public DepthFirstSearch(QueueSearch search)
        {
            this.search = search;
        }

        public List<Operator> Search(Problem p)
        {
            return search.Search(p, new LIFOQueue<Node>()); 
        }

        public Metrics GetMetrics()
        {
            return search.GetMetrics();
        }
    }
}