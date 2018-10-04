using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace geb
{
    public class global : MonoBehaviour
    {
        public static int   maxLevelX = 15,
                            maxLevelY = 15,
                            maxLevelZ = 15;

        //BLOCK/ENTITY TYPES AND ID VALUES
        public static int
            BLOCK_NULL = 0,
            BLOCK_DIRT = 1,
            BLOCK_STONE = 2,
            ENTITY_MEEP_SPROUT_PLAINS = 3,
            ENTITY_MEEP_PLAINS = 4,
            ENTITY_GOAL = 5,
            BLOCK_WATER = 6,
            BLOCK_GRASS = 7,
            BLOCK_WATER_SPAWN = 8,
            BLOCK_GRAVEL = 9,
            ENTITY_GOAL_SPROUT = 10,
            ENTITY_MEEP_SPROUT_GREATER_PLAINS = 11,
            ENTITY_MEEP_GREATER_PLAINS = 12;

        //TOOL TYPES
        public static int
            TOOL_SHOVEL = 101,
            TOOL_PICK = 102,
            TOOL_AXE = 103;


        public struct GridPos
        {
            public int x, y, z;

            public GridPos(int gridx, int gridy, int gridz)
            {
                x = gridx;
                y = gridy;
                z = gridz;
            }
        }

        public bool BlockTypeIsSolid(int blockType)
        {
            switch(blockType)
            {
                case 1:
                    return true;
                case 2:
                    return true;
                case 7:
                    return true;
                case 8:
                    return true;
                case 9:
                    return true;
            }
            return false;
        }

        public bool BlockTypeIsSoft(int blockType)
        {
            switch (blockType)
            {
                case 1:
                    return true;
                case 7:
                    return true;
                case 9:
                    return true;
            }
            return false;
        }

        public bool BlockTypeIsDryGround(int blockType)
        {
            switch(blockType)
            {
                case 1:
                    return true;
            }
            return false;
        }

        public static void DataSave(saveData saveFile)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;
            if (!File.Exists(Application.persistentDataPath + "/saveData.dat"))
            {
                Debug.Log("No save file. Creating new");
                file = File.Create(Application.persistentDataPath + "/saveData.dat");
                bf.Serialize(file, saveFile);
                file.Close();
            }
            else
            {
                Debug.Log("Saving");
                file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.Open);
                bf.Serialize(file, saveFile);
                file.Close();
            }
        }

        public static saveData DataLoad()
        {
            if (File.Exists(Application.persistentDataPath + "/saveData.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.Open);
                saveData toReturn = (saveData)bf.Deserialize(file);
                file.Close();
                return toReturn;
            }
            else
                return new saveData();
        }
    }

    [System.Serializable]
    public class saveData
    {
        public float cameraSensitivity = 200;

        public List<string> completedLevels = new List<string>();
        
        public void ResetProgress()
        {
            completedLevels.Clear();
        }

        public void ResetOptions()
        {
            cameraSensitivity = 200;
        }
    }
}
