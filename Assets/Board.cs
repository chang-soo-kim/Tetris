using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] Transform spawnPosition = null;
    [SerializeField] Transform nextspawnPosition = null;
    [SerializeField] Block[] prefabBlocks = null;
    [SerializeField] GuideBlock[] prefabGuideBlocks = null;


    const int WIDTH = 12;
    const int HEIGHT = 24;
    Transform[,] myBoard = new Transform[HEIGHT, WIDTH];
    int BlockIdx;
    Block instBlock = null;

    // ����
    int CurScore = 0;
    int HighScore
    {
        get { return PlayerPrefs.GetInt("highscore", 0); }
        set { PlayerPrefs.SetInt("highscore", value); }
    }

    [SerializeField] UIManager myUIManager = null;


    IEnumerator Start()
    {
        
        while (GameLogic.instance.IsGameStart == false )
        {
            yield return null;
        }

        myUIManager.UpdateScore(HighScore, CurScore);
        newRandomBlock();
        activeNextBlock();
        newRandomBlock();
    }

    void arriveAtBottom(Block block)
    {
        //���� ����� ���忡 ���
        if(!copyBlockToBoard(block))
        {
            Debug.Log("���ӿ���");
            myUIManager.ShowGameOver();
            return;
        }

        //��Ͽ� ���ؼ� ���� ���� �� á���� üũ
        // �� á���� �ش� �� ����
        checkFullLine();

        if(activeNextBlock()==false)
        {
            // ���� ����
            // ���� ����

            Debug.Log("## �׿���");
            myUIManager.ShowGameOver();

            return;
        }


        //���ο� ����� ����
        newRandomBlock();
    }

    void newRandomBlock()
    {
        BlockIdx = Random.Range(0, prefabBlocks.Length);
        instBlock = Instantiate<Block>(prefabBlocks[BlockIdx], nextspawnPosition.position, Quaternion.identity);
        instBlock.checkBoard = checkBoard;
        instBlock.arriveAtBottom = arriveAtBottom;

        instBlock.dropTime = Mathf.Clamp((1 - 0.1f * (CurScore/50)), 0.1f, 1f) ;
        instBlock.enabled = false;
    }

    // ��������� Ȱ��ȭ
    bool activeNextBlock()
    {

        
        instBlock.transform.position = spawnPosition.position;
        if (!checkBoard(instBlock))
        {
            return false;
        }
        instBlock.enabled = true; //

        GuideBlock instguideBlock = null;
        instguideBlock = Instantiate<GuideBlock>(prefabGuideBlocks[BlockIdx], spawnPosition.position, Quaternion.identity);
        instguideBlock.checkBoard = checkBoard;
        instguideBlock.HardDropRoutine(spawnPosition.position);
        instBlock.SetGuide(instguideBlock);
        return true;
    }


    bool checkBoard(ParentBlock block)
    {
        foreach(Transform child in block.transform)// block�� �ڽĵ��� ��ȸ
        {
            int idxX = Mathf.RoundToInt(child.position.x);
            int idxY = Mathf.RoundToInt(child.position.y);

            if(idxX <0 || idxX >= WIDTH || idxY < 0)
                return false;
            if(idxY >= HEIGHT)
                return true;
            if (myBoard[idxY, idxX] != null)
                return false;
             

        }
        return true;
    }

    bool copyBlockToBoard(Block block)
    {
        foreach (Transform child in block.transform)// block�� �ڽĵ��� ��ȸ
        {
            int idxX = Mathf.RoundToInt(child.position.x);
            int idxY = Mathf.RoundToInt(child.position.y);

            if(idxY >= HEIGHT)
                return false;

            myBoard[idxY, idxX] = child; // �ڽ� ���� ���忡 ���
        }
        return true;

    }
    void checkFullLine()
    {
        for (int ln = HEIGHT-1; ln >= 0; --ln)
        {
            //In�� ��á���� üũ
            if(isFullLine(ln))
            {
                // �� ã���� �ش� ���� �����ϰ� �ش��� ���� ������ �Ʒ��� �̵�
                deleteLine(ln);
                downLine(ln);


                CurScore += 10;
                
                if (HighScore < CurScore)
                    HighScore = CurScore;

                myUIManager.UpdateScore(HighScore, CurScore);


                

            }
        }
    }
    bool isFullLine(int ln)
    {
        for (int w = 0; w < WIDTH; ++w)
        {
            if (myBoard[ln,w] == null)
            {
                return false;
            }    
        }
        return true;
    }
    //ln ���� �����
    void deleteLine (int ln)
    {
        for (int w = 0; w < WIDTH; w++)
        {
        Destroy(myBoard[ln, w].gameObject);
        myBoard[ln, w] = null;
        }
    }
    // �� ���� ���� ����� ������ ä����.
    void downLine(int ln)
    {
        for (int h = ln; h < HEIGHT; h++)
        {
            for (int w = 0; w < WIDTH; w++)
            {
                if (h + 1 < HEIGHT && myBoard[h + 1,w] != null)
                {
                    myBoard[h, w] = myBoard[h+1, w];
                    myBoard[h, w].transform.position += Vector3.down;
                    myBoard[h + 1, w] = null;
                    
                }
            }
        }
    }
}
