using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    public int genislik;
    public int y�kseklik;

    public GameObject tilePrefab;

    public M�cevher[] mucevherler;

    public M�cevher[,] tumMucevherler; 

    public float mucevherHiz;

    public EslesmeController eslesmeController;

    public enum BoardDurum {bekliyor,hareketEdiyor } 
    public BoardDurum gecerliDurum = BoardDurum.hareketEdiyor;

    public M�cevher bomba;
    public float bombaCikmaSansi = 2f;
    private void Awake()
    {
        eslesmeController = Object.FindObjectOfType<EslesmeController>();
    }
    private void Start()
    {
        tumMucevherler = new M�cevher[genislik, y�kseklik]; 

        DuzenleFCN();
        
    }

    private void Update()
    {
        
    }


   
    void DuzenleFCN()
    {
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < y�kseklik; y++)
            {
                Vector2 pos = new Vector2(x, y); 
                GameObject bgTile = Instantiate(tilePrefab, pos, Quaternion.identity);
                bgTile.transform.parent = this.transform; 
                bgTile.name = "BG Tile -" + x + ", " + y; 
                int rastgeleMucevher = Random.Range(0, mucevherler.Length);

                
                int kontrolSayaci = 0; 
                while (EslesmeVarmiFNC(new Vector2Int(x,y),mucevherler[rastgeleMucevher]) && kontrolSayaci < 100)
                {
                    rastgeleMucevher = Random.Range(0, mucevherler.Length);
                    kontrolSayaci++;
                    if(kontrolSayaci > 0)
                    {

                    }
                }

                MucevherOlustur(new Vector2Int(x,y), mucevherler[rastgeleMucevher]);

            }
        }
    }

    void MucevherOlustur(Vector2Int pos,M�cevher olusacakMucevher)
    {

        if(Random.Range(0f,100f) < bombaCikmaSansi)
        {
            olusacakMucevher = bomba;
        }

        M�cevher mucevher = Instantiate(olusacakMucevher, new Vector3(pos.x, pos.y+y�kseklik, 0f), Quaternion.identity); 
        mucevher.transform.parent = this.transform;
        mucevher.name = "M�cevher - " + pos.x + ", " + pos.y;

        tumMucevherler[pos.x, pos.y] = mucevher; 
        mucevher.MucevheriDuzenle(pos, this); 
    }

   
    bool EslesmeVarmiFNC(Vector2Int posKontrol,M�cevher kontrolEdilenMucevher) 
    {
        if(posKontrol.x > 1)
        {
            //Kontrol edilen m�cevherin 1 �ncesine bak sonra 1 �ncesine daha bak e�er bunlar birbirine e�itse o zaman ko�
            if(tumMucevherler[posKontrol.x-1,posKontrol.y].tipi== kontrolEdilenMucevher.tipi && tumMucevherler[posKontrol.x-2,posKontrol.y].tipi == kontrolEdilenMucevher.tipi)
            {
                return true;        
            }
        }

        if (posKontrol.y > 1)
        {
            //Kontrol edilen m�cevherin 1 �ncesine bak sonra 1 �ncesine daha bak e�er bunlar birbirine e�itse o zaman ko�
            if (tumMucevherler[posKontrol.x, posKontrol.y-1].tipi == kontrolEdilenMucevher.tipi && tumMucevherler[posKontrol.x, posKontrol.y-2].tipi == kontrolEdilenMucevher.tipi)
            {
                return true;
            }
        }


        return false;
    }


    
    void EslesenMucevheriYokEt(Vector2Int pos)
    {
        if(tumMucevherler[pos.x,pos.y] != null)
        {
            if (tumMucevherler[pos.x, pos.y].eslestimi)
            {
                if(tumMucevherler[pos.x,pos.y].tipi == M�cevher.MucevherTipi.bomba)
                {
                    SoundManager.instance.PatlamaSesiCikar();
                }
                else
                {
                    SoundManager.instance.MucevherSesiCikar();
                }

                Instantiate(tumMucevherler[pos.x, pos.y].mucevherEfekt, new Vector2(pos.x, pos.y), Quaternion.identity); 
                Destroy(tumMucevherler[pos.x, pos.y].gameObject); 
                tumMucevherler[pos.x, pos.y] = null; 
            }
        }
    }

    public void TumEslesenleriYokEt()
    {
        for (int i = 0; i < eslesmeController.BulunanMucevherlerListe.Count; i++)
        {
            if (eslesmeController.BulunanMucevherlerListe[i] != null)
            {
                UIManager.instance.puaniArttirFNC(eslesmeController.BulunanMucevherlerListe[i].skorDegeri);
                EslesenMucevheriYokEt(eslesmeController.BulunanMucevherlerListe[i].posIndex);
            }
        }

        StartCoroutine(AltaKaydirRoutine());
    }

    
    IEnumerator AltaKaydirRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        int boslukSayac = 0; 

        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < y�kseklik; y++)
            {
                if(tumMucevherler[x,y] == null) 
                {
                    boslukSayac++;
                }
                else if(boslukSayac > 0)
                {
                    tumMucevherler[x, y].posIndex.y -= boslukSayac;
                    //patlayan m�cevherlerin yerine kaydirma
                    tumMucevherler[x, y - boslukSayac] = tumMucevherler[x, y];
                    tumMucevherler[x, y] = null;
                }
            }
            boslukSayac = 0;
        }

        StartCoroutine(BoardYenidenDoldurRouitine());
    }

    IEnumerator BoardYenidenDoldurRouitine()
    {
        yield return new WaitForSeconds(0.5f);
        UstBosluklariDoldurFNC();

        //yeniden dolan m�cevherlerden eslesenleri patlatma
        yield return new WaitForSeconds(0.5f);
        eslesmeController.EslesmeleriBulFNC();
        if(eslesmeController.BulunanMucevherlerListe.Count > 0)
        {
            yield return new WaitForSeconds(1.5f);
            TumEslesenleriYokEt();
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            gecerliDurum = BoardDurum.hareketEdiyor;
        }

    }

    void UstBosluklariDoldurFNC()
    {
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < y�kseklik; y++)
            {
                if(tumMucevherler[x,y] == null) 
                {
                    int rastgeleMucevher = Random.Range(0, mucevherler.Length);
                    MucevherOlustur(new Vector2Int(x, y), mucevherler[rastgeleMucevher]);
                }
               
            }
        }

        YanlisYerlestirmeleriKontrolEt();
    }

    void YanlisYerlestirmeleriKontrolEt()
    {
        List<M�cevher> bulunanMucevherList = new List<M�cevher>();

        bulunanMucevherList.AddRange(FindObjectsOfType<M�cevher>());

        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < y�kseklik; y++)
            {
                if (bulunanMucevherList.Contains(tumMucevherler[x, y]))
                {
                    bulunanMucevherList.Remove(tumMucevherler[x, y]);
                } 

            }
        }

        foreach (M�cevher mucevher in bulunanMucevherList)
        {
            Destroy(mucevher.gameObject);
        }

    }

    
    public void BoardKaristirFNC()
    {
        if(gecerliDurum != BoardDurum.bekliyor)
        {
            gecerliDurum = BoardDurum.bekliyor;

            List<M�cevher> sahnedekiMucevherlerList = new List<M�cevher>();

            
            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < y�kseklik; y++)
                {
                    sahnedekiMucevherlerList.Add(tumMucevherler[x, y]);
                    tumMucevherler[x, y] = null;
                }
            }

           
            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < y�kseklik; y++)
                {
                    int kullanilacakMucevher = Random.Range(0, sahnedekiMucevherlerList.Count);
                    int kontrolSayac = 0;
                    //eslesme varsa 
                    while(EslesmeVarmiFNC(new Vector2Int(x, y), sahnedekiMucevherlerList[kullanilacakMucevher])&& kontrolSayac < 100 && sahnedekiMucevherlerList.Count > 1)
                    {
                        kullanilacakMucevher = Random.Range(0, sahnedekiMucevherlerList.Count);
                        kontrolSayac++;
                    }

                    sahnedekiMucevherlerList[kullanilacakMucevher].MucevheriDuzenle(new Vector2Int(x, y), this);
                    tumMucevherler[x, y] = sahnedekiMucevherlerList[kullanilacakMucevher];
                    sahnedekiMucevherlerList.RemoveAt(kullanilacakMucevher);

                }
            }
            StartCoroutine(AltaKaydirRoutine());

        }
    }



}
