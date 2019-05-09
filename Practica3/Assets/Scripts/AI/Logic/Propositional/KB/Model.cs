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

    using System.Collections.Generic;

    using UCM.IAV.AI.Logic.Propositional.Parsing;

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): pages 240, 245.
     * Los modelos son abstracciones matemáticas, cada una de las cuales simplemente fija la verdad o falsedad de cada sentencia relevante. 
     * En lógica proposicional un modelo simplemente fija el valor de verdad (True o False) para cada símbolo de proposición. 
     * 
     * Los modelos que se implementan aquí pueden representar asignaciones parciales del conjunto de símbolos de proposición de una base de conocimiento (es decir, lo que se conoce como modelo parcial). 
     */
    // HE TENIDO QUE HACER TANTO EL PRIMER BOOL COMO EL SEGUNDO COMO NULLABLES... ha supuesto cambiar esto en muchas partes
    public class Model : PLVisitor<bool?, bool?> { // implements de Java, Boolean de Java es bool

        private IDictionary<PropositionSymbol, bool?> assignments = new Dictionary<PropositionSymbol, bool?>(); // Era HashMap en los dos lados de la asignación y he cambiado mucho eso

        /**
	     * Constructor por defecto.
         * 
         * Creo que habrá que mantenerlo aquí vacío por si luego otra clase lo sobrescribe o algo (???)
	     */
        public Model() {
        }

        /**
         * Constructor a partir de un mapa de símbolos proposicionales y sus asignaciones booleanas. 
         */
        public Model(IDictionary<PropositionSymbol, bool?> values) { // Este Boolean creo que no es el boolean de Java
            // assignments.putAll(values); Ojo porque putAll sería respetando lo que esto pudiera tener por dentro...
            assignments = new Dictionary<PropositionSymbol, bool?>(values);
        }

        /**
         * Obtiene el valor de un símbolo proposicional.
         */
        public bool? GetValue(PropositionSymbol symbol) {
            bool? result = null;  // O no haría falta inicializarlo, y ya se sobreentiende que está a nulo
            assignments.TryGetValue(symbol, out result);

            return result;
        }


        /**
         * Dice si es cierto un símbolo proposicional.
         */
        public bool IsTrue(PropositionSymbol symbol) {
            bool? result = false; // ESTO LO HE PUESTO YO... si no está el símbolo va a devolver false (no sé si es correcto!)
            assignments.TryGetValue(symbol, out result);

            return true.Equals(result); //Había un bool.TrueString ... que no tiene mucho sentido
        }

        /**
         * Dice si es falso un símbolo proposicional.
         */
        public bool IsFalse(PropositionSymbol symbol) {
            bool? result = false; // ESTO LO HE PUESTO YO... si no está el símbolo va a devolver false (no sé si es correcto!)
            assignments.TryGetValue(symbol, out result);

            return bool.FalseString.Equals(result); //Boolean.FALSE lo he cambiado a esto
        }

        public Model Union(PropositionSymbol symbol, bool? b) {
            Model m = new Model();
            m.assignments = new Dictionary<PropositionSymbol, bool?>(assignments); //m.assignments.putAll(this.assignments);
            m.assignments.Add(symbol, b);
            return m;
        }

        public Model UnionInPlace(PropositionSymbol symbol, bool? b) {
            assignments.Add(symbol, b);
            return this;
        }

        public bool Remove(PropositionSymbol p) {
            return assignments.Remove(p);
        }

        public bool IsTrue(Sentence s) {
            return true.Equals(s.Accept(this, null)); // Había un bool.TrueString ... que no tiene mucho sentido
        }

        public bool IsFalse(Sentence s) {
            return false.Equals(s.Accept(this, null)); // Había un bool.FalseString ... que no tiene mucho sentido
        }

        public bool IsUnknown(Sentence s) {
            return null == s.Accept(this, null);
        }

        public Model Flip(PropositionSymbol s) {
            if (IsTrue(s)) {
                return Union(s, false);
            }
            if (IsFalse(s)) {
                return Union(s, true);
            }
            return this;
        }

        public ISet<PropositionSymbol> GetAssignedSymbols() { // Tal vez una property
            return new HashSet<PropositionSymbol>(assignments.Keys); // OJO, HE IGNORADO ESTO: Collections.unmodifiableSet( ) Hago una conversión directa a HashSet y se acabó
        }

        /**
	     * Determina si el modelo satisface un conjunto de cláusulas.
	     * 
	     * @param clauses
	     *            un conjunto de cláusulas proposicionales.
	     * @return si el modelo satisface las cláusulas, falso en caso contrario.
	     */
        public bool Satisfies(ISet<Clause> clauses) {
            foreach (Clause c in clauses) {
                // Todas deben ser verdad
                if (!true.Equals(DetermineValue(c))) // Había un bool.TrueString ... que no tiene mucho sentido
                    return false;
            }
            return true;
        }

        /**
	     * Determina basándose en las asignaciones actuales del modelo, cuando una cláusula se sabe que es cierta, falsa o desconocida. 
	     * 
	     * @param c
	     *            una cláusula proposicional.
	     * @return cierto, si se conoce que la cláusula es cierta bajo las actuales asignaciones del modelo.
         *         falso, si se conoce que la cláusula es falsa bajo las actuales asignaciones del modelo.
         *         nulo, si se desconoce que la cláusula sea cierta o falsa bajo las actuales asignaciones del modelo.          
	     */
        // Aquí claramente se está apostando por usar un bool nullable
        public bool? DetermineValue(Clause c) {
            bool? result = null; // es decir, desconocido

            if (c.IsTautology()) { // Prueba independiente de las asignaciones del modelo 
                result = true; // Boolean.TRUE ponía
            } else if (c.IsFalse()) { // Prueba independiente de las asignaciones del modelo 
                result = false; // Boolean.FALSE ponía
            } else {
                bool unassignedSymbols = false;
                foreach (Literal literal in c.GetLiterals()) {
                    PropositionSymbol symbol = literal.GetAtomicSentence();
                    bool? value;
                    assignments.TryGetValue(symbol, out value);
                    if (value == null) { // Debería meterse el TryGetValue aquí en vez de ver si es nulo
                        unassignedSymbols = true;
                    } else if (value.Equals(literal.IsPositiveLiteral())) {
                        result = true;
                        break;
                    }
                }
                if (result == null && !unassignedSymbols) {
                    // Si no se ha determinado que sea cierto y no hay símbolos sin asignar entonces podemos determinaar falsedad
                    // (es decir, todos sus literales se asignan como falsos bajo el modelo) 
                    result = false;
                }
            }
            return result;
        }

        // Según donde se use este print seguramente podría ser reemplazado por el ToString... no es buena idea imprimir en consola y menos trabajando sobre Unity 
        /*
        public void print() {
            foreach (var e in assignments) { //assignments.entrySet()  Aquí había algo de este estilo IDictionary.Entry<PropositionSymbol, bool?>
                System.Diagnostics.Debug.Write(e.Key + " = " + e.Value + " ");
            }
            System.Diagnostics.Debug.WriteLine(""); // He puesto "" porque obliga a meter un argumento

        } */

        // Cadena de texto representativa  
        public override string ToString() {
            return assignments.ToString();
        }

        //
        // START-PLVisitor 
        // No hace falta override porque PLVisitor es una interfaz
        public bool? VisitPropositionSymbol(PropositionSymbol s, bool? arg) {
            if (s.IsAlwaysTrue())
                return true;

            if (s.IsAlwaysFalse())
                return false;

            return GetValue(s);
        }


        // No hace falta override porque PLVisitor es una interfaz
        public bool? VisitUnarySentence(ComplexSentence fs, bool? arg) {
            object negatedValue = fs.GetSimplerSentence(0).Accept(this, null);
            if (negatedValue != null) {
                return !(bool)negatedValue;
            } else {
                return null;
            }
        }

        // No hace falta override porque PLVisitor es una interfaz
        public bool? VisitBinarySentence(ComplexSentence bs, bool? arg) {
            bool? firstValue = (bool?)bs.GetSimplerSentence(0).Accept(this, null);
            bool? secondValue = (bool?)bs.GetSimplerSentence(1).Accept(this, null);
            bool bothValuesKnown = firstValue != null && secondValue != null;
            Connective connective = bs.GetConnective();

            // He reprogramado todo esto
            if (connective.Equals(Connective.AND)) { 
                if (firstValue.Equals(false) || secondValue.Equals(false))
                    return false;
                else if (bothValuesKnown) return true; // Si no se cumple, saldrá como null

            } else if (connective.Equals(Connective.OR)) {
                if (firstValue.Equals(true) || secondValue.Equals(true))
                    return true;
                else if (bothValuesKnown) return false; // Si no se cumple, saldrá como null

            } else if (connective.Equals(Connective.IMPLICATION)) { 
                if (firstValue.Equals(false) || secondValue.Equals(true))
                    return true;
                else if (bothValuesKnown) return false; // Si no se cumple, saldrá como null

            } else if (connective.Equals(Connective.BICONDITIONAL)) { 
                if (bothValuesKnown) return firstValue.Equals(secondValue); // Si no se cumple, saldrá como null
            }

            return null;
	    }

        // END-PLVisitor
        //
    }
}