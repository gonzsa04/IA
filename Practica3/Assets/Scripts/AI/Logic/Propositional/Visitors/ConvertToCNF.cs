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
     * Convierte una sentencia su sentencia equivalente en forma normal conjuntiva (CNF).
     * Una sentencia est� en CNF si es una conjunci�n de disyunciones de literales. 
     */
    public class ConvertToCNF {

        /**
         * Devuelve la sentencia especificada en su sentencia equivalente en forma normal conjuntiva.
         * 
         * @param s
         *            una sentencia de l�gica proposicional
         * 
         * @return la sentencia de entrada convertida en su equivalente l�gica de forma normal conjuntiva.
         */
        public static Sentence Convert(Sentence s) {
            Sentence result = null;

            Sentence nnfSentence = ConvertToNNF.Convert(s);
            Sentence cnfSentence = DistributeOrOverAnd.Distribute(nnfSentence);

            result = cnfSentence;

            return result;
        }
    }
}