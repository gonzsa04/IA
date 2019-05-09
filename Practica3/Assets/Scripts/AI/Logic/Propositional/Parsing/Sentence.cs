/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Parsing {

    using System;
    using System.Linq;
    using System.Collections.Generic;

    using UCM.IAV.AI.Util; // Util
    using UCM.IAV.AI.Logic; // ParseTreeNode

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 244. 
     * La base del lenguaje de representación de conocimiento de la Lógica Proposicional.
     * Nota: Esta jerarquía de clases define la representación sintáctica abstracta utilizada para representar Lógica Proposicional.
     */
    public abstract class Sentence : ParseTreeNode {

        /**
	     * Devuelve la conectiva lógica asociada con esta sentencia si tiene una (es decir, si es una sentencia compleja), null en caso contrario
	     * @return la conectiva lógica asociada con esta sentencia si tiene una (es decir, si es una sentencia compleja), null en caso contrario
	     */
        // Es virtual para que pueda ser sobreescrito
        public virtual Connective GetConnective() {
		    return null;
	    }

        /**
	     * Devuelve el número de sentencias más simples contenidas en esta sentencia.
         * Sólo será > 0 si es una sentencia compleja.
	     * @return el número de sentencias más simples contenidas en esta sentencia.
	     */
        // Es virtual para que pueda ser sobreescrito
        public virtual int GetNumberSimplerSentences() {
		    return 0;
	    }

        /**
	     * Devuelve la sentencia simplificada, según el desplazamiento especificado (comienza en 0), contenida en esta sentencia si es una sentencia compleja, null en caso contrario. 
	     * 
	     * @param offset
	     *            el desplazamiento especificado de la sentencia simplificada contenida que hay que devolver
	     * @return la sentencia simplificada, según el desplazamiento especificado (comienza en 0), contenida en esta sentencia si es una sentencia compleja, null en caso contrario. 
	     */
        // Es virtual para que pueda ser sobreescrito
        public virtual Sentence GetSimplerSentence(int offset) {
		    return null;
	    }

        /**
	     * Devuelve cierto si es una sentencia compleja con una conectiva Not, falso en caso contrario.
	     * @return cierto si es una sentencia compleja con una conectiva Not, falso en caso contrario.
	     */
        public bool IsNotSentence() {
		    return HasConnective(Connective.NOT);
	    }

	    /**
	     * Devuelve cierto si es una sentencia compleja con una conectiva And, falso en caso contrario. 
	     */
	    public bool IsAndSentence() {
		    return HasConnective(Connective.AND);
	    }

	    /**
	     * Devuelve cierto si es una sentencia compleja con una conectiva Or, falso en caso contrario. 
	     */
	    public bool IsOrSentence() {
		    return HasConnective(Connective.OR);
	    }

        /**
	     * Devuelve cierto si es una sentencia compleja con una conectiva Implicación, falso en caso contrario. 
	     * @return cierto si es una sentencia compleja con una conectiva Implicación, falso en caso contrario. 
	     */
        public bool IsImplicationSentence() {
		    return HasConnective(Connective.IMPLICATION);
	    }

        /**
	     * Devuelve cierto si es una sentencia compleja con una conectiva Bicondicional, falso en caso contrario. 
	     * @return cierto si es una sentencia compleja con una conectiva Bicondicional, falso en caso contrario. 
	     */
        public bool IsBiconditionalSentence() {
		    return HasConnective(Connective.BICONDITIONAL);
	    }

        /**
	     * Devuelve cierto si es un símbolo de proposición, falso en caso contrario.
	     * @return cierto si es un símbolo de proposición, falso en caso contrario.
	     */
        public bool IsPropositionSymbol() {
		    return GetConnective() == null;
	    }

        /**
	     * Devuelve cierto si es una sentencia compleja que contiene una única sentencia más simple, falso en caso contrario.
	     * @return cierto si es una sentencia compleja que contiene una única sentencia más simple, falso en caso contrario.
	     */
        public bool IsUnarySentence() {
		    return HasConnective(Connective.NOT);
	    }

        /**
	     * Devuelve cierto si es una sentencia compleja que contiene dos sentencia más simple, falso en caso contrario.
	     * @return cierto si es una sentencia compleja que contiene dos sentencia más simple, falso en caso contrario.
	     */
        public bool IsBinarySentence() {
		    return GetConnective() != null && !HasConnective(Connective.NOT);
	    }

	    /**
	     * Permite a un PLVisitor recorrer el árbol de sintaxis abstracta representado por esta sentencia. 
	     * 
	     * @param plv
	     *            un visitante de Lógica Proposicional.
	     * @param arg
	     *            un argumento opcional que puede ser usado por el visitante.
	     * @return un resultado específico del comportamiento del visitante. 
	     */
	    public R Accept<A, R>(PLVisitor<A, R> plv, A arg) { // Tupla de Java ponía <A, R> R ... hay que poner los tipos genéricos en el nombre del método también (y lo que se devuelve es R no Tuple<A, R>)
            R result = default(R); // Por si R no admite nulos
		    if (IsPropositionSymbol()) {
			    result = plv.VisitPropositionSymbol((PropositionSymbol) this, arg);
		    } else if (IsUnarySentence()) {
			    result = plv.VisitUnarySentence((ComplexSentence) this, arg);
		    } else if (IsBinarySentence()) {
			    result = plv.VisitBinarySentence((ComplexSentence) this, arg);
		    }

		    return result;
	    }

	    /**
	     * Rutina de utilidad que creará una representación en forma de cadena de una sentencia dada y la colocará entre paréntesis si es una sentencia compleja de menor precedencia que esta sentencia compleja. 
	     * Nota: Esta es una forma de imprimir más bonita, donde sólo añadimos paréntesis en la representación de la sintaxis concreta si se necesitan 
         * para asegurarnos que se puede parsear de vuelta en la representación de sintaxix abstracta equivalente que usamos aquí. 
	     * 
	     * @param parentConnective
	     *            la conectiva de la sentencia padre.
	     * @param childSentence
	     *            una sentencia hija más simple.
	     * @return una representación en forma de cadena de la sentencia, entre paréntesis si el padre tiene mayor precedencia basándonos en su conectiva. 
	     */
	    public string BracketSentenceIfNecessary(Connective parentConnective, Sentence childSentence) {
		    string result = null;
		    if (childSentence is ComplexSentence) {
			    ComplexSentence cs = (ComplexSentence) childSentence;
			    if (cs.GetConnective().Precedence < parentConnective.Precedence) {
				    result = "(" + childSentence + ")";
			    }
		    }

		    if (result == null) {
			    result = childSentence.ToString();
		    }

		    return result;
	    }
	
	    /**
	     * Crea una disyunción de disyunciones.
	     * @param disjuncts
	     * 			las disyunciones con las que crear la disyunción.
	     * @return una disyunción de las disyunciones dadas.
	     */
	    public static Sentence NewDisjunction(params Sentence[] disjuncts) {
            return NewDisjunction(disjuncts.ToList<Sentence>()); // ToList<Sentence>(), usando LINQ, en vez de Arrays.asList
        }

        /**
	     * Crea una disyunción de disyunciones.
	     * @param disjuncts
	     * 			las disyunciones con las que crear la disyunción.
	     * @return una disyunción de las disyunciones dadas.
	     */
        public static Sentence NewDisjunction(IList<Sentence> disjuncts)  { //List<? extends Sentence>   lo podría sustituir por List<T> where T: Sentence .... creo que de todas formas funcionará si pongo directamente IList<Sentence>
            if (disjuncts.Count == 0) {
			    return PropositionSymbol.FALSE;
		    }
		    else if (disjuncts.Count == 1) {
			    return disjuncts[0]; // Tal vez se acceda de otra manera
		    }
		    return new ComplexSentence(Util.First(disjuncts), Connective.OR, NewDisjunction(Util.Rest(disjuncts)));		
	    }

        /**
	     * Crea una conjunción de conjunciones.
	     * @param conjuncts
	     * 			las conjunciones con las que crear la conjunción.
	     * @return una conjunción de las conjunciones dadas.
	     */
        public static Sentence NewConjunction(params Sentence[] conjuncts) {
		    return NewConjunction(conjuncts.ToList<Sentence>()); // ToList(), usando LINQ, en vez de Arrays.asList
        }

        /**
	     * Crea una conjunción de conjunciones.
	     * @param conjuncts
	     * 			las conjunciones con las que crear la conjunción.
	     * @return una conjunción de las conjunciones dadas.
	     */
        public static Sentence NewConjunction(IList<Sentence> conjuncts) { //List<? extends Sentence>   lo he sustituido por List<T> where T: Sentence ... aún así creo qu e IList<Sentence> directamente funcionará
            if (conjuncts.Count == 0) {
			    return PropositionSymbol.TRUE;
		    }
		    else if (conjuncts.Count == 1) {
			    return conjuncts[0]; // Tal vez se acceda de otra manera
            }
		    return new ComplexSentence(Util.First(conjuncts), Connective.AND, NewConjunction(Util.Rest(conjuncts)));		
	    }
	
	    //
	    // PROTECTED
	    //
	    protected bool HasConnective(Connective connective) {
		    // Nota: se puede usar '==' ya que Connective es un enumerado.
		    return GetConnective() == connective;
	    }
    }
}