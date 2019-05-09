/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.AI.Logic.Propositional.KB {

    using System.Text;

    using UCM.IAV.AI.Logic.Propositional.Parsing;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 244. 
     * Un litereal es o bien una sentencia atómica (un literal positivo) o una sentencia atómica negada (un literal negativo).
     * En lógica proposicional las sentencias atómicas consisten en un único símbolo de proposición. 
     * Además, un literal se implementa como inmutable. 
     */
    public class Literal {

        private PropositionSymbol atom = null;
        private bool positive = true; // Se asume positivo por defecto 
                                      //
        private string cachedStringRep = null; // Para no tener que recalcularla si ya lo he hecho
        private int cachedHashCode = -1; // Para no tener que recalcularlo si ya lo he hecho

        /**
	     * Constructor de un literal positivo.
	     * 
	     * @param atom
	     *            la sentencia atómica que constituye el literal.
	     */
        public Literal(PropositionSymbol atom) : this(atom, true) {
        }

        /**
	     * Constructor genérico de un literal.
	     * 
	     * @param atom
	     *            la sentencia atómica que constituye el literal.
	     * @param positive
	     *            cierto para que sea un literal positivo, falso para que sea un literal negativo. 
	     */
        public Literal(PropositionSymbol atom, bool positive) {
            this.atom = atom;
            this.positive = positive;
        }

        /**
	     * Dice si es un literal positivo.
         * 
	     * @return cierto si es un literal positivo, falso en otro caso.
	     */
        public bool IsPositiveLiteral() {
            return positive;
        }

        /**
	     * Dice si es un literal negativo.
         * 
	     * @return cierto si es un literal negativo, falso en otro caso.
	     */
        public bool IsNegativeLiteral() {
            return !positive;
        }

        /**
	     * Devuelve la sentencia atómica que constituye el literal.
         * 
	     * @return la sentencia atómica que constituye el literal.
	     */
        public PropositionSymbol GetAtomicSentence() {
            return atom;
        }

        /**
	     * Dice si el literal representa una proposición siempre cierta.
         * 
	     * @return cierto si el literal representa una proposición siempre cierta (es decir, True o ~False), falso en caso contrario.
	     */
        public bool IsAlwaysTrue() {
            // True | ~False
            if (IsPositiveLiteral()) {
                return GetAtomicSentence().IsAlwaysTrue();
            } else {
                return GetAtomicSentence().IsAlwaysFalse();
            }
        }

        /**
	     * Dice si el literal representa una proposición siempre falsa.
         * 
	     * @return cierto si el literal representa una proposición siempre falsa (es decir, False o ~True), falso en caso contrario.
	     */
        public bool IsAlwaysFalse() {
            // False | ~True
            if (IsPositiveLiteral()) {
                return GetAtomicSentence().IsAlwaysFalse();
            } else {
                return GetAtomicSentence().IsAlwaysTrue();
            }
        }

        // Compara este literal con otro y dice si son iguales
        public bool Equals(Literal l) {
            if (l == null)
                return false;
            if (this == l)
                return true;
            return l.IsPositiveLiteral() == IsPositiveLiteral() && l.GetAtomicSentence().Equals(GetAtomicSentence());
        }

        // Compara este literal con otro objeto y dice si son iguales 
        public override bool Equals(object obj) {
            if (obj is Literal) // is en vez de instanceof (y mejor que typeof que obligaría a ser del tipo exacto)
                return this.Equals(obj as Literal);
            return false;
        }

        // Devuelve código hash del literal (para optimizar el acceso en colecciones y así)
        // No debe contener bucles, tiene que ser muy rápida
        public override int GetHashCode() {
            if (cachedHashCode == -1) {
                cachedHashCode = 17;
                // Esto de pedir los códigos hash de estas strings es un tanto raro
                cachedHashCode = (cachedHashCode * 37) + (IsPositiveLiteral() ? "+".GetHashCode() : "-".GetHashCode());
                cachedHashCode = (cachedHashCode * 37) + atom.GetHashCode();
            }
            return cachedHashCode;
        }

        // Cadena de texto representativa  
        public override string ToString() {
            if (null == cachedStringRep) {
                StringBuilder sb = new StringBuilder(); // Hay StringBuilder en C#
                if (IsNegativeLiteral())
                    sb.Append(Connective.NOT.ToString());
                sb.Append(GetAtomicSentence().ToString());
                cachedStringRep = sb.ToString();
            }
            return cachedStringRep;
        }
    }
}