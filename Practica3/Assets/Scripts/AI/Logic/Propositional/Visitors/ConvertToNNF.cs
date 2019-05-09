/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.Visitors {

    using UCM.IAV.AI.Logic.Propositional.Parsing;

    /**
    * Convierte una sentencia su sentencia equivalente en forma normal de negaci�n (NNF).
    * Una sentencia est� en NNF si s�lo se permite la negaci�n sobre �tomos y la conjunci�n, la disyunci�n y la negaci�n son las �ltimas conectiva booleanas permitidas.
    */
    public class ConvertToNNF {

        /**
         * Devuelve la sentencia especificada en su sentencia equivalente en forma normal de negaci�n.
         * 
         * @param s
         *            una sentencia de l�gica proposicional
         * 
         * @return la sentencia de entrada convertida en su equivalente l�gica de forma normal de negaci�n.
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
