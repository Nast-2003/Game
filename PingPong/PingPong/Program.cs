using System;
using System.Threading;
using System.Threading.Tasks;

namespace PingPong
{
    public class Program
    {
        public int RocketFirstX { get; set; } = 1;
        public int RocketFirstY { get; set; } = 1;
        public int RocketSecondX { get; set; } = 0;
        public int RocketSecondY { get; set; } = 0;
        public int FirstScore { get; set; } = 0;
        public int SecondScore { get; set; } = 0;
        public int BallX { get; set; } = 3;
        public int BallY { get; set; } = 3;
        public int BallDirX { get; set; } = 1; // движение вправо/влево
        public int BallDirY { get; set; } = 1; // движение вверх/вниз
        public Buffer Buffer { get; set; }

        private Random rnd = new Random(); // генератор случайных чисел

        const int FieldX = 100;
        const int FieldY = 25;
        const int FinalScore = 5;

        public static void Main()
        {
            var program = new Program()
            {
                BallX = FieldX / 2,
                BallY = FieldY / 2,
                RocketSecondX = FieldX - 2,
                RocketSecondY = 1
            };

            // Случайное направление мяча в начале игры
            program.BallDirX = (program.rnd.Next(2) == 0) ? -1 : 1;
            program.BallDirY = (program.rnd.Next(2) == 0) ? -1 : 1;

            program.FirstDraw();
            Task.Run(program.Draw);

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    // Левая ракетка
                    case ConsoleKey.W:
                        if (program.RocketFirstY > 1)
                        {
                            program.Buffer.RocketFirstY = program.RocketFirstY;
                            program.RocketFirstY--;
                            program.DrawRocket();
                        }
                        break;

                    case ConsoleKey.S:
                        if (program.RocketFirstY < FieldY - 4)
                        {
                            program.Buffer.RocketFirstY = program.RocketFirstY;
                            program.RocketFirstY++;
                            program.DrawRocket();
                        }
                        break;

                    // Правая ракетка
                    case ConsoleKey.O:
                        if (program.RocketSecondY > 1)
                        {
                            program.Buffer.RocketSecondY = program.RocketSecondY;
                            program.RocketSecondY--;
                            program.DrawRocket();
                        }
                        break;

                    case ConsoleKey.L:
                        if (program.RocketSecondY < FieldY - 4)
                        {
                            program.Buffer.RocketSecondY = program.RocketSecondY;
                            program.RocketSecondY++;
                            program.DrawRocket();
                        }
                        break;
                }

            } while (key.Key != ConsoleKey.Escape); // выход по Esc
        }

        public void FirstDraw()
        {
            Console.Clear();

            for (int y = 0; y < FieldY; y++)
            {
                for (int x = 0; x < FieldX; x++)
                {
                    // Левая ракетка
                    if (RocketFirstX == x && RocketFirstY == y ||
                        RocketFirstX == x && RocketFirstY + 1 == y ||
                        RocketFirstX == x && RocketFirstY + 2 == y)
                    {
                        Console.Write("I");
                    }
                    // Правая ракетка
                    else if (RocketSecondX == x && RocketSecondY == y ||
                             RocketSecondX == x && RocketSecondY + 1 == y ||
                             RocketSecondX == x && RocketSecondY + 2 == y)
                    {
                        Console.Write("I");
                    }
                    // Шарик
                    else if (BallX == x && BallY == y)
                    {
                        Console.Write("O");
                    }
                    // Верхняя граница
                    else if (y == 0)
                    {
                        if (x == FieldX - 1) Console.WriteLine("\\");
                        else if (x == 0) Console.Write("/");
                        else Console.Write("=");
                    }
                    // Нижняя граница
                    else if (y == FieldY - 1)
                    {
                        if (x == FieldX - 1) Console.WriteLine("/");
                        else if (x == 0) Console.Write("\\");
                        else Console.Write("=");
                    }
                    // Левая/правая стенки
                    else if (x == 0) Console.Write("|");
                    else if (x == FieldX - 1) Console.WriteLine("|");
                    else Console.Write(" ");
                }
            }
            Console.WriteLine($@"Счет: {FirstScore} : {SecondScore}");
            Console.WriteLine("Управление левой ракеткой: W - вверх, S - вниз");
            Console.WriteLine("Управление правой ракеткой: O - вверх, L - вниз");
            Console.WriteLine("Чтобы выйти нажмите Esc");

            Buffer = new Buffer(this);
        }

        public void Draw()
        {
            while (true)
            {
                MoveBall();
                DrawBall();

                if (FirstScore >= FinalScore || SecondScore >= FinalScore)
                {
                    break; // игра завершается без сообщения о победе
                }

                Thread.Sleep(100);
            }
        }

        public void MoveBall()
        {
            // Стираем старый шарик
            Console.SetCursorPosition(BallX, BallY);
            Console.Write(" ");

            // Двигаем
            BallX += BallDirX;
            BallY += BallDirY;

            // --- Проверка столкновений ---

            // Верх/низ поля
            if (BallY <= 1 || BallY >= FieldY - 2)
            {
                BallDirY = -BallDirY; // отражение по вертикали
            }

            // Левая ракетка
            if (BallX == RocketFirstX + 1 &&
                BallY >= RocketFirstY &&
                BallY <= RocketFirstY + 2)
            {
                BallDirX = 1; // направляем вправо
            }

            // Правая ракетка
            if (BallX == RocketSecondX - 1 &&
                BallY >= RocketSecondY &&
                BallY <= RocketSecondY + 2)
            {
                BallDirX = -1; // направляем влево
            }

            // Если мяч улетел за левую границу
            if (BallX <= 0)
            {
                SecondScore++;
                ResetBall();
            }

            // Если мяч улетел за правую границу
            if (BallX >= FieldX - 1)
            {
                FirstScore++;
                ResetBall();
            }
        }

        public void ResetBall()
        {
            BallX = FieldX / 2;
            BallY = FieldY / 2;

            // случайное направление при каждом новом розыгрыше
            BallDirX = (rnd.Next(2) == 0) ? -1 : 1;
            BallDirY = (rnd.Next(2) == 0) ? -1 : 1;
        }

        public void DrawBall()
        {
            Console.SetCursorPosition(BallX, BallY);
            Console.Write("O");
        }

        public void DrawRocket()
        {
            // Стираем старую левую ракетку
            Console.SetCursorPosition(Buffer.RocketFirstX, Buffer.RocketFirstY);
            Console.Write(" ");
            Console.SetCursorPosition(Buffer.RocketFirstX, Buffer.RocketFirstY + 1);
            Console.Write(" ");
            Console.SetCursorPosition(Buffer.RocketFirstX, Buffer.RocketFirstY + 2);
            Console.Write(" ");
            // Рисуем новую
            Console.SetCursorPosition(RocketFirstX, RocketFirstY);
            Console.Write("I");
            Console.SetCursorPosition(RocketFirstX, RocketFirstY + 1);
            Console.Write("I");
            Console.SetCursorPosition(RocketFirstX, RocketFirstY + 2);
            Console.Write("I");

            // Стираем старую правую ракетку
            Console.SetCursorPosition(Buffer.RocketSecondX, Buffer.RocketSecondY);
            Console.Write(" ");
            Console.SetCursorPosition(Buffer.RocketSecondX, Buffer.RocketSecondY + 1);
            Console.Write(" ");
            Console.SetCursorPosition(Buffer.RocketSecondX, Buffer.RocketSecondY + 2);
            Console.Write(" ");
            // Рисуем новую
            Console.SetCursorPosition(RocketSecondX, RocketSecondY);
            Console.Write("I");
            Console.SetCursorPosition(RocketSecondX, RocketSecondY + 1);
            Console.Write("I");
            Console.SetCursorPosition(RocketSecondX, RocketSecondY + 2);
            Console.Write("I");
        }
    }

    public class Buffer
    {
        public int RocketFirstX { get; set; }
        public int RocketFirstY { get; set; }
        public int RocketSecondX { get; set; }
        public int RocketSecondY { get; set; }
        public int BallX { get; set; }
        public int BallY { get; set; }

        public Buffer(Program program)
        {
            RocketFirstX = program.RocketFirstX;
            RocketFirstY = program.RocketFirstY;
            RocketSecondX = program.RocketSecondX;
            RocketSecondY = program.RocketSecondY;
            BallX = program.BallX;
            BallY = program.BallY;
        }
    }
}
