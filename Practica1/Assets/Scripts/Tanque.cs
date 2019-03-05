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