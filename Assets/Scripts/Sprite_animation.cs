using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sprite_animation : MonoBehaviour {
    public Sprite[] spriteAnimation;
    public float timeOffset = 0;
    public float lifetime = 1;
    public bool restart = true;

    SpriteRenderer spriteRenderer;
    float lifeTimer;
    int numberSrite;
    float discreteLifeTime;
    float frameTime;

    enum States { INIT, BEFORE_START, STARTED, STOP };   //перечислением уставлеваем сотояние эффекта
    States state = States.INIT;

    public void Initialization()    // функция Инициализация сброс переменных и состяния на первоначальные значения
    {

        if (timeOffset == 0)  // установка значений пользователя в таймер время жизни
        {
            lifeTimer = lifetime;
            state = States.STARTED;
        }
        else
        {
            lifeTimer = timeOffset;
            spriteRenderer.sprite = null;
            state = States.BEFORE_START;
        }
        numberSrite = 0;
        frameTime = lifetime / spriteAnimation.Length;
        discreteLifeTime = lifetime;
        //Debug.Log(userTimeFrame);
    }

    // Use this for initialization
    void Start () {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {

        if (state == States.INIT) Initialization();
        
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0)
        {
           switch(state)
            {
                case States.BEFORE_START:
                    lifeTimer = lifetime;
                    state = States.STARTED;
                    break;
                case States.STARTED:
                    if (restart == true)
                    {
                        Initialization();
                    }
                    else
                    {
                        state = States.STOP;
                        spriteRenderer.sprite = null;
                    }
                    break;
            }
        }
        switch (state)
        {
            case States.BEFORE_START:
                break;
            case States.STARTED:

                // Debug.Log(userTimeFrame);
                // Debug.Log(timeFrame);
                
                if (lifeTimer < discreteLifeTime)
                {
                    //Debug.Log(numberSrite);
                    Sprite currentSprite = spriteAnimation[numberSrite];
                    spriteRenderer.sprite = currentSprite;
                    numberSrite++;
                    discreteLifeTime = discreteLifeTime - frameTime;
                }
                break;
            case States.STOP:
                //
                break;       
        }

    }
}
