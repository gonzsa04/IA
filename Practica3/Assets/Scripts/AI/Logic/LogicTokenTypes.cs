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

    // En lugar de un interfaz, en C# las constantes se ponen en una clase estática
    public static class LogicTokenTypes {

        public static readonly int SYMBOL = 1;

        public static readonly int LPAREN = 2; // (

        public static readonly int RPAREN = 3; // )

        public static readonly int LSQRBRACKET = 4; // [

        public static readonly int RSQRBRACKET = 5; // ]

        public static readonly int COMMA = 6;

        public static readonly int CONNECTIVE = 7;

        public static readonly int QUANTIFIER = 8;

        public static readonly int PREDICATE = 9;

        public static readonly int FUNCTION = 10;

        public static readonly int VARIABLE = 11;

        public static readonly int CONSTANT = 12;

        public static readonly int TRUE = 13;

        public static readonly int FALSE = 14;

        public static readonly int EQUALS = 15;

        public static readonly int WHITESPACE = 1000;

        public static readonly int EOI = 9999; // End of Input.
    }
}