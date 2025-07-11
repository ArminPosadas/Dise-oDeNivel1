using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void NewScene()
    {
        SceneManager.LoadScene("Test_Sergio");
    }
}
