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
     * Devuelve un conjunto de operadores aplicables dada la configuración que sea.
     * 
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public interface OperatorsFunction
    {
        // Devuelve los operadores aplicables en la configuración setup 
        // Se devuelve un HashSet porque es un conjunto
        HashSet<Operator> Operators(object setup);
    }
}