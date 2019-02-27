namespace UCM.IAV.Puzzles
{

    using System;
    using UnityEngine;
    using Model;

    public class Tanque : MonoBehaviour
    {
        private Vector3 initialPos;

        public void Initialize()
        {
            UpdateColor();
            initialPos = this.transform.position;
        }

        public void setPosition(Vector3 pos)
        {
            this.transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
        }

        public void OnMouseUpAsButton()
        {
            GameManager.instance.changeTankSelected();
        }

        public void Reset()
        {
            this.transform.position = initialPos;
        }

        public void UpdateColor() {
            if (GameManager.instance.isTankSelected()) this.GetComponent<Renderer>().material.color = Color.red;
            else this.GetComponent<Renderer>().material.color = Color.green;
        }

        // Cadena de texto representativa
        public override string ToString()
        {
            return  this.transform.position.ToString();
        }
    }
}