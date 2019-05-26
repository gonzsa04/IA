using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// lo tienen el jugador y la pelota
namespace Model
{
    public class Reset : MonoBehaviour
    {

        Vector3 initialPos = new Vector3();
        Vector3 initialVel = new Vector3();
        Vector3 initialAnVel = new Vector3();
        Quaternion initialRot = new Quaternion();
        Rigidbody rb;

        void Awake()
        {
            initialPos.Set(transform.position.x, transform.position.y, transform.position.z);
            initialRot.Set(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            rb = gameObject.GetComponent<Rigidbody>();
        }

        // restaura la posicion inicial (jugador y pelota)
        void resetTrans()
        {
            transform.position = initialPos;
            transform.rotation = initialRot;

            if (rb != null)
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
                rb.angularVelocity = new Vector3(0f, 0f, 0f);
            }
        }

        // activa/desactiva el rigidbody para reiniciar velocidades e inercias (pelota)
        void toggleRB()
        {
            if (rb != null)
            {
                if (rb.isKinematic)
                {
                    rb.isKinematic = false;
                    rb.velocity = initialVel;
                    rb.angularVelocity = initialAnVel;
                }
                else
                {
                    initialVel = rb.velocity;
                    initialAnVel = rb.angularVelocity;
                    rb.isKinematic = true;
                }
            }
        }
    }
}
