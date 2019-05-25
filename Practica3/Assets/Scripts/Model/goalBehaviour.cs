namespace Model
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    namespace Model
    {
        public class goalBehaviour : MonoBehaviour
        {
            public string Tag;
            public string team;

            private void OnTriggerEnter(Collider other)
            {
                if (other.tag == Tag) GameManager.instance.addScore(team);
            }
        }
    }
}

