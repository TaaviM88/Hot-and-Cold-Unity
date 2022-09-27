using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SaveSlotMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;
    private SaveSlot[] saveSlots;
    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;
    private bool isLoadingGame = false;
    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        //disable all buttons
        DisableMenuButtons();
        //Update the selected profile id to be used for  data persistence
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
        if(!isLoadingGame)
        {
            //create a new game - which will initialize our data to a clean slate
            DataPersistenceManager.instance.NewGame();
        }
        SceneManager.LoadSceneAsync("SampleScene");
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        //Set this menu to be active
        this.gameObject.SetActive(true);

        //set mode
        this.isLoadingGame = isLoadingGame;

        //load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();
        GameObject firstSelected = backButton.gameObject;
        //Loop through each save slot in the UI and set the content appropriately
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if(firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        StartCoroutine(this.SetFirstSelected(firstSelected));
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}