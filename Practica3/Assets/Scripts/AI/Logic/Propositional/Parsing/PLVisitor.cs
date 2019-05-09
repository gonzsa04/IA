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

    using UCM.IAV.AI.Logic.Propositional.KB;

    /**
     * Visitante de Lógica Proposicional: Un patrón Visitante para recorrer la representación estructura de árbol de sintaxis abstracta de la lógica proposicional que se usa en esta librería.
     * La diferencia clave entre el patrón Visitante por defecto y el codificado aquí, es que en el primero los métodos visit tienen la forma void visit(ConcreteNode) mientras que los visitantes
     * que se usan aquí tienen esta otra forma: Object visit(ConcreteNode, Object arg)
     * Esto simplifica las pruebas y permite cierto código recursivo que sería complicado utilizando el patrón por defecto.  
     * 
     * @param <A>
     *            el tipo de argumento que será pasado a los métodos visitantes.
     * @param <R>
     *            el tipo de retorno que será devuelto por los métodos visitantes.
     */
    public interface PLVisitor<A, R> {
        /**
         * Visita un símbolo de proposición (por ejemplo A).
         * 
         * @param sentence
         *            una sentencia que es un símbolo proposicional.
         * @param arg
         *            argumento opcional que puede usar el visitante.
         * @return valor de retorno opcional que será utilizado por el visitante.
         */
        R VisitPropositionSymbol(PropositionSymbol sentence, A arg);

        /**
         * Visita una sentencia compuesta unaria (por ejemplo ~A).
         * 
         * @param sentence
         *            una sentencia que es una sentencia compuesta unaria.
         * @param arg
         *            argumento opcional que puede usar el visitante.
         * @return valor de retorno opcional que será utilizado por el visitante.
         */
        R VisitUnarySentence(ComplexSentence sentence, A arg);

        /**
         * Visita una sentencia compuesta binaria (por ejemplo A & B).
         * 
         * @param sentence
         *            una sentencia que es una sentencia compuesta binaria.
         * @param arg
         *            argumento opcional que puede usar el visitante.
         * @return valor de retorno opcional que será utilizado por el visitante.
         */
        R VisitBinarySentence(ComplexSentence sentence, A arg);
    }
}