using UnityEngine;
using System.Collections;

namespace geb
{
    public class soilBlock : global
    {
        public float checkDelay;
        private float currTimer;

        private blockData bData;
        private levelManager lManage;

        public int fertilizedForm;

        void Start()
        {
            currTimer = checkDelay;
            bData = gameObject.GetComponent<blockData>();
            if (GameObject.Find("levelManager") != null)
                lManage = GameObject.Find("levelManager").GetComponent<levelManager>();
        }

        void Update()
        {
            if ((lManage != null) && (!BlockTypeIsSolid(lManage.ReturnAdjacentUp(bData))))
            {
                currTimer -= Time.deltaTime;

                if (currTimer <= 0)
                {
                    currTimer = checkDelay;

                    if ((lManage.ReturnAdjacentRight(bData) == BLOCK_WATER) ||
                        (lManage.ReturnAdjacentLeft(bData) == BLOCK_WATER) ||
                        (lManage.ReturnAdjacentForward(bData) == BLOCK_WATER) ||
                        (lManage.ReturnAdjacentBack(bData) == BLOCK_WATER))
                    {
                        lManage.ReplaceBlock(bData, fertilizedForm);
                    }

                    if ((lManage.ReturnAdjacentRight(bData) == BLOCK_GRASS) ||
                        (lManage.ReturnAdjacentLeft(bData) == BLOCK_GRASS) ||
                        (lManage.ReturnAdjacentForward(bData) == BLOCK_GRASS) ||
                        (lManage.ReturnAdjacentBack(bData) == BLOCK_GRASS))
                    {
                        lManage.ReplaceBlock(bData, fertilizedForm);
                    }
                }
            }
        }
    }
}