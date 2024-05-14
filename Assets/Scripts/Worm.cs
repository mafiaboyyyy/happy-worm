using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Worm : MonoBehaviour
{
    [SerializeField] private List<GameObject> segmentWorms;
    private bool isPass;
    private float speed = 1;
    public Transform targetMove;
    public void AddSegmentWorm(GameObject segment)
    {
        segmentWorms.Insert(1, segment);
    }
    public void Move()
    {
        for (int i = segmentWorms.Count - 1; i >= 1; i--)
        {
            segmentWorms[i].transform.position = segmentWorms[i - 1].transform.position;
        }
    }

    public void MovePass()
    {
        isPass = true;
        transform.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void Update()
    {
        if (isPass)
        {
            transform.position = Vector3.Lerp(transform.position, targetMove.position, speed * Time.deltaTime);
        }
    }
}
