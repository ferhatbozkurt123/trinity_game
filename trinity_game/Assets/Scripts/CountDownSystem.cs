using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownSystem : MonoBehaviour
{
    /// <summary>
    /// Trinty 1.0
    /// Oyun baþlangýcýndaki 3 2 1 GO! sayacý
    /// </summary>

    [SerializeField] public int CountDown;           // Kalan süre (saniye)
    [SerializeField] public Text CountDownDisplay;       // Sayac Text 
    // Oyun baþladýðýnda Geri sayaç CoRoutine i baþlat
    void Start()
    {
        
    }

    public void CountDownStart()
    {
        //CountDownDisplay.gameObject.SetActive(false);
        CountDown = global.CountDownTime;
        StartCoroutine(CountDownRoutine());
    }

    IEnumerator CountDownRoutine()
    {
        float displayTime = 1f;                         // Her saniye sayacý yenile
        yield return new WaitForSeconds(0.2f);          // Az bekle
        CountDownDisplay.gameObject.SetActive(true);    // Sayacý aç

        // Sayaç 0 dan büyükse
        while (CountDown>0)             
        {
            CountDownDisplay.gameObject.SetActive(true);
            CountDownDisplay.transform.gameObject.LeanRotateZ(10, 0f);
            //CountDownDisplay.transform.localRotation = Vector3.zero;
            CountDownDisplay.transform.localScale = Vector3.one;
            //CountDownDisplay.transform.gameObject.LeanScale(new Vector3(1, 1, 1),0f);
            CountDownDisplay.transform.gameObject.LeanScale(new Vector3(1.8f,1.8f,1.8f),displayTime).setEaseOutQuart();
            CountDownDisplay.transform.gameObject.LeanRotateZ(-20,displayTime).setEaseOutQuart();
            CountDownDisplay.text = CountDown.ToString();
            yield return new WaitForSeconds(displayTime); // 1 saniye bekle

            CountDown--;            // Sayacý azalt
        }

        // Baþlama zamaný Geldi GO!
        CountDownDisplay.transform.gameObject.LeanRotateZ(0, 0f);
        CountDownDisplay.transform.gameObject.LeanScale(new Vector3(1, 1, 1), 0f);
        CountDownDisplay.text = "GO!";
        CountDownDisplay.transform.gameObject.LeanScale(new Vector3(1.7f, 1.7f, 1.7f), 0.5f).setEaseOutQuart();
        ///GameController.instance.BeginGame();
        global.BeginGame();                                 // Oyunu baþlat
        yield return new WaitForSeconds(1f);                // 1 sn bekle
        CountDownDisplay.gameObject.SetActive(false);       // Sayacý gizle
    }
}
