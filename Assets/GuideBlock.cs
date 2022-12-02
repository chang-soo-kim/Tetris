using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideBlock : ParentBlock
{
    // 가이드 블록을 바닥까지 내린다.
    public void HardDropRoutine(Vector3 startPos)
    {
        transform.position = startPos;
        while (true)
        {
            if (lineDropRoutine() == false)
                return;
        }
    }
}
