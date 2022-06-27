using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [SerializeField]
    TMP_Text kalanZamanTxt;

    [SerializeField]
    TMP_Text skorTxt,bitisMevcutSkorTxt,bitisMaxSkorTxt;

    public int kalanZaman = 5;

    public bool turBittimi,oyundurdumu;

    public string sahneAdi;

    [SerializeField]
    GameObject turSonucPanel,board,sonucskor,mevcutskortxt,mevcutskor,maxskortxt,maxskor,yenidenOynaBtn;

    [SerializeField]
    GameObject pauseButton;


    public Sprite PauseImg, ResumeImg;

    [HideInInspector]
    public int gecerliPuan,maxPuan;

    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        turBittimi = false;
        StartCoroutine(GeriSayRoutine());

    }

    IEnumerator GeriSayRoutine()
    {
        while (kalanZaman > 0)
        {
            yield return new WaitForSeconds(1f);
            kalanZamanTxt.text = kalanZaman.ToString() + " s";
            kalanZaman--;

            if(kalanZaman <= 0)
            {
                //oyun bitti
                SoundManager.instance.OyunBittiSesiCikar();
                turBittimi = true;
                board.SetActive(false);
                turSonucPanel.SetActive(true);
                StartCoroutine(BitisEkraniniAc());
            }
        }
    }

    public void puaniArttirFNC(int gelenPuan)
    {
        gecerliPuan += gelenPuan;
        skorTxt.text = gecerliPuan.ToString() + " Puan";
    }

    public void GüncelSkorBitisEkrani()
    {
        bitisMevcutSkorTxt.text = gecerliPuan.ToString();
        if(gecerliPuan >= maxPuan)
        {
            maxPuan = gecerliPuan;
            bitisMaxSkorTxt.text = gecerliPuan.ToString();
        }
    }

    IEnumerator BitisEkraniniAc()
    {
        yield return new WaitForSeconds(0.1f);
        turSonucPanel.GetComponent<CanvasGroup>().DOFade(1, 1f);
        yield return new WaitForSeconds(0.5f);
        sonucskor.GetComponent<CanvasGroup>().DOFade(1, 1f);
        yield return new WaitForSeconds(0.5f);
        mevcutskortxt.GetComponent<CanvasGroup>().DOFade(1, 1f);
        GüncelSkorBitisEkrani();
        yield return new WaitForSeconds(0.5f);
        mevcutskor.GetComponent<CanvasGroup>().DOFade(1, 1f);
        yield return new WaitForSeconds(0.5f);
        maxskortxt.GetComponent<CanvasGroup>().DOFade(1, 1f);
        yield return new WaitForSeconds(0.5f);
        maxskor.GetComponent<CanvasGroup>().DOFade(1, 1f);
        yield return new WaitForSeconds(0.5f);
        yenidenOynaBtn.GetComponent<CanvasGroup>().DOFade(1, 1f);

    }

    public void YenidenOynaFNC()
    {
        SceneManager.LoadScene(sahneAdi);
    }

    public void OyunuDurdurAc()
    {
        if(oyundurdumu == false)
        {
            SoundManager.instance.sahne1.Stop();
            pauseButton.GetComponent<Image>().sprite = ResumeImg;
            Time.timeScale = 0f; // Oyundaki her þeyi durdurur
            oyundurdumu = true;
        }
        else
        {
            SoundManager.instance.sahne1.Play();
            pauseButton.GetComponent<Image>().sprite = PauseImg;
            Time.timeScale = 1f; // Oyundaki her þeyi açar
            oyundurdumu = false;
        }
    }
        
}
