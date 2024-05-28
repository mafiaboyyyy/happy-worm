using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[Serializable]
//public class DirData
//{
//    public float offset;
//    public float rotate;
//}

public class Segment : MonoBehaviour
{
    public Segment next;
    public Segment pre;
    public SpriteRenderer spriteRenderer;
    public Sprite straightBody;
    public Sprite curveBody;
    //public List<DirData> dirDatas;
    public Vector2 dir;
    public void OnMove()
    {
        if (next != null)
        {
            transform.position = next.transform.position;
            // Move smoothly
        }
    }

    private void Update()
    {
        
    }

    public void UpdateArt()
    {
        var directionWithPre = Vector2.zero;
        if (pre != null && next != null && spriteRenderer != null)
        {
            var firstVector = next.transform.position - transform.position;
            var secondVector = pre.transform.position - transform.position;

            float dotProduct = Vector3.Dot(firstVector, secondVector);
            //Debug.Log("Dot Product: " + dotProduct);

            float angle = Vector3.Angle(firstVector, secondVector);
            float tempAngle = 0;

            var posNext = next.transform.position;
            var posPre = pre.transform.position;
            var posCurrent = transform.position;

            if (angle == 90f)
            {
                if (posNext.x > posPre.x && posNext.y > posPre.y)
                {
                    if (posCurrent.x == posPre.x)
                    {
                        tempAngle = 90;
                    }
                    else
                    {
                        tempAngle = -90;
                    }
                }
                else if (posNext.x > posPre.x && posNext.y < posPre.y)
                {
                    if (posCurrent.x == posPre.x)
                    {
                        tempAngle = -180;
                    }
                    else
                    {
                        tempAngle = 0;
                    }
                }

                if (posNext.x < posPre.x && posNext.y > posPre.y)
                {
                    if (posCurrent.x == posPre.x)
                    {
                        tempAngle = 0;
                    }
                    else
                    {
                        tempAngle = -180;
                    }
                }
                else if (posNext.x < posPre.x && posNext.y < posPre.y)
                {
                    if (posCurrent.x == posPre.x)
                    {
                        tempAngle = -90;
                    }
                    else
                    {
                        tempAngle = 90;
                    }
                }
                spriteRenderer.sprite = curveBody;
                spriteRenderer.transform.localEulerAngles = new Vector3(0, 0, tempAngle);
            }
            else
            {
                spriteRenderer.sprite = straightBody;
                if (posCurrent.y == posNext.y)
                {
                    spriteRenderer.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    spriteRenderer.transform.localEulerAngles = new Vector3(0, 0, 90);
                }
            }
        }
    }
}
