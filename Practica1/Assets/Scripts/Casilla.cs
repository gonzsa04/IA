﻿namespace UCM.IAV.Puzzles
{
    using System;
    using UnityEngine;
    using Model;

    public enum TipoCasilla { Libre, Agua, Barro, Rocas };

    // casilla que puede ser de tipo libre, agua, barro o rocas, 
    // cada tipo con un valor y color distintos
    public class Casilla : MonoBehaviour
    {
        private Tablero tablero; // tablero de casillas

        // colores posibles y sus respectivos valores (dependen del tipo)
        private Color[] colors = { Color.white, Color.cyan, Color.yellow, Color.black };
        private double[] values = { 1, 2, 4, 1e9 };

        // tipo, tipo inicial y valor, valor inicial
        private TipoCasilla type;
        private TipoCasilla initialType;
        private double value;
        private double initialValue;

        // actualiza el color dependiendo del tipo de casilla que sea actualmente
        private void UpdateColor() { this.GetComponent<Renderer>().material.color = colors[(uint)this.type]; }
        
        public Position position;
        
        public void Initialize(Tablero tablero, uint type)
        {
            if (tablero == null) throw new ArgumentNullException(nameof(tablero));

            this.tablero = tablero;
            this.type = (TipoCasilla)type;
            this.initialType = this.type;
            this.value = values[type];
            this.initialValue = this.value;

            UpdateColor();

            Debug.Log(ToString() + " initialized.");
        }
        
        // al ser pulsado cambia al siguiente tipo (al llegar al ultimo da la vuelta)
        public bool OnMouseUpAsButton()
        {
            if (tablero == null) throw new InvalidOperationException("This object has not been initialized");

            if (GameManager.instance.isTankSelected())
            {
                if (this.type != TipoCasilla.Rocas)GameManager.instance.setTankPosition(this.transform.position);
                GameManager.instance.changeTankSelected();
            }
            else
            {
                if ((uint)this.type + 1 > 3) this.type = (TipoCasilla)0;
                else this.type++;
                this.value = values[(uint)this.type];

                UpdateColor();

            }
                return false;
        }

        public double getValue() { return this.value; }
        public double getInitialValue() { return this.initialValue; }
        public uint getInitialType() { return (uint)this.initialType; }

        // establece su valor, tipo y color a los iniciales
        public void Reset() { this.value = this.initialValue; this.type = this.initialType; UpdateColor(); }

        // Cadena de texto representativa
        public override string ToString()
        {
            return "Casilla de tipo " + this.type.ToString() + " en " + position;
        }
    }
}