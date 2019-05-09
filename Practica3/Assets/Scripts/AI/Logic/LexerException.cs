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

    using System;

    /**
     * Una excepci�n en tiempo de ejecuci�n que se utiliza para describir las excepciones del analizador l�xico.
     * En particular proporcionar informaci�n para ayudar a identificar donde ha ocurrido la excepci�n dentro de la secuencia de caracteres de entrada. 
     */
    public class LexerException : Exception { //RuntimeException en Java, Exception en C# porque creo que no hay obligaci�n de capturarlas

        //private static readonly long serialVersionUID = 1L; // Creo que en C# no hace falta esto...

	    private int currentPositionInInput;

	    public LexerException(string message, int currentPositionInInput) : base(message) { 
		    this.currentPositionInInput = currentPositionInInput;
	    }

        // El Throwable cause de Java viene a ser la InnerException (o incluso la GetBaseException, que es la que est� en la ra�z) de C# 
	    public LexerException(string message, int currentPositionInInput, Exception inner) : base(message, inner) {
		    this.currentPositionInInput = currentPositionInInput;
	    }

        /**
	     * Devuelve la posici�n actual del flujo de entrada de caracteres donde estaba el analizador l�xico antes de que nos encontr�semos con la excepci�n.
	     * @return la posici�n actual del flujo de entrada de caracteres donde estaba el analizador l�xico antes de que nos encontr�semos con la excepci�n.
	     */
        public int GetCurrentPositionInInputExceptionThrown() {
		    return currentPositionInInput;
	    }
    }
}
