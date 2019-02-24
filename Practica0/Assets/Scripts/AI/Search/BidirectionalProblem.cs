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
     * Describe un problema que puede ser abordado en ambas direcciones al mismo tiempo: desde la configuración inicial hasta la final y viceversa.
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public interface BidirectionalProblem2 {

        // Obtiene el problema original
        Problem GetOriginalProblem();

        // Obtiene el problema invertido (la configuración inicial será la final)
        Problem GetReverseProblem();
    }
}