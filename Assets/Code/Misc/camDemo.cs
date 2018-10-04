using UnityEngine;
using System.Collections;

namespace geb
{

    public class camDemo : global
    {
        Vector3 targetPos;
        Vector3 targetRot;

        public float rotSpeed;
        public float dropSpeed;

        public bool demo = false;

        void Start()
        {
            targetPos = transform.position;
            targetRot = transform.rotation.eulerAngles;

            transform.position = new Vector3(transform.position.x, transform.position.y + 50.0f, transform.position.z);
        }

        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, dropSpeed * Time.deltaTime);

            if (demo)
            {
                targetRot.y += (rotSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(targetRot);
            }
        }
    }
}