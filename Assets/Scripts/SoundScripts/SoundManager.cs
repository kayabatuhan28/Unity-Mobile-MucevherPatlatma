using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    public AudioSource sahne1,mucevherSesi, patlamaSesi, oyunBittiSesi;

    public void MucevherSesiCikar()
    {
        mucevherSesi.Stop();

        mucevherSesi.pitch = Random.Range(0.8f, 1.2f);

        mucevherSesi.Play();
    }

    public void PatlamaSesiCikar()
    {
        patlamaSesi.Stop();

        patlamaSesi.pitch = Random.Range(0.8f, 1.2f);

        patlamaSesi.Play();
    }

    public void OyunBittiSesiCikar()
    {
        sahne1.Stop();
        oyunBittiSesi.Play();
    }


}
