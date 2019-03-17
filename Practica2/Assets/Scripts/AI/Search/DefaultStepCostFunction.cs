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

    using UCM.IAV.IA;

    /**
     * Relaciona cualquier paso de la soluci�n con su coste por defecto.
     * Esto pertenece a la infraestructura (framework) de la b�squeda.
     */
    public class DefaultStepCostFunction : StepCostFunction {

        // Devuelve 1 como coste por defecto para cualquier paso de la soluci�n
        public double GetCost (object setupFrom, Operator op, object setupTo) {
            return 1.0d;
        }
    }
}