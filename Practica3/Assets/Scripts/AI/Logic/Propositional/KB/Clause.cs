/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
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
     * Una cl�usula es una disyunci�n de literales. Aqu� es vista como un conjunto de literales. 
     * Esto respeta la restricci�n, bajo resoluci�n, que una cl�usula resultante debe contener s�lo 1 copia de un literal resultante.
     * Adem�s las cl�usulas, tal y como est�n implementadas, son inmutables. 
     */
    public class Clause {
        public static readonly Clause EMPTY = new Clause(); // static readonly en vez de static final
        //
        private ISet<Literal> literals = new HashSet<Literal>(); // ISet en vez de Set, y HashSet en vez de LinkedHashSet... aunque una alternativa interesante que me dar�a las ventajas de la lista enlazada ser�a crear mi propio LinkedHashSet de C#: https://stackoverflow.com/questions/9346526/what-is-the-equivalent-of-linkedhashset-java-in-c
        //
        private ISet<PropositionSymbol> cachedPositiveSymbols = new HashSet<PropositionSymbol>(); // ISet en vez de Set, y HashSet en vez de LinkedHashSet... aunque una alternativa interesante que me dar�a las ventajas de la lista enlazada ser�a crear mi propio LinkedHashSet de C#: https://stackoverflow.com/questions/9346526/what-is-the-equivalent-of-linkedhashset-java-in-c
        private ISet<PropositionSymbol> cachedNegativeSymbols = new HashSet<PropositionSymbol>(); // ISet en vez de Set, y HashSet en vez de LinkedHashSet... aunque una alternativa interesante que me dar�a las ventajas de la lista enlazada ser�a crear mi propio LinkedHashSet de C#: https://stackoverflow.com/questions/9346526/what-is-the-equivalent-of-linkedhashset-java-in-c
        private ISet<PropositionSymbol> cachedSymbols = new HashSet<PropositionSymbol>(); // ISet en vez de Set, y HashSet en vez de LinkedHashSet... aunque una alternativa interesante que me dar�a las ventajas de la lista enlazada ser�a crear mi propio LinkedHashSet de C#: https://stackoverflow.com/questions/9346526/what-is-the-equivalent-of-linkedhashset-java-in-c
        //
        private bool? cachedIsTautologyResult = null; // En C# bool no puede ser null, para replicar c�mo est� hecho en Java usamos un nullable bool (bool?)
        private string cachedStringRep = null; // Para no tener que recalcularla si ya lo he hecho
        private int cachedHashCode = -1; // Para no tener que recalcularlo si ya lo he hecho

        /**
	     * Constructor por defecto de la cl�usula vac�a (que es 'False').
	     */
        public Clause() : this(new List<Literal>()) { // List en vez de ArrayList 
        }

        /**
	     * Construye una cl�usula con los literales dados. 
         * Nota: Los literales que son siempre 'False' (es decir False o ~True) no se a�aden a la cl�usula instanciada.
	     * 
	     * @param literals
	     *            los literales a ser a�adidos a la cl�usula
	     */
        public Clause(params Literal[] literals) : // palabra clave params en vez de Literal...
                            this(literals.ToList<Literal>()) { } // ToList<Literal> de LINQ en vez de Arrays.asList

        /**
	     * Construye una cl�usula con los literales dados. 
         * Nota: Los literales que son siempre 'False' (es decir False o ~True) no se a�aden a la cl�usula instanciada.
	     * 
	     * @param literals
         *          los literales a ser a�adidos a la cl�usula
	     */
        public Clause(IList<Literal> literals) { // List<Literal> en vez de Collection<Literal>

            foreach (Literal l in literals) {
                if (l.IsAlwaysFalse()) 
                    // No a�adir literales con la forma False o ~True
                    continue; 
                if (this.literals.Add(l)) {
                    // S�lo a�adir a las cach�s si no han sido a�adidos ya 
                    if (l.IsPositiveLiteral()) {
                        this.cachedPositiveSymbols.Add(l.GetAtomicSentence());
                    } else {
                        this.cachedNegativeSymbols.Add(l.GetAtomicSentence());
                    }
                }
            }

            cachedSymbols.UnionWith(cachedPositiveSymbols); // UnionWith de ISet en vez del m�todo addAll de Set
            cachedSymbols.UnionWith(cachedNegativeSymbols); // UnionWith de ISet en vez del m�todo addAll de Set

            // Hacer inmutable (AHORA MISMO NO LO ESTOY HACIENDO, OJO)
            // using System.Collections.Immutable; y usar as� ImmutableHashSet
            // Las clases de colecci�n inmutables est�n disponibles con.NET Core, pero no son parte de la biblioteca de clases b�sica distribuida con.NET Framework. Est�n disponibles a partir de.NET Framework 4.5 a trav�s de NuGet.
            // Aqu� sugieren crearnos nuestro propio tipo ReadOnlySet<T> (https://stackoverflow.com/questions/36815062/c-sharp-hashsett-read-only-workaround)
            // new ReadOnlyCollection<Literal> en vez de Collections.unmodifiableSet no funciona porque es sobre listas
            //this.literals = this.literals; 
            //cachedSymbols = cachedSymbols;  
            //cachedPositiveSymbols = cachedPositiveSymbols;  
            //cachedNegativeSymbols = cachedNegativeSymbols;  
        }

        /**
	     * Si una cl�usula est� vac�a -una disyunci�n sin disyunciones- es equivalente a 'False' porque una disyunci�n s�lo es cierta si al menos una de sus disyunciones es cierta. 
	     * 
	     * @return cierto si es una cl�usula vac�a, falso en caso contrario.
	     */
        public bool IsFalse() {
            return IsEmpty();
        }

        /**
	     * Devuelve si la cl�usula es vac�a.
         * 
	     * @return cierto si la cl�usula es vac�a (es decir, 'False'), falso en caso contrario.
	     */
        public bool IsEmpty() { 
            return literals.Count == 0;
        }

        /**
	     * Determina si una cl�usula es unitaria, es decir, si contiene un �nico literal. 
	     * 
	     * @return cierto si la cl�usula es unitaria, falso en caso contrario.
	     */
        public bool IsUnitClause() {
            return literals.Count == 1;
        }

        /**
	     * Determina si es una cl�usula definida. 
         * Una cl�usula definida es una disyunci�n de literales en el que EXACTAMENTE UNO es positivo. 	     * 
	     * 
	     * @return cierto si es una cl�usula definida, falso en caso contrario.
	     */
        public bool IsDefiniteClause() {
            return cachedPositiveSymbols.Count == 1;
        }

        /**
	     * Determina si es una cl�usula definida de implicaci�n.
         * Una cl�usula definida de implicaci�n es una disyunci�n de literales donde EXACTAMENTE UNO es positivo y hay UNO O M�S literales negativos. 
	     * 
	     * @return cierto si es una cl�usula definida de implicaci�n, falso en caso contrario.
	     */
        public bool IsImplicationDefiniteClause() {
            return IsDefiniteClause() && cachedNegativeSymbols.Count >= 1;
        }

        /**
	     * Determina si es una cl�usula de Horn. 
         * Una cl�usula de Horn es una disyunci�n de literales donde A LO SUMO UNO es positivo. 
	     * 
	     * @return cierto si es una cl�usula de Horn, falso en caso contrario.
	     */
        public bool IsHornClause() {
            return !IsEmpty() && cachedPositiveSymbols.Count <= 1;
        }

        /**
	     * Determina si es una cl�usula objetivo.
         * Una cl�usula objetivo es una disyunci�n sin literales positivos.
	     * 
	     * @return cierto si es una cl�usula objetivo, falso en caso contrario.
	     */
        public bool IsGoalClause() {
            return !IsEmpty() && cachedPositiveSymbols.Count == 0;
        }

        /**
	     * Determina si la cl�usula representa una tautolog�a, si por ejemplo contiene True, ~False o (P o ~P).
	     * 
	     * @return cierto si la cl�usula representa una tautolog�a, falso en caso contrario.
	     */
        public bool IsTautology() {
            if (cachedIsTautologyResult == null) {
                foreach (Literal l in literals) {
                    if (l.IsAlwaysTrue()) {
                        // Si contiene True o contiene ~False es una tautolog�a 
                        cachedIsTautologyResult = true;
                    }
                }
                // Si seguimos sin saberlo
                if (cachedIsTautologyResult == null) {
                    if (SetOps.Intersection(cachedPositiveSymbols, cachedNegativeSymbols).Count > 0) { // Esto de SetOps.intersection me recuerda al Sets.intersection de Java... tendr� que ver c�mo sustituirlo en C#
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
	     * Devuelve el n�mero de literales contenidos en la cl�usula.
         * 
	     * @return el n�mero de literales contenidos en la cl�usula.
	     */
        public int GetNumberLiterals() {
            return literals.Count;
        }

        /**
	     * Devuelve el n�mero de literales positivos contenidos en la cl�usula.
         * 
	     * @return el n�mero de literales positivos contenidos en la cl�usula.
	     */
        public int GetNumberPositiveLiterals() {
            return cachedPositiveSymbols.Count;
        }

        /**
	     * Devuelve el n�mero de literales negativos contenidos en la cl�usula.
         * 
	     * @return el n�mero de literales negativos contenidos en la cl�usula.
	     */
        public int GetNumberNegativeLiterals() {
            return cachedNegativeSymbols.Count;
        }

        /**
	     * Devuelve el conjunto de literales que forman la cl�usula.
         * 
	     * @return el conjunto de literales que forman la cl�usula.
	     */
        public ISet<Literal> GetLiterals() {
            return literals;
        }

        /**
	     * Devuelve el conjunto de s�mbolos de los literales positivos y negativos de la cl�usula.
         * 
	     * @return el conjunto de s�mbolos de los literales positivos y negativos de la cl�usula.
	     */
        public ISet<PropositionSymbol> GetSymbols() {
            return cachedSymbols;
        }

        /**
	     * Devuelve el conjunto de s�mbolos de los literales positivos de la cl�usula.
         * 
	     * @return el conjunto de s�mbolos de los literales positivos de la cl�usula.
	     */
        public ISet<PropositionSymbol> GetPositiveSymbols() {
            return cachedPositiveSymbols;
        }

        /**
	     * Devuelve el conjunto de s�mbolos de los literales negativos de la cl�usula.
         * 
	     * @return el conjunto de s�mbolos de los literales negativos de la cl�usula.
	     */
        public ISet<PropositionSymbol> GetNegativeSymbols() {
            return cachedNegativeSymbols;
        }

        // Compara esta cl�usula con otra y dice si son iguales
        public bool Equals(Clause c) {
            if (c == null) // Estando el otro m�todo Equals no s� si van a llamar con un null a este...
                return false;
            if (this == c)
                return true;
            return c.literals.SetEquals(this.literals); //Equals entre conjuntos es demasiado general (no consideraba iguales dos conjuntos vac�os, por ejemplo ???)... mejor SetEquals que ignora el orden interno, duplicados, etc.
        }

        // Compara esta cl�usula con otro objeto y dice si son iguales 
        public override bool Equals(object obj) {
            if (obj is Clause) // is en vez de instanceof (y mejor que typeof que obligar�a a ser del tipo exacto)
                return this.Equals(obj as Clause);
            return false;
        }

        // Devuelve c�digo hash del literal (para optimizar el acceso en colecciones y as�)
        // No debe contener bucles, tiene que ser muy r�pida
        public override int GetHashCode() {
            if (cachedHashCode == -1) {
                // Antes ten�a esto pero no es correcto preguntar por el hashcode del ISet: cachedHashCode = literals.GetHashCode();
                // Ahora tengo esta implementaci�n, que tengo entendido que es la manera en que el Set de Java calcula su hashcode (as� no importa el orden... y supongo que asumimos que duplicados no puede haber)
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