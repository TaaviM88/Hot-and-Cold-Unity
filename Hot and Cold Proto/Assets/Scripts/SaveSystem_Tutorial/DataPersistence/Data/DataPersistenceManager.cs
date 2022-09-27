using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistance = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool ovverideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    public static DataPersistenceManager instance { get; private set; }
    public List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileId = "";
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);

        }
        instance = this;

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);


        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();

        DontDestroyOnLoad(this.gameObject);

        if(disableDataPersistance)
        {
            Debug.LogWarning("Data Persistance is currently disabled");
        }

        if(ovverideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id :" + testSelectedProfileId);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    public void OnSceneUnLoaded(Scene scene)
    {
        SaveGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        //update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
        //Load the game, which will use that profile, updating our game data accordingly
        LoadGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence> (dataPersistenceObjects);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if (disableDataPersistance)
        {
            return;
        }

        // Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load(selectedProfileId);

        //start a new game if the data is null and we're configured to initialize data for debugging puposes
        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        //if no data can be loaded, initialize to a new game

        if(this.gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }
        //TODO - push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        if (disableDataPersistance)
        {
            return;
        }

        //if we don't have any data to save, log a warning here
        if (this.gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        //timestamp thedata so we know when it was last saved
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        // save that data to a file using the data handler

        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
