using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SettingMenu : MonoBehaviour
{
    public GameObject SettingsMenu;
    public Transform box;
    public CanvasGroup background;
    public void SetQuality(int qual)
    {
       QualitySettings.SetQualityLevel(qual);
    }

    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    //public void SetMusic(bool isMusic) { 
    //    theme.mute = !isMusic;  
    //}

    

    private void OnEnable()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f); //Burada karartma yani contrast arttirma islemi yapiyor. 0'dan 1 e 0.5f saniyede

        box.localPosition = new Vector2(0, -Screen.height); //baslangic pozisyonu (ekranda gozukmeyecek sekilde)
        box.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f; //yavasca kaymasini ve ne kadar surede yapmasi gerektigi
    }


    public void CloseDialog()
    {
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnComplete); //asagi kaysin


    }

    void OnComplete()
    {
        gameObject.SetActive(false);
    }
}
