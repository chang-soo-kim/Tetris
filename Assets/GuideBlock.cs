using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideBlock : ParentBlock
{
    // ���̵� ����� �ٴڱ��� ������.
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
