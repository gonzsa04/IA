/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Puzzles.Model {

    using System;
    using UCM.IAV.Util;

    /*
     * El modelo lógico del puzle deslizante, internamente guarda los datos en una matriz.
     * Permite copias profundas de objetos de este tipo.
     */
    public class SlidingPuzzle : IDeepCloneable<SlidingPuzzle>{

        // Dimensión de las filas
        public uint rows;
        // Dimensión de las columnas
        public uint columns;

        // La matriz de valores (enteros sin signo) 
        // Podría definirse como tipo genérico, SlidingPuzzle<E> para contener otro tipo de valores.
        private uint[,] matrix;
        
        // Mantiene una referencia actualizada a la posición del hueco, siempre (por eficiencia) 
        // Lo podría llamar sólo Gap (o SpecialValue)
        public Position GapPosition { get; private set; }

        // Mantiene una referencia actualizada a la posición del hueco, siempre (por eficiencia)

        private static readonly uint GAP_VALUE = 0u;
        private static readonly uint DEFAULT_ROWS = 3u;
        private static readonly uint DEFAULT_COLUMNS = 3u;

        // Construye la matriz de dimensiones (3) filas por (3) columnas por defecto
        // Esto se conoce como Constructor chaining
        public SlidingPuzzle() : this(DEFAULT_ROWS, DEFAULT_COLUMNS) {
        }

        // Construye la matriz de dimensiones (rows) por (columns)
        // Como mínimo el puzle debe ser de 1x1
        public SlidingPuzzle(uint rows, uint columns) {
            if (rows == 0) throw new ArgumentException(string.Format("{0} is not a valid rows value", rows), "rows");
            if (columns == 0) throw new ArgumentException(string.Format("{0} is not a valid columns value", columns), "columns");

            this.Initialize(rows, columns);
        }

        // Constructor de copia, en realidad sirve igual que clonar el puzle
        public SlidingPuzzle(SlidingPuzzle puzzle) {
            if (puzzle == null) throw new ArgumentNullException(nameof(puzzle));

            this.rows = puzzle.rows;
            this.columns = puzzle.columns;

            // Como es de tipos simples, podría aprovecharse la matriz que ya estuviese creada, pero por ahora no lo hacemos
            matrix = new uint[rows, columns];
            for (var r = 0u; r < rows; r++)
                for (var c = 0u; c < columns; c++) 
                    matrix[r, c] = puzzle.matrix[r, c];

            // Si la posición del hueco estuviese mal indicada en el otro puzzle se produciría una excepción
            if (puzzle.matrix[puzzle.GapPosition.GetRow(), puzzle.GapPosition.GetColumn()] != GAP_VALUE)
                throw new ArgumentException(string.Format("{0} is not a valid rows value", rows), "rows");

            GapPosition = puzzle.GapPosition; 
        }

        // Devuelve este objeto clonado a nivel profundo
        public SlidingPuzzle DeepClone() {
            // Uso el constructor de copia para generar un clon
            return new SlidingPuzzle(this);
        }

        // Devuelve este objeto de tipo SlidingPuzzle clonado a nivel profundo 
        object IDeepCloneable.DeepClone() {
            return this.DeepClone();
        }

        // Inicializa o reinicia el puzle, con los valores por defecto (S es el valor que representa al hueco)
        //   Por ejemplo, para rows == 4 and columns == 3
        //   1   2   3
        //   4   5   6
        //   7   8   9
        //   10  11  S
        // Como mínimo el puzle debe ser de 1x1
        public void Initialize(uint rows, uint columns) {
            if (rows == 0) throw new ArgumentException(string.Format("{0} is not a valid rows value", rows), "rows");
            if (columns == 0) throw new ArgumentException(string.Format("{0} is not a valid columns value", columns), "columns");

            this.rows = rows;
            this.columns = columns;

            // Podría aprovecharse la matriz que ya estuviese creada, pero por ahora no lo hacemos
            matrix = new uint[rows, columns];

            for (var r = 0u; r < rows; r++)
                for (var c = 0u; c < columns; c++) {
                    matrix[r, c] = GetDefaultValue(r, c);
                    if (matrix[r, c] == GAP_VALUE)
                        GapPosition = new Position(r, c);
                }
        }

        // Devuelve el que se considera valor inicial por defecto de una posición
        private uint GetDefaultValue(uint row, uint column) {
            // Control de excepciones
            uint aux = (row * columns) + column + 1u; // Se suma 1 porque los seres humanos preferimos empezar contando desde el 1 para los valores
            if (aux != rows * columns)
                return aux;
            else
                // Quedará a GAP_VALUE la posición de la matriz a la que le tocaría recibir el rows * columns - 1
                // En realidad ya vale 0 al estar inicializada (si siempre fuese a ser el 0 podría hacerse con la operación módulo)
                return GAP_VALUE;
        }

        // Devuelve el valor contenido (uint) en una determinada posición
        // Si no hay ningún valor, se devolverá nulo
        public uint GetValue(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");

            // Se podría ver si la posición es la de GapPosition y devolver el valor de hueco directamente
            return matrix[position.GetRow(), position.GetColumn()];
        }

        // Comprueba que los valores están en el orden por defecto (que la matriz está en su configuración inicial)
        public bool IsInDefaultOrder() {

            // Hacer directamente un recorrido por la matrix, con un método que la ponga
            for (uint r = 0; r < rows; r++)
                for (uint c = 0; c < columns; c++)
                    if (matrix[r, c] != GetDefaultValue(r, c)) // Debería usar el operador módulo o una función para dar el valor correcto a cada posición
                        return false;

            return true;
        }

        // Devuelve cierto si es posible mover un valor desde una determinada posición a algunas de las colindantes
        // En este caso, como no se especifica ninguna dirección a donde moverlo, el hueco no se considera un valor "movible" así en general. 
        public bool CanMoveByDefault(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");

            // El hueco no se puede mover directamente, sin especificar ninguna dirección
            if (position.Equals(GapPosition))
                return false;

            return CanMoveUp(position) || CanMoveDown(position) || CanMoveLeft(position) || CanMoveRight(position);
        }

        // Mueve el valor de una posición, devolviendo la nueva posición si es cierto que se ha podido hacer el movimiento
        // Los intentos para ver a que posición colindante se mueve el valor se realizan en este orden POR DEFECTO: arriba, abajo, izquierda y derecha. 
        // En este caso, como no se especifica ninguna dirección a donde moverlo, el hueco no se considera un valor "movible por defecto" 
        public Position MoveByDefault(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");
            if (!CanMoveByDefault(position)) throw new InvalidOperationException("The required movement is not possible");

            UnityEngine.Debug.Log(ToString() + " is moving " + position.ToString());

            if (CanMoveUp(position))
                return MoveUp(position);
            if (CanMoveDown(position))
                return MoveDown(position);
            if (CanMoveLeft(position))
                return MoveLeft(position);
            //if (CanMoveRight(position)) Tiene que poderse mover a la derecha
                return MoveRight(position); 
        }

        // Coloca el hueco en esta posición, y lo que hubiera en esa posición donde estaba el hueco (intercambio de valores entre esas dos posiciones)
        // Para hacerlo público convendría comprobar que este movimiento tan directo -que es un intercambio, no un intento- es factible
        private void Move(Position origin, Position target) {
            if (origin == null) throw new ArgumentNullException(nameof(origin));
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (origin.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", origin.GetRow()), "row");
            if (target.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", target.GetRow()), "row");
            if (origin.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", origin.GetColumn()), "column");
            if (target.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", target.GetColumn()), "column");

            // Intercambio de valores entre las dos posiciones de la matriz
            uint auxValue = GetValue(origin);
            matrix[origin.GetRow(), origin.GetColumn()] = matrix[target.GetRow(), target.GetColumn()]; // Al final aquí no pongo matrix[GapPosition.GetRow(), GapPosition.GetColumn()] ... ni directamente GAP_VALUE
            matrix[target.GetRow(), target.GetColumn()] = auxValue;

            /* Para qué querría hacer esto?
            // Intercambio de coordenadas entre las dos posiciones
            origin.Exchange(GapPosition);
            */

            // No hay que olvidarse de mantener la posición del hueco
            if (auxValue.Equals(GAP_VALUE))
                GapPosition = target;
            else
                GapPosition = origin; // Porque uno de los dos tiene que ser el hueco

            UnityEngine.Debug.Log(ToString() + " sucessfully moved " + origin.ToString() + ".");
        }

        // Devuelve cierto si es posible mover un valor a la posición de arriba de una determinada posición (sea el hueco o no) 
        public bool CanMoveUp(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");

            return GapPosition.IsUp(position) || (GapPosition.Equals(position) && GapPosition.GetRow() > 0u);
        }

        // Mueve el valor que haya en la posición 'position' (sea hueco o no) a la posición de arriba, devolviendo dicha posición de destino   
        // Falla si no es posible realizar el movimiento
        public Position MoveUp(Position origin) {
            if (origin == null) throw new ArgumentNullException(nameof(origin));
            if (origin.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", origin.GetRow()), "row");
            if (origin.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", origin.GetColumn()), "column");
            if (!CanMoveUp(origin)) throw new InvalidOperationException("The required movement is not possible");

            Position target = origin.Up(); // Ya hemos comprobado que es posible el movimiento y por tanto existe la posición de destino
            UnityEngine.Debug.Log(ToString() + " is 'moving up' position " + origin.ToString() + " to " + target.ToString());
            Move(origin, target);
            return target;
        }

        // Devuelve cierto si es posible mover un valor a la posición de abajo de una determinada posición (sea el hueco o no) 
        public bool CanMoveDown(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");

            return GapPosition.IsDown(position) || (GapPosition.Equals(position) && GapPosition.GetRow() + 1u < rows);
        }

        // Mueve el valor que haya en la posición 'position' (sea hueco o no) a la posición de abajo, devolviendo dicha posición de destino   
        // Falla si no es posible realizar el movimiento
        public Position MoveDown(Position origin) {
            if (origin == null) throw new ArgumentNullException(nameof(origin));
            if (origin.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", origin.GetRow()), "row");
            if (origin.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", origin.GetColumn()), "column");
            if (!CanMoveDown(origin)) throw new InvalidOperationException("The required movement is not possible");

            Position target = origin.Down(); // Ya hemos comprobado que es posible el movimiento y por tanto existe la posición de destino
            UnityEngine.Debug.Log(ToString() + " is 'moving down' position " + origin.ToString() + " to " + target.ToString());
            Move(origin, target);
            return target;
        }
        
        // Devuelve cierto si es posible mover un valor a la posición izquierda de una determinada posición (sea el hueco o no) 
        public bool CanMoveLeft(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");

            return GapPosition.IsLeft(position) || (GapPosition.Equals(position) && GapPosition.GetColumn() > 0u);
        }

        // Mueve el valor que haya en la posición 'position' (sea hueco o no) a la posición de la izquierda, devolviendo dicha posición de destino   
        // Falla si no es posible realizar el movimiento
        public Position MoveLeft(Position origin) {
            if (origin == null) throw new ArgumentNullException(nameof(origin));
            if (origin.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", origin.GetRow()), "row");
            if (origin.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", origin.GetColumn()), "column");
            if (!CanMoveLeft(origin)) throw new InvalidOperationException("The required movement is not possible");

            Position target = origin.Left(); // Ya hemos comprobado que es posible el movimiento y por tanto existe la posición de destino
            UnityEngine.Debug.Log(ToString() + " is 'moving left' position " + origin.ToString() + " to " + target.ToString());
            Move(origin, target);
            return target;
        }
        
        // Devuelve cierto si es posible mover un valor a la posición derecha de una determinada posición (sea el hueco o no) 
        public bool CanMoveRight(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");

            return GapPosition.IsRight(position) || (GapPosition.Equals(position) && GapPosition.GetColumn() + 1u < columns);
        }

        // Mueve el valor que haya en la posición 'position' (sea hueco o no) a la posición de la derecha, devolviendo dicha posición de destino   
        // Falla si no es posible realizar el movimiento
        public Position MoveRight(Position origin) {
            if (origin == null) throw new ArgumentNullException(nameof(origin));
            if (origin.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", origin.GetRow()), "row");
            if (origin.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", origin.GetColumn()), "column");
            if (!CanMoveRight(origin)) throw new InvalidOperationException("The required movement is not possible");

            Position target = origin.Right(); // Ya hemos comprobado que es posible el movimiento y por tanto existe la posición de destino
            UnityEngine.Debug.Log(ToString() + " is 'moving right' position " + origin.ToString() + " to " + target.ToString());
            Move(origin, target);
            return target;
        }

        // Compara este puzle con otro objeto y dice si son iguales
        // Sobreescribe la función Equals de object
        public override bool Equals(object o) {
            return Equals(o as SlidingPuzzle);
        }

        // Compara este puzle con otro y dice si sus configuraciones son iguales
        public bool Equals(SlidingPuzzle puzzle) {
            if (puzzle == null || puzzle.rows != rows || puzzle.columns != columns) {
                return false;
            }

            // Recorrer todos los elementos de ambas matrices
            for (uint r = 0; r < rows; r++)
                for (uint c = 0; c < columns; c++)
                    if (!matrix[r, c].Equals(puzzle.matrix[r, c]))
                        return false;

            return true;
        }

        // Devuelve código hash del puzle (para optimizar el acceso en colecciones y así)
        public override int GetHashCode() {
            int result = 17;

            for (uint r = 0; r < rows; r++)
                for (uint c = 0; c < columns; c++)
                    result = 37 * result + Convert.ToInt32(matrix[r, c]);

            return result;
        }

        // Cadena de texto representativa (dibujar una matriz de posiciones separadas por espacios, con \n al final de cada columna o algo así
        public override string ToString() {
            return "Puzzle{" + string.Join(",", matrix) + "}"; //Join en realidad sólo funciona con matrices de string, voy a tener que hacer el bucle
        }

    }
}

