using UnityEngine;
using System.Collections;
using System.IO;

namespace geb
{
    public class levelManager : global
    {
        protected TextAsset levelDataFile;
        protected TextAsset invDataFile;
        public bool editorIDOverride = false;
        public string levelID;

        //Create a 3D Array for storing the block data
        public int[,,] levelBlocks = new int[maxLevelX, maxLevelY, maxLevelZ];

        //An array of all block types.
        public GameObject[] blockTypes = new GameObject[20];

        // Use this for initialization
        void Start()
        {
            if (!editorIDOverride)
            {
                levelID = GameObject.Find("persistentInfo").GetComponent<persistentInfo>().levelCode;
            }
            GetLevelLayout(levelID);
            GenerateLevel();
            GetInventory(levelID);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateBlock(GridPos newPos, int bID)
        {
            GameObject newBlock = Instantiate(blockTypes[bID]);
            newBlock.transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
            newBlock.GetComponent<blockData>().blockPos = newPos;
            levelBlocks[newPos.x, newPos.y, newPos.z] = bID;
        }

        public void DeleteBlock(blockData block)
        {
            levelBlocks[block.blockPos.x, block.blockPos.y, block.blockPos.z] = BLOCK_NULL;
            Destroy(block.gameObject);
        }

        public void ReplaceBlock(blockData block, int newBlock)
        {
            levelBlocks[block.blockPos.x, block.blockPos.y, block.blockPos.z] = newBlock;
            CreateBlock(block.blockPos, newBlock);
            Destroy(block.gameObject);
        }

        public void MoveEntity(blockData entity, GridPos newPos)
        {
            levelBlocks[entity.blockPos.x, entity.blockPos.y, entity.blockPos.z] = BLOCK_NULL;
            levelBlocks[newPos.x, newPos.y, newPos.z] = entity.blockID;
            entity.blockPos = newPos;
        }

        public bool IsValidMoveLocation(blockData mover, blockData target)
        {
            if (target.blockPos.y == maxLevelY)
            {
                return false;
            }
            else if ((mover.blockID == ENTITY_MEEP_PLAINS))
            {
                //If the mover is a basic meep, without the ability to navigate upwards without plants.
                if ((mover.blockPos.y != (target.blockPos.y + 1)) && (mover.blockPos.y != target.blockPos.y))
                    return false;
                else
                {
                    int totalMovement = 0;

                    totalMovement += Mathf.Abs((target.blockPos.x - mover.blockPos.x));
                    totalMovement += Mathf.Abs((target.blockPos.z - mover.blockPos.z));

                    if ((totalMovement == 1) && (target.walkable))
                    {
                        return true;
                    }
                }
            }
            else if ((mover.blockID == ENTITY_MEEP_GREATER_PLAINS))
            {
                //If the mover is a greater meep, with the ability to navigate upwards without plants.
                if ((mover.blockPos.y != (target.blockPos.y + 2)) && (mover.blockPos.y != target.blockPos.y) && (mover.blockPos.y != (target.blockPos.y + 1)))
                    return false;
                else
                {
                    int totalMovement = 0;

                    totalMovement += Mathf.Abs((target.blockPos.x - mover.blockPos.x));
                    totalMovement += Mathf.Abs((target.blockPos.z - mover.blockPos.z));

                    if ((totalMovement == 1) && (target.walkable))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsValidToolingLocation(int tool, blockData target)
        {
            if ((tool == TOOL_SHOVEL) && (BlockTypeIsSoft(target.blockID)))
            {
                GameObject[] allMinions = GameObject.FindGameObjectsWithTag("minion");

                if (allMinions.Length == 0)
                    return false;
                foreach (GameObject item in allMinions)
                {
                    GridPos minionPos = item.GetComponent<blockData>().blockPos;

                    //If block is by the minion's feet and not holding up something.
                    if ((target.blockPos.y == (minionPos.y - 1)) && (ReturnAdjacentUp(target) == BLOCK_NULL))
                    {
                        int totalMovement = 0;

                        totalMovement += Mathf.Abs((target.blockPos.x - minionPos.x));
                        totalMovement += Mathf.Abs((target.blockPos.z - minionPos.z));

                        if (totalMovement == 1)
                        {
                            return true;
                        }
                    }
                    //If block is level with the minion
                    else if (target.blockPos.y == minionPos.y)
                    {
                        int totalMovement = 0;

                        totalMovement += Mathf.Abs((target.blockPos.x - minionPos.x));
                        totalMovement += Mathf.Abs((target.blockPos.z - minionPos.z));

                        if (totalMovement == 1)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            else
                return false;
        }

        public int ReturnAdjacentUp(blockData block)
        {
            if (block.blockPos.y != maxLevelY)
                return levelBlocks[block.blockPos.x, block.blockPos.y + 1, block.blockPos.z];
            else
                return -1;
        }

        public int ReturnAdjacentDown(blockData block)
        {
            if (block.blockPos.y != 0)
                return levelBlocks[block.blockPos.x, block.blockPos.y - 1, block.blockPos.z];
            else
                return -1;
        }

        public int ReturnAdjacentRight(blockData block)
        {
            if (block.blockPos.x != maxLevelX)
                return levelBlocks[block.blockPos.x + 1, block.blockPos.y, block.blockPos.z];
            else
                return -1;
        }

        public int ReturnAdjacentLeft(blockData block)
        {
            if (block.blockPos.x != 0)
                return levelBlocks[block.blockPos.x - 1, block.blockPos.y, block.blockPos.z];
            else
                return -1;
        }

        public int ReturnAdjacentForward(blockData block)
        {
            if (block.blockPos.z != maxLevelZ)
                return levelBlocks[block.blockPos.x, block.blockPos.y, block.blockPos.z + 1];
            else
                return -1;
        }

        public int ReturnAdjacentBack(blockData block)
        {
            if (block.blockPos.z != 0)
                return levelBlocks[block.blockPos.x, block.blockPos.y, block.blockPos.z - 1];
            else
                return -1;
        }

        //For each value in the level's grid data, check the blockID and then spawn it if it's not 0.
        void GenerateLevel()
        {
            for (int currY = 0; currY < maxLevelY; currY++)
            {
                for (int currX = 0; currX < maxLevelX; currX++)
                {
                    for (int currZ = 0; currZ < maxLevelZ; currZ++)
                    {
                        //Because of the way the code reads the level data it is specifically inputted into the array y then z then x
                        if (levelBlocks[currX, currY, currZ] != BLOCK_NULL)
                        {
                            CreateBlock(new GridPos(currX, currY, currZ), levelBlocks[currX, currY, currZ]);
                        }
                    }
                }
            }
        }

        void GetLevelLayout(string level_id)
        {
            //Load the text file. It's name MUST match the inputted level_id and it must be in the /levels domain in Assets
            levelDataFile = (TextAsset)Resources.Load("levels/" + level_id, typeof(TextAsset));


            //Split all the "rows" into one huge string. To reduce complexity, the step of splitting them by y is taken out.
            //It reads the first 'maxLevelY' number of rows as it would be on one floor, then reads the next set as the one above it.
            //And so on.
            string[] levelDataLines = levelDataFile.text.Split('\n');

            for (int cY = 0; cY < maxLevelY; cY++)
            {
                for (int cX = 0; cX < maxLevelX; cX++)
                {
                    for (int cZ = 0; cZ < maxLevelZ; cZ++)
                    {
                        levelBlocks[cX, cY, cZ] = int.Parse(levelDataLines[cX + (cY * maxLevelX)].Split(',')[cZ]);
                    }
                }
            }
        }

        void GetInventory(string level_id)
        {
            invDataFile = (TextAsset)Resources.Load("levels/" + level_id + "inv", typeof(TextAsset));

            string[] invDataLines = invDataFile.text.Split(',');

            inventory invVals = gameObject.GetComponent<inventory>();

            for (int i = 0; i < invDataLines.Length; i++)
            {
                invVals.PlayerInventory[i] = int.Parse(invDataLines[i]);
            }
            invVals.GenerateSlots();
        }
    }
}