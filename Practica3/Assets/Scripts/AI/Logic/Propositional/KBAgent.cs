/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional {

    using UCM.IAV.AI.Agency;
    using UCM.IAV.AI.Logic.Propositional.Parsing;
    using UCM.IAV.AI.Logic.Propositional.KB;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 7.1, page 236.
     * 
     * function KB-AGENT(percept) returns an action
     *   persistent: KB, a knowledge base
     *               t, a counter, initially 0, indicating time
     *           
     *   TELL(KB, MAKE-PERCEPT-SENTENCE(percept, t))
     *   action &lt;- ASK(KB, MAKE-ACTION-QUERY(t))
     *   TELL(KB, MAKE-ACTION-SENTENCE(action, t))
     *   t &lt;- t + 1
     *   return action
     * 
     * Un agente basado en conocimiento gen�rico. Dada una percepci�n, el agente la a�ade a su base de conocimiento,
     * pregunta a la base de conocimiento cual es la mejor acci�n a realizar, y (supuestamente despu�s de hacerla) le dice a la base de conocimiento que la ha hecho.  
     */
    public abstract class KBAgent : AbstractAgent {

	    // persistente: KB, una base de conocimiento
	    protected KnowledgeBase KB;
	    // t, un contador, inicialmente a 0, indicando el tiempo
	    private int t = 0;

	    public KBAgent(KnowledgeBase KB) {
		    this.KB = KB;
	    }

	    // La funci�n KB-AGENT(percept) devuelve una acci�n
	    public override Action Execute(Percept percept) {
		    // TELL(KB, MAKE-PERCEPT-SENTENCE(percept, t))
		    KB.Tell(MakePerceptSentence(percept, t));
		    // action &lt;- ASK(KB, MAKE-ACTION-QUERY(t))
		    Action action = Ask(KB, MakeActionQuery(t));
		
		    // TELL(KB, MAKE-ACTION-SENTENCE(action, t))
		    KB.Tell(MakeActionSentence(action, t));

		    t = t + 1; 
		    return action;
	    }

        /**
	     * MAKE-PERCEPT-SENTENCE construye una sentencia asertando que el agente percibe la percepci�n dada en el momento dado. 
	     * 
	     * @param percept
	     *            la percepci�n dada
	     * @param t
	     *            el momento dado
	     * @return una sentencia asertando que el agente percibe la percepci�n dada en el momento dado. 
	     */
        // MAKE-PERCEPT-SENTENCE(percept, t)
        public abstract Sentence MakePerceptSentence(Percept percept, int t);

        /**
	     * MAKE-ACTION-QUERY construye una sentencia que pregunta qu� acci�n se podr�a realizar en el momento dado. 
	     * 
	     * @param t
	     *            el momento dado
	     * @return una sentencia que pregunta qu� acci�n se podr�a realizar en el momento dado. 
	     */
        // MAKE-ACTION-QUERY(t)
        public abstract Sentence MakeActionQuery(int t);

        /**
	     * MAKE-ACTION-SENTENCE construye una sentencia asertando que la acci�n elegida fue ejecutada. 
	     * @param action
	     *        la acci�n elegida
	     * @param t
	     *        el tiempo en que la acci�n fue ejecutada
	     * @return una sentencia asertando que la acci�n elegida fue ejecutada. 
	     */
        // MAKE-ACTION-SENTENCE(action, t)
        public abstract Sentence MakeActionSentence(Action action, int t);
	
	    /**
	     * Un envoltorio (wrapper) al rededor del m�todo ask() de la BC que traduce la acci�n (en forma de sentencia) determinada por la BC  
	     * en un objeto 'Action' permitido para el actual entorno en el que el KB-AGENT reside.
	     * 
	     * @param KB
	     *        la BC a la que preguntar.
	     * @param actionQuery
	     *        una consulta de acci�n
	     * @return la acci�n a ser realizada en respuesta a la consultada dada.
	     */
	    // ASK(KB, MAKE-ACTION-QUERY(t))
	    public abstract Action Ask(KnowledgeBase KB, Sentence actionQuery);
    }
}