using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public GameObject Icon;
    public Text txtLoading;
    void Start()
    {
        Icon.transform.LeanScale(new Vector3(1.4f, 1.4f, 1.4f), 2f).setEaseInOutQuart().setLoopPingPong();
        txtLoading.transform.LeanScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).setEaseInOutQuart().setLoopPingPong();
        //txtLoading.text = global.LoadingSceneName + " Loading...";
        //SceneManager.LoadScene("SampleScene");
        StartCoroutine(startLoading());
    }

    IEnumerator startLoading()
    {
        Debug.Log("Loading baþladý.");
        // Patlama süresine kadar bekle
        yield return new WaitForSeconds(1.5f);
        // Süre dolunca Patlama gerçekleþsin
        SceneManager.LoadScene(global.LoadingSceneName);
        //SceneManager.LoadScene("SampleScene");
        //StopCoroutine(startLoading());   // CoRoutine i hafýzadan siliyoruz
    }
}
