using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mücevher : MonoBehaviour
{
    [HideInInspector] 
    public Vector2Int posIndex;

    [HideInInspector]
    public Board board;

    public Vector2 birinciBasilanPos;
    public Vector2 sonBasilanPos;

    bool mouseBasildi;
    float suruklemeAngle;

    Mücevher digerMucevher; 

    
    public enum MucevherTipi {mavi,pembe,sari,acikYesil,koyuYesil,bomba};
    public MucevherTipi tipi;

    
    public bool eslestimi;

    Vector2Int ilkPos;

    public GameObject mucevherEfekt;

    public int bombaHacmi;

    public int skorDegeri;


    
    public void MucevheriDuzenle(Vector2Int pos,Board theboard)
    {
        posIndex = pos;
        board = theboard;
    }

    private void Update()
    {
       
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.mucevherHiz * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
        }
        

        if(mouseBasildi && Input.GetMouseButtonUp(0)) 
        {
            mouseBasildi = false;

            if (board.gecerliDurum == Board.BoardDurum.hareketEdiyor && !UIManager.instance.turBittimi)
            {
                sonBasilanPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                HesaplaAngleFNC();
            }
                
        }
    }

    private void OnMouseDown() 
    {
        if(board.gecerliDurum == Board.BoardDurum.hareketEdiyor && !UIManager.instance.turBittimi)
        {
            
            
            birinciBasilanPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            mouseBasildi = true;
        }
      
    }
    
    
    void HesaplaAngleFNC()
    {
        float dx = sonBasilanPos.x - birinciBasilanPos.x;
        float dy = sonBasilanPos.y - birinciBasilanPos.y;

        suruklemeAngle = Mathf.Atan2(dy, dx); 
        
        suruklemeAngle = suruklemeAngle * 180 / Mathf.PI; 
        
        
        if(Vector3.Distance(birinciBasilanPos,sonBasilanPos) > 0.5f)
        {
            TileHareketFNC();
        }

    }

    
    void TileHareketFNC()
    {
        ilkPos = posIndex;

       
        if(suruklemeAngle < 45 && suruklemeAngle > -45 && posIndex.x < board.genislik-1) 
        {
            
            digerMucevher = board.tumMucevherler[posIndex.x + 1, posIndex.y];
            digerMucevher.posIndex.x--; 
            posIndex.x++; 
        }
        else if (suruklemeAngle > 45 && suruklemeAngle <= 135 && posIndex.y < board.yükseklik - 1)
        {
            
            digerMucevher = board.tumMucevherler[posIndex.x, posIndex.y+1];
            digerMucevher.posIndex.y--;
            posIndex.y++;
        }
        else if (suruklemeAngle < -45 && suruklemeAngle >= -135 && posIndex.y > 0)
        {
            
            digerMucevher = board.tumMucevherler[posIndex.x, posIndex.y - 1];
            digerMucevher.posIndex.y++; 
            posIndex.y--;
        }
        else if (suruklemeAngle > 135 || suruklemeAngle < -135 && posIndex.x > 0)
        {
          
            digerMucevher = board.tumMucevherler[posIndex.x-1, posIndex.y];
            digerMucevher.posIndex.x++; 
            posIndex.x--;
        }

        board.tumMucevherler[posIndex.x, posIndex.y] = this; 
        board.tumMucevherler[digerMucevher.posIndex.x, digerMucevher.posIndex.y] = digerMucevher;

        StartCoroutine(HareketiKontrolEtRoutine()); 
    }

  
    public IEnumerator HareketiKontrolEtRoutine()
    {
        board.gecerliDurum = Board.BoardDurum.bekliyor;

        yield return new WaitForSeconds(.5f);
        board.eslesmeController.EslesmeleriBulFNC();

        if(digerMucevher != null)
        {
            if(!eslestimi && !digerMucevher.eslestimi) 
            {
                digerMucevher.posIndex = posIndex; 
                posIndex = ilkPos; 

               
                board.tumMucevherler[posIndex.x, posIndex.y] = this; 
                board.tumMucevherler[digerMucevher.posIndex.x, digerMucevher.posIndex.y] = digerMucevher;

                
                yield return new WaitForSeconds(.5f);
                board.gecerliDurum = Board.BoardDurum.hareketEdiyor;
            }
            else
            {
                board.TumEslesenleriYokEt();
            }
        }
    }



}
