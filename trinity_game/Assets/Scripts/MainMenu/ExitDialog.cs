using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class ExitDialog : MonoBehaviour
{
    public Transform box;
    public CanvasGroup background;

    private void OnEnable()
    {
        
        background.alpha = 0;
        background.LeanAlpha(1,0.5f); //Burada karartma yani contrast arttirma islemi yapiyor. 0'dan 1 e 0.5f saniyede

        box.localPosition = new Vector2(0, -Screen.height); //baslangic pozisyonu (ekranda gozukmeyecek sekilde)
        box.LeanMoveLocalY(0,0f).setEaseOutExpo().delay = 0.1f; //yavasca kaymasini ve ne kadar surede yapmasi gerektigi
    }


    public void CloseDialog()
    {
        background.LeanAlpha(0,0.5f); 
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnComplete); //asagi kaysin
        

    }

    void OnComplete()
    {
        //gameObject.SetActive(false);
        box.gameObject.SetActive(false);
    }
}
