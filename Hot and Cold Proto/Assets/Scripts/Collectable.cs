using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private SpriteRenderer visual;
    bool collected = false;
    // Start is called before the first frame update
    void Start()
    {
        GetVisualComponent();
    }


    void GetVisualComponent()
    {
        visual = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collected = true;
            visual.gameObject.SetActive(false);
        }
    }

    public void LoadData(GameData data)
    {
        GetVisualComponent();
        data.coinsCollected.TryGetValue(id, out collected);
        if(collected)
        {
            visual.gameObject.SetActive(false);
        }

    }

    public void SaveData(GameData data)
    {
        if(data.coinsCollected.ContainsKey(id))
        {
            data.coinsCollected.Remove(id);
        }
        data.coinsCollected.Add(id, collected);
    }
}
