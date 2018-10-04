using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace geb
{
    public class inventory : global
    {
        public int[] PlayerInventory = new int[7];
        [HideInInspector]
        public int currentlyHeldItem, currentlySelectedSlot;

        [HideInInspector]
        public bool paused = false, win = false;

        GameObject camPivot;
        GameObject padMarker;

        GameObject minionPointer;
        GameObject selectedMinion;

        GameObject toolHighlight;

        GameObject stepPad;

        private Vector3 newRot;

        levelManager gridParent;

        Ray pointerRay;
        RaycastHit hit;

        public float cameraSensitivity;

        protected GameObject[] slots;
        private GameObject pause_bg;

        private GameObject pauseMenu;
        private GameObject victoryMenu;

        public GameObject[] itemIcons;
        public GameObject[] toolIcons;

        void Start()
        {
            cameraSensitivity = DataLoad().cameraSensitivity;
            gridParent = gameObject.GetComponent<levelManager>();
            camPivot = GameObject.Find("cameraPivot");
            newRot = camPivot.transform.rotation.eulerAngles;

            
            padMarker = GameObject.Find("selector");

            padMarker.SetActive(false);

            pause_bg = GameObject.Find("pause_bg");
            pause_bg.SetActive(false);

            minionPointer = GameObject.Find("minionPointer");
            minionPointer.SetActive(false);

            stepPad = GameObject.Find("minionStep");
            stepPad.SetActive(false);

            pauseMenu = GameObject.Find("pausemenu");
            pauseMenu.SetActive(false);

            toolHighlight = GameObject.Find("toolHighlight");
            toolHighlight.SetActive(false);

            victoryMenu = GameObject.Find("victoryScreen");
            victoryMenu.SetActive(false);
        }

        void Update()
        {

            if (paused || win)
            {
                padMarker.SetActive(false);
                minionPointer.SetActive(false);
                stepPad.SetActive(false);
                toolHighlight.SetActive(false);
            }
            else if (Input.GetMouseButton(1))
            {
                newRot.y += ((Input.GetAxis("Mouse X") * cameraSensitivity) * Time.deltaTime);
                padMarker.SetActive(false);
                stepPad.SetActive(false);
                toolHighlight.SetActive(false);
                if (selectedMinion == null)
                    minionPointer.SetActive(false);
                else
                {
                    minionPointer.SetActive(true);
                    minionPointer.transform.Rotate(new Vector3(0, 80.0f * Time.deltaTime, 0));
                    minionPointer.transform.position = new Vector3(selectedMinion.transform.position.x,
                                                                    selectedMinion.transform.position.y - 0.3f,
                                                                    selectedMinion.transform.position.z);
                }
            }
            else if ((currentlyHeldItem >= 101) && (selectedMinion == null))
            {
                padMarker.SetActive(false);
                pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(pointerRay, out hit))
                {
                    blockData hitBlock = hit.transform.GetComponent<blockData>();
                    if (hitBlock != null)
                    {
                        if (gridParent.IsValidToolingLocation(currentlyHeldItem, hitBlock))
                        {
                            toolHighlight.SetActive(true);
                            toolHighlight.transform.position = hitBlock.transform.position;
                            if (Input.GetMouseButtonUp(0))
                            {
                                gridParent.DeleteBlock(hitBlock);

                                //Remove icon
                                Destroy(slots[currentlySelectedSlot - 1].transform.GetChild(0).gameObject);
                                //Deselect slot
                                slots[currentlySelectedSlot - 1].GetComponent<Image>().color = Color.black;
                                PlayerInventory[currentlySelectedSlot - 1] = 0;
                                currentlySelectedSlot = 0;
                                currentlyHeldItem = 0;
                                toolHighlight.SetActive(false);
                            }
                        }
                    }
                }
            }
            else if ((currentlyHeldItem != 0) && (selectedMinion == null))
            {
                toolHighlight.SetActive(false);
                pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(pointerRay, out hit))
                {
                    blockData hitBlock = hit.transform.GetComponent<blockData>();
                    if ((hitBlock != null) && (hitBlock.blockPos.y != maxLevelY))
                    {
                        if (!hitBlock.isEntity &&
                            (gridParent.levelBlocks[hitBlock.blockPos.x, hitBlock.blockPos.y + 1, hitBlock.blockPos.z] == BLOCK_NULL))
                        {
                            padMarker.SetActive(true);
                            padMarker.transform.position = new Vector3(hitBlock.transform.position.x,
                                                                       hitBlock.transform.position.y + 0.55f,
                                                                       hitBlock.transform.position.z);
                            if (Input.GetMouseButtonUp(0) && IsValidPlace(currentlyHeldItem, hitBlock.blockID))
                            {
                                gridParent.CreateBlock(new GridPos(hitBlock.blockPos.x, hitBlock.blockPos.y + 1, hitBlock.blockPos.z), currentlyHeldItem);

                                //Remove icon
                                Destroy(slots[currentlySelectedSlot - 1].transform.GetChild(0).gameObject);
                                //Deselect slot
                                slots[currentlySelectedSlot - 1].GetComponent<Image>().color = Color.black;
                                PlayerInventory[currentlySelectedSlot - 1] = 0;
                                currentlySelectedSlot = 0;
                                currentlyHeldItem = 0;
                                //Disable padMarker
                                padMarker.SetActive(false);
                            }

                        }
                        else
                        {
                            padMarker.SetActive(false);
                            minionPointer.SetActive(false);
                        }
                    }
                    else
                    {
                        //When there's no block data
                        padMarker.SetActive(false);
                        minionPointer.SetActive(false);
                    }
                }
                else
                {
                    //When there's no raycast hit.
                    padMarker.SetActive(false);
                    minionPointer.SetActive(false);
                }
            }
            else if (selectedMinion == null)
            {
                pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(pointerRay, out hit))
                {
                    if (hit.transform.tag == "minion")
                    {
                        minionPointer.SetActive(true);
                        minionPointer.transform.position = new Vector3(hit.transform.position.x,
                                                                        hit.transform.position.y - 0.3f,
                                                                        hit.transform.position.z);

                        if (Input.GetMouseButtonUp(0))
                        {
                            selectedMinion = hit.transform.gameObject;
                        }
                    }
                    else
                    {
                        minionPointer.SetActive(false);
                    }
                }
            }
            else if (selectedMinion != null)
            {
                toolHighlight.SetActive(false);
                minionPointer.SetActive(true);
                stepPad.SetActive(false);
                pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                minionPointer.transform.Rotate(new Vector3(0, 80.0f * Time.deltaTime, 0));
                minionPointer.transform.position = new Vector3(selectedMinion.transform.position.x,
                                                                    selectedMinion.transform.position.y - 0.3f,
                                                                    selectedMinion.transform.position.z);

                if (Physics.Raycast(pointerRay, out hit))
                {
                    if ((hit.transform.gameObject == selectedMinion) && (Input.GetMouseButtonUp(0)))
                    {
                        selectedMinion = null;
                        minionPointer.SetActive(false);
                    }
                    else if (hit.transform.tag == "minion")
                    {
                        minionPointer.transform.position = new Vector3(hit.transform.position.x,
                                                                        hit.transform.position.y - 0.3f,
                                                                        hit.transform.position.z);

                        if (Input.GetMouseButtonUp(0))
                        {
                            selectedMinion = hit.transform.gameObject;
                        }
                    }
                    else if ((hit.transform.GetComponent<blockData>() != null) && (hit.transform.GetComponent<blockData>().blockPos.y != maxLevelY))
                    {


                        blockData targetBlock = hit.transform.GetComponent<blockData>();
                        GridPos aboveTarget = new GridPos(targetBlock.blockPos.x, targetBlock.blockPos.y + 1, targetBlock.blockPos.z);

                        if ((gridParent.levelBlocks[aboveTarget.x, aboveTarget.y, aboveTarget.z] == ENTITY_GOAL) &&
                            (gridParent.IsValidMoveLocation(selectedMinion.GetComponent<blockData>(), targetBlock)))
                        {
                            stepPad.SetActive(true);
                            stepPad.transform.position = new Vector3(hit.transform.position.x,
                                                                     hit.transform.position.y + 0.6f,
                                                                     hit.transform.position.z);
                            if (Input.GetMouseButtonUp(0))
                            {
                                Win();
                                gridParent.DeleteBlock(GameObject.Find("goal(Clone)").GetComponent<blockData>());
                                gridParent.MoveEntity(selectedMinion.GetComponent<blockData>(), aboveTarget);
                            }
                        }
                        else if ((gridParent.levelBlocks[aboveTarget.x, aboveTarget.y, aboveTarget.z] == BLOCK_NULL) &&
                            (gridParent.IsValidMoveLocation(selectedMinion.GetComponent<blockData>(), targetBlock)))
                        {
                            stepPad.SetActive(true);
                            stepPad.transform.position = new Vector3(hit.transform.position.x,
                                                                     hit.transform.position.y + 0.6f,
                                                                     hit.transform.position.z);
                            if (Input.GetMouseButtonUp(0))
                            {
                                gridParent.MoveEntity(selectedMinion.GetComponent<blockData>(), aboveTarget);
                            }
                        }
                    }
                    else
                    {
                        minionPointer.SetActive(false);
                        stepPad.SetActive(false);
                    }
                }
            }
        }

        void FixedUpdate()
        {
            camPivot.transform.rotation = Quaternion.Euler(newRot);
        }

        public void SelectSlot(int slotNum)
        {
            if ((!Input.GetMouseButton(1)) && (PlayerInventory[slotNum - 1] != BLOCK_NULL) && !paused)
            {
                selectedMinion = null;
                minionPointer.SetActive(false);

                if (currentlySelectedSlot == 0)
                {
                    currentlySelectedSlot = slotNum;
                    currentlyHeldItem = PlayerInventory[slotNum - 1];
                    slots[slotNum - 1].GetComponent<Image>().color = Color.white;
                }
                else if (currentlySelectedSlot != slotNum)
                {
                    slots[currentlySelectedSlot - 1].GetComponent<Image>().color = Color.black;
                    currentlySelectedSlot = slotNum;
                    currentlyHeldItem = PlayerInventory[slotNum - 1];
                    slots[slotNum - 1].GetComponent<Image>().color = Color.white;
                }
                else if (currentlySelectedSlot == slotNum)
                {
                    slots[currentlySelectedSlot - 1].GetComponent<Image>().color = Color.black;
                    currentlySelectedSlot = 0;
                    currentlyHeldItem = 0;
                }
            }
        }

        public bool IsValidPlace(int item, int place)
        {
            if (item == ENTITY_MEEP_SPROUT_PLAINS)
            {
                if ((place == BLOCK_DIRT) ||
                    (place == BLOCK_GRASS))
                    return true;
            }
            
            return false;
        }

        public void Pause()
        {
            if (paused)
            {
                paused = false;
                pause_bg.SetActive(false);
                pauseMenu.SetActive(false);
            }
            else
            {
                paused = true;
                pause_bg.SetActive(true);
                pauseMenu.SetActive(true);
            }
        }

        public void Win()
        {
            win = true;
            pause_bg.SetActive(true);
            pauseMenu.SetActive(false);
            victoryMenu.SetActive(true);

            saveData file = DataLoad();
            if (!file.completedLevels.Contains(gridParent.levelID))
            {
                file.completedLevels.Add(gridParent.levelID);
                DataSave(file);
            }
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene("playArea");
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("mainMenu");
        }

        public void GenerateSlots()
        {
            slots = new GameObject[7];

            for (int i = 0; i < 7; i++)
            {
                slots[i] = GameObject.Find("slot" + (i + 1));
                slots[i].GetComponent<Image>().color = Color.black;
                if (PlayerInventory[i] != 0)
                {
                    if (PlayerInventory[i] < 101)
                    {
                        GameObject slotIcon = Instantiate(itemIcons[PlayerInventory[i]]);
                        slotIcon.transform.SetParent(slots[i].transform);
                        slotIcon.transform.position = slots[i].transform.position;
                    }
                    else
                    {
                        GameObject slotIcon = Instantiate(toolIcons[PlayerInventory[i] - 101]);
                        slotIcon.transform.SetParent(slots[i].transform);
                        slotIcon.transform.position = slots[i].transform.position;
                    }
                }
            }

        }
    }
}