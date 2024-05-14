using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentLevel = 0;
    void Start()
    {
        handleLevel();
    }

    public void win()
    {
        Debug.Log("Win");
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void onClickNext()
    {
        Debug.Log("click next");
        transform.GetChild(0).gameObject.SetActive(false);
        handleLevel();
    }

    public void handleLevel()
    {
        currentLevel++;
        GameObject level = Instantiate(Resources.Load(currentLevel.ToString())) as GameObject;
        if (level)
        {
            level.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
