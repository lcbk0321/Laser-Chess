using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    // Update is called once per frame
    public void ChangeSC()
    {
        SceneManager.LoadScene("Laser Game");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
