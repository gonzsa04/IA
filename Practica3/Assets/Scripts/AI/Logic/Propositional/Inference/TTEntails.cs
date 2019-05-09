/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Inference {

    using System.Collections.Generic;

    using UCM.IAV.AI.Util;
    using UCM.IAV.AI.Logic.Propositional.Parsing;
    using UCM.IAV.AI.Logic.Propositional.KB;
    using UCM.IAV.AI.Logic.Propositional.Visitors;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 7.10, page 248. 
     *
     * function TT-ENTAILS?(KB, &alpha;) returns true or false
     *   inputs: KB, the knowledge base, a sentence in propositional logic
     *           &alpha;, the query, a sentence in propositional logic
     *           
     *   symbols <- a list of proposition symbols in KB and &alpha
     *   return TT-CHECK-ALL(KB, &alpha; symbols, {})
     *   
     * 
     * function TT-CHECK-ALL(KB, &alpha; symbols, model) returns true or false
     *   if EMPTY?(symbols) then
     *     if PL-TRUE?(KB, model) then return PL-TRUE?(&alpha;, model)
     *     else return true // when KB is false, always return true
     *   else do
     *     P <- FIRST(symbols)
     *     rest <- REST(symbols)
     *     return (TT-CHECK-ALL(KB, &alpha;, rest, model &cup; { P = true })
     *            and
     *            TT-CHECK-ALL(KB, &alpha;, rest, model &cup; { P = false }))
     * 
     * Un algoritmo de enumeraci�n de la tabla de verdad para decidir la consecuenci� l�gica proposicional. (lo de TT viene de tabla de verdad en ingl�s, truth table).
     * PL-TRUE? devuelve cierto si una sentencia se satisface dentro de un modelo. La variable modelo representa un modelo parcial - una asignaci�n para algunos de los s�mbolos.
     * La palabra clave "and" se usa aqu� como una operaci�n l�gica sobre dos argumentos, devolviendo cierto o falso.
     */
    public class TT_Entails { // No podemos llamar a la clase igual que a uno de sus m�todos...

        /**
         * La funci�n TT-ENTAILS?(KB, &alpha;) devuelve cierto o falso.
         * 
         * @param kb
         *            KB, la base de conocimiento, una sentencia en l�gica proposicional
         * @param alpha
         *            &alpha;, la consulta, una sentencia en l�gica proposicional
         * 
         * @return cierto si &alpha; es consecuencia l�gica de KB, falso en otro caso.
         */
        public bool TTEntails(KnowledgeBase kb, Sentence alpha) {
            // symbols <- una lista de s�mbolos de proposici�n de KB y &alpha
            IList<PropositionSymbol> symbols = new List<PropositionSymbol>(SymbolCollector.GetSymbolsFrom(kb.AsSentence(), alpha));

            // return TT-CHECK-ALL(KB, &alpha; symbols, {})
            return TTCheckAll(kb, alpha, symbols, new Model());
        }

        /**
         * La funci�n TT-CHECK-ALL(KB, &alpha; symbols, model) devuelve cierto o falso
         * 
         * @param kb
         *            KB, la base de conocimiento, una sentencia en l�gica proposicional
         * @param alpha
         *            &alpha;, la consulta, una sentencia en l�gica proposicional
         * @param symbols
         *            una lista de s�mbolos proposicionales no asignados actualmente en el modelo 
         * @param model
         *            una asignaci�n del modelo parcial o completa para la BC dada y la consulta.
         * @return cierto si &alpha; es consecuencia l�gica de KB, falso en otro caso.
         */
        public bool TTCheckAll(KnowledgeBase kb, Sentence alpha, IList<PropositionSymbol> symbols, Model model) {
            // si EMPTY?(symbols) entonces
            if (symbols.Count == 0) {
                // si PL-TRUE?(KB, model) entonces return PL-TRUE?(&alpha;, model)
                if (model.IsTrue(kb.AsSentence())) {
                    return model.IsTrue(alpha);
                } else {
                    // en caso contrario devolver cierto (cuando la KB es falsa, SIEMPRE se devuelve cierto)
                    return true;
                }
            }

            // en caso contrario hacer
            // P <- FIRST(symbols)
            PropositionSymbol p = Util.First(symbols);
            // rest <- REST(symbols)
            IList<PropositionSymbol> rest = Util.Rest(symbols);
            // return (TT-CHECK-ALL(KB, &alpha;, rest, model &cup; { P = true })
            // and
            // TT-CHECK-ALL(KB, &alpha;, rest, model U { P = false }))
            var checkIfTrue = TTCheckAll(kb, alpha, rest, model.Union(p, true));
            var checkIfFalse = TTCheckAll(kb, alpha, rest, model.Union(p, false));
            return checkIfTrue && checkIfFalse;
        }
    }
}