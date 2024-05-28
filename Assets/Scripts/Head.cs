using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Head : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pass, move, eat, bom, fall;
    public float offet = 1;
    public List<Segment> bodys;
    public Tail tail;
    public Vector2 dir;
    public UnityEvent levelCollect;
    public GameObject segmentPrefab;
    public bool isSound;
    private bool isMoving;

    private void Start()
    {
        levelCollect.AddListener(GameObject.FindGameObjectWithTag("Levels").GetComponent<LevelManager>().win);
        audioSource = GameObject.Find("SrcSound").GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isMoving) return;
        Vector2 newOffset = Vector2.zero;
        float rotation = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (dir.y != -1)
            {
                newOffset = new Vector2(0, offet);
                dir = newOffset;
                rotation = 90;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (dir.y != 1)
            {
                newOffset = new Vector2(0, -offet);
                dir = newOffset;
                rotation = -90;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (dir.x != 1)
            {
                newOffset = new Vector2(-offet, 0);
                dir = newOffset;
                rotation = 180;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (dir.x != -1)
            {
                newOffset = new Vector2(offet, 0);
                dir = newOffset;
                rotation = 0;
            }
        }

        if (newOffset != Vector2.zero)
        {
            StartCoroutine(Move(newOffset, rotation));
        }
    }

    IEnumerator Move(Vector2 offset, float rot)
    {
        isMoving = true;
        if (isSound)
        {
            audioSource.PlayOneShot(move);
        }
        detecObj();
        for (int i = bodys.Count - 1; i >= 0; --i)
        {
            bodys[i].GetComponent<Segment>().dir = offset;
            bodys[i].OnMove();
        }

        // move smoothly
        transform.position = transform.position + new Vector3(offset.x, offset.y);
        transform.localEulerAngles = new Vector3(0, 0, rot);

        for (int i = bodys.Count - 1; i >= 0; --i)
        {
            bodys[i].UpdateArt();
        }
        tail.OnMove();
        yield return new WaitForSeconds(0.1f);
        isMoving = false;
    }

    private void detecObj()
    {
        RaycastHit2D detect = Physics2D.Raycast(transform.position, dir, 1f);
        if (detect && detect.collider.tag == "Food")
        {
            Eat(dir);
        }
        else if (detect && detect.collider.tag == "Map")
        {
            return;
        }
        else if (detect && detect.collider.tag == "Gate")
        {
            Invoke("nextLevel", 1);
            GameObject worm = GameObject.Find("Worm");
            GameObject gate = GameObject.Find("Gate");
            worm.GetComponent<Rigidbody2D>().isKinematic = true;

            // Move Smoothly
            

            // Sound
            if (isSound)
            {
                audioSource.PlayOneShot(pass);
            }
        }
        else if (detect && detect.collider.tag == "Body")
        {
            return;
        }
    }

    private void Eat(Vector2 dir)
    {
        Collider2D col = GetCollider2D(dir);
        string tagName = GetTag(dir);
        if (tagName != "Food")
        {
            return;
        }
        Destroy(col.gameObject);
        if (isSound)
        {
            audioSource.PlayOneShot(eat);
        }
        var newSegment = Instantiate(segmentPrefab, transform.parent);
        newSegment.transform.position = transform.position;
        transform.position = transform.position + new Vector3(dir.x * 1, dir.y * 1, 0);
        bodys.Insert(0, newSegment.GetComponent<Segment>());
        bodys[1].GetComponent<Segment>().next = newSegment.GetComponent<Segment>();
        newSegment.GetComponent<Segment>().next = transform.GetComponent<Segment>();
        newSegment.GetComponent<Segment>().pre = bodys[1].GetComponent<Segment>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject.Find("Fall").gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        if (collision.gameObject.CompareTag("Death"))
        {
            transform.GetComponentInParent<LevelManager>().playSound(bom);
            transform.GetComponentInParent<LevelManager>().onClickResetLevel();
        } else if (collision.gameObject.CompareTag("Fall"))
        {
            Debug.Log("Fall");
            GameObject.Find("Fall").gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            transform.GetComponentInParent<LevelManager>().playSound(fall);
        }
    }


    private string GetTag(Vector2 dir)
    {
        Collider2D col = GetCollider2D(dir);
        if (col != null) return col.gameObject.tag;
        return "";
    }
    private Collider2D GetCollider2D(Vector2 dir)
    {
        Vector2 temp = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
        Collider2D col = Physics2D.OverlapCircle(temp, 0.2f);
        if (col != null)
        {
            return col;
        }
        return null;
    }

    private void nextLevel()
    {
        GameObject parentObject = gameObject.transform.parent.gameObject;
        GameObject parent2Object = parentObject.transform.parent.gameObject;
        Destroy(parent2Object);
        levelCollect.Invoke();
    }
}
