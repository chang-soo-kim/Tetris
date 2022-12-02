using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentBlock : MonoBehaviour
{
    public Func<ParentBlock, bool> checkBoard = null;

    public bool lineDropRoutine()
    {
        transform.Translate(Vector3.down, Space.World);
        if (!checkBoard(this))
        {
            transform.Translate(Vector3.up, Space.World);
            return false;
        }
        return true;
    }

}