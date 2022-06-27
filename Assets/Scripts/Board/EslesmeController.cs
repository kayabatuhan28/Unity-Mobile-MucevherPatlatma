using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EslesmeController : MonoBehaviour
{
    Board board;

    public List<M�cevher> BulunanMucevherlerListe = new List<M�cevher>(); 

    private void Awake()
    {
        board = Object.FindObjectOfType<Board>();
    }

    public void EslesmeleriBulFNC()
    {
        BulunanMucevherlerListe.Clear();

        for (int x = 0; x < board.genislik; x++)
        {
            for (int y = 0; y < board.y�kseklik; y++)
            {
                M�cevher gecerliMucevher = board.tumMucevherler[x, y];

                if (gecerliMucevher != null)
                {
                    
                    if (x > 0 && x < board.genislik-1)
                    {
                        
                        M�cevher solMucevher = board.tumMucevherler[x - 1, y];
                        M�cevher sagMucevher = board.tumMucevherler[x + 1, y]; 

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

                    if (y > 0 && y < board.y�kseklik - 1)
                    {
                        
                        M�cevher altMucevher = board.tumMucevherler[x, y-1];
                        M�cevher ustMucevher = board.tumMucevherler[x, y+1]; 

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

    //�nce bombay� buldururuz
    public void BombayiBulFNC()
    {
        for (int i = 0; i < BulunanMucevherlerListe.Count; i++)
        {
            M�cevher mucevher = BulunanMucevherlerListe[i];
            int x = mucevher.posIndex.x;
            int y = mucevher.posIndex.y;

            if(mucevher.posIndex.x > 0)
            {
                if(board.tumMucevherler[x-1,y] != null)
                {
                    if(board.tumMucevherler[x-1,y].tipi == M�cevher.MucevherTipi.bomba)
                    {
                        BombaBolgesiniIsaretleFNC(new Vector2Int(x-1,y),board.tumMucevherler[x-1,y]);
                    }
                }
            }

            if (mucevher.posIndex.x < board.genislik-1)
            {
                if (board.tumMucevherler[x + 1, y] != null)
                {
                    if (board.tumMucevherler[x +1, y].tipi == M�cevher.MucevherTipi.bomba)
                    {
                        BombaBolgesiniIsaretleFNC(new Vector2Int(x+1, y), board.tumMucevherler[x+1 , y]);
                    }
                }
            }

            if (mucevher.posIndex.y > 0)
            {
                if (board.tumMucevherler[x, y-1] != null)
                {
                    if (board.tumMucevherler[x , y-1].tipi == M�cevher.MucevherTipi.bomba)
                    {
                        BombaBolgesiniIsaretleFNC(new Vector2Int(x , y-1), board.tumMucevherler[x, y-1]);
                    }
                }
            }

            if (mucevher.posIndex.y < board.y�kseklik-1)
            {
                if (board.tumMucevherler[x , y+1] != null)
                {
                    if (board.tumMucevherler[x , y+1].tipi == M�cevher.MucevherTipi.bomba)
                    {
                        BombaBolgesiniIsaretleFNC(new Vector2Int(x , y+1), board.tumMucevherler[x, y+1]);
                    }
                }
            }

        }
    }

    public void BombaBolgesiniIsaretleFNC(Vector2Int bombaPos,M�cevher bomba) //Bomban�n oldu�u pozisyon birde bomban�n kendisi
    {
        for (int x = bombaPos.x -bomba.bombaHacmi; x <= bombaPos.x+bomba.bombaHacmi; x++) // E�er patlayacak bir yerdeyse bombahacmi kadar �ncesine bomba hacmi kadar sagina git
        {
            for (int y = bombaPos.y-bomba.bombaHacmi; y <= bombaPos.y+bomba.bombaHacmi; y++)
            {
                if(x >= 0 && x <board.genislik-1 && y >= 0 && y < board.y�kseklik - 1)//Patlama etksinin Tahtan�n d���na ��kmamas� i�in
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
