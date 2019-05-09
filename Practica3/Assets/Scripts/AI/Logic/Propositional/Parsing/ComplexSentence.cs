/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com
    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/

namespace UCM.IAV.AI.Logic.Propositional.Parsing {

    using System; 
	
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 244.
     * Las sentencias compuestas (aquí complex/complejas) se construyen de sentencias más simples, usando paréntesis (y corchetes) y conectivas lógicas.
     */
    public class ComplexSentence : Sentence {

	    private Connective connective;
	    private Sentence[] simplerSentences;
	    // Inicialización perezosa de estos valores
	    private int cachedHashCode = -1;
	    private string cachedConcreteSyntax = null;

	    /**
	     * Constructor.
	     * 
	     * @param connective
	     *            la conectiva de la sentencia compuesta
	     * @param sentences
	     *            las sentencias más simples que forma la sentencia compuesta.
	     */
	    public ComplexSentence(Connective connective, params Sentence[] sentences) { // Sentence...
		    // Aserciones para comprobar los argumentos
		    AssertLegalArguments(connective, sentences);
	
		    this.connective = connective;
		    simplerSentences = new Sentence[sentences.Length];   
		    Array.Copy(sentences, 0, simplerSentences, 0, sentences.Length); // System.arraycopy en Java, creo que los parámetros son equivalentes todos
	    }
	
	    /**
	     * Constructor conveniente para sentencias binarias.
	     * 
	     * @param sentenceL
	     * 			la sentencia de la izquierda.
	     * @param binaryConnective
	     * 			la conectiva binaria.
	     * @param sentenceR
	     *  		la sentencia de la derecha.
	     */
	    public ComplexSentence(Sentence sentenceL, Connective binaryConnective, Sentence sentenceR) : this(binaryConnective, sentenceL, sentenceR) {
	    }
         
	    public override Connective GetConnective() { // Property mejor
		    return connective;
	    }

	    public override int GetNumberSimplerSentences() {
		    return simplerSentences.Length; // Por qué Length en lugar de otro nombre
	    }

	    public override Sentence GetSimplerSentence(int offset) {
		    return simplerSentences[offset];
	    }

        // Compara esta sentencia compuesta con otra y dice si son iguales
        public bool Equals(ComplexSentence cs) {
            if (cs == null)
                return false;
            if (this == cs)
                return true;

            bool result = false;
            if (cs.GetHashCode() == this.GetHashCode()) {
                if (cs.GetConnective().Equals(this.GetConnective())
                        && cs.GetNumberSimplerSentences() == this.GetNumberSimplerSentences()) {
                    // Si la conectiva y el número de sentencias más simples coincide, asumir que hay coincidencia y pasar a comprobar CADA sentencia simple 
                    result = true;
                    for (int i = 0; i < this.GetNumberSimplerSentences(); i++) {
                        if (!cs.GetSimplerSentence(i).Equals(this.GetSimplerSentence(i))) {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        // Compara esta sentencia compuesta con otro objeto y dice si son iguales 
        public override bool Equals(object obj) {
            if (obj is ComplexSentence) // is en vez de instanceof (y mejor que typeof que obligaría a ser del tipo exacto)
                return this.Equals(obj as ComplexSentence);
            return false;
        }

        // Devuelve código hash del literal (para optimizar el acceso en colecciones y así)
        // No debe contener bucles, tiene que ser muy rápida
        public override int GetHashCode() {
            if (cachedHashCode == -1) {
                cachedHashCode = 17 * GetConnective().GetHashCode();
                foreach (Sentence s in simplerSentences) 
                    cachedHashCode = (cachedHashCode * 37) + s.GetHashCode(); 
            }
            return cachedHashCode;
        }

        // Cadena de texto representativa  
        public override string ToString() {
            if (cachedConcreteSyntax == null) {
                if (IsUnarySentence()) {
                    cachedConcreteSyntax = GetConnective() + BracketSentenceIfNecessary(GetConnective(), GetSimplerSentence(0));
                } else if (IsBinarySentence()) {
                    cachedConcreteSyntax = BracketSentenceIfNecessary(GetConnective(), GetSimplerSentence(0))
                            + " "
                            + GetConnective()
                            + " "
                            + BracketSentenceIfNecessary(GetConnective(), GetSimplerSentence(1));
                }
            }
            return cachedConcreteSyntax;
        }

	    //
	    // PRIVATE
	    //
	    private void AssertLegalArguments(Connective connective, params Sentence[] sentences) { // Sentence...
            if (connective == null) {
			    throw new ArgumentNullException("Connective must be specified."); // IllegalArgumentException
            }
		    if (sentences == null) {
			    throw new ArgumentNullException("> 0 simpler sentences must be specified."); // IllegalArgumentException
            }
		    if (connective == Connective.NOT) {
			    if (sentences.Length != 1) { //en vez de Count (puede que sea incluso un array vacío... no nulo, pero vacío, ojo)
				    throw new ArgumentException("A not (~) complex sentence take exactly 1 simpler sentence not " + sentences.Length); // IllegalArgumentException
                }
		    }
		    else {
			    if (sentences.Length != 2) {
				    throw new ArgumentException("Connective is binary (" + connective + ") but only " + sentences.Length + " simpler sentences provided"); // IllegalArgumentException
                }
		    }
	    }
    }
}
