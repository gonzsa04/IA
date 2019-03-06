namespace UCM.IAV.Puzzles
{
    using System;
    using UnityEngine;
    using Model;

    // casilla que puede ser de tipo libre, agua, barro o rocas, 
    // cada tipo con un valor y color distintos (representacion grafica)
    public class Casilla : MonoBehaviour
    {
        private Tablero tablero; // tablero de casillas

        // colores posibles y sus respectivos valores (dependen del tipo)
        private Color[] colors = { Color.gray, new Color(0.05f, 0.35f, 0.6f, 0),
            new Color(0.6f, 0.3f, 0.1f, 0), new Color(0.3f, 0.25f, 0.2f, 0) };


        // tipo, tipo inicial y valor, valor inicial
        private TipoCasilla type;
        private TipoCasilla initialType;
        private double value;
        private double initialValue;

        // actualiza el color dependiendo del tipo de casilla que sea actualmente
        private void UpdateColor() { this.GetComponent<Renderer>().material.color = colors[(uint)this.type]; }
        
        public Position position; // posicion logica dentro de la matriz logica de puzzle
        
        public void Initialize(Tablero tablero, uint type)
        {
            if (tablero == null) throw new ArgumentNullException(nameof(tablero));

            this.tablero = tablero;
            this.type = (TipoCasilla)type;
            this.initialType = this.type;
            this.value = GameManager.instance.values[type];
            this.initialValue = this.value;

            UpdateColor();
        }
        
        // al ser pulsado cambia al siguiente tipo (al llegar al ultimo da la vuelta)
        public bool OnMouseUpAsButton()
        {
            if (tablero == null) throw new InvalidOperationException("This object has not been initialized");

            // si el tanque no esta ya en movimiento
            if (!GameManager.instance.isTankMoving())
            { 
                // si el tanque esta seleccionado, empezamos a moverlo (esta casilla sera el destino)
                if (GameManager.instance.isTankSelected())
                {
                    if (this.type != TipoCasilla.Rocas)
                        StartCoroutine(GameManager.instance.createPath((int)this.position.GetRow(), (int)this.position.GetColumn()));
                    else StartCoroutine(GameManager.instance.changeCanMove());
                }
                // si no, esta casilla cambiara al tipo correspondiente
                else
                {
                    double prevValue = this.value;
                    TipoCasilla prevType = this.type;

                    if ((uint)this.type + 1 > 3) this.type = (TipoCasilla)0;
                    else this.type++;
                    this.value = GameManager.instance.values[(uint)this.type];

                    UpdateColor();

                    GameManager.instance.updatePuzzle(this.type, (int)this.position.GetRow(), (int)this.position.GetColumn());
                    GameManager.instance.updateGraph(prevType, prevValue, (int)this.position.GetRow(), (int)this.position.GetColumn());
                }
            }
                return false;
        }

        public double getValue() { return this.value; }
        public TipoCasilla getType() { return this.type; }
        public double getInitialValue() { return this.initialValue; }
        public uint getInitialType() { return (uint)this.initialType; }

        // establece su valor, tipo y color a los iniciales
        public void Reset() {
            GameManager.instance.updateGraph(this.type, this.value, (int)this.position.GetRow(), (int)this.position.GetColumn());
            this.value = this.initialValue; this.type = this.initialType; UpdateColor();
        }

        // Cadena de texto representativa
        public override string ToString()
        {
            return "Casilla de tipo " + this.type.ToString() + " en " + position;
        }
    }
}