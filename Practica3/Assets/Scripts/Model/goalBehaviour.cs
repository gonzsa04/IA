namespace Model
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // lo tienen las porterias
    namespace Model
    {
        public class goalBehaviour : MonoBehaviour
        {
            public string Tag;
            public string team;

            // si colisiona con la pelota se añade un goal al equipo correspondiente
            private void OnTriggerEnter(Collider other)
            {
                if (other.tag == Tag) GameManager.instance.addScore(team);
            }
        }
    }
}

