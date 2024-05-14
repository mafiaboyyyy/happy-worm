using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadWorm : MonoBehaviour
{
    private Worm worm;
    public int currentLevel = 1;
    public UnityEvent levelCollect;
    private Vector2Int direction = new Vector2Int(0, 1);

    [SerializeField] private GameObject segmentWormPrefab;

    private void Awake()
    {
        worm = GetComponentInParent<Worm>();
    }

    private void Start()
    {
        levelCollect.AddListener(GameObject.FindGameObjectWithTag("Levels").GetComponent<LevelManager>().win);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
     
            direction.x = 1;
            direction.y = 0;
            RaycastHit2D detect = Physics2D.Raycast(transform.position, direction, 1f);
            if (detect && detect.collider.tag == "Food")
            {
                Eat(direction);
            }
            else if (detect && detect.collider.tag == "Gate")
            {
                worm.MovePass();
                Invoke("nextLevel", 1);
            }
            else if (detect && detect.collider.tag == "Map")
            {
                return;
            }
            worm.Move();
            transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(direction));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
           
            direction.x = -1;
            direction.y = 0;
            RaycastHit2D detect = Physics2D.Raycast(transform.position, direction, 1f);
            if (detect && detect.collider.tag == "Food")
            {
                Eat(direction);
            }
            else if (detect && detect.collider.tag == "Gate")
            {
                worm.MovePass();
                Invoke("nextLevel", 1);
            }
            else if (detect && detect.collider.tag == "Map")
            {
                return;
            }
            worm.Move();
            transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(direction));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
            direction.x = 0;
            direction.y = 1;
            RaycastHit2D detect = Physics2D.Raycast(transform.position, direction, 1f);
            if (detect && detect.collider.tag == "Food")
            {
                Eat(direction);
            }
            else if (detect && detect.collider.tag == "Gate")
            {
                worm.MovePass();
                Invoke("nextLevel", 1);
            }
            else if (detect && detect.collider.tag == "Map")
            {
                return;
            }
            worm.Move();
            transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(direction));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            
            direction.x = 0;
            direction.y = -1;
            RaycastHit2D detect = Physics2D.Raycast(transform.position, direction, 1f);
            if (detect && detect.collider.tag == "Food")
            {
                Eat(direction);
            }
            else if (detect && detect.collider.tag == "Gate")
            {
                worm.MovePass();
                Invoke("nextLevel", 1);
            }
            else if (detect && detect.collider.tag == "Map")
            {
                return;
            }
            worm.Move();
            transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(direction));
        }
    }

    private void DetectObject(Vector2 dir)
    {
        RaycastHit2D detect = Physics2D.Raycast(transform.position, dir, 1f);
        if (detect && detect.collider.tag == "Food")
        {
            Eat(dir);
        }
        else if (detect && detect.collider.tag == "Gate")
        {
            worm.MovePass();
            Invoke("nextLevel", 1);
        }
        else if (detect && detect.collider.tag == "Map")
        {
            Debug.Log("detect map");
            return;
        }
    }

    private void nextLevel()
    {
        GameObject parentObject = gameObject.transform.parent.gameObject;
        GameObject parent2Object = parentObject.transform.parent.gameObject;
        Destroy(parent2Object);
        levelCollect.Invoke();
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
    private void Eat(Vector2 dir)
    {
        Collider2D col = GetCollider2D(dir);
        string tagName = GetTag(dir);
        if (tagName != "Food")
        {
            return;
        }
        Destroy(col.gameObject);

        var segment = Instantiate(segmentWormPrefab, worm.gameObject.transform);
        segment.transform.position = transform.position;
        transform.position = new Vector3(transform.position.x + dir.x, transform.position.y + dir.y, transform.position.z);
        worm.AddSegmentWorm(segment);
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
