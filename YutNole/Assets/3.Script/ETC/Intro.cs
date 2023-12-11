using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.isStart = true;
        SceneManager.LoadScene(1);
    }
}
