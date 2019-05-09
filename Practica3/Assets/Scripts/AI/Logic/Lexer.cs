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

    using System.IO;

    /**
     * Una clase base abstracta para construir analizadores léxicos para lenguajes de representación de conocimiento.
     * Proporciona un mecanismo para convertir una secuencia de caracters en una secuencia de tokens que sean significativos en el lenguaje de representación de interés. 
     */
    public abstract class Lexer {

        // Algunas de estas ponerlas como propiedades...
        protected int lookAheadBufferSize = 1; // Tamaño del almacén de anticipación
        //
        // Se usa el -1 como código ASCII que no existe para delimitar el final de la cadena
        private static readonly int END_OF_INPUT = -1; //redonly en vez de final 
        //
        private TextReader input; // Reader en Java, ahora TextReader (que es más genérico que StringReader o StreamReader). Quizá podría usarse también Reader, para caracteres sin codificar...
        private int[] lookAheadBuffer; // de int en vez de char
        private int currentPositionInInput; 

        /**
         * Establece el flujo de caracteres del analizador léxico.
         * 
         * @param inputReader
         *            un TextReader (que podría ser incluso un StreamReader) sobre una secuencia de caracteres para ser convertidas en una secuencia de tokens
         */
         // Este mismo método 
        public void SetInput(TextReader inputReader) {
            input = inputReader;
            // Inicializamos todo
            lookAheadBuffer = new int[lookAheadBufferSize];
            currentPositionInInput = 0;
            InitializeLookAheadBuffer();
        }

        /**
         * Establece el flujo de caracteres del analizador léxico.
         * 
         * @param inputString
         *            una secuencia de caracteres para ser convertidas en una secuencia de tokens
         */
        public void SetInput(string inputString) {
            SetInput(new StringReader(inputString)); // StringReader porque es aún más específico que TextReader
        }

        /**
         * Para ser implementados por implementaciones concretas
         * 
         * @return el siguiente token de la entrada.
         */
        public abstract Token NextToken();

        //
        // PROTECTED
        //
        protected int GetCurrentPositionInInput() {
            return currentPositionInInput;
        }

        /*
         * Devuelve el carácter en la posición específica del almacén de los siguientes caracteres.
         */
        protected char LookAhead(int position) {
            return (char)lookAheadBuffer[position - 1]; // Se asume que me van a acceder a la posición 1... curiosamente... y yo debo dar lo primero del array
        }

        /**
         * Consume 1 carácter de la entrada.
         */
        protected void Consume() {
            currentPositionInInput++;
            LoadNextCharacterFromInput();
        }

        //
        // PRIVATE
        //

        /**
         * Devuelve cierto si se ha alcanzado el final del flujo.
         */
        private bool IsEndOfInput(int i) {
            return (i == END_OF_INPUT);
        }

        /**
         * Inicializa el almacén de los siguientes caracteres para la entrada.
         */
        private void InitializeLookAheadBuffer() {
            for (int i = 0; i < lookAheadBufferSize; i++) {
                // Marca el almacén entero como el final de la entrada 
                lookAheadBuffer[i] = END_OF_INPUT;
            }
            for (int i = 0; i < lookAheadBufferSize; i++) {
                // Ahora rellena el almacén (si es posible) desde la entrada
                lookAheadBuffer[i] = ReadInput();
                if (IsEndOfInput(lookAheadBuffer[i])) {
                    // La entrada es más pequeña que el tamaño del almacén
                    break;
                }
            }
        }

        /**
         * Carga el siguiente carácter en el almacén de los siguientes caracteres si todavía no se ha alcanzado el final del flujo. 
         */
        private void LoadNextCharacterFromInput() {
            bool eoiEncountered = false;
            for (int i = 0; i < lookAheadBufferSize - 1; i++) {
                lookAheadBuffer[i] = lookAheadBuffer[i + 1];
                if (IsEndOfInput(lookAheadBuffer[i])) {
                    eoiEncountered = true;
                    break;
                }
            }
            if (!eoiEncountered) {
                lookAheadBuffer[lookAheadBufferSize - 1] = ReadInput();
            }
        }

        // Faltan comentarios por aquí
        private int ReadInput() {
            int read = -1;

            try {
                read = input.Read();
            } catch (IOException ioe) {  
                throw new LexerException("IOException thrown reading input.", currentPositionInInput, ioe); // Meto ioe, la excepción causante...  
            }

            return read;
        }
    }
}