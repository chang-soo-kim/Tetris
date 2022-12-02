using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;



public class Block : ParentBlock
{
    [SerializeField] Vector3 blockPivot = Vector3.zero;

    //public Action newBlock = null; // 블록 새로 생성
    public Action<Block> arriveAtBottom = null; // 블록을 보드에 복사 기록

    float oldTime = 0f;
    GuideBlock guideBlock = null;
    public float dropTime = 1f; //떨어지는 시간 간격;
    public void SetGuide(GuideBlock guideBlock)
    {
        this.guideBlock = guideBlock;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left, Space.World);
            if (!checkBoard(this))
            {
                transform.Translate(Vector3.right, Space.World);
            }
            else
            {
                //guideBlock.transform.Translate(Vector3.left, Space.World);
                guideBlock.HardDropRoutine(transform.position);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right, Space.World);
            if (!checkBoard(this))
            {
                transform.Translate(Vector3.left, Space.World);
            }
            else
            {
                //guideBlock.transform.Translate(Vector3.right, Space.World);
                guideBlock.HardDropRoutine(transform.position);
            }
        }
        //회전
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(blockPivot), Vector3.forward, 90);
            if (!checkBoard(this))
            {
                transform.RotateAround(transform.TransformPoint(blockPivot), Vector3.forward, -90);
            }
            else
            {
                guideBlock.transform.RotateAround(transform.TransformPoint(guideBlock.transform.position), Vector3.forward, 90);
                guideBlock.HardDropRoutine(transform.position);
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (true)
            {
                if (!lineDropRoutine())
                {
                    arriveAtBottom(this);
                    Destroy(guideBlock.gameObject);
                    this.enabled = false;
                    return;
                }

            }
        }

        if (Time.time - oldTime >= (Input.GetKey(KeyCode.DownArrow) ? dropTime * 0.1f : dropTime))
        {

            if (!lineDropRoutine())
            {
                arriveAtBottom(this);
                Destroy(guideBlock.gameObject);
                this.enabled = false;
                return;
            }

            oldTime = Time.time;
        }

    }



    

}
