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

    using System;
    using UCM.IAV.AI.Logic.Propositional.Parsing;

    /**
    * Artificial Intelligence A Modern Approach (3rd Edition): page 254. 
    * Mueve ~ hacia dentro mediante la aplicación repetida de las siguientes equivalencias: 
    * ~(~&alpha;) &equiv; &alpha; (eliminación de la doble negación)
    * ~(&alpha; & &beta;) &equiv; (~&alpha; | ~&beta;) (De Morgan)
    * ~(&alpha; | &beta;) &equiv; (~&alpha; & ~&beta;) (De Morgan)
    */
    public class MoveNotInwards : AbstractPLVisitor<object> {

	    /**
	     * Mueve ~ hacia dentro.
	     * 
	     * @param sentence
	     *            una sentencia de lógica proposicional que tiene sus bicondicionales e implicaciones quitadas. 
	     * @return una sentencia equivalente a la de la entrada con todas las negaciones movidas hacia dentro. 
	     * @throws ArgumentException
	     *             si se encuentra una bicondicional o una implicación en la entrada 
	     */
	    public static Sentence MoveNotsInward(Sentence sentence) {
		    Sentence result = null;

		    MoveNotInwards moveNotsIn = new MoveNotInwards();
		    result = sentence.Accept(moveNotsIn, null);

		    return result;
	    }
         
	    public override Sentence VisitUnarySentence(ComplexSentence s, object arg) {
		    Sentence result = null;

		    Sentence negated = s.GetSimplerSentence(0);
		    if (negated.IsPropositionSymbol()) {
			    // Ya se ha movido completamente
			    result = s;
		    } else if (negated.IsNotSentence()) {
			    // ~(~&alpha;) &equiv; &alpha; (eliminación de la doble negación)
			    Sentence alpha = negated.GetSimplerSentence(0);
			    result = alpha.Accept(this, arg);
		    } else if (negated.IsAndSentence() || negated.IsOrSentence()) {
			    Sentence alpha = negated.GetSimplerSentence(0);
			    Sentence beta = negated.GetSimplerSentence(1);

			    // Esto asegura que la eliminación de la doble negación sucede 
			    Sentence notAlpha = (new ComplexSentence(Connective.NOT, alpha))
					    .Accept(this, null);
			    Sentence notBeta = (new ComplexSentence(Connective.NOT, beta))
					    .Accept(this, null);
			    if (negated.IsAndSentence()) {
				    // ~(&alpha; & &beta;) &equiv; (~&alpha; | ~&beta;) (De Morgan)
				    result = new ComplexSentence(Connective.OR, notAlpha, notBeta);
			    } else {
				    // ~(&alpha; | &beta;) &equiv; (~&alpha; & ~&beta;) (De Morgan)
				    result = new ComplexSentence(Connective.AND, notAlpha, notBeta);
			    }
		    } else {
			    throw new ArgumentException( //IllegalArgumentException
                        "Biconditionals and Implications should not exist in input: "
							    + s);
		    }

		    return result;
	    }
    }
}