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

    /**
     * Devuelve un conjunto de operadores aplicables dada la configuraci�n que sea.
     * 
     * Esto pertenece a la infraestructura (framework) de la b�squeda.
     */
    public interface OperatorsFunction
    {
        // Devuelve los operadores aplicables en la configuraci�n setup 
        // Se devuelve un HashSet porque es un conjunto
        HashSet<Operator> Operators(object setup);
    }
}