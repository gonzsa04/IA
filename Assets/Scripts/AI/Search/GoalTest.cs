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

    /**
     * Indica sobre cualquier configuración del problema si es una configuración objetivo. 
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public interface GoalTest {

        // Devuelve cierto o falso según esta configuración sea o no una configuración objetivo
        bool IsGoalSetup(object setup);
    }
}