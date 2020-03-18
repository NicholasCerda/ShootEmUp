using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int highScore=0;
    bool credits=false;
    private float timeLapse;
    void Start()
    {
        timeLapse = 0;
        Time.timeScale = 1.0f;
        if (gameObject.name!="StartB")
            DontDestroyOnLoad(this.gameObject);
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameObject.name == "StartB")
            {
                SceneManager.LoadScene("Level");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (credits)
        {
            timeLapse += Time.deltaTime;
            if (timeLapse >= 5)
            {
                MainMenu();
            }
        }
    }
    public void Credits()
    {
        credits = true;
        timeLapse = 0;
        SceneManager.LoadScene("Credits");
    }
    public void MainMenu()
    {
        credits = false;
        SceneManager.LoadScene("MainMenu");
    }
}
