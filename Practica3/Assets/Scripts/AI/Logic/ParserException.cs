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

    using System;
    using System.Collections.Generic;

    /**
     * A runtime exception to be used to describe Parser exceptions. In particular
     * it provides information to help in identifying which tokens proved
     * problematic in the parse.  
     */
    public class ParserException : Exception { // En vez de RuntimeException

	    //private static readonly long serialVersionUID = 1L; // Creo que en C# esto ya no hace falta

	    private IList<Token> problematicTokens = new List<Token>(); // En vez de ArrayList

        public ParserException(string message, params Token[] problematicTokens) : base(message) { 
		    if (problematicTokens != null) 
			    foreach (Token pt in problematicTokens) 
				    this.problematicTokens.Add(pt);
	    }
        
        // El Throwable cause de Java viene a ser la InnerException (o incluso la GetBaseException, que es la que está en la raíz) de C# 
        public ParserException(string message, Exception inner, params Token[] problematicTokens) : base(message, inner) {
            if (problematicTokens != null)
                foreach (Token pt in problematicTokens)
                    this.problematicTokens.Add(pt);
        }

        /**
	     * Devuelve una lista de 0 o más tokens del flujo de entrada que se cree que han contribuido a la excepción de análisis sintáctico. 
	     * @return una lista de 0 o más tokens del flujo de entrada que se cree que han contribuido a la excepción de análisis sintáctico. 
	     */
        public IList<Token> GetProblematicTokens() {
		    return problematicTokens;
	    }
    }
}
