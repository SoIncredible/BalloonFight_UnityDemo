using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadScene01()
    {
        SceneManager.LoadScene("Level01");
    }
    public void LoadScene02()
    {
        SceneManager.LoadScene("Level02");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
