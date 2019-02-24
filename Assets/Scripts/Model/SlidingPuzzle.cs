namespace UCM.IAV.Puzzles.Model {

    using System;
    using UCM.IAV.Util;
    
    public class SlidingPuzzle : IDeepCloneable<SlidingPuzzle>{

        private System.Random rnd = new System.Random();

        // Dimensión de las filas
        public uint rows;
        // Dimensión de las columnas
        public uint columns;
        
        private uint[,] matrix; // matriz de tipos de cada casilla
        
        public Position TankPosition { get; private set; }
        public Position GapPosition { get; private set; }

        private static readonly uint GAP_VALUE = 0u;
        private static readonly uint DEFAULT_ROWS = 3u;
        private static readonly uint DEFAULT_COLUMNS = 3u;
        
        public SlidingPuzzle() : this(DEFAULT_ROWS, DEFAULT_COLUMNS) { }
        
        public SlidingPuzzle(uint rows, uint columns) {
            if (rows == 0) throw new ArgumentException(string.Format("{0} is not a valid rows value", rows), "rows");
            if (columns == 0) throw new ArgumentException(string.Format("{0} is not a valid columns value", columns), "columns");

            this.Initialize(rows, columns);
        }

        // crea una nueva matriz de rows * cols de tipos de casilla aleatorios
        public void Initialize(uint rows, uint columns)
        {
            if (rows == 0) throw new ArgumentException(string.Format("{0} is not a valid rows value", rows), "rows");
            if (columns == 0) throw new ArgumentException(string.Format("{0} is not a valid columns value", columns), "columns");

            this.rows = rows;
            this.columns = columns;

            matrix = new uint[rows, columns];

            for (var r = 0u; r < rows; r++)
                for (var c = 0u; c < columns; c++)
                    matrix[r, c] = (uint)rnd.Next(0, 4);
        }

        // copia un juego recibido
        public SlidingPuzzle(SlidingPuzzle puzzle) {
            if (puzzle == null) throw new ArgumentNullException(nameof(puzzle));

            this.rows = puzzle.rows;
            this.columns = puzzle.columns;
            
            matrix = new uint[rows, columns];
            for (var r = 0u; r < rows; r++)
                for (var c = 0u; c < columns; c++) 
                    matrix[r, c] = puzzle.matrix[r, c];
            
            if (puzzle.matrix[puzzle.TankPosition.GetRow(), puzzle.TankPosition.GetColumn()] != GAP_VALUE)
                throw new ArgumentException(string.Format("{0} is not a valid rows value", rows), "rows");

            TankPosition = puzzle.TankPosition; 
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
        
        public uint GetType(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");
            
            return matrix[position.GetRow(), position.GetColumn()];
        }

        public void SetType(Position position, uint value)
        {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");

            matrix[position.GetRow(), position.GetColumn()] = value;
        }

        // Devuelve cierto si es posible mover un valor desde una determinada posición a algunas de las colindantes
        // En este caso, como no se especifica ninguna dirección a donde moverlo, el hueco no se considera un valor "movible" así en general. 
        public bool CanMoveByDefault(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");

            // El hueco no se puede mover directamente, sin especificar ninguna dirección
            if (position.Equals(TankPosition))
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
            uint auxValue = GetType(origin);
            matrix[origin.GetRow(), origin.GetColumn()] = matrix[target.GetRow(), target.GetColumn()]; // Al final aquí no pongo matrix[GapPosition.GetRow(), GapPosition.GetColumn()] ... ni directamente GAP_VALUE
            matrix[target.GetRow(), target.GetColumn()] = auxValue;

            // Para qué querría hacer esto?
            // Intercambio de coordenadas entre las dos posiciones
            // origin.Exchange(GapPosition);

            // No hay que olvidarse de mantener la posición del hueco
            if (auxValue.Equals(GAP_VALUE))
                TankPosition = target;
            else
                TankPosition = origin; // Porque uno de los dos tiene que ser el hueco

            UnityEngine.Debug.Log(ToString() + " sucessfully moved " + origin.ToString() + ".");
        }

        // Devuelve cierto si es posible mover un valor a la posición de arriba de una determinada posición (sea el hueco o no) 
        public bool CanMoveUp(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");

            return TankPosition.IsUp(position) || (TankPosition.Equals(position) && TankPosition.GetRow() > 0u);
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

            return TankPosition.IsDown(position) || (TankPosition.Equals(position) && TankPosition.GetRow() + 1u < rows);
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

            return TankPosition.IsLeft(position) || (TankPosition.Equals(position) && TankPosition.GetColumn() > 0u);
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

            return TankPosition.IsRight(position) || (TankPosition.Equals(position) && TankPosition.GetColumn() + 1u < columns);
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
        
        public override bool Equals(object o) {
            return Equals(o as SlidingPuzzle);
        }
        
        public bool Equals(SlidingPuzzle puzzle) {
            if (puzzle == null || puzzle.rows != rows || puzzle.columns != columns) {
                return false;
            }
            
            for (uint r = 0; r < rows; r++)
                for (uint c = 0; c < columns; c++)
                    if (!matrix[r, c].Equals(puzzle.matrix[r, c]))
                        return false;

            return true;
        }
        
        public override int GetHashCode() {
            int result = 17;

            for (uint r = 0; r < rows; r++)
                for (uint c = 0; c < columns; c++)
                    result = 37 * result + Convert.ToInt32(matrix[r, c]);

            return result;
        }
        
        public override string ToString() {
            return "Puzzle{" + string.Join(",", matrix) + "}";
        }

    }
}

