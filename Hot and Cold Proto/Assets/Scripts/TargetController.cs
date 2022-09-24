using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField]
    float minX = -6;
    [SerializeField]
    float maxX = 6;
    [SerializeField]
    float minY = -3.9f;
    [SerializeField]
    float maxY = 3.9f;
    [SerializeField]
    int maxDeep = 10;
    public int currentDeep;
    SpriteRenderer render;
    public GameObject particle;
    public AudioManager audioManager;
    public AudioClip audioSFX;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        GoRandomPosition();
        RandomizeDeep();

    }


    public bool Dig()
    {
        currentDeep--;
        if (currentDeep <= 0)
        {
            audioManager.PlaySFX(audioSFX);
            ShowTarget();
            return true;
        }
        else
            return false;
    }

    private void RandomizeDeep()
    {
        currentDeep = UnityEngine.Random.Range(1, maxDeep);
    }

    public void ShowTarget()
    {
        render.enabled = true;
        Instantiate(particle, transform.position, Quaternion.identity);
        GoRandomPosition();
    }

    public void GoRandomPosition()
    {
        Vector2 newPos = new Vector2();
        newPos.x = UnityEngine.Random.Range(minX, maxX);
        newPos.y = UnityEngine.Random.Range(minY, maxY);

        transform.position = newPos;
        render.enabled = false;
        RandomizeDeep();
    }
}
    