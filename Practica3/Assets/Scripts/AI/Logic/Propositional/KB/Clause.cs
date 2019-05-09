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
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    using UCM.IAV.AI.Logic.Propositional.Parsing;
    using UCM.IAV.AI.Util; // Para SetOps

    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 253. 
     * Una cláusula es una disyunción de literales. Aquí es vista como un conjunto de literales. 
     * Esto respeta la restricción, bajo resolución, que una cláusula resultante debe contener sólo 1 copia de un literal resultante.
     * Además las cláusulas, tal y como están implementadas, son inmutables. 
     */
    public class Clause {
        public static readonly Clause EMPTY = new Clause(); // static readonly en vez de static final
        //
        private ISet<Literal> literals = new HashSet<Literal>(); // ISet en vez de Set, y HashSet en vez de LinkedHashSet... aunque una alternativa interesante que me daría las ventajas de la lista enlazada sería crear mi propio LinkedHashSet de C#: https://stackoverflow.com/questions/9346526/what-is-the-equivalent-of-linkedhashset-java-in-c
        //
        private ISet<PropositionSymbol> cachedPositiveSymbols = new HashSet<PropositionSymbol>(); // ISet en vez de Set, y HashSet en vez de LinkedHashSet... aunque una alternativa interesante que me daría las ventajas de la lista enlazada sería crear mi propio LinkedHashSet de C#: https://stackoverflow.com/questions/9346526/what-is-the-equivalent-of-linkedhashset-java-in-c
        private ISet<PropositionSymbol> cachedNegativeSymbols = new HashSet<PropositionSymbol>(); // ISet en vez de Set, y HashSet en vez de LinkedHashSet... aunque una alternativa interesante que me daría las ventajas de la lista enlazada sería crear mi propio LinkedHashSet de C#: https://stackoverflow.com/questions/9346526/what-is-the-equivalent-of-linkedhashset-java-in-c
        private ISet<PropositionSymbol> cachedSymbols = new HashSet<PropositionSymbol>(); // ISet en vez de Set, y HashSet en vez de LinkedHashSet... aunque una alternativa interesante que me daría las ventajas de la lista enlazada sería crear mi propio LinkedHashSet de C#: https://stackoverflow.com/questions/9346526/what-is-the-equivalent-of-linkedhashset-java-in-c
        //
        private bool? cachedIsTautologyResult = null; // En C# bool no puede ser null, para replicar cómo está hecho en Java usamos un nullable bool (bool?)
        private string cachedStringRep = null; // Para no tener que recalcularla si ya lo he hecho
        private int cachedHashCode = -1; // Para no tener que recalcularlo si ya lo he hecho

        /**
	     * Constructor por defecto de la cláusula vacía (que es 'False').
	     */
        public Clause() : this(new List<Literal>()) { // List en vez de ArrayList 
        }

        /**
	     * Construye una cláusula con los literales dados. 
         * Nota: Los literales que son siempre 'False' (es decir False o ~True) no se añaden a la cláusula instanciada.
	     * 
	     * @param literals
	     *            los literales a ser añadidos a la cláusula
	     */
        public Clause(params Literal[] literals) : // palabra clave params en vez de Literal...
                            this(literals.ToList<Literal>()) { } // ToList<Literal> de LINQ en vez de Arrays.asList

        /**
	     * Construye una cláusula con los literales dados. 
         * Nota: Los literales que son siempre 'False' (es decir False o ~True) no se añaden a la cláusula instanciada.
	     * 
	     * @param literals
         *          los literales a ser añadidos a la cláusula
	     */
        public Clause(IList<Literal> literals) { // List<Literal> en vez de Collection<Literal>

            foreach (Literal l in literals) {
                if (l.IsAlwaysFalse()) 
                    // No añadir literales con la forma False o ~True
                    continue; 
                if (this.literals.Add(l)) {
                    // Sólo añadir a las cachés si no han sido añadidos ya 
                    if (l.IsPositiveLiteral()) {
                        this.cachedPositiveSymbols.Add(l.GetAtomicSentence());
                    } else {
                        this.cachedNegativeSymbols.Add(l.GetAtomicSentence());
                    }
                }
            }

            cachedSymbols.UnionWith(cachedPositiveSymbols); // UnionWith de ISet en vez del método addAll de Set
            cachedSymbols.UnionWith(cachedNegativeSymbols); // UnionWith de ISet en vez del método addAll de Set

            // Hacer inmutable (AHORA MISMO NO LO ESTOY HACIENDO, OJO)
            // using System.Collections.Immutable; y usar así ImmutableHashSet
            // Las clases de colección inmutables están disponibles con.NET Core, pero no son parte de la biblioteca de clases básica distribuida con.NET Framework. Están disponibles a partir de.NET Framework 4.5 a través de NuGet.
            // Aquí sugieren crearnos nuestro propio tipo ReadOnlySet<T> (https://stackoverflow.com/questions/36815062/c-sharp-hashsett-read-only-workaround)
            // new ReadOnlyCollection<Literal> en vez de Collections.unmodifiableSet no funciona porque es sobre listas
            //this.literals = this.literals; 
            //cachedSymbols = cachedSymbols;  
            //cachedPositiveSymbols = cachedPositiveSymbols;  
            //cachedNegativeSymbols = cachedNegativeSymbols;  
        }

        /**
	     * Si una cláusula está vacía -una disyunción sin disyunciones- es equivalente a 'False' porque una disyunción sólo es cierta si al menos una de sus disyunciones es cierta. 
	     * 
	     * @return cierto si es una cláusula vacía, falso en caso contrario.
	     */
        public bool IsFalse() {
            return IsEmpty();
        }

        /**
	     * Devuelve si la cláusula es vacía.
         * 
	     * @return cierto si la cláusula es vacía (es decir, 'False'), falso en caso contrario.
	     */
        public bool IsEmpty() { 
            return literals.Count == 0;
        }

        /**
	     * Determina si una cláusula es unitaria, es decir, si contiene un único literal. 
	     * 
	     * @return cierto si la cláusula es unitaria, falso en caso contrario.
	     */
        public bool IsUnitClause() {
            return literals.Count == 1;
        }

        /**
	     * Determina si es una cláusula definida. 
         * Una cláusula definida es una disyunción de literales en el que EXACTAMENTE UNO es positivo. 	     * 
	     * 
	     * @return cierto si es una cláusula definida, falso en caso contrario.
	     */
        public bool IsDefiniteClause() {
            return cachedPositiveSymbols.Count == 1;
        }

        /**
	     * Determina si es una cláusula definida de implicación.
         * Una cláusula definida de implicación es una disyunción de literales donde EXACTAMENTE UNO es positivo y hay UNO O MÁS literales negativos. 
	     * 
	     * @return cierto si es una cláusula definida de implicación, falso en caso contrario.
	     */
        public bool IsImplicationDefiniteClause() {
            return IsDefiniteClause() && cachedNegativeSymbols.Count >= 1;
        }

        /**
	     * Determina si es una cláusula de Horn. 
         * Una cláusula de Horn es una disyunción de literales donde A LO SUMO UNO es positivo. 
	     * 
	     * @return cierto si es una cláusula de Horn, falso en caso contrario.
	     */
        public bool IsHornClause() {
            return !IsEmpty() && cachedPositiveSymbols.Count <= 1;
        }

        /**
	     * Determina si es una cláusula objetivo.
         * Una cláusula objetivo es una disyunción sin literales positivos.
	     * 
	     * @return cierto si es una cláusula objetivo, falso en caso contrario.
	     */
        public bool IsGoalClause() {
            return !IsEmpty() && cachedPositiveSymbols.Count == 0;
        }

        /**
	     * Determina si la cláusula representa una tautología, si por ejemplo contiene True, ~False o (P o ~P).
	     * 
	     * @return cierto si la cláusula representa una tautología, falso en caso contrario.
	     */
        public bool IsTautology() {
            if (cachedIsTautologyResult == null) {
                foreach (Literal l in literals) {
                    if (l.IsAlwaysTrue()) {
                        // Si contiene True o contiene ~False es una tautología 
                        cachedIsTautologyResult = true;
                    }
                }
                // Si seguimos sin saberlo
                if (cachedIsTautologyResult == null) {
                    if (SetOps.Intersection(cachedPositiveSymbols, cachedNegativeSymbols).Count > 0) { // Esto de SetOps.intersection me recuerda al Sets.intersection de Java... tendré que ver cómo sustituirlo en C#
                        // Tenemos P | ~P que siempre es cierto. 
                        cachedIsTautologyResult = true;
                    } else {
                        cachedIsTautologyResult = false;
                    }
                }
            }

            return (bool)cachedIsTautologyResult; // No puede ser null por la forma en que se ha programado arriba
        }

        /**
	     * Devuelve el número de literales contenidos en la cláusula.
         * 
	     * @return el número de literales contenidos en la cláusula.
	     */
        public int GetNumberLiterals() {
            return literals.Count;
        }

        /**
	     * Devuelve el número de literales positivos contenidos en la cláusula.
         * 
	     * @return el número de literales positivos contenidos en la cláusula.
	     */
        public int GetNumberPositiveLiterals() {
            return cachedPositiveSymbols.Count;
        }

        /**
	     * Devuelve el número de literales negativos contenidos en la cláusula.
         * 
	     * @return el número de literales negativos contenidos en la cláusula.
	     */
        public int GetNumberNegativeLiterals() {
            return cachedNegativeSymbols.Count;
        }

        /**
	     * Devuelve el conjunto de literales que forman la cláusula.
         * 
	     * @return el conjunto de literales que forman la cláusula.
	     */
        public ISet<Literal> GetLiterals() {
            return literals;
        }

        /**
	     * Devuelve el conjunto de símbolos de los literales positivos y negativos de la cláusula.
         * 
	     * @return el conjunto de símbolos de los literales positivos y negativos de la cláusula.
	     */
        public ISet<PropositionSymbol> GetSymbols() {
            return cachedSymbols;
        }

        /**
	     * Devuelve el conjunto de símbolos de los literales positivos de la cláusula.
         * 
	     * @return el conjunto de símbolos de los literales positivos de la cláusula.
	     */
        public ISet<PropositionSymbol> GetPositiveSymbols() {
            return cachedPositiveSymbols;
        }

        /**
	     * Devuelve el conjunto de símbolos de los literales negativos de la cláusula.
         * 
	     * @return el conjunto de símbolos de los literales negativos de la cláusula.
	     */
        public ISet<PropositionSymbol> GetNegativeSymbols() {
            return cachedNegativeSymbols;
        }

        // Compara esta cláusula con otra y dice si son iguales
        public bool Equals(Clause c) {
            if (c == null) // Estando el otro método Equals no sé si van a llamar con un null a este...
                return false;
            if (this == c)
                return true;
            return c.literals.SetEquals(this.literals); //Equals entre conjuntos es demasiado general (no consideraba iguales dos conjuntos vacíos, por ejemplo ???)... mejor SetEquals que ignora el orden interno, duplicados, etc.
        }

        // Compara esta cláusula con otro objeto y dice si son iguales 
        public override bool Equals(object obj) {
            if (obj is Clause) // is en vez de instanceof (y mejor que typeof que obligaría a ser del tipo exacto)
                return this.Equals(obj as Clause);
            return false;
        }

        // Devuelve código hash del literal (para optimizar el acceso en colecciones y así)
        // No debe contener bucles, tiene que ser muy rápida
        public override int GetHashCode() {
            if (cachedHashCode == -1) {
                // Antes tenía esto pero no es correcto preguntar por el hashcode del ISet: cachedHashCode = literals.GetHashCode();
                // Ahora tengo esta implementación, que tengo entendido que es la manera en que el Set de Java calcula su hashcode (así no importa el orden... y supongo que asumimos que duplicados no puede haber)
                cachedHashCode = 0;
                foreach (var l in literals)
                    cachedHashCode += l.GetHashCode();
            }
            return cachedHashCode;
        }

        // Cadena de texto representativa  
        public override string ToString() {
            if (cachedStringRep == null) {
                StringBuilder sb = new StringBuilder(); // Hay StringBuilder en C#
                bool first = true;
                sb.Append("{");
                foreach (Literal l in literals) {
                    if (first) 
                        first = false;
                    else 
                        sb.Append(", ");
                    sb.Append(l);
                }
                sb.Append("}");
                cachedStringRep = sb.ToString();
            }
            return cachedStringRep;
        }
    }
}