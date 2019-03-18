namespace UCM.IAV.Puzzles {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Model;

    // clase ficha (sospechosos y jugadores)
    public class Ficha : MonoBehaviour {
        private string name_;     // nombre que escribira el texto

        public Position position; // posicion logica

        public void Initialize(string name, Position position)
        {
            this.name_ = name;
            this.position = position;
            this.gameObject.GetComponent<TextMesh>().text = name;
        }

        // set/get de la posicion fisica
        public void setPosition(Vector3 pos) { this.transform.position = pos; }
        public Vector3 getPosition() { return this.transform.position; }
    }
}
