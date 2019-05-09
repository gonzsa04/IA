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
     * Artificial Intelligence A Modern Approach (3rd Edition): pg 35. 
     * Un comportamiento del agente se describe por la 'funci�n de agente' que relacionado cualquier secuencia de percepciones dada con una acci�n.
     * Internamente, la funci�n de agente de un agente artificial ser� implementada por un programa de agente.
     */
    public interface AgentProgram {
        /**
         * El programa del agente, que relaciona cualquier secuencia de percepcione sdada a una acci�n. 
         * 
         * @param percept
         *            La actual percepci�n de una secuencia percibida por el agente.
         * @return la acci�n a ser tomada en respuesta por la percepci�n actualmente percibida.
         */
        Action Execute(Percept percept);
    }
}
