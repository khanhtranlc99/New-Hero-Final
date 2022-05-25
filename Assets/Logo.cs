using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Logo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Test",2.2f);
    }

   private void Test()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
        CancelInvoke();
    }
}
