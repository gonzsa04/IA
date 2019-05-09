/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Visitors {

    using UCM.IAV.AI.Logic.Propositional.Parsing;

        /**
         * Artificial Intelligence A Modern Approach (3rd Edition): page 253. 
         * 
         * Elimina =>, reemplazando &alpha; => &beta;<br> con ~&alpha; | &beta;
         */
        public class ImplicationElimination : AbstractPLVisitor<object> {

        /**
	     * Elimina las implicaciones de una sentencia.
	     * 
	     * @param sentence
	     *            una sentencia de lógica proposicional
	     * @return una sentencia equivalente a la de entrada con todas las implicaciones eliminadas. 
	     */
        public static Sentence Eliminate(Sentence sentence) {
		    ImplicationElimination eliminator = new ImplicationElimination();

		    Sentence result = sentence.Accept(eliminator, null);

		    return result;
	    }

	    public override Sentence VisitBinarySentence(ComplexSentence s, object arg) {
		    Sentence result = null;
		    if (s.IsImplicationSentence()) {
                // Elimina =>, reemplazando & alpha; => &beta;< br > con ~&alpha; | &beta;
                Sentence alpha = s.GetSimplerSentence(0).Accept(this, arg);
			    Sentence beta = s.GetSimplerSentence(1).Accept(this, arg);
			    Sentence notAlpha = new ComplexSentence(Connective.NOT, alpha);
			
			    result = new ComplexSentence(Connective.OR, notAlpha, beta);
		    } else {
			    result = base.VisitBinarySentence(s, arg);
		    }
		    return result;
	    }
    }
}
