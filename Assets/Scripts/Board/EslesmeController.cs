using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EslesmeController : MonoBehaviour
{
    Board board;

    public List<Mücevher> BulunanMucevherlerListe = new List<Mücevher>(); 

    private void Awake()
    {
        board = Object.FindObjectOfType<Board>();
    }

    public void EslesmeleriBulFNC()
    {
        BulunanMucevherlerListe.Clear();

        for (int x = 0; x < board.genislik; x++)
        {
            for (int y = 0; y < board.yükseklik; y++)
            {
                Mücevher gecerliMucevher = board.tumMucevherler[x, y];

                if (gecerliMucevher != null)
                {
                    
                    if (x > 0 && x < board.genislik-1)
                    {
                        
                        Mücevher solMucevher = board.tumMucevherler[x - 1, y];
                        Mücevher sagMucevher = board.tumMucevherler[x + 1, y]; 

                        if (solMucevher != null && sagMucevher != null) 
                        {
                            if (solMucevher.tipi == gecerliMucevher.tipi && sagMucevher.tipi == gecerliMucevher.tipi) 
                            {
                                gecerliMucevher.eslestimi = true;
                                solMucevher.eslestimi = true;
                                sagMucevher.eslestimi = true;
                                BulunanMucevherlerListe.Add(gecerliMucevher);
                                BulunanMucevherlerListe.Add(solMucevher);
                                BulunanMucevherlerListe.Add(sagMucevher);

                            }
                        }
                    }

                    if (y > 0 && y < board.yükseklik - 1)
                    {
                        
                        Mücevher altMucevher = board.tumMucevherler[x, y-1];
                        Mücevher ustMucevher = board.tumMucevherler[x, y+1]; 

                        if (altMucevher != null && ustMucevher != null) 
                        {
                            if (altMucevher.tipi == gecerliMucevher.tipi && ustMucevher.tipi == gecerliMucevher.tipi) 
                            {
                                gecerliMucevher.eslestimi = true;
                                altMucevher.eslestimi = true;
                                ustMucevher.eslestimi = true;
                                BulunanMucevherlerListe.Add(gecerliMucevher);
                                BulunanMucevherlerListe.Add(altMucevher);
                                BulunanMucevherlerListe.Add(ustMucevher);

                            }
                        }
                    }

                }
            }
        }
        
        if(BulunanMucevherlerListe.Count > 0)
        {
            BulunanMucevherlerListe = BulunanMucevherlerListe.Distinct().ToList();
        }

        BombayiBulFNC();

    }

    //Önce bombayý buldururuz
    public void BombayiBulFNC()
    {
        for (int i = 0; i < BulunanMucevherlerListe.Count; i++)
        {
            Mücevher mucevher = BulunanMucevherlerListe[i];
            int x = mucevher.posIndex.x;
            int y = mucevher.posIndex.y;

            if(mucevher.posIndex.x > 0)
            {
                if(board.tumMucevherler[x-1,y] != null)
                {
                    if(board.tumMucevherler[x-1,y].tipi == Mücevher.MucevherTipi.bomba)
                    {
                        BombaBolgesiniIsaretleFNC(new Vector2Int(x-1,y),board.tumMucevherler[x-1,y]);
                    }
                }
            }

            if (mucevher.posIndex.x < board.genislik-1)
            {
                if (board.tumMucevherler[x + 1, y] != null)
                {
                    if (board.tumMucevherler[x +1, y].tipi == Mücevher.MucevherTipi.bomba)
                    {
                        BombaBolgesiniIsaretleFNC(new Vector2Int(x+1, y), board.tumMucevherler[x+1 , y]);
                    }
                }
            }

            if (mucevher.posIndex.y > 0)
            {
                if (board.tumMucevherler[x, y-1] != null)
                {
                    if (board.tumMucevherler[x , y-1].tipi == Mücevher.MucevherTipi.bomba)
                    {
                        BombaBolgesiniIsaretleFNC(new Vector2Int(x , y-1), board.tumMucevherler[x, y-1]);
                    }
                }
            }

            if (mucevher.posIndex.y < board.yükseklik-1)
            {
                if (board.tumMucevherler[x , y+1] != null)
                {
                    if (board.tumMucevherler[x , y+1].tipi == Mücevher.MucevherTipi.bomba)
                    {
                        BombaBolgesiniIsaretleFNC(new Vector2Int(x , y+1), board.tumMucevherler[x, y+1]);
                    }
                }
            }

        }
    }

    public void BombaBolgesiniIsaretleFNC(Vector2Int bombaPos,Mücevher bomba) //Bombanýn olduðu pozisyon birde bombanýn kendisi
    {
        for (int x = bombaPos.x -bomba.bombaHacmi; x <= bombaPos.x+bomba.bombaHacmi; x++) // Eðer patlayacak bir yerdeyse bombahacmi kadar öncesine bomba hacmi kadar sagina git
        {
            for (int y = bombaPos.y-bomba.bombaHacmi; y <= bombaPos.y+bomba.bombaHacmi; y++)
            {
                if(x >= 0 && x <board.genislik-1 && y >= 0 && y < board.yükseklik - 1)//Patlama etksinin Tahtanýn dýþýna çýkmamasý için
                {
                    if(board.tumMucevherler[x,y]!= null)
                    {
                        board.tumMucevherler[x, y].eslestimi = true;
                        BulunanMucevherlerListe.Add(board.tumMucevherler[x, y]);
                    }
                } 
            }
        }

        if (BulunanMucevherlerListe.Count > 0)
        {
            BulunanMucevherlerListe = BulunanMucevherlerListe.Distinct().ToList();
        }
    }









}
