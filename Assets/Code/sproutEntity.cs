using UnityEngine;
using System.Collections;

namespace geb
{
    public class sproutEntity : global
    {
        public int targetEntity;
        public bool requiresABlock;
        public int requiredBlock;

        public float plantedDelay;
        private float currTimer;

        private blockData bData;
        private levelManager lManage;

        void Start()
        {
            currTimer = plantedDelay;
            bData = gameObject.GetComponent<blockData>();
            lManage = GameObject.Find("levelManager").GetComponent<levelManager>();
        }

        void Update()
        {
            currTimer -= Time.deltaTime;
            if (currTimer < 0)
            {
                if (!requiresABlock)
                    lManage.ReplaceBlock(bData, targetEntity);
                else if (lManage.ReturnAdjacentDown(bData) == requiredBlock)
                    lManage.ReplaceBlock(bData, targetEntity);
            }
        }
    }
}