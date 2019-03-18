namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Libreta : MonoBehaviour {

        private enum TipoLibreta { nada, noTiene, tiene };
        private TipoLibreta[,] libreta;
        private static readonly int DEFAULT_ROWS = 21;
        private static readonly int DEFAULT_COLUMNS = 3;

        public void Initialize()
        {
            libreta = new TipoLibreta[DEFAULT_ROWS, DEFAULT_COLUMNS];
            for(int i = 0; i < DEFAULT_ROWS; i++)
            {
                for(int j = 0; j < DEFAULT_COLUMNS; j++)
                {
                    libreta[i, j] = TipoLibreta.nada;
                }
            }
        }
    }
}
