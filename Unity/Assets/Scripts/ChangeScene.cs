using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void ChangeSC()
    {
        SceneManager.LoadScene("Laser Game");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
