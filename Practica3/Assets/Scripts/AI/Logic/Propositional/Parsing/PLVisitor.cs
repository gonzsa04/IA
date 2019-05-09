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

    using UCM.IAV.AI.Logic.Propositional.KB;

    /**
     * Visitante de L�gica Proposicional: Un patr�n Visitante para recorrer la representaci�n estructura de �rbol de sintaxis abstracta de la l�gica proposicional que se usa en esta librer�a.
     * La diferencia clave entre el patr�n Visitante por defecto y el codificado aqu�, es que en el primero los m�todos visit tienen la forma void visit(ConcreteNode) mientras que los visitantes
     * que se usan aqu� tienen esta otra forma: Object visit(ConcreteNode, Object arg)
     * Esto simplifica las pruebas y permite cierto c�digo recursivo que ser�a complicado utilizando el patr�n por defecto.  
     * 
     * @param <A>
     *            el tipo de argumento que ser� pasado a los m�todos visitantes.
     * @param <R>
     *            el tipo de retorno que ser� devuelto por los m�todos visitantes.
     */
    public interface PLVisitor<A, R> {
        /**
         * Visita un s�mbolo de proposici�n (por ejemplo A).
         * 
         * @param sentence
         *            una sentencia que es un s�mbolo proposicional.
         * @param arg
         *            argumento opcional que puede usar el visitante.
         * @return valor de retorno opcional que ser� utilizado por el visitante.
         */
        R VisitPropositionSymbol(PropositionSymbol sentence, A arg);

        /**
         * Visita una sentencia compuesta unaria (por ejemplo ~A).
         * 
         * @param sentence
         *            una sentencia que es una sentencia compuesta unaria.
         * @param arg
         *            argumento opcional que puede usar el visitante.
         * @return valor de retorno opcional que ser� utilizado por el visitante.
         */
        R VisitUnarySentence(ComplexSentence sentence, A arg);

        /**
         * Visita una sentencia compuesta binaria (por ejemplo A & B).
         * 
         * @param sentence
         *            una sentencia que es una sentencia compuesta binaria.
         * @param arg
         *            argumento opcional que puede usar el visitante.
         * @return valor de retorno opcional que ser� utilizado por el visitante.
         */
        R VisitBinarySentence(ComplexSentence sentence, A arg);
    }
}