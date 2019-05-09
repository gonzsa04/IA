/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
// El espacio de nombres no se puede llamar Agent porque ya tenemos una clase con ese nombre
namespace UCM.IAV.AI.Agency {

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.1, page 35. 
     * 
     * El agente interactúa con el entorno a través de sus sensores y actuadores.
     */
    public interface Agent { // NO HE AÑADIDO : EnvironmentObject
        /**
         * Llama al programa del agente, que relaciona cualquier secuencia de percepciones dadas con una acción.
         * 
         * @param percept
         *            La actual secuencia de percepciones percibida por el agente.
         * @return la acción a ser tomada en respuesta a la percepción percibida actualmente 
         */
        Action Execute(Percept percept);

        /**
         * Indicador del ciclo de vida sobre si está vivo el agente. 
         * 
         * @return cierto si el agente está considerado vivo, falso en caso contrario. 
         */
        bool IsAlive();

	    /**
	     * Establece la vida actual del agente.
	     * 
	     * @param alive
	     *            establece la vida a cierto si el agente es considerado vivo, a falso en otro caso.
	     */
	    void SetAlive(bool alive);
    }
}
