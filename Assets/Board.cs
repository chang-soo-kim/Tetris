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

    // 점수
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
        //현재 블록을 보드에 기록
        if(!copyBlockToBoard(block))
        {
            Debug.Log("게임오버");
            myUIManager.ShowGameOver();
            return;
        }

        //블록에 의해서 쌓인 줄이 다 찼는지 체크
        // 다 찼으면 해당 줄 제거
        checkFullLine();

        if(activeNextBlock()==false)
        {
            // 게임 오버
            // 게임 멈춤

            Debug.Log("## 겜오버");
            myUIManager.ShowGameOver();

            return;
        }


        //새로운 블록을 생성
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

    // 다음블록을 활성화
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
        foreach(Transform child in block.transform)// block의 자식들을 순회
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
        foreach (Transform child in block.transform)// block의 자식들을 순회
        {
            int idxX = Mathf.RoundToInt(child.position.x);
            int idxY = Mathf.RoundToInt(child.position.y);

            if(idxY >= HEIGHT)
                return false;

            myBoard[idxY, idxX] = child; // 자식 블럭을 보드에 기록
        }
        return true;

    }
    void checkFullLine()
    {
        for (int ln = HEIGHT-1; ln >= 0; --ln)
        {
            //In이 다찼는지 체크
            if(isFullLine(ln))
            {
                // 다 찾으면 해당 줄을 삭제하고 해당줄 위에 블럭들을 아래로 이동
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
    //ln 한줄 지우기
    void deleteLine (int ln)
    {
        for (int w = 0; w < WIDTH; w++)
        {
        Destroy(myBoard[ln, w].gameObject);
        myBoard[ln, w] = null;
        }
    }
    // 빈 줄을 위에 블록을 내려서 채우자.
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
