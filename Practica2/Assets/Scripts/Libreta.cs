namespace UCM.IAV.Puzzles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Libreta : MonoBehaviour {

        private static readonly int DEFAULT_ROWS = 21;
        private static readonly int DEFAULT_COLUMNS = 3;

        public enum TipoLibreta { N, X, O };
        public TipoLibreta[,] libreta;
        public TipoEstancia estanciaActual;
        public int sospechosoActual;

        public void Initialize()
        {
            this.gameObject.SetActive(false);
            libreta = new TipoLibreta[DEFAULT_ROWS, DEFAULT_COLUMNS];
            for(int i = 0; i < DEFAULT_ROWS; i++)
            {
                for(int j = 0; j < DEFAULT_COLUMNS; j++)
                {
                    libreta[i, j] = TipoLibreta.N;
                }
            }
        }
    }
}
