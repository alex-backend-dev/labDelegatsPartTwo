using System;
using System.Threading;

namespace labDelegatesPartTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            ICutDownNotifier[] interfaceArray = new ICutDownNotifier[3]; // работаем с массивом объектов типа IcutDownNotifier
            ReversTimer[] RtArray = new ReversTimer[3];

            Console.Write("Введите имя первого таймера: ");
            RtArray[0] = new ReversTimer(Console.ReadLine());
            Console.Write("Установите время: ");
            interfaceArray[0] = new TimerMethod(Convert.ToInt32(Console.ReadLine()));

            Console.Write("Введите имя второго таймера: ");
            RtArray[1] = new ReversTimer(Console.ReadLine());
            Console.Write("Установите время: ");
            interfaceArray[1] = new TimerMethod(Convert.ToInt32(Console.ReadLine()));

            Console.Write("Введите имя третьего таймера: ");
            RtArray[2] = new ReversTimer(Console.ReadLine());
            Console.Write("Установите время: ");
            interfaceArray[2] = new TimerMethod(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine();

            for (int i = 0; i < interfaceArray.Length; i++)
            {
                interfaceArray[i].init(RtArray[i]);
                interfaceArray[i].Run(RtArray[i]);
                Console.WriteLine();
            }
        }
    }

    public class ReversTimer
    {
        public delegate void timerStart(string source, string name);
        public event timerStart start;

        public delegate void timerLeft(string source, string name, int N);
        public event timerLeft left;

        public delegate void timerEnd(string source, string name);
        public event timerEnd end;

        public string name { get; set; }

        public ReversTimer(string name)
        {
            this.name = name;
        }
        public void go(int wait)
        {
            start?.Invoke("timer", this.name); 
            for (int i = wait; i > 0; i--)
            {
                Thread.Sleep(1000);
                left?.Invoke("timer", this.name, i);
            }
            end?.Invoke("timer", this.name);
        }
    }

    public interface ICutDownNotifier //использование интерфейса
    {
        void init(ReversTimer RtInit);
        void Run(ReversTimer RtRun);
    }

    class TimerMethod : ICutDownNotifier // наследование класса от интерфейса, улучшая функционал
    {
        int time;
        public TimerMethod(int time)
        {
            this.time = time;
        }
        public void init(ReversTimer RtInit)
        {
            RtInit.start += Starting; // добавление обработчика (подписываемся на событие)
            RtInit.left += Waiting; // добавление обработчика (подписываемся на событие)
            RtInit.end += TheEnd; // добавление обработчика (подписываемся на событие)
        }

        public void Run(ReversTimer RtInit) // метод Run без возврата значения
        {
            RtInit.go(time);
        }

        public void Starting(string source, string name) // реализация метода Starting
        { Console.WriteLine($"Старт обратного отсчёта {name}:"); }

        private void Waiting(string source, string name, int N) // реализация метода Waiting
        { Console.WriteLine($"У таймера {name} осталось секунд: {N} "); }

        private void TheEnd(string source, string name) // реализация метода TheEnd
        { Console.WriteLine("Обратный отсчёт завершён"); }

    }

    class TimerAnonMethod : ICutDownNotifier // создание класса и наследование от интерфейса
    {
        int time;
        public TimerAnonMethod(int time)
        {
            this.time = time;
        }
        public void init(ReversTimer RtInit)
        {
            RtInit.start += delegate (string source, string name)
            { Console.WriteLine($"Старт обратного отсчёта {name}:"); };

            RtInit.left += delegate (string source, string name, int N)
            { Console.WriteLine($"У таймера {name} осталось секунд: {N} "); };

            RtInit.end += delegate (string source, string name)
            { Console.WriteLine("Обратный отсчёт завершён"); };
        }

        public void Run(ReversTimer RtRun) // метод Run 
        {
            RtRun.go(time);
        }
    }

    class TimerLambdas : ICutDownNotifier // создание класса и наследование от интерфейса
    {
        int time;
        public TimerLambdas(int time)
        {
            this.time = time;
        }
        public void init(ReversTimer RtInit)
        {
            RtInit.start += (string source, string name) => { Console.WriteLine($"Старт обратного отсчёта {name}:"); };
            RtInit.left += (string source, string name, int N) => { Console.WriteLine($"У таймера {name} осталось секунд: {N} "); };
            RtInit.end += (string source, string name) => { Console.WriteLine("Обратный отсчёт завершён"); };
        }

        public void Run(ReversTimer RtRun)
        {
            RtRun.go(time);
        }
    }
}