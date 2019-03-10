namespace UCM.IAV.Puzzles
{
    using System;
    using UnityEngine;
    using Model;

    // tanque que se movera por el tablero
    public class Tanque : MonoBehaviour
    {
        private Vector3 initialPos; // posicion fisica

        public void Initialize()
        {
            UpdateColor();
            initialPos = this.transform.position;
        }

        public void setPosition(Vector3 pos)
        {
            this.transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
        }

        public void setRotation(Vector3 rot)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(this.transform.rotation.eulerAngles.x, rot.y, rot.z));
        }

        public void OnMouseUpAsButton()
        {
            if (!GameManager.instance.isTankMoving())
                GameManager.instance.changeTankSelected();
        }

        public void Reset()
        {
            this.transform.position = initialPos;
        }

        // cambia de color en funcion de si esta seleccionado o no
        public void UpdateColor() {
            int numOfChildren = transform.childCount;

            for (int i = 0; i < numOfChildren; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (GameManager.instance.isTankSelected()) child.GetComponent<Renderer>().material.color = new Color(0.6f, 0.1f, 0.1f, 0);
                else child.GetComponent<Renderer>().material.color = new Color(0.1f, 0.6f, 0.1f, 0);
            }
        }

        // Cadena de texto representativa
        public override string ToString()
        {
            return  this.transform.position.ToString();
        }
    }
}