namespace UCM.IAV.Puzzles {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    // clase ficha (sospechosos y jugadores)
    public class Ficha : MonoBehaviour {
        private Character character_;

        public string name_;      // nombre que escribira el texto
        public Position position; // posicion logica

        public void setCharacter(Character character) {
            character_ = character;
        }

        public Ficha() : base() { }

        public void Initialize(string name, Position position)
        {
            this.name_ = name;
            this.position = position;
            this.gameObject.GetComponent<TextMesh>().text = this.name_;
        }

        // al ser pulsado
        public bool OnMouseUpAsButton()
        {
            if (GameManager.instance.isPlayerTurn())
                character_.onClicked();
            return false;
        }

        // set/get de la posicion fisica
        public void setPosition(Vector3 pos) { this.transform.position = pos; }
        public void setLogicPosition(Position pos) { this.position = pos; }
        public Vector3 getPosition() { return this.transform.position; }
        public Position getLogicPosition() { return this.position; }
    }
}
