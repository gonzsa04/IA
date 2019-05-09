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


	// Era una clase privada y estática dentro de ClauseCollector, pero la hemos sacado a fuera en C#
	public class LiteralCollector : BasicGatherer<Literal> {
		
        // Era privado, pero al sacar esta clase fuera ahora es público
		public static ISet<Literal> getLiterals(Sentence disjunctiveSentence) {
			ISet<Literal> result = new HashSet<Literal>();
			
			LiteralCollector literalCollector = new LiteralCollector();
			result = disjunctiveSentence.Accept(literalCollector, result);
			
			return result;
		}
		
		public override ISet<Literal> VisitPropositionSymbol(PropositionSymbol s, ISet<Literal> arg) {
			// un literal positivo
			Literal positiveLiteral = new Literal(s);
			arg.Add(positiveLiteral);
			
			return arg;
		}
		 
		public override ISet<Literal> VisitUnarySentence(ComplexSentence s, ISet<Literal> arg) {
			
			if (!s.GetSimplerSentence(0).IsPropositionSymbol()) {
				throw new InvalidOperationException("Sentence is not in CNF: "+s); //IllegalStateException
            }
			
			// un literal negativo 
			Literal negativeLiteral = new Literal((PropositionSymbol)s.GetSimplerSentence(0), false);

			arg.Add(negativeLiteral);
			
			return arg;
		}
		
		public override ISet<Literal> VisitBinarySentence(ComplexSentence s, ISet<Literal> arg) {
			if (s.IsOrSentence()) {
				s.GetSimplerSentence(0).Accept(this, arg);
				s.GetSimplerSentence(1).Accept(this, arg);
			} else {
				throw new ArgumentException("Sentence is not in CNF: "+s); //IllegalArgumentException
            }
			return arg;
		}
	} 
}
