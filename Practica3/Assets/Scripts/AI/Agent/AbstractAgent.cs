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
    
    // Faltan más comentarios por aquí
    public abstract class AbstractAgent : Agent {

	    protected AgentProgram program;
	    private bool alive = true; // Convertirlo en una propiedad, mejor

        // Constructor vacío? No creo que se pueda quitar...
	    public AbstractAgent() {

	    }

	    /**
	     * Construye un agente con el programa de agente especificado.
	     * 
	     * @param aProgram
	     *            el programa de agente, que relaciona cada secuencia de percepción dada con una acción. 
	     */
	    public AbstractAgent(AgentProgram aProgram) {
		    program = aProgram;
	    }

	    //
	    // START-Agent
        // Se marca como virtual para que luego se pueda sobreescribir
	    public virtual Action Execute(Percept p) {
		    if (program != null) {
			    return program.Execute(p);
		    }
		    return NoOpAction.NO_OP;
	    }

	    public bool IsAlive() {
		    return alive;
	    }

	    public void SetAlive(bool alive) {
		    this.alive = alive;
	    }
	    // END-Agent
	    //
    }
}