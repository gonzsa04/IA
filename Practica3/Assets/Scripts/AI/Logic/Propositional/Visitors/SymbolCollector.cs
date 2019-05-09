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

    using System.Collections.Generic;

    using UCM.IAV.AI.Logic.Propositional.Parsing; 

    /**
     * Clase de utilidad para recoger símbolos proposicionales de sentencias. Excluirá los símbolos que son siempre falsos o siempre ciertos.
     */
    public class SymbolCollector : BasicGatherer<PropositionSymbol> {

	    /**
	     * Recoge un conjunto de símbolos proposicionales de una lista de sentencias dadas.
	     * 
	     * @param sentences
	     *            una lista de sentencias en la que recoger símbolos.
	     * @return un conjunto de todos los símbolos de proposición que no son siempre ciertos o falsos contenidos en las sentencias de entrada. 
	     */
	    public static ISet<PropositionSymbol> GetSymbolsFrom(params Sentence[] sentences) {
		    ISet<PropositionSymbol> result = new HashSet<PropositionSymbol>(); // LinkedHashSet

		    SymbolCollector symbolCollector = new SymbolCollector();
		    foreach (Sentence s in sentences) 
			    result = s.Accept(symbolCollector, result);

		    return result;
	    }

	    public override ISet<PropositionSymbol> VisitPropositionSymbol(PropositionSymbol s, ISet<PropositionSymbol> arg) {
		    // No se añaden los símbolos siempre cierto o siempre falso
		    if (!s.IsAlwaysTrue() && !s.IsAlwaysFalse()) {
			    arg.Add(s);
		    }
		    return arg;
	    }
    }
}
