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

    /**
     * Implementación abstracta de la interfaz PLVisitor que proporciona comportamiento por defecto para cada uno de los métodos.
     * 
     * @param <A>
     *            el tipo de argumento que se pasará a los métodos visitantes.
     */
    public abstract class AbstractPLVisitor<A> : PLVisitor<A, Sentence> {

        // No hace falta override porque PLVisitor es una interfaz, pero pongo virtual por si queremos seguir sobreescribiendo
	    public virtual Sentence VisitPropositionSymbol(PropositionSymbol s, A arg) {
		    // El comportamiento por defecto es tratar los símbolos proposicionales como áromos y dejarlos sin cambios.
		    return s;
	    }

        // No hace falta override porque PLVisitor es una interfaz, pero pongo virtual por si queremos seguir sobreescribiendo
        public virtual Sentence VisitUnarySentence(ComplexSentence s, A arg) {
		    // Una nueva sentencia compleja con la misma conectiva pero posiblemente con su sentencia más simple reemplazada por el visitante.
		    return new ComplexSentence(s.GetConnective(), s.GetSimplerSentence(0).Accept(this, arg));
	    }

        // No hace falta override porque PLVisitor es una interfaz, pero pongo virtual por si queremos seguir sobreescribiendo
        public virtual Sentence VisitBinarySentence(ComplexSentence s, A arg) {
            // Una nueva sentencia compleja con la misma conectiva pero posiblemente con sus sentencias más simples reemplazadas por el visitante.
            return new ComplexSentence(s.GetConnective(), s.GetSimplerSentence(0).Accept(this, arg), s.GetSimplerSentence(1).Accept(this, arg));
	    }
    }
}