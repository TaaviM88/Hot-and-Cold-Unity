using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_Text scoreTMP;
    public TMP_Text timerTMP;
    public GameObject endScreen;
    public GameManager gameManager;
    public GameObject digUIObj;
    public bool digging { get; set; }
    bool blinkingEffect = false;
    public TMP_Text digTMP;
    public string digText { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreTMP.text = $"Score: {gameManager.score}";
        timerTMP.text = $"Time: {gameManager.time}";
        digTMP.text = digText;
        if (!gameManager.gameOn)
        {
            endScreen.SetActive(true);
        }


        digUIObj.SetActive(digging);
        
    }




}
