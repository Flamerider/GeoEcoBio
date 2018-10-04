using UnityEngine;
using System.Collections;

namespace geb
{
    public class fluidBlock : global
    {
        public float flowDelay;
        private float currTimer;

        private blockData bData;
        private levelManager lManage;

        void Start()
        {
            currTimer = flowDelay;
            bData = gameObject.GetComponent<blockData>();
            lManage = GameObject.Find("levelManager").GetComponent<levelManager>();
        }

        void Update()
        {
            currTimer -= Time.deltaTime;

            if (currTimer <= 0)
            {
                currTimer = flowDelay;

                if (lManage.ReturnAdjacentDown(bData) == BLOCK_NULL)
                {
                    lManage.CreateBlock(new GridPos(bData.blockPos.x, bData.blockPos.y - 1, bData.blockPos.z), bData.blockID);
                }
                else
                {
                    if (lManage.ReturnAdjacentDown(bData) != bData.blockID)
                    {
                        if (lManage.ReturnAdjacentLeft(bData) == BLOCK_NULL)
                            lManage.CreateBlock(new GridPos(bData.blockPos.x - 1, bData.blockPos.y, bData.blockPos.z), bData.blockID);
                        if (lManage.ReturnAdjacentRight(bData) == BLOCK_NULL)
                            lManage.CreateBlock(new GridPos(bData.blockPos.x + 1, bData.blockPos.y, bData.blockPos.z), bData.blockID);
                        if (lManage.ReturnAdjacentForward(bData) == BLOCK_NULL)
                            lManage.CreateBlock(new GridPos(bData.blockPos.x, bData.blockPos.y, bData.blockPos.z + 1), bData.blockID);
                        if (lManage.ReturnAdjacentBack(bData) == BLOCK_NULL)
                            lManage.CreateBlock(new GridPos(bData.blockPos.x, bData.blockPos.y, bData.blockPos.z - 1), bData.blockID);
                    }
                }
            }
        }
    }
}