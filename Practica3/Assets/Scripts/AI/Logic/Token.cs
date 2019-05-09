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

    /**
     * Un token generado por un analizador léxico desde una secuencia de caracteres.
     */
    public class Token {

        private int type;
        private string text;
        private int startCharPositionInInput;

        /**
         * Construye un token con el nombre de token especificado y el atributo-valor
         * 
         * @param type
         *            el nombre de token
         * @param text
         *            the atributo-valor
         * @param startCharPositionInInput
         *            la posición (empezando desde 0) en la cual este token comienza en la entrada 
         */
        public Token(int type, string text, int startCharPositionInInput) {
            this.type = type;
            this.text = text;
            this.startCharPositionInInput = startCharPositionInInput;
        }

        /**
         * Devuelve el atributo-valor de este token.
         * 
         * @return el atributo-valor de este token.
         */
        public string GetText() {
            return text;
        }

        /**
         * Devuelve el nombre de token de este token.
         * 
         * @return el nombre de token de este token.
         */
        public int GetTokenType() { // No lo podemos llamar igual que el GetType de object
            return type;
        }

        /**
         * @return la posición (empezando desde 0) en la cual este token comienza en la entrada 
         */
        public int GetStartCharPositionInInput() {
            return startCharPositionInInput;
        }


        // Compara este token con otro y dice si son iguales
        public bool Equals(Token t) {
            if (t == null)
                return false;
            if (this == t)
                return true;
            return ((t.type == type) && (t.text.Equals(text)) && (t.startCharPositionInInput == startCharPositionInInput));
        }

        // Compara este token con otro objeto y dice si son iguales 
        public override bool Equals(object obj) {
            if (obj is Token) // is en vez de instanceof (y mejor que typeof que obligaría a ser del tipo exacto)
                return this.Equals(obj as Token);
            return false;
        }
               
        // Devuelve código hash del literal (para optimizar el acceso en colecciones y así)
        // No debe contener bucles, tiene que ser muy rápida
        public override int GetHashCode() {
            // Aquí no cacheamos este valor...
            int result = 17;
            result = 37 * result + type;
            result = 37 * result + text.GetHashCode();
            result = 37 * result + startCharPositionInInput;
            return result;
        }

        // Cadena de texto representativa  
        public override string ToString() {
            // Aquí no cacheamos este valor...
            return "[ " + type + " " + text + " " + startCharPositionInInput + " ]";
        }
    }
}
