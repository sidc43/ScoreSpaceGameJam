using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utility : MonoBehaviour
{
    public static bool LMB => Input.GetMouseButtonDown(0);

    public void Restart() { GOTO("Main"); }
    public static void GOTO(string scene)
    {
        Debug.Log("GOTO Called");
        SceneManager.LoadScene(scene);
    }
}
