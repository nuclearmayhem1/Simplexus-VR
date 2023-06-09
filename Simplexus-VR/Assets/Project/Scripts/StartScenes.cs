using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScenes : MonoBehaviour
{
    public string scene1;
    public string scene2;
    public string scene3;
    public string scene4;

    public void StartScene(int num)
    {
        if (num == 1)
        {
            SceneManager.LoadScene(scene1);
        }
        if (num == 2)
        {
            SceneManager.LoadScene(scene2);
        }
        if (num == 3)
        {
            SceneManager.LoadScene(scene3);
        }
        if (num == 4)
        {
            SceneManager.LoadScene(scene4);
        }
    }

}
