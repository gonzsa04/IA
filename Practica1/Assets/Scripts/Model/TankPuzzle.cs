namespace UCM.IAV.Puzzles.Model {

    using System;
    using UCM.IAV.Util;
    
    // contendra una matriz logica del juego con los tipos (0 a 3) de las casillas
    // que puede haber en el tablero (libre, agua, barro, rocas), elegidos aleatoriamente
    public class TankPuzzle : IDeepCloneable<TankPuzzle>
    {
        private System.Random rnd = new System.Random();

        public uint rows;                          // Dimensión de las filas       
        public uint columns;                       // Dimensión de las columnas

        private uint[,] matrix;                    // matriz de tipos de cada casilla
        
        public Position TankPosition { get; set; } // posicion logica del tanque (casilla en la que esta)
        public Position InitialTankPosition { get; set; } // posicion logica inicial del tanque
        
        private static readonly uint DEFAULT_ROWS = 3u;
        private static readonly uint DEFAULT_COLUMNS = 3u;
        
        public TankPuzzle() : this(DEFAULT_ROWS, DEFAULT_COLUMNS) { }
        
        public TankPuzzle(uint rows, uint columns) {
            if (rows == 0) throw new ArgumentException(string.Format("{0} is not a valid rows value", rows), "rows");
            if (columns == 0) throw new ArgumentException(string.Format("{0} is not a valid columns value", columns), "columns");

            this.Initialize(rows, columns);
        }

        // crea una nueva matriz de rows * cols de tipos de casilla aleatorios (del 0 al 3)
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
        
        // devuelve el tipo de la casilla en esa posicion
        public uint GetType(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");
            
            return matrix[position.GetRow(), position.GetColumn()];
        }

        // establece el tipo de la casilla en esa posicion
        public void SetType(Position position, uint type)
        {
            if (position == null) throw new ArgumentNullException(nameof(position));
            if (position.GetRow() >= rows) throw new ArgumentException(string.Format("{0} is not a valid row for this matrix", position.GetRow()), "row");
            if (position.GetColumn() >= columns) throw new ArgumentException(string.Format("{0} is not a valid column for this matrix", position.GetColumn()), "column");

            matrix[position.GetRow(), position.GetColumn()] = type;
        }

        // copia un juego recibido
        public TankPuzzle(TankPuzzle puzzle)
        {
            if (puzzle == null) throw new ArgumentNullException(nameof(puzzle));

            this.rows = puzzle.rows;
            this.columns = puzzle.columns;

            matrix = new uint[rows, columns];
            for (var r = 0u; r < rows; r++)
                for (var c = 0u; c < columns; c++)
                    matrix[r, c] = puzzle.matrix[r, c];

            TankPosition = puzzle.TankPosition;
        }

        // Devuelve este objeto clonado a nivel profundo
        public TankPuzzle DeepClone()
        {
            // Uso el constructor de copia para generar un clon
            return new TankPuzzle(this);
        }

        // Devuelve este objeto de tipo TankPuzzle clonado a nivel profundo 
        object IDeepCloneable.DeepClone()
        {
            return this.DeepClone();
        }

        public override string ToString() {
            return "Puzzle{" + string.Join(",", matrix) + "}";
        }

    }
}

