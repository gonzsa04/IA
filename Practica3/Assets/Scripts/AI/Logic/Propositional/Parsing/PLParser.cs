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
    /*
    import javax.lang.model.SourceVersion; No recuerdo para qué era esto...
    */
    using System.Collections.Generic;

    /**
    * Artificial Intelligence A Modern Approach (3rd Edition): Figure 7.7, page 244.
    * 
    * Implementación de un analizador sintáctico de lógica proposicional basado en: 
    * Sentence        -> AtomicSentence : ComplexStence
    * AtomicSentence  -> True : False : P : Q : R : ... // (1)
    * ComplexSentence -> (Sentence) | [Sentence]
    *                 :  ~Sentence
    *                 :  Sentence & Sentence
    *                 :  Sentence | Sentence
    *                 :  Sentence => Sentence
    *                 :  Sentence <=> Sentence
    * 
    * OPERATOR PRECEDENCE: ~, &, |, =>, <=> // (2)
    * 
    * En el AIMA essto es Figure 7.7 A BNF (Backus-Naur Form) grammar of sentences in propositional logic, along with operator precedences, from highest to lowest. 
    * 
    * Nota (1): Aunque el libro dice que se usarán símbolos que comiencen con una letra mayúscula y que pueden contener otras letras o subscripts,
    * en esta implementación ser permite que cualquier identificador válido de Java (ahora C#) pueda ser un símbolo de proposición. 
    * 
    * Nota (2): Esta implementación es asociativa por la derecha (que es algo que tiende a ser más intuitivo para este lenguaje), por ejemplo:
    * A & B & C & D 
    * se analizaría sintácticamente como:
    * (A & (B & (C & D)))
    */
    public class PLParser : Parser<Sentence> {

        private PLLexer lexer = new PLLexer();

        /**
	     * Constructor por defecto.
	     */
        public PLParser() {
        }

        public override Lexer GetLexer() {
	        return lexer;
        }

        //
        // PROTECTED
        //
        protected override Sentence Parse() {
	        Sentence result = null;

	        ParseNode root = ParseSentence(0);
	        if (root != null && root.node is Sentence) { //instanceof
		        result = (Sentence) root.node; // O root.node as Sentence
	        }

	        return result;
        }

        //
        // PRIVATE
        //
        private ParseNode ParseSentence(int level) { // Sentence en singular... ¿y con el nivel que quieran?
	        IList<ParseNode> levelParseNodes = ParseLevel(level);

	        ParseNode result = null;

	        // Ahora se agrupan los tokens basándonos en el orden de precedencia de más alto a más bajo. 
	        levelParseNodes = GroupSimplerSentencesByConnective(Connective.NOT, levelParseNodes);
	        levelParseNodes = GroupSimplerSentencesByConnective(Connective.AND, levelParseNodes);
	        levelParseNodes = GroupSimplerSentencesByConnective(Connective.OR,  levelParseNodes);
	        levelParseNodes = GroupSimplerSentencesByConnective(Connective.IMPLICATION, levelParseNodes);
	        levelParseNodes = GroupSimplerSentencesByConnective(Connective.BICONDITIONAL, levelParseNodes);

	        // En este punto sólo debería haber la fórmula raíz para este nivel. 
	        if (levelParseNodes.Count == 1  && levelParseNodes[0].node is Sentence) { //instanceof
		        result = levelParseNodes[0];
	        } else {
		        // Si no se identifica una sentencia raíz para este nivel, entonces se lanza una excepción indicando el problema. 
		        throw new ParserException("Unable to correctly parse sentence: " + levelParseNodes, GetTokens(levelParseNodes));
	        }

	        return result;
        }


        // Faltan por poner muchos comentarios aquí...

        private IList<ParseNode> GroupSimplerSentencesByConnective(
		        Connective connectiveToConstruct, IList<ParseNode> parseNodes) {
	        IList<ParseNode> newParseNodes = new List<ParseNode>(); // En vez de ArrayList, una List normal
	        int numSentencesMade = 0;
	        // Ir desde la derecha a la izquierda en orden para hacer asociatividad a la derecha, que es lo más natural para tener por defecto en lógica proposicional. 
	        for (int i = parseNodes.Count - 1; i >= 0; i--) {
		        ParseNode parseNode = parseNodes[i];
		        if (parseNode.node is Connective) {
			        Connective tokenConnective = (Connective) parseNode.node;
			        if (tokenConnective == Connective.NOT) {
				        // A unary connective
				        if (i + 1 < parseNodes.Count && parseNodes[i + 1].node is Sentence) {
					        if (tokenConnective == connectiveToConstruct) {
						        ComplexSentence newSentence = new ComplexSentence(
								        connectiveToConstruct,
								        (Sentence) parseNodes[i + 1].node);
						        parseNodes[i] = new ParseNode(newSentence, parseNode.token);
						        parseNodes[i + 1] = null;
						        numSentencesMade++;
					        }
				        } else {
					        throw new ParserException(
							        "Unary connective argurment is not a sentence at input position "
									        + parseNode.token.GetStartCharPositionInInput(),
							        parseNode.token);
				        }
			        } else {
				        // A Binary connective
				        if ((i - 1 >= 0 && parseNodes[i - 1].node is Sentence)
						        && (i + 1 < parseNodes.Count && parseNodes[i + 1].node is Sentence)) {
					        // A binary connective
					        if (tokenConnective == connectiveToConstruct) {
						        ComplexSentence newSentence = new ComplexSentence(
								        connectiveToConstruct,
								        (Sentence) parseNodes[i - 1].node,
								        (Sentence) parseNodes[i + 1].node);
						        parseNodes[i - 1] = new ParseNode(newSentence, parseNode.token);
						        parseNodes[i] = null;
						        parseNodes[i + 1] = null;
						        numSentencesMade++;
					        }
				        } else {
					        throw new ParserException(
							        "Binary connective argurments are not sentences at input position "
									        + parseNode.token
											        .GetStartCharPositionInInput(),
							        parseNode.token);
				        }
			        }
		        }
	        }

	        for (int i = 0; i < parseNodes.Count; i++) {
		        ParseNode parseNode = parseNodes[i];
		        if (parseNode != null) {
			        newParseNodes.Add(parseNode);
		        }
	        }

	        // Se asegura de que no quedan tokens sin contar en esta pasada.
	        int toSubtract = 0;
	        if (connectiveToConstruct == Connective.NOT) {
		        toSubtract = (numSentencesMade * 2) - numSentencesMade;
	        } else {
		        toSubtract = (numSentencesMade * 3) - numSentencesMade;
	        }

	        if (parseNodes.Count - toSubtract != newParseNodes.Count) {
		        throw new ParserException(
				        "Unable to construct sentence for connective: "
						        + connectiveToConstruct + " from: " + parseNodes,
				        GetTokens(parseNodes));
	        }

	        return newParseNodes;
        }

        private IList<ParseNode> ParseLevel(int level) {
	        IList<ParseNode> tokens = new List<ParseNode>(); // Nada de ArrayList

	        while (LookAhead(1).GetTokenType() != LogicTokenTypes.EOI
			        && LookAhead(1).GetTokenType() != LogicTokenTypes.RPAREN
			        && LookAhead(1).GetTokenType() != LogicTokenTypes.RSQRBRACKET) {
		        if (DetectConnective()) {
			        tokens.Add(ParseConnective());
		        } else if (DetectAtomicSentence()) {
			        tokens.Add(ParseAtomicSentence());
		        } else if (DetectBracket()) {
			        tokens.Add(ParseBracketedSentence(level));
		        }
	        }

	        if (level > 0 && LookAhead(1).GetTokenType() == LogicTokenTypes.EOI) {
		        throw new ParserException(
				        "Parser Error: end of input not expected at level " + level, LookAhead(1));
	        }

	        return tokens;
        }

        private bool DetectConnective() {
	        return LookAhead(1).GetTokenType() == LogicTokenTypes.CONNECTIVE;
        }

        private ParseNode ParseConnective() {
	        Token token = LookAhead(1);
	        Connective connective = Connective.Get(token.GetText());
	        Consume();
	        return new ParseNode(connective, token);
        }

        private bool DetectAtomicSentence() {
	        int type = LookAhead(1).GetTokenType();
	        return type == LogicTokenTypes.TRUE || type == LogicTokenTypes.FALSE
			        || type == LogicTokenTypes.SYMBOL;
        }

        private ParseNode ParseAtomicSentence() {
	        Token t = LookAhead(1);
	        if (t.GetTokenType() == LogicTokenTypes.TRUE) {
		        return ParseTrue();
	        } else if (t.GetTokenType() == LogicTokenTypes.FALSE) {
		        return ParseFalse();
	        } else if (t.GetTokenType() == LogicTokenTypes.SYMBOL) {
		        return ParseSymbol();
	        } else {
		        throw new ParserException(
				        "Error parsing atomic sentence at position "
						        + t.GetStartCharPositionInInput(), t);
	        }
        }

        private ParseNode ParseTrue() {
	        Token token = LookAhead(1);
	        Consume();
	        return new ParseNode(new PropositionSymbol(PropositionSymbol.TRUE_SYMBOL), token);
        }

        private ParseNode ParseFalse() {
	        Token token = LookAhead(1);
	        Consume();
	        return new ParseNode(new PropositionSymbol(PropositionSymbol.FALSE_SYMBOL), token);
        }

        private ParseNode ParseSymbol() {
	        Token token = LookAhead(1);
	        string sym = token.GetText();
	        Consume();
	        return new ParseNode(new PropositionSymbol(sym), token);
        }

        private bool DetectBracket() {
	        return LookAhead(1).GetTokenType() == LogicTokenTypes.LPAREN
			        || LookAhead(1).GetTokenType() == LogicTokenTypes.LSQRBRACKET;
        }

        private ParseNode ParseBracketedSentence(int level) {
	        Token startToken = LookAhead(1);

	        string start = "(";
	        string end = ")";
	        if (startToken.GetTokenType() == LogicTokenTypes.LSQRBRACKET) {
		        start = "[";
		        end = "]";
	        }

	        Match(start);
	        ParseNode bracketedSentence = ParseSentence(level + 1);
	        Match(end);

	        return bracketedSentence;
        }

        private Token[] GetTokens(IList<ParseNode> parseNodes) {
	        Token[] result = new Token[parseNodes.Count];

	        for (int i = 0; i < parseNodes.Count; i++) {
		        result[i] = parseNodes[i].token;
	        }

	        return result;
        }


        // Clase privada interna
        private class ParseNode {
	        public object node = null;
	        public Token token = null;

	        public ParseNode(object node, Token token) {
		        this.node = node;
		        this.token = token;
	        }
            // Cadena de texto representativa  
            public override string ToString() {
		        return node.ToString() + " at " + token.GetStartCharPositionInInput();
	        }
        }
    }
}