/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search {

    using System.Collections.Generic;
    using UCM.IAV.IA;

    /*
     * Busca la soluci�n a un problema en forma de lista de operadores
     */
    public interface Search {

        List<Operator> Search(Problem p);

        Metrics GetMetrics();
    }
}