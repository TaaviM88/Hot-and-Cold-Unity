using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    public static DataPersistenceManager instance { get; private set; }
    public List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);

        }
        instance = this;

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        DontDestroyOnLoad(this.gameObject);
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
        // Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

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
        //if we don't have any data to save, log a warning here
        if(this.gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }


        // save that data to a file using the data handler

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public bool HasGameData()
    {
        return gameData != null;
    }
}
