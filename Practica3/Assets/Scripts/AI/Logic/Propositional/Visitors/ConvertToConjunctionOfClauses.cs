/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Visitors {
    using System.Collections.Generic;
    using UCM.IAV.AI.Logic.Propositional.KB;
    using UCM.IAV.AI.Logic.Propositional.Parsing;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 253. 
     * Una expresi�n de sentencia en forma de conjunci�n de cl�usulas se dice que est� en forma normal conjuntiva o CNF (Conjunctive Normal Form).
     * 
     * CNFSentence -> Clause_1 & ... & Clause_n
     *      Clause -> Literal_1 | ... | Literal_m
     *     Literal -> Symbol : ~Symbol
     *      Symbol -> P : Q : R : ... // (1)
     * 
     * Una gram�tica para una forma normal conjuntiva. 
     * 
     * Nota: Mientras que en el libro se dice que se usan s�mbolos que comienzan con una letra may�scula y pueden contener otras letras o subscripts, 
     * en esta implementaci�n permitimos cualquier identificador legal en Java (ahora C#) para representar un s�mbolo de proposici�n. 
     */
    public class ConvertToConjunctionOfClauses {

        /**
         * Devuelve la sentencia especificada en su equivalente l�gica de forma de conjunci�n de cl�usulas. 
         * 
         * @param s
         *            una sentencia de l�gica proposicional
         * 
         * @return la sentencia de entrada convertida en su equivalente l�gica de forma de conjunci�n de cl�usulas. 
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