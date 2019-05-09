/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Parsing {

    /**
     * Implementaci�n abstracta de la interfaz PLVisitor que proporciona comportamiento por defecto para cada uno de los m�todos.
     * 
     * @param <A>
     *            el tipo de argumento que se pasar� a los m�todos visitantes.
     */
    public abstract class AbstractPLVisitor<A> : PLVisitor<A, Sentence> {

        // No hace falta override porque PLVisitor es una interfaz, pero pongo virtual por si queremos seguir sobreescribiendo
	    public virtual Sentence VisitPropositionSymbol(PropositionSymbol s, A arg) {
		    // El comportamiento por defecto es tratar los s�mbolos proposicionales como �romos y dejarlos sin cambios.
		    return s;
	    }

        // No hace falta override porque PLVisitor es una interfaz, pero pongo virtual por si queremos seguir sobreescribiendo
        public virtual Sentence VisitUnarySentence(ComplexSentence s, A arg) {
		    // Una nueva sentencia compleja con la misma conectiva pero posiblemente con su sentencia m�s simple reemplazada por el visitante.
		    return new ComplexSentence(s.GetConnective(), s.GetSimplerSentence(0).Accept(this, arg));
	    }

        // No hace falta override porque PLVisitor es una interfaz, pero pongo virtual por si queremos seguir sobreescribiendo
        public virtual Sentence VisitBinarySentence(ComplexSentence s, A arg) {
            // Una nueva sentencia compleja con la misma conectiva pero posiblemente con sus sentencias m�s simples reemplazadas por el visitante.
            return new ComplexSentence(s.GetConnective(), s.GetSimplerSentence(0).Accept(this, arg), s.GetSimplerSentence(1).Accept(this, arg));
	    }
    }
}