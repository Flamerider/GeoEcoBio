using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace geb
{
    public class mainMenu : global
    {
        GameObject menuMain, menuOptions;

        GameObject[] levelSelects = new GameObject[1];

        protected saveData savedStuff;

        void Start()
        {
            menuMain = GameObject.Find("main");
            menuOptions = GameObject.Find("options");

            GameObject.Find("camsens_value").GetComponent<Text>().text = GameObject.Find("camsensitivity").GetComponent<Slider>().value.ToString();

            menuOptions.SetActive(false);

            savedStuff = DataLoad();
            for (int i = 0; i < levelSelects.Length; i++)
            {
                Debug.Log("levelSelect" + i);
                levelSelects[i] = GameObject.Find("levelSelect" + i);
                levelSelects[i].SetActive(false);
            }
        }

        void Update()
        {
            
        }


        public void SwitchMenu(string menuState)
        {
            if (menuState == ("main"))
            {
                if (menuOptions.activeInHierarchy)
                {
                    DataSave(savedStuff);
                }

                menuMain.SetActive(true);
                menuOptions.SetActive(false);
                foreach (GameObject selectScreen in levelSelects)
                {
                    selectScreen.SetActive(false);
                }
            }
            else if (menuState == ("options"))
            {
                menuMain.SetActive(false);
                menuOptions.SetActive(true);
            }
            else if (menuState == ("levelSelect1"))
            {
                menuMain.SetActive(false);
                levelSelects[0].SetActive(true);
            }
        }

        public void UpdateCameraSensitivity()
        {
            GameObject.Find("camsens_value").GetComponent<Text>().text = GameObject.Find("camsensitivity").GetComponent<Slider>().value.ToString();
            savedStuff.cameraSensitivity = GameObject.Find("camsensitivity").GetComponent<Slider>().value;
        }

        public void LoadLevel(string levelID)
        {
            GameObject.Find("persistentInfo").GetComponent<persistentInfo>().levelCode = levelID;
            SceneManager.LoadScene("playArea");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
