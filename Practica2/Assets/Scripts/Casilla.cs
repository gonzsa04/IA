namespace UCM.IAV.Puzzles
{
    using System;
    using UnityEngine;
    using Model;

    // casilla que puede ser del tipo de cualquier estancia
    // cada tipo con un valor y color distintos (representacion grafica)
    public class Casilla : MonoBehaviour
    {
        private Tablero tablero; // tablero de casillas
        private GameManager gm;

        // colores posibles y sus respectivos valores (dependen del tipo)
        private Color[] colors = {
            new Color(0.3f, 0.85f, 0.8f), new Color(0.5f, 0.5f, 0.5f), new Color(0.85f, 0.85f, 0),
            new Color(0.55f, 0, 0.81f), new Color(1, 0.71f, 0.76f), new Color(1, 0.55f, 0),
           new Color(0, 0.5f, 0), new Color(0.5f, 0, 0), new Color(0.54f, 0.27f, 0.1f)
        };

        // tipo y tipo inicial
        private TipoEstancia type;
        public bool tienePlayer = false;
        public bool tieneSuspect = false;

        // actualiza el color dependiendo del tipo de casilla que sea actualmente
        private void UpdateColor() { this.GetComponent<Renderer>().material.color = colors[(uint)this.type]; }
        
        public Position position; // posicion logica dentro de la matriz logica de puzzle

        public void Initialize(Tablero tablero, int type, bool tienePlayer = false, bool tieneSuspect = false)
        {
            gm = GameManager.instance;

            this.tablero = tablero;
            this.type = (TipoEstancia)type;
            this.tieneSuspect = tieneSuspect;
            this.tienePlayer = tienePlayer;

            UpdateColor();
        }

        // al ser pulsado, si es el turno del jugador, le movemos
        public bool OnMouseUpAsButton()
        {
            if (gm.isPlayerTurn())
                onClicked();
            
            return false;
        }

        // mueve al jugador actual a esta casilla
        public void onClicked()
        {
            if (tablero == null) throw new InvalidOperationException("This object has not been initialized");

            if (!gm.GameOver)
            {
                if (!tienePlayer && !tieneSuspect)
                {
                    gm.movePlayer(this.position, this.transform.position);
                }
                else gm.startCanMoveRoutine();
            }
        }
        
        public TipoEstancia getTypeEstancia() { return this.type; }

        // Cadena de texto representativa
        public override string ToString()
        {
            return "Casilla de tipo " + this.type.ToString() + " en " + position;
        }
    }
}