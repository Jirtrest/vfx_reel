using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFX : MonoBehaviour
{



    #region// глобальные переменные
    public GameObject prefabFX;              // префаб
    public float timeOffset = 1;             // время до старта
    public float lifetime = 2;               // время жизни
    public bool restart = false;             // включить выключить рестарт
    public float restartTime = 4;            // время до рестарта
    public GameObject caster;                // начальная тока
    public GameObject target;                // наша цель
    public float moveTime = 1;               // время движения
    #endregion

    enum States { BEFORE_START, STARTED_MOVABLE, STARTED_UNMOVABLE, DESTROYED }; //перечислением уставлеваем сотояние эффекта

    //локальные переменные
    GameObject fx;                // наш эффект
    float startTimer;             // таймер отсрочка старта
    float deathTimer;             // таймер жизни эффекта
    float restartTimer;           // таймер рестарта
    float moveTimer;              // таймер движения
    Vector3 velocity;             // скорость перемещения

    States state = States.BEFORE_START;   //инициализируем начальное состояние эффета

    void Intitialisation() //функция Инициализация сброс переменных и состяния на первоначальные значения
    {
        startTimer = timeOffset;        // установка значений пользователя в таймер отсрочка старта
        deathTimer = lifetime;          // установка значений пользователя в таймер время жизни
        restartTimer = restartTime;     // установка значений пользователя в таймер рестарта
        moveTimer = moveTime;           // установка значений пользователя в таймер движения
        moveTimer += float.Epsilon;     // прибавляем минимальное значение чтоб не было 0
        state = States.BEFORE_START;    // устанавливаем значение статуса States в начальное
    }


    void Start()
    {

        Intitialisation(); //вызов функции Инициализация
    }

    void Update()
    {

        restartTimer = (restart == true) ? restartTimer - Time.deltaTime : restartTime; //счетчик с условием рестарта, выполняется только когда включен рестарт
        startTimer = (state == States.BEFORE_START) ? startTimer - Time.deltaTime : timeOffset; //счетчик времени отсрочки выполнения эффекта с условием, выполняется пока состояние эффекта До старта
        deathTimer = (state == States.STARTED_MOVABLE || state == States.STARTED_UNMOVABLE) ? deathTimer - Time.deltaTime : lifetime; //счетчик с время жизни с условием, выполняется пока статус эфекта Запущен

        if (restartTimer <= 0)                                   //если время пришло перезапускаем эффект
        {
            if (fx != null) Destroy(fx);                         // разрушение эффекта
            Intitialisation();                                   // вызов функции Инициализация

            return;
        }

        if (state == States.STARTED_MOVABLE)                     // выполняем если статус эффекта  Запущен в движении
        {

            fx.transform.position += velocity * Time.deltaTime;  // двигаем эффекты
            moveTimer -= Time.deltaTime;                         // инкрементируем время движения эффекта
            if (moveTimer <= 0)                                  // выполняем если время движения вышло
            {
                state = States.STARTED_UNMOVABLE;                // меняем статус эффекта на Запусщен без движения
            }
        }


        if ((startTimer <= 0) && (state == States.BEFORE_START)) // выполняем если тамер отложенного старта вышел и статус До старта
        {
            velocity = Vector3.zero;                             // обнуляем координаты
            if (caster != null)                                  // выполняем если существует Кастер
            {
                if (target != null)                              // выполняем если существует Цель
                {
                    velocity = (target.transform.position - caster.transform.position) / moveTime; //на сколько переместился за время
                }
                fx = Instantiate(prefabFX, caster.transform);        // создаем эффект с координатами кастер
                fx.transform.localPosition = Vector3.zero;           // обнуляем локальные координаты
                fx.transform.localRotation = Quaternion.identity;    // обнуляем локальное значение вращения 
                fx.transform.localScale = Vector3.one;               // обнуляем локальное значение маштаба

            }
            else                                                     // выполняем если не существует Кастер
            {
                fx = Instantiate(prefabFX);                          // создаем эффект
            }

            state = States.STARTED_MOVABLE;                          // меняем статус эффекта на Запущен в движении

            
        }

        if ((deathTimer <= 0) && (state == States.STARTED_MOVABLE || state == States.STARTED_UNMOVABLE)) // проверяем истекло ли время жизни и разрушаем эффект меняем состояния на Разрушенный
        {
            if (fx != null) Destroy(fx);                            //разрушение эффекта
            state = States.DESTROYED;                               // меняем статус эффекта на Разрушен

        }

    }
}
