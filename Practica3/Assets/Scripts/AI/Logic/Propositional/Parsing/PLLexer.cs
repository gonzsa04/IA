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

    using System.Text;

    /**
     * Una implementación concreta de un analizador léxico para el lenguaje proposicional. 
     */
    public class PLLexer : Lexer {
	
	    /**
	     * Constructor por defecto.
	     */
	    public PLLexer() {
	    }

	    /**
	     * Construye un analizador léxico de expresiones proposicionales para el flujo de caracteres (de entrada) específico.
	     * 
	     * @param inputString
	     *            una secuencia de caracteres que serán convertidos en una secuencia de tokens.
	     */
	    public PLLexer(string inputString) {
		    SetInput(inputString);
	    }

        /**
	     * Devuelve el siguiente token proposicional para el flujo de caracteres 
	     * 
	     * @return el siguiente token proposicional del flujo de caracteres.
	     */ 
        public override Token NextToken() {
		    int startPosition = GetCurrentPositionInInput();
		    if (LookAhead(1) == '(') {
			    Consume();
			    return new Token(LogicTokenTypes.LPAREN, "(", startPosition);
		    } else if (LookAhead(1) == '[') {
				    Consume();
				    return new Token(LogicTokenTypes.LSQRBRACKET, "[", startPosition);
		    } else if (LookAhead(1) == ')') {
			    Consume();
			    return new Token(LogicTokenTypes.RPAREN, ")", startPosition);
		    } else if (LookAhead(1) == ']') {
			    Consume();
			    return new Token(LogicTokenTypes.RSQRBRACKET, "]", startPosition);
		    } else if (char.IsWhiteSpace(LookAhead(1))) {
			    Consume();
			    return NextToken();
		    } else if (ConnectiveDetected(LookAhead(1))) {
			    return Connective();
		    } else if (SymbolDetected(LookAhead(1))) {
			    return Symbol();
		    } else if (LookAhead(1) == (char) 0xFFFF) { // (char) -1 .... creo que a lo mejor el caracter equivalente es el 0xFFFF
                return new Token(LogicTokenTypes.EOI, "EOI", startPosition);
		    } else {
			    throw new LexerException("Lexing error on character " + LookAhead(1) + " at position " + GetCurrentPositionInInput(), GetCurrentPositionInInput());
		    }
	    }

        // Faltan por poner muchos comentarios aquí...

	    private bool ConnectiveDetected(char leadingChar) {
		    return Parsing.Connective.IsConnectiveIdentifierStart(leadingChar);
	    }
	
	    private bool SymbolDetected(char leadingChar) {
		    return PropositionSymbol.IsPropositionSymbolIdentifierStart(leadingChar);
	    }
	
        // Faltan comentarios
	    private Token Connective() {
		    int startPosition = GetCurrentPositionInInput();
		    StringBuilder sbuf = new StringBuilder(); // StringBuffer en Java... lo hemos modernizado a StringBuilder
		    // Hay que asegurarse de sacar sólo una conectiva cada vez, la prueba ade isConnective(...) nos aseguram que manejamos expresiones encadenadas como por ejemplo ~~P
		    while (Parsing.Connective.IsConnectiveIdentifierPart(LookAhead(1)) && !IsConnective(sbuf.ToString())) {
			    sbuf.Append(LookAhead(1));
                Consume();
		    }
		
		    string symbol = sbuf.ToString();
		    if (IsConnective(symbol)) {
			    return new Token(LogicTokenTypes.CONNECTIVE, sbuf.ToString(), startPosition);
		    }
		
		    throw new LexerException("Lexing error on connective " + symbol + " at position " + GetCurrentPositionInInput(), GetCurrentPositionInInput());
	    }

	    private Token Symbol() {
		    int startPosition = GetCurrentPositionInInput();
            StringBuilder sbuf = new StringBuilder(); // StringBuffer en Java lo hemos modernizado a StringBuilder
            while (PropositionSymbol.IsPropositionSymbolIdentifierPart(LookAhead(1))) {
			    sbuf.Append(LookAhead(1));
			    Consume();
		    }
		    string symbol = sbuf.ToString();
		    if (PropositionSymbol.IsAlwaysTrueSymbol(symbol)) {
			    return new Token(LogicTokenTypes.TRUE, PropositionSymbol.TRUE_SYMBOL, startPosition);
		    } else if (PropositionSymbol.IsAlwaysFalseSymbol(symbol)) {
			    return new Token(LogicTokenTypes.FALSE, PropositionSymbol.FALSE_SYMBOL, startPosition);
		    } else if (PropositionSymbol.IsPropositionSymbol(symbol)){
			    return new Token(LogicTokenTypes.SYMBOL, sbuf.ToString(), startPosition);
		    }
		
		    throw new LexerException("Lexing error on symbol " + symbol + " at position "+ GetCurrentPositionInInput(), GetCurrentPositionInInput());
	    }

	    private bool IsConnective(string aSymbol) {
		    return Parsing.Connective.IsConnective(aSymbol);
	    }
    }
}