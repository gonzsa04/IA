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
    using System.Collections.Generic;

    using UCM.IAV.AI.Logic.Propositional.Parsing;
    using UCM.IAV.AI.Logic.Propositional.KB;

    /**
     * Clase de utilidad para recoplar cláusulas de sentencias CNF. 
     */
    public class ClauseCollector : BasicGatherer<Clause> {

	    /**
	     * Recopila un conjunto de cláusulas de una lista de sentencias dada.
	     * 
	     * @param cnfSentences
	     *            una lista de sentencias CNF de donde vamos a recopilar cláusulas.
	     * @return un conjunto de todas las cláusulas contenidas.
	     * @throws ArgumentException
	     *             si cualquiera de las sentencias dadas no está en CNF
	     */
	    public static ISet<Clause> GetClausesFrom(params Sentence[] cnfSentences) {
		    ISet<Clause> result = new HashSet<Clause>(); //LinkedHashSet

            ClauseCollector clauseCollector = new ClauseCollector();
		    foreach (Sentence cnfSentence in cnfSentences) 			
			    result = cnfSentence.Accept(clauseCollector, result);

		    return result;
	    }
	
	    public override ISet<Clause> VisitPropositionSymbol(PropositionSymbol s, ISet<Clause> arg) {
		    // una cláusula unidad positiva
		    Literal positiveLiteral = new Literal(s);
		    arg.Add(new Clause(positiveLiteral));
		
		    return arg;
	    }
	
	    public override ISet<Clause> VisitUnarySentence(ComplexSentence s, ISet<Clause> arg) {
		
		    if (!s.GetSimplerSentence(0).IsPropositionSymbol()) {
			    throw new InvalidOperationException("Sentence is not in CNF: "+s); //IllegalStateException
            }
		
		    // a negative unit clause
		    Literal negativeLiteral = new Literal((PropositionSymbol)s.GetSimplerSentence(0), false);
		    arg.Add(new Clause(negativeLiteral));
		
		    return arg;
	    }
	 
	    public override ISet<Clause> VisitBinarySentence(ComplexSentence s, ISet<Clause> arg) {
		
		    if (s.IsAndSentence()) {
			    s.GetSimplerSentence(0).Accept(this, arg);
			    s.GetSimplerSentence(1).Accept(this, arg);			
		    } else if (s.IsOrSentence()) {
			    IList<Literal> literals = new List<Literal>(LiteralCollector.getLiterals(s)); //ArrayList
                arg.Add(new Clause(literals));			
		    } else {
			    throw new ArgumentException("Sentence is not in CNF: "+s); //IllegalArgumentException
            }
		
		    return arg;
	    }
    }
}
