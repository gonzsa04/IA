/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.KB {

    using System.Collections.Generic;
    using UCM.IAV.AI.Logic.Propositional.Parsing;
    using UCM.IAV.AI.Logic.Propositional.Inference;
    using UCM.IAV.AI.Logic.Propositional.Visitors;

    /**
     * Base de conocimiento (BC) espec�fica para trabajar con L�gica Proposicional.
     * El nombre podr�a ser PLKnowledgeBase.
     */
    public class KnowledgeBase {

        private IList<Sentence> sentences = new List<Sentence>(); // Se implementa con List en vez de ArrayList de Java, se guarda en IList en vez de List
        private ConjunctionOfClauses _asCNF = new ConjunctionOfClauses(new Clause[0]); //Se usa un array vac�o new Clause[0] en vez de Collections.emptySet()
        private ISet<PropositionSymbol> symbols = new HashSet<PropositionSymbol>(); // ISet en vez de Set, y HashSet en vez de LinkedHashSet... aunque una alternativa interesante que me dar�a las ventajas de la lista enlazada ser�a crear mi propio LinkedHashSet de C#: https://stackoverflow.com/questions/9346526/what-is-the-equivalent-of-linkedhashset-java-in-c
        private PLParser parser = new PLParser();

        /**
	     * A�ade la sentencia espec�fica a la BC. 
	     * 
	     * @param aSentence
	     *            hecho para ser a�adido a la BC.
	     */
        public void Tell(string aSentence) {
            Tell((Sentence)parser.Parse(aSentence));
        }

        /**
	     * A�ade la sentencia espec�fica a la BC. 
	     * 
	     * @param aSentence
	     *            hecho para ser a�adido a la BC.
	     */
        public void Tell(Sentence aSentence) {
            if (!(sentences.Contains(aSentence))) {
                sentences.Add(aSentence);
                _asCNF = _asCNF.Extend(ConvertToConjunctionOfClauses.Convert(aSentence).GetClauses());
                symbols.UnionWith(SymbolCollector.GetSymbolsFrom(aSentence)); // UnionWith de ISet en vez del m�todo addAll de Set
            }
        }

        /**
	     * Cada vez que se llama al programa del agente, este le DICE a la BC lo que percibe 
	     * 
	     * @param percepts
	     *            lo que el agente percibe
	     */
        public void TellAll(string[] percepts) {
            foreach (string percept in percepts) {
                Tell(percept);
            }
        }

        /**
	     * Devuelve el n�mero de sentencias en la BC. 
	     * 
	     * @return el n�mero de sentencias en la BC. 
	     */
        public int Count() { // Deber�a ser una property...
            return sentences.Count; // Count de IList en vez de size de List
        }

        /**
	     * Devuelve la lista de sentencias en la BC encadenadas como una �nica sentencia. 
	     * 
	     * @return la lista de sentencias en la BC encadenadas como una �nica sentencia.
	     */
        public Sentence AsSentence() {
            return Sentence.NewConjunction(sentences);
        }

        /**
	     * Devuelve una representaci�n en Forma Normal Conjuntiva (CNF) de la BC.
         * 
	     * @return una representaci�n en Forma Normal Conjuntiva (CNF) de la BC.
	     */
        public ISet<Clause> AsCNF() {
            return _asCNF.GetClauses();
        }

        /**
	     * Devuelve el conjunto �nico de los s�mbolos actualmente contenidos en la BC.
         * 
	     * @return el conjunto �nico de los s�mbolos actualmente contenidos en la BC.
	     */
        public ISet<PropositionSymbol> GetSymbols() { // Tal vez otra property
            return symbols;
        }

        /**
	     * Devuelve la respuesta a la consulta espec�fica usando el algoritmo TT-Entails.
	     * 
	     * @param queryString
	     *            una consulta para CONSULTAR a la BC 
	     * 
	     * @return la respuesta a la consulta espec�fica usando el algoritmo TT-Entails.
	     */
        public bool AskWithTTEntails(string queryString) {
            PLParser parser = new PLParser();

            Sentence alpha = parser.Parse(queryString);

            return new TT_Entails().TTEntails(this, alpha);
        }

        /**
	     * Devuelve la lista de sentencias de la BC.
	     * 
	     * @return la lista de sentencias de la BC.
	     */
        public IList<Sentence> GetSentences() {
            return sentences;
        }

        // Cadena de texto representativa  
        public override string ToString() {
            return sentences.Count == 0 ? "" : AsSentence().ToString(); // Count == 0 en vez de isEmpty()
        }
    }
}