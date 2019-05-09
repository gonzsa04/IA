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
     * Artificial Intelligence A Modern Approach (3rd Edition): page 249. 
     * 
     * Distributividad de | over &: 
     * (&alpha; | (&beta; & &gamma;)) 
     * &equiv; 
     * ((&alpha; | &beta;) & (&alpha; | &gamma;)) 
     */
    public class DistributeOrOverAnd : AbstractPLVisitor<object> {

	    /**
	     * Distribuye o's (|) sobre y's (&).
	     * 
	     * @param sentence
	     *            una sentencia en lógica proposicional. A esta sentencia se le debe haber quitado bicondicionales, implicaciones y las negaciones tienen que haberse llevado hacia dentro. 
	     * @return una sentencia equivalente a la de entrada con las o'es distribuidas por las y'es. 
	     */
	    public static Sentence Distribute(Sentence sentence) {
		    Sentence result = null;

		    DistributeOrOverAnd distributeOrOverAnd = new DistributeOrOverAnd();
		    result = sentence.Accept(distributeOrOverAnd, null);

		    return result;
	    }

	    public override Sentence VisitBinarySentence(ComplexSentence s, object arg) {
		    Sentence result = null;

		    if (s.IsOrSentence()) {
			    Sentence s1 = s.GetSimplerSentence(0).Accept(this, arg);
			    Sentence s2 = s.GetSimplerSentence(1).Accept(this, arg);
			    if (s1.IsAndSentence() || s2.IsAndSentence()) {
				    Sentence alpha, betaAndGamma;
				    if (s2.IsAndSentence()) {
					    // (&alpha; | (&beta; & &gamma;))
					    // Nota: incluso si ambos son sentencias 'and' preferiremos usar s2
					    alpha = s1;
					    betaAndGamma = s2;
				    } else {
					    // Nota: Hace falta manejar este caso también 
					    // ((&beta; & &gamma;) | &alpha;)
					    alpha = s2;
					    betaAndGamma = s1;
				    }

				    Sentence beta = betaAndGamma.GetSimplerSentence(0);
				    Sentence gamma = betaAndGamma.GetSimplerSentence(1);

				    if (s2.IsAndSentence()) {
					    // ((&alpha; | &beta;) & (&alpha; | &gamma;))
					    Sentence alphaOrBeta = (new ComplexSentence(Connective.OR,
							    alpha, beta)).Accept(this, null);
					    Sentence alphaOrGamma = (new ComplexSentence(Connective.OR,
							    alpha, gamma)).Accept(this, null);

					    result = new ComplexSentence(Connective.AND, alphaOrBeta,
							    alphaOrGamma);
				    } else {
					    // ((&beta; | &alpha;) & (&gamma; | &alpha;))
					    Sentence betaOrAlpha = (new ComplexSentence(Connective.OR,
							    beta, alpha)).Accept(this, null);
					    Sentence gammaOrAlpha = (new ComplexSentence(Connective.OR,
							    gamma, alpha)).Accept(this, null);

					    result = new ComplexSentence(Connective.AND, betaOrAlpha,
							    gammaOrAlpha);
				    }
			    } else {
				    result = new ComplexSentence(Connective.OR, s1, s2);
			    }
		    } else {
			    result = base.VisitBinarySentence(s, arg);
		    }

		    return result;
	    }
    }
}
