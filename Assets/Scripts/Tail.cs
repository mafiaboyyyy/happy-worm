using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public Segment next;
    public SpriteRenderer spriteRenderer;

    public void OnMove()
    {
        if (next != null)
        {
            var nextPos = next.transform.position;
            var currentPos = transform.position;
            float angle = 0;

            if (nextPos.x > currentPos.x && nextPos.y == currentPos.y)
            {
                angle = 0;
            }

            if (nextPos.x < currentPos.x && nextPos.y == currentPos.y)
            {
                angle = 180;
            }

            if (nextPos.y < currentPos.y && nextPos.x == currentPos.x)
            {
                angle = -90;
            }

            if (nextPos.y > currentPos.y && nextPos.x == currentPos.x)
            {
                angle = 90;
            }
            spriteRenderer.transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }

}