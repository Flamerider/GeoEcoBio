using UnityEngine;
using System.Collections;

namespace geb
{
    public class persistentInfo : global
    {
        public string levelCode;

        void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}