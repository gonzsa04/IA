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
    import javax.lang.model.SourceVersion;
    */
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 244.
     * Símbolo de proposición: Cada uno de estos símbolos mantiene una proposición que puede ser cierta o falsa.
     * Existen dos símbolos de proposición con significados fijos: True, que es la proposición siempre-cierta y False, que es la siempre-falsa. 
     * 
     * Nota: Mientras que en el libro se dice que se usarán símbolos que comiencen con una letra mayúscula y que puedan contener otras letras, 
     * en esta implementación se permite a todo identificador legal (de Java o bueno, ahora de C#) formar parte de una proposición. 
     */
    public class PropositionSymbol : AtomicSentence {
        //
        public static readonly string TRUE_SYMBOL = "True"; // public static final es ahora public static readonly
        public static readonly string FALSE_SYMBOL = "False";
        public static readonly PropositionSymbol TRUE = new PropositionSymbol(TRUE_SYMBOL);
        public static readonly PropositionSymbol FALSE = new PropositionSymbol(FALSE_SYMBOL);
        //
        private string symbol;

        /**
	     * Constructor.
	     * 
	     * @param symbol
	     *            el símbolo que identifica unívocamente la proposición. 
	     */
        public PropositionSymbol(string symbol) {
            // Se asegura de que casos diferentes para las constantes proposicionales 'True' y 'False' se representan de una forma canónica.
            if (string.Equals(symbol, TRUE_SYMBOL, StringComparison.OrdinalIgnoreCase)) { //equalsIgnoreCase                
                this.symbol = TRUE_SYMBOL;
            } else if (string.Equals(symbol, FALSE_SYMBOL, StringComparison.OrdinalIgnoreCase)) {
                this.symbol = FALSE_SYMBOL;
            } else if (IsPropositionSymbol(symbol)) {
                this.symbol = symbol;
            } else {
                throw new ArgumentException("Not a legal proposition symbol: " + symbol); //IllegalArgumentException
            }
        }

        /**
         * Devuelve cierto si este es el símbolo de proposición siempre 'True', falso en otro caso.
         * @return cierto si este es el símbolo de proposición siempre 'True', falso en otro caso.
         */
        public bool IsAlwaysTrue() {
            return TRUE_SYMBOL.Equals(symbol);
        }

        /**
        * Devuelve cierto si el símbolo que se le pasa es el símbolo de proposición de siempre 'True', falso en otro caso.
        * @return cierto si el símbolo que se le pasa es el símbolo de proposición de siempre 'True', falso en otro caso.
        */
        public static bool IsAlwaysTrueSymbol(string symbol) {
            return string.Equals(symbol, TRUE_SYMBOL, StringComparison.OrdinalIgnoreCase);  
        }

        /**
            * Devuelve cierto si este es el símbolo de proposición siempre 'False', falso en otro caso.
            * @return cierto si este es el símbolo de proposición siempre 'False', falso en otro caso.
            */
        public bool IsAlwaysFalse() {
            return FALSE_SYMBOL.Equals(symbol);
        }

        /**
        * Devuelve cierto si el símbolo que se le pasa es el símbolo de proposición de siempre 'False', falso en otro caso.
        * @return cierto si el símbolo que se le pasa es el símbolo de proposición de siempre 'False', falso en otro caso.
        */
        public static bool IsAlwaysFalseSymbol(string symbol) {
            return string.Equals(symbol, FALSE_SYMBOL, StringComparison.OrdinalIgnoreCase);
        }

        /**
	     * Determina si el símbolo dado es un símbolo de proposición legal.
	     * 
	     * @param symbol
	     *            un símbolo para comprobarlo.
	     * @return cierto si el símbolo dado es un símbolo de proposición legal, falso en caso contrario. 
	     */
        public static bool IsPropositionSymbol(string symbol) {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#"); // En Java era SourceVersion.isIdentifier(symbol);
            return provider.IsValidIdentifier(symbol); 
        }

        /**
        * Determina si el carácter dado puede estar al comienzo de un símbolo de proposición. 
        * 
        * @param ch
        *            un carácter.
        * @return cierto si el carácter dado puede estar al comienzo de un símbolo de proposición, falso en caso contrario. 
        */
        public static bool IsPropositionSymbolIdentifierStart(char ch) {
            // En lugar de Character.isJavaIdentifierStart(ch) pongo todo esto... que es una manera de ver si el carácter suelto ya sería un identificador válido... ejem, no es exactamente lo mismo pero bueno

            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#"); // Ver si puedo evitar hacer esta llamada cada vez
            return provider.IsValidIdentifier(ch.ToString());  // Imagino que el carácter se convierte a cadena trivialmente, si hace falta con un new string
        }


        /**
            * Determina si el carácter dado puede formar parte de una representación de símbolo de proposición.
            * 
            * @param ch
            *            un carácter.
            * @return cierto si el carácter dado puede formar parte de una representación de símbolo de proposición, falso en caso contrario. 
            */
        public static bool IsPropositionSymbolIdentifierPart(char ch) {
            // En lugar de Character.isJavaIdentifierStart(ch) pongo todo esto... que es una manera de ver si el carácter tras una simple '_' ya sería un identificador válido... ejem, no es exactamente lo mismo pero bueno
            // Anécdota: si haces que lo primero sea una simple 'a' pueden formarse frases como 'as'... ¡¡que NO son identificadores válidos por ser palabras reservadas del lenguaje!!!
            char[] chars = { '_', ch };
            string s = new string(chars);
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#"); // Ver si puedo evitar hacer esta llamada cada vez
            return provider.IsValidIdentifier(s);
        }

        /**
	     * Devuelve el símbolo que identifica unívocamente la proposición.
	     * @return el símbolo que identifica unívocamente la proposición.
	     */
        public string GetSymbol() {
            return symbol;
        }

        // Compara este símbolo de proposición con otro y dice si son iguales
        public bool Equals(PropositionSymbol sym) {
            if (sym == null)
                return false;
            if (this == sym)
                return true;
            return symbol.Equals(sym.symbol); // No equivocarse con esto que había: sym.Equals(sym.symbol)
        }

        // Compara este símbolo de proposición con otro objeto y dice si son iguales 
        public override bool Equals(object obj) {
            if (obj is PropositionSymbol) // is en vez de instanceof (y mejor que typeof que obligaría a ser del tipo exacto)
                return this.Equals(obj as PropositionSymbol);
            return false;
        }

        // Devuelve código hash del símbolo (para optimizar el acceso en colecciones y así)
        // No debe contener bucles, tiene que ser muy rápida
        public override int GetHashCode() {
            return symbol.GetHashCode();
        }

        // Cadena de texto representativa  
        public override string ToString() {
            return GetSymbol();
        }
    }
}