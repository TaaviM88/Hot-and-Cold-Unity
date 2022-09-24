using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour, IDataPersistence
{
    public int score  { get;set; }
    public int time = 60;
    public bool gameOn { get; private set; }
    public UIManager uimanager;
    int ogTime;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        ogTime = time;
        StartCoroutine(Timer());
        gameOn = true;
    }

    IEnumerator Timer()
    {
        for (int i = 0; i < ogTime; i++)
        {
            yield return new WaitForSecondsRealtime(1);
            time--;
        }
        if(time <= 0)
        {
            gameOn = false;
        }
    }

    public void CurrentLoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadData(GameData data)
    {
        this.score = data.scoreCount;
    }

    public void SaveData(ref GameData data)
    {
        data.scoreCount = this.score ;
    }


}
