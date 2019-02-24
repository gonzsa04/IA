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
     * Una especialización de la prueba objetivo que permite comprobar si la solución es aceptable tal cual, una vez identificado el objetivo. 
     * Esto permite continuar la búsqueda de soluciones alternativas sin tener que reiniciar la búsqueda. 
     * No siempre tendrá sentido continuar con una búsqueda cuando ya se ha encontrado un objetivo.
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public interface SolutionChecker : GoalTest
    {
        // Sólo si la prueba objetivo da cierto se llama a este método, para saber si la solución es aceptable tal cual o si hay que seguir la búsqueda 
        bool IsAcceptableSolution(List<Operator> operators, object goal);
    }
}