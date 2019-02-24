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
     
    using UCM.IAV.IA;

    /**
     * El coste de un paso consistente en aplicar el operador 'op' a la configuración setup para obtener la configuración resultSetup o setupTarget (formalmente c(sO, op, sT)).
     * Esto pertenece a la infraestructura (framework) de la búsqueda.
     */
    public interface StepCostFunction {

         // Devuelve el coste del paso consistente en aplicar un operador a una configuración, para obtener otra
        double GetCost(object setup, Operator op, object resultSetup);
    }
}