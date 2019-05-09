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
    using UCM.IAV.AI.Logic.Propositional.KB;
    using UCM.IAV.AI.Logic.Propositional.Parsing;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 253. 
     * Una expresión de sentencia en forma de conjunción de cláusulas se dice que está en forma normal conjuntiva o CNF (Conjunctive Normal Form).
     * 
     * CNFSentence -> Clause_1 & ... & Clause_n
     *      Clause -> Literal_1 | ... | Literal_m
     *     Literal -> Symbol : ~Symbol
     *      Symbol -> P : Q : R : ... // (1)
     * 
     * Una gramática para una forma normal conjuntiva. 
     * 
     * Nota: Mientras que en el libro se dice que se usan símbolos que comienzan con una letra mayúscula y pueden contener otras letras o subscripts, 
     * en esta implementación permitimos cualquier identificador legal en Java (ahora C#) para representar un símbolo de proposición. 
     */
    public class ConvertToConjunctionOfClauses {

        /**
         * Devuelve la sentencia especificada en su equivalente lógica de forma de conjunción de cláusulas. 
         * 
         * @param s
         *            una sentencia de lógica proposicional
         * 
         * @return la sentencia de entrada convertida en su equivalente lógica de forma de conjunción de cláusulas. 
         */
        public static ConjunctionOfClauses Convert(Sentence s) {
            ConjunctionOfClauses result = null;

            Sentence cnfSentence = ConvertToCNF.Convert(s);

            IList<Clause> clauses = new List<Clause>(); //ArrayList
            foreach (var c in ClauseCollector.GetClausesFrom(cnfSentence))
                clauses.Add(c);

            result = new ConjunctionOfClauses(clauses);

            return result;
        }
    }
}