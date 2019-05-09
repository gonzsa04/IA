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
     * Artificial Intelligence A Modern Approach (3rd Edition): pg 35. 
     * Un comportamiento del agente se describe por la 'función de agente' que relacionado cualquier secuencia de percepciones dada con una acción.
     * Internamente, la función de agente de un agente artificial será implementada por un programa de agente.
     */
    public interface AgentProgram {
        /**
         * El programa del agente, que relaciona cualquier secuencia de percepcione sdada a una acción. 
         * 
         * @param percept
         *            La actual percepción de una secuencia percibida por el agente.
         * @return la acción a ser tomada en respuesta por la percepción actualmente percibida.
         */
        Action Execute(Percept percept);
    }
}
