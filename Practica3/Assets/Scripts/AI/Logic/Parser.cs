/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic {

    /*
    import java.io.Reader;
    import java.io.StringReader;
    */
    using System.IO;

    /**
     * Una clase base abstracta para construir analizadores sintácticos para los lenguajes de representación de conocimiento.
     * Proporciona mecanismos para convertir una secuencia de tokens (obtenida con un analizador léxico apropiado) en un árbol de sintaxis abstracta sintácticamente correcto que representa el lenguaje.
     * 
     * @param <S> el tipo raíz del árbol de sintaxis abstracta que está siendo analizado.
     */
    public abstract class Parser<S> {

        protected int lookAheadBufferSize = 1;
        //
        private Token[] lookAheadBuffer = null;

        /**
         * Devuelve un ejemplar del analizador léxito para ser usado con una implementación concreta de esta clase.
         * @return un ejemplar del analizador léxito para ser usado con una implementación concreta de esta clase.
         */
        public abstract Lexer GetLexer();

        /**
         * Analiza la sintaxis concreta de la entrada en forma de un árbol sintáctico abstracto.
         * 
         * @param input
         *            una cadena representando la sintaxis concreta a ser analizada.
         * @return el nodo raíz de una representación de árbol de sintaxis abstracta de la sintaxis de entrada concreta que fue analizada.
         */
        public S Parse(string input) {
            return Parse(new StringReader(input)); // StringReader (quizá debería tenerlo ya creado y no hacer new aquí cada vez...)
        }

        /**
         * Analiza la sintaxis concreta de la entrada en forma de un árbol sintáctico abstracto.
         * 
         * @param inputReader
         *            un Reader para la sintaxis concreta que va a ser analizada.
         * @return el nodo raíz de una representación de árbol de sintaxis abstracta de la sintaxis de entrada concreta que fue analizada.
         */
        public S Parse(TextReader inputReader) {
            S result = default(S); // Por si S fuese un tipo que no aceptase nulls

            try {
                GetLexer().SetInput(inputReader);  
                InitializeLookAheadBuffer(); // Esto no se hace ya dentro del lexer?

                result = Parse();
            } catch (LexerException le) {
                throw new ParserException("Lexer Exception thrown during parsing at position " + le.GetCurrentPositionInInputExceptionThrown(), le); // Meto le, la excepción causante
            }

            return result;
        }

        //
        // PROTECTED
        //

        /**
         * Será implementado luego por las implementaciones concretas de esta clase.
         * 
         * @return el nodo raíz de una representación de árbol de sintaxis abstracta de la sintaxis de entrada concreta que fue analizada.
         */
        protected abstract S Parse();

        /**
         * Devolver el token en la posición específica en el almacén de antelación (lookahead buffer).
         */
        protected Token LookAhead(int i) {
            return lookAheadBuffer[i - 1];
        }

        /**
         * Consume 1 token de la entrada.
         */
        protected void Consume() {
            LoadNextTokenFromInput();
        }

        /**
         * Consume el símbolo correspondiente si se corresponde con el token de entrada actual.
         * Si no se corresponde lanza una excepción ParserException detalladno el error de correspondencia.
         * 
         * @param toMatchSymbol
         *            el símbolo con el que corresponderse antes de consumirlo.
         */
        protected void Match(string toMatchSymbol) {
            if (LookAhead(1).GetText().Equals(toMatchSymbol)) {
                Consume();
            } else {
                throw new ParserException(
                        "Parser: Syntax error detected at match. Expected "
                                + toMatchSymbol + " but got "
                                + LookAhead(1).GetText(), LookAhead(1));
            }
        }

        //
        // PRIVATE
        //
        private void InitializeLookAheadBuffer() {
            lookAheadBuffer = new Token[lookAheadBufferSize];
            for (int i = 0; i < lookAheadBufferSize; i++) {
                // Ahora rellena el buffer (si es posible) desde la entrada.
                lookAheadBuffer[i] = GetLexer().NextToken();
                if (IsEndOfInput(lookAheadBuffer[i])) {
                    // La entrada es más pequeña que el tamaño del almacén
                    break;
                }
            }
        }

        /*
         * Carga el siguiente token en el almacén de anticipación si no se ha alcanzado todavía el final del flujo.
         */
        private void LoadNextTokenFromInput() {
            bool eoiEncountered = false;
            for (int i = 0; i < lookAheadBufferSize - 1; i++) {
                lookAheadBuffer[i] = lookAheadBuffer[i + 1];
                if (IsEndOfInput(lookAheadBuffer[i])) {
                    eoiEncountered = true;
                    break;
                }
            }
            if (!eoiEncountered) {
                lookAheadBuffer[lookAheadBufferSize - 1] = GetLexer().NextToken();
            }
        }

        /*
         * Devuelve cierto si el final del flujo ha sido alcanzado.
         */
        private bool IsEndOfInput(Token t) {
            return (t == null || t.GetTokenType() == LogicTokenTypes.EOI);
        }
    }
}