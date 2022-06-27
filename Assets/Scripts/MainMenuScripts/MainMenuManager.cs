using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{

    public string sahneAdi;

    public void OyundanCikFNC()
    {
        Application.Quit();
    }


    public void OyunuBaslatFNC()
    {
        SceneManager.LoadScene(sahneAdi);
    }

}
