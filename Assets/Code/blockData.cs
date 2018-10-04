using UnityEngine;
using System.Collections;

namespace geb
{
    public class blockData : global
    {
        public GridPos blockPos;
        public int blockID;
        public bool isEntity;
        public bool walkable;
        public bool collectable;
    }
}