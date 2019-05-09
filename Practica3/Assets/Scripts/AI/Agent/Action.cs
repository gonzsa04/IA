/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Agency {

    /**
     * Describe una acci�n que puede o que ha sido realizada por un agente a trav�s de sus actuadores.
     */
    public interface Action {

        /**
         * Indica si esta acci�n es una 'No Operation' o no.
         * 
         * Nota: La no operacci�n del AIMA3e - NoOp, es el nombre de una instrucci�n del lenguaje ensamblador que no hace nada. 
         * 
         * @return cierto si esto es una NoOp Action.
         */
        bool IsNoOp(); // o IsNoOperator, por coherencia a c�mo se llama el otro m�todo
    }
}
