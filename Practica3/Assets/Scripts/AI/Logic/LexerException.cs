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

    /**
     * Una excepción en tiempo de ejecución que se utiliza para describir las excepciones del analizador léxico.
     * En particular proporcionar información para ayudar a identificar donde ha ocurrido la excepción dentro de la secuencia de caracteres de entrada. 
     */
    public class LexerException : Exception { //RuntimeException en Java, Exception en C# porque creo que no hay obligación de capturarlas

        //private static readonly long serialVersionUID = 1L; // Creo que en C# no hace falta esto...

	    private int currentPositionInInput;

	    public LexerException(string message, int currentPositionInInput) : base(message) { 
		    this.currentPositionInInput = currentPositionInInput;
	    }

        // El Throwable cause de Java viene a ser la InnerException (o incluso la GetBaseException, que es la que está en la raíz) de C# 
	    public LexerException(string message, int currentPositionInInput, Exception inner) : base(message, inner) {
		    this.currentPositionInInput = currentPositionInInput;
	    }

        /**
	     * Devuelve la posición actual del flujo de entrada de caracteres donde estaba el analizador léxico antes de que nos encontrásemos con la excepción.
	     * @return la posición actual del flujo de entrada de caracteres donde estaba el analizador léxico antes de que nos encontrásemos con la excepción.
	     */
        public int GetCurrentPositionInInputExceptionThrown() {
		    return currentPositionInInput;
	    }
    }
}
