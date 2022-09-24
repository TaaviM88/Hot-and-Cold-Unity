using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerController : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    float speed = 3;
    Vector2 movement;
    Rigidbody2D rb2D;
    public float searchCooldown = 0.25f;
    public Transform target;
    public Transform startPoint;
    public TMP_Text textTMP;
    public GameManager gameManager;
    public GameObject hitEffect;
    public GameObject digEffect;
    bool canSearch;
    SpriteRenderer sRenderer;
    public AudioManager audioManager;
    public AudioClip digSFX;
    public AudioClip[] searchSFX;
    float q = 0;
    bool increasing = true;
    bool canMove = true;
    bool digging = false;
    // Start is called before the first frame update
    void Start()
    {
        textTMP.text = "";
        rb2D = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();

        canSearch = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        RotateTowardDirection();
        ColorEffect();
    }

    private void ColorEffect()
    {
        Color color = new Color(q, 1, q);
        if(increasing)
        {
            q += 0.02f;
            if(q > 0.8)
            {
                increasing = false;
            }
        }
        else
        {
            q -= 0.02f;
            if (q <= 0.2f)
            {
                increasing = true;
            }
        }
        sRenderer.color = color;
    }

    private void Movement()
    {
        if (!gameManager.gameOn)
        {
            return;
        }

        if(!canMove)
        {
            return;
        }

        Vector2 currentPos = rb2D.position;
        Vector2 adjustedMovement = movement * speed;
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
        rb2D.MovePosition(newPos);
    }

    public void RotateTowardDirection()
    {
        if (!gameManager.gameOn)
        {
            return;
        }

        if(!canMove)
        {
            return;
        }
        if (movement != Vector2.zero)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back, movement);
        }
    }

    void OnMove(InputValue value)
    {

        movement = value.Get<Vector2>();
    }

    void OnFire()
    {
        if (!gameManager.gameOn)
        {
            return;
        }

        if(canSearch)
        {
           /* var gamepad = InputSystem.GetDevice<Gamepad>();
            if(gamepad.rightShoulder.isPressed)
            {

                gamepad.SetMotorSpeeds(0.25f, 0.75f);
            }
           */
            if(digging)
            {
                Dig();
            }
            else
            {
                Search();
            }
            StartCoroutine(Cooldown());
        }
   
        
    }

    private void Dig()
    {
      
        bool b = target.GetComponent<TargetController>().Dig();
        gameManager.uimanager.digText = $"DIG!!\n {target.GetComponent<TargetController>().currentDeep}";
        Instantiate(digEffect, startPoint.position, Quaternion.identity);
        
        if (b)
        {
            canMove = true;
            digging = false;
            gameManager.uimanager.digging = false;
            gameManager.score++;
        }else
            audioManager.PlaySFX(digSFX);

    }

    IEnumerator Cooldown()
    {
        canSearch = false;
        yield return new WaitForSecondsRealtime(searchCooldown);
        canSearch = true;
    }

    private void Search()
    {
        float distance = (target.position - startPoint.position).sqrMagnitude;
       // Debug.Log("Etäisyys: " + distance);
        Instantiate(hitEffect, startPoint.position, Quaternion.identity);
        audioManager.PlaySFX(searchSFX[UnityEngine.Random.Range(0, searchSFX.Length)]);
        if(distance > 10)
        {
            textTMP.text = "";
        }
        else if(distance < 10 && distance > 5)
        {
            textTMP.text = "!";
        }
        else if(distance <= 5 &&  distance > 2){
            textTMP.text = "!!";
        }
        else if(distance <= 2 && distance > 1)
        {
            textTMP.text = "!!!";
        }
        else if(distance <= 1)
        {
            textTMP.text = "!!!!";
        }
       
        if(distance <= 0.1f)
        {
            canMove = false;
            digging = true;
            gameManager.uimanager.digging = true;
            gameManager.uimanager.digText = $"DIG!!\n {target.GetComponent<TargetController>().currentDeep}";
            Debug.Log("Löyty!");
        }
        
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
