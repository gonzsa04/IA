/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Visitors {

    using UCM.IAV.AI.Logic.Propositional.Parsing;

    /**
    * Convierte una sentencia su sentencia equivalente en forma normal de negación (NNF).
    * Una sentencia está en NNF si sólo se permite la negación sobre átomos y la conjunción, la disyunción y la negación son las últimas conectiva booleanas permitidas.
    */
    public class ConvertToNNF {

        /**
         * Devuelve la sentencia especificada en su sentencia equivalente en forma normal de negación.
         * 
         * @param s
         *            una sentencia de lógica proposicional
         * 
         * @return la sentencia de entrada convertida en su equivalente lógica de forma normal de negación.
         */
        public static Sentence Convert(Sentence s) {
            Sentence result = null;

            Sentence biconditionalsRemoved = BiconditionalElimination.Eliminate(s);
            Sentence implicationsRemoved = ImplicationElimination
                    .Eliminate(biconditionalsRemoved);
            Sentence notsMovedIn = MoveNotInwards
                    .MoveNotsInward(implicationsRemoved);

            result = notsMovedIn;

            return result;
        }
    }
}
