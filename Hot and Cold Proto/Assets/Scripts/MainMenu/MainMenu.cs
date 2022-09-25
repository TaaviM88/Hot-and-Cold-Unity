using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;

    private void Start()
    {
        if(!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }
    public void OnNewGameClicked()
    {
        DisableMenuButtons();
        // create a new game - which will initialize our game data
        DataPersistenceManager.instance.NewGame();
        //Load the gameplay scene - which will in turn save the game because of
        //OnSceneUnLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void OnContinueGameClicked()
    {
        DisableMenuButtons();
        //Load the next scene - which will in turn load the game because of
        //OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
}