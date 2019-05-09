/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic {

    using System.IO;

    /**
     * Una clase base abstracta para construir analizadores l�xicos para lenguajes de representaci�n de conocimiento.
     * Proporciona un mecanismo para convertir una secuencia de caracters en una secuencia de tokens que sean significativos en el lenguaje de representaci�n de inter�s. 
     */
    public abstract class Lexer {

        // Algunas de estas ponerlas como propiedades...
        protected int lookAheadBufferSize = 1; // Tama�o del almac�n de anticipaci�n
        //
        // Se usa el -1 como c�digo ASCII que no existe para delimitar el final de la cadena
        private static readonly int END_OF_INPUT = -1; //redonly en vez de final 
        //
        private TextReader input; // Reader en Java, ahora TextReader (que es m�s gen�rico que StringReader o StreamReader). Quiz� podr�a usarse tambi�n Reader, para caracteres sin codificar...
        private int[] lookAheadBuffer; // de int en vez de char
        private int currentPositionInInput; 

        /**
         * Establece el flujo de caracteres del analizador l�xico.
         * 
         * @param inputReader
         *            un TextReader (que podr�a ser incluso un StreamReader) sobre una secuencia de caracteres para ser convertidas en una secuencia de tokens
         */
         // Este mismo m�todo 
        public void SetInput(TextReader inputReader) {
            input = inputReader;
            // Inicializamos todo
            lookAheadBuffer = new int[lookAheadBufferSize];
            currentPositionInInput = 0;
            InitializeLookAheadBuffer();
        }

        /**
         * Establece el flujo de caracteres del analizador l�xico.
         * 
         * @param inputString
         *            una secuencia de caracteres para ser convertidas en una secuencia de tokens
         */
        public void SetInput(string inputString) {
            SetInput(new StringReader(inputString)); // StringReader porque es a�n m�s espec�fico que TextReader
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
         * Devuelve el car�cter en la posici�n espec�fica del almac�n de los siguientes caracteres.
         */
        protected char LookAhead(int position) {
            return (char)lookAheadBuffer[position - 1]; // Se asume que me van a acceder a la posici�n 1... curiosamente... y yo debo dar lo primero del array
        }

        /**
         * Consume 1 car�cter de la entrada.
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
         * Inicializa el almac�n de los siguientes caracteres para la entrada.
         */
        private void InitializeLookAheadBuffer() {
            for (int i = 0; i < lookAheadBufferSize; i++) {
                // Marca el almac�n entero como el final de la entrada 
                lookAheadBuffer[i] = END_OF_INPUT;
            }
            for (int i = 0; i < lookAheadBufferSize; i++) {
                // Ahora rellena el almac�n (si es posible) desde la entrada
                lookAheadBuffer[i] = ReadInput();
                if (IsEndOfInput(lookAheadBuffer[i])) {
                    // La entrada es m�s peque�a que el tama�o del almac�n
                    break;
                }
            }
        }

        /**
         * Carga el siguiente car�cter en el almac�n de los siguientes caracteres si todav�a no se ha alcanzado el final del flujo. 
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

        // Faltan comentarios por aqu�
        private int ReadInput() {
            int read = -1;

            try {
                read = input.Read();
            } catch (IOException ioe) {  
                throw new LexerException("IOException thrown reading input.", currentPositionInInput, ioe); // Meto ioe, la excepci�n causante...  
            }

            return read;
        }
    }
}