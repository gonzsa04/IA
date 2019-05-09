/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
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
     * S�mbolo de proposici�n: Cada uno de estos s�mbolos mantiene una proposici�n que puede ser cierta o falsa.
     * Existen dos s�mbolos de proposici�n con significados fijos: True, que es la proposici�n siempre-cierta y False, que es la siempre-falsa. 
     * 
     * Nota: Mientras que en el libro se dice que se usar�n s�mbolos que comiencen con una letra may�scula y que puedan contener otras letras, 
     * en esta implementaci�n se permite a todo identificador legal (de Java o bueno, ahora de C#) formar parte de una proposici�n. 
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
	     *            el s�mbolo que identifica un�vocamente la proposici�n. 
	     */
        public PropositionSymbol(string symbol) {
            // Se asegura de que casos diferentes para las constantes proposicionales 'True' y 'False' se representan de una forma can�nica.
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
         * Devuelve cierto si este es el s�mbolo de proposici�n siempre 'True', falso en otro caso.
         * @return cierto si este es el s�mbolo de proposici�n siempre 'True', falso en otro caso.
         */
        public bool IsAlwaysTrue() {
            return TRUE_SYMBOL.Equals(symbol);
        }

        /**
        * Devuelve cierto si el s�mbolo que se le pasa es el s�mbolo de proposici�n de siempre 'True', falso en otro caso.
        * @return cierto si el s�mbolo que se le pasa es el s�mbolo de proposici�n de siempre 'True', falso en otro caso.
        */
        public static bool IsAlwaysTrueSymbol(string symbol) {
            return string.Equals(symbol, TRUE_SYMBOL, StringComparison.OrdinalIgnoreCase);  
        }

        /**
            * Devuelve cierto si este es el s�mbolo de proposici�n siempre 'False', falso en otro caso.
            * @return cierto si este es el s�mbolo de proposici�n siempre 'False', falso en otro caso.
            */
        public bool IsAlwaysFalse() {
            return FALSE_SYMBOL.Equals(symbol);
        }

        /**
        * Devuelve cierto si el s�mbolo que se le pasa es el s�mbolo de proposici�n de siempre 'False', falso en otro caso.
        * @return cierto si el s�mbolo que se le pasa es el s�mbolo de proposici�n de siempre 'False', falso en otro caso.
        */
        public static bool IsAlwaysFalseSymbol(string symbol) {
            return string.Equals(symbol, FALSE_SYMBOL, StringComparison.OrdinalIgnoreCase);
        }

        /**
	     * Determina si el s�mbolo dado es un s�mbolo de proposici�n legal.
	     * 
	     * @param symbol
	     *            un s�mbolo para comprobarlo.
	     * @return cierto si el s�mbolo dado es un s�mbolo de proposici�n legal, falso en caso contrario. 
	     */
        public static bool IsPropositionSymbol(string symbol) {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#"); // En Java era SourceVersion.isIdentifier(symbol);
            return provider.IsValidIdentifier(symbol); 
        }

        /**
        * Determina si el car�cter dado puede estar al comienzo de un s�mbolo de proposici�n. 
        * 
        * @param ch
        *            un car�cter.
        * @return cierto si el car�cter dado puede estar al comienzo de un s�mbolo de proposici�n, falso en caso contrario. 
        */
        public static bool IsPropositionSymbolIdentifierStart(char ch) {
            // En lugar de Character.isJavaIdentifierStart(ch) pongo todo esto... que es una manera de ver si el car�cter suelto ya ser�a un identificador v�lido... ejem, no es exactamente lo mismo pero bueno

            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#"); // Ver si puedo evitar hacer esta llamada cada vez
            return provider.IsValidIdentifier(ch.ToString());  // Imagino que el car�cter se convierte a cadena trivialmente, si hace falta con un new string
        }


        /**
            * Determina si el car�cter dado puede formar parte de una representaci�n de s�mbolo de proposici�n.
            * 
            * @param ch
            *            un car�cter.
            * @return cierto si el car�cter dado puede formar parte de una representaci�n de s�mbolo de proposici�n, falso en caso contrario. 
            */
        public static bool IsPropositionSymbolIdentifierPart(char ch) {
            // En lugar de Character.isJavaIdentifierStart(ch) pongo todo esto... que es una manera de ver si el car�cter tras una simple '_' ya ser�a un identificador v�lido... ejem, no es exactamente lo mismo pero bueno
            // An�cdota: si haces que lo primero sea una simple 'a' pueden formarse frases como 'as'... ��que NO son identificadores v�lidos por ser palabras reservadas del lenguaje!!!
            char[] chars = { '_', ch };
            string s = new string(chars);
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#"); // Ver si puedo evitar hacer esta llamada cada vez
            return provider.IsValidIdentifier(s);
        }

        /**
	     * Devuelve el s�mbolo que identifica un�vocamente la proposici�n.
	     * @return el s�mbolo que identifica un�vocamente la proposici�n.
	     */
        public string GetSymbol() {
            return symbol;
        }

        // Compara este s�mbolo de proposici�n con otro y dice si son iguales
        public bool Equals(PropositionSymbol sym) {
            if (sym == null)
                return false;
            if (this == sym)
                return true;
            return symbol.Equals(sym.symbol); // No equivocarse con esto que hab�a: sym.Equals(sym.symbol)
        }

        // Compara este s�mbolo de proposici�n con otro objeto y dice si son iguales 
        public override bool Equals(object obj) {
            if (obj is PropositionSymbol) // is en vez de instanceof (y mejor que typeof que obligar�a a ser del tipo exacto)
                return this.Equals(obj as PropositionSymbol);
            return false;
        }

        // Devuelve c�digo hash del s�mbolo (para optimizar el acceso en colecciones y as�)
        // No debe contener bucles, tiene que ser muy r�pida
        public override int GetHashCode() {
            return symbol.GetHashCode();
        }

        // Cadena de texto representativa  
        public override string ToString() {
            return GetSymbol();
        }
    }
}