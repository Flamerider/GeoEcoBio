using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class introAnimate : MonoBehaviour
{
    Image namescreen;
    Text disclaimer;

    float timer;
    public float fadeSpeed;
    private float currentFade;

	// Use this for initialization
	void Start ()
    {
        namescreen = gameObject.GetComponent<Image>();
        disclaimer = GameObject.Find("disclaimer").GetComponent<Text>();

        timer = 0;
        currentFade = 0;

        disclaimer.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timer > 20.0f)
        {
            SceneManager.LoadSceneAsync("mainMenu");
        }
        else if (timer > 18.0f)
        {
            Debug.Log(currentFade);
            if (currentFade > 0)
                currentFade -= (fadeSpeed * Time.deltaTime);
            else
                currentFade = 0;
            disclaimer.color = new Color(255, 255, 255, currentFade);
        }
        else if (timer > 6.0f)
        {
            Debug.Log(currentFade);
            disclaimer.gameObject.SetActive(true);
            if (currentFade < 1)
                currentFade += (fadeSpeed * Time.deltaTime);
            else
                currentFade = 1;
            disclaimer.color = new Color(255, 255, 255, currentFade);
        }
	    else if (timer > 4.0f)
        {
            if (currentFade > 0)
                currentFade -= (fadeSpeed * Time.deltaTime);
            else
                currentFade = 0;
            namescreen.color = new Color(255, 255, 255, currentFade);
        }
        else if (timer > 1.0f)
        {
            if (currentFade < 1)
                currentFade += (fadeSpeed * Time.deltaTime);
            else
                currentFade = 1;
            namescreen.color = new Color(255, 255, 255, currentFade);
        }

        timer += Time.deltaTime;
	}
}
