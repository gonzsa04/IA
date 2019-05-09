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
    using System.Collections.Generic;

    using UCM.IAV.AI.Util;  // Para el Util createSet

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 244. 
     * Conectivas lógicas existen 5 de uso común:
     * 1. ~   (no).
     * 2. &   (y).
     * 3. |   (o).
     * 4. =>  (implicación).
     * 5. <=> (bicondicional).
     * 
     * Nota: Utilizamos caracteres ASCII que generalmente tienen el mismo significado que aquellos símbolos usados en el libro del AIMA.
     * 
     * PRECEDENCIA DE OPERADORES: ~, &, |, =>, <=>
     */

    // Se podría recrear un enum de Java con un enum de C# + extension methods + custom attributes como aquí:
    // https://stackoverflow.com/questions/469287/c-sharp-vs-java-enum-for-those-new-to-c/4778347#4778347
    // Estaría el enumerado propiamente dicho, al que se añaden los custom attributes y allí los extension methods
    //
    // Pero se opta por dejar una clase inmutable con los ejemplares estáticos

    // El enumerado convertido en una clase         
    public class Connective {
        // es decir, las ordena de mayor a menor precedencia
        public static readonly Connective NOT = new Connective("~", 10);
        public static readonly Connective AND = new Connective("&", 8);
        public static readonly Connective OR = new Connective("|", 6); 
        public static readonly Connective IMPLICATION = new Connective("=>", 4); 
        public static readonly Connective BICONDITIONAL = new Connective("<=>", 2);

        // Esto podría ser privado, tal vez
        public string Symbol { get; private set; }
        public int Precedence { get; private set; }

        // No sé si esto deberá seguir estando aquí y en privado...
        private static readonly ISet<Char> _connectiveLeadingChars = Util.CreateSet('~', '&', '|', '=', '<');
        private static readonly ISet<Char> _connectiveChars = Util.CreateSet('~', '&', '|', '=', '<', '>');

        // Constructor de una conectiva, con un símbolo y una precedencia
        public Connective(string symbol, int precedence) {

                this.Symbol = symbol;
                this.Precedence = precedence;
        }

        // Cadena de texto representativa  
        public override string ToString() {
            return Symbol;
        }


        // Y estos métodos de aquí abajo ya serían más en plan factoría para tenerlos quizá en otra clase de Util o algo así

        /**
	        * Determina si un símbolo dado representa una conectiva.
	        * 
	        * @param symbol
	        *            un símbolo para comprobar si representa o no a una conectiva. 
	        * @return cierto si el símbolo dado representa una conectiva.
	        */
        public static bool IsConnective(string symbol) {
		    if (NOT.Symbol.Equals(symbol)) {
			    return true;
		    } else if (AND.Symbol.Equals(symbol)) {
			    return true;
		    } else if (OR.Symbol.Equals(symbol)) {
			    return true;
		    } else if (IMPLICATION.Symbol.Equals(symbol)) {
			    return true;
		    } else if (BICONDITIONAL.Symbol.Equals(symbol)) {
			    return true;
		    }
		    return false;
	    }

	    /**
	        * Devolver la conectiva asociada con la representación simbólica dada.
	        * 
	        * @param symbol
	        *            un símbolo para el que se pide la conectiva correspondiente.
	        * @return la conectiva asociada con un símbolo dado.
	        * @throws ArgumentException
	        *             si no se puede encontrar una conectiva que se corresponda con el símbolo dado. 
	        */
	    public static Connective Get(string symbol) {
		    if (NOT.Symbol.Equals(symbol)) {
			    return NOT;
		    } else if (AND.Symbol.Equals(symbol)) {
			    return AND;
		    } else if (OR.Symbol.Equals(symbol)) {
			    return OR;
		    } else if (IMPLICATION.Symbol.Equals(symbol)) {
			    return IMPLICATION;
		    } else if (BICONDITIONAL.Symbol.Equals(symbol)) {
			    return BICONDITIONAL;
		    }

		    throw new ArgumentException( //IllegalArgumentException
                    "Not a valid symbol for a connective: " + symbol);
	    }

	    /**
	        * Determina si el carácter dado puede ser el comienzo de una conectiva.
	        * 
	        * @param ch
	        *            un personaje.
	        * @return cierto si el carácter dado puede ser el comienzo de la representación simbólica de una conectiva, falso en otro caso. 
	        */
	    public static bool IsConnectiveIdentifierStart(char ch) {
		    return _connectiveLeadingChars.Contains(ch);
	    }

	    /**
	        * Determina si el personaje dado es parte de una conectiva.
	        * 
	        * @param ch
	        *            un personaje.
	        * @return cierto si el personaje dado es parte de la representación simbólica de una conectiva, falso en otro caso. 
	        */
	    public static bool IsConnectiveIdentifierPart(char ch) {
		    return _connectiveChars.Contains(ch);
	    }
    }
}
