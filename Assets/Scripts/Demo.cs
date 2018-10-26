using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour {

    [System.Serializable]
    public class DemoElement             // шаблон демо
    {
        GameObject _fx;                  // наш эффект

        public GameObject prefabFX;      // префаб
        public GameObject caster;        // начальная тока
        public GameObject target;        // наша цель
        public float lifetime = 1;       // время жизни
        public float startTime;          // время до старта
        public float moveTime = 1;       // время движения 

        enum States { BEFORE_START, STARTED_MOVABLE, STARTED_UNMOVABLE, DESTROYED };   //перечислением уставлеваем сотояние эффекта

         //локальные переменные
        float startTimer;                // таймер отсрочка старта
        float lifeTimer;                 // таймер жизни эффекта
        float moveTimer;                 // таймер движения
        Vector3 velocity;                // скорость перемещения
        States state = States.BEFORE_START;   //инициализируем начальное состояние эффета

        public GameObject fx             // наш эффект защищен от записи
        {
            get
            {
                return _fx;              // чтение
            }

            set
            {
                _fx = value;             // запись
            }
        }

        public void Intitialisation()    // функция Инициализация сброс переменных и состяния на первоначальные значения
        {
            startTimer = startTime;      // установка значений пользователя в таймер отсрочка старта
            lifeTimer = lifetime;        // установка значений пользователя в таймер время жизни
            moveTimer = moveTime;        // установка значений пользователя в таймер движения
            moveTimer += float.Epsilon;  // прибавляем минимальное значение чтоб не было 0
            state = States.BEFORE_START; // устанавливаем значение статуса States в начальное
        }
        public void Update(float deltaTime) // функция Update
        {
            startTimer = (state == States.BEFORE_START) ? startTimer - deltaTime : startTime; //счетчик времени отсрочки выполнения эффекта с условием, выполняется пока состояние эффекта До старта
            lifeTimer = (state == States.STARTED_MOVABLE || state == States.STARTED_UNMOVABLE) ? lifeTimer - deltaTime : lifetime; //счетчик с время жизни с условием, выполняется пока статус эфекта Запущен

            if (state == States.STARTED_MOVABLE)                       // выполняем если статус эффекта  Запущен в движении
            {

                fx.transform.position += velocity * Time.deltaTime;    //двигаем эффекты
                moveTimer -= Time.deltaTime;                           // инкрементируем время движения эффекта
                if (moveTimer <= 0)                                    // выполняем если время движения вышло
                {
                    state = States.STARTED_UNMOVABLE;                  // меняем статус эффекта на Запусщен без движения
                }
            }


            if ((startTimer <= 0) && (state == States.BEFORE_START))   // выполняем если тамер отложенного старта вышел и статус До старта
            {
                velocity = Vector3.zero;                               // обнуляем координаты
                if (caster != null)                                    // выполняем если существует Кастер
                {
                    if (target != null)                                // выполняем если существует Цель
                    {
                        velocity = (target.transform.position - caster.transform.position) / moveTime; //на сколько переместился за время
                    }
                    fx = Instantiate(prefabFX, caster.transform);      // создаем эффект с координатами кастер
                    fx.transform.localPosition = Vector3.zero;         // обнуляем локальные координаты
                    fx.transform.localRotation = Quaternion.identity;  // обнуляем локальное значение вращения
                    fx.transform.localScale = Vector3.one;             // обнуляем локальное значение маштаба

                }
                else                                                   // выполняем если не существует Кастер
                {
                    fx = Instantiate(prefabFX);                        // создаем эффект
                }

                state = States.STARTED_MOVABLE;                        // меняем статус эффекта на Запущен в движении


            }

            if ((lifeTimer <= 0) && (state == States.STARTED_MOVABLE || state == States.STARTED_UNMOVABLE)) // проверяем истекло ли время жизни и разрушаем эффект меняем состояния на Разрушенный
            {
                if (fx != null) Destroy(fx);                           //разрушение эффекта
                state = States.DESTROYED;                              // меняем статус эффекта на Разрушен
            }
        }

    }

    [SerializeField] public DemoElement[] demoElements;                // создаем массив DemoElement

    public bool restart = false;                                       // включить выключить рестарт
    public float restartTime = 4;

    float restartTimer;                                                // таймер рестарта

    public void Initialisation()                                       // функция Инициализация сброс счетчика рестарта                             
    {
        restartTimer = restartTime;                                    // сброс счетчика рестарта 
    }

    void Start()
    {
        foreach (DemoElement demoElement in demoElements){             // цикл дя каждого элемента из массива
            demoElement.Intitialisation();                             // вызов функции инициализации из класса demoElement
        }

        Initialisation();                                              // вызов функции инициализации
    }

    void Update()
    {
        foreach (DemoElement demoElement in demoElements)              // цикл дя каждого элемента из массива
        {
            demoElement.Update(Time.deltaTime);                        // вызов функции Update из класса demoElement
        }
        restartTimer = (restart == true) ? restartTimer - Time.deltaTime : restartTime; //счетчик с условием рестарта, выполняется только когда включен рестарт
        if (restartTimer <= 0)                                         //если время пришло перезапускаем эффект
        {
            foreach (DemoElement demoElement in demoElements)          // цикл дя каждого элемента из массива
            {
                if (demoElement.fx != null) Destroy(demoElement.fx);   // разрушение эффекта
                demoElement.Intitialisation();                         // вызов функции инициализации из класса demoElement
            }

            Initialisation();                                          //вызов функции Инициализация
        }


    }

}

