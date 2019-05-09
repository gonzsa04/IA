/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Agency {

    /**
     * Describe una acción que puede o que ha sido realizada por un agente a través de sus actuadores.
     */
    public interface Action {

        /**
         * Indica si esta acción es una 'No Operation' o no.
         * 
         * Nota: La no operacción del AIMA3e - NoOp, es el nombre de una instrucción del lenguaje ensamblador que no hace nada. 
         * 
         * @return cierto si esto es una NoOp Action.
         */
        bool IsNoOp(); // o IsNoOperator, por coherencia a cómo se llama el otro método
    }
}
