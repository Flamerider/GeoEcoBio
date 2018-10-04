using UnityEngine;
using System.Collections;

namespace geb
{
    public class minionMover : global
    {
        blockData bData;

        public float speed;

        void Start()
        {
            bData = GetComponent<blockData>();
        }

        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(bData.blockPos.x, bData.blockPos.y, bData.blockPos.z), speed * Time.deltaTime);
        }
    }
}