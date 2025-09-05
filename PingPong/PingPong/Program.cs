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
        public Buffer Buffer { get; set; }

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
                        if (program.RocketFirstY > 1) // ограничение сверху
                        {
                            program.Buffer.RocketFirstY = program.RocketFirstY;
                            program.RocketFirstY--;
                            program.DrawRocket();
                        }
                        break;

                    case ConsoleKey.S:
                        if (program.RocketFirstY < FieldY - 4) // ограничение снизу
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
                    //Местоположение левой ракетки
                    if (RocketFirstX == x && RocketFirstY == y)
                    {
                        Console.Write("I");
                    }
                    else if (RocketFirstX == x && RocketFirstY + 1 == y)
                    {
                        Console.Write("I");
                    }
                    else if (RocketFirstX == x && RocketFirstY + 2 == y)
                    {
                        Console.Write("I");
                    }
                    //Местоположение правой ракетки
                    else if (RocketSecondX == x && RocketSecondY == y)
                    {
                        Console.Write("I");
                    }
                    else if (RocketSecondX == x && RocketSecondY + 1 == y)
                    {
                        Console.Write("I");
                    }
                    else if (RocketSecondX == x && RocketSecondY + 2 == y)
                    {
                        Console.Write("I");
                    }
                    //Заполенение шара
                    else if (BallX == x && BallY == y)
                    {
                        Console.Write("O");
                    }
                    //Заполнение поля
                    else if (y == 0)
                    {
                        if (x == FieldX - 1)
                        {
                            Console.WriteLine("\\");
                        }
                        else if (x == 0)
                        {
                            Console.Write("/");
                        }
                        else
                        {
                            Console.Write("=");
                        }
                    }
                    else if (y == FieldY - 1)
                    {
                        if (x == FieldX - 1)
                        {
                            Console.WriteLine("/");
                        }
                        else if (x == 0)
                        {
                            Console.Write("\\");
                        }
                        else
                        {
                            Console.Write("=");
                        }
                    }
                    else if (x == 0)
                    {
                        Console.Write("|");
                    }
                    else if (x == FieldX - 1)
                    {
                        Console.WriteLine("|");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
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
                DrawBall();

                if (FirstScore >= FinalScore)
                {
                    Console.WriteLine(" Победил правый игрок!");
                    break;
                }
                else if (SecondScore >= FinalScore)
                {
                    Console.WriteLine(" Победил левый игрок!");
                    break;
                }
                Thread.Sleep(100);
            }
        }

        public void DrawBall()
        {
            Console.SetCursorPosition(Buffer.BallX, Buffer.BallY);
            Console.Write(" ");
            Console.SetCursorPosition(BallX, BallY);
            Console.Write("O");
        }

        public void DrawRocket()
        {
            Console.SetCursorPosition(Buffer.RocketFirstX, Buffer.RocketFirstY);
            Console.Write(" ");
            Console.SetCursorPosition(Buffer.RocketFirstX, Buffer.RocketFirstY + 1);
            Console.Write(" ");
            Console.SetCursorPosition(Buffer.RocketFirstX, Buffer.RocketFirstY + 2);
            Console.Write(" ");
            Console.SetCursorPosition(RocketFirstX, RocketFirstY);
            Console.Write("I");
            Console.SetCursorPosition(RocketFirstX, RocketFirstY + 1);
            Console.Write("I");
            Console.SetCursorPosition(RocketFirstX, RocketFirstY + 2);
            Console.Write("I");

            Console.SetCursorPosition(Buffer.RocketSecondX, Buffer.RocketSecondY);
            Console.Write(" ");
            Console.SetCursorPosition(Buffer.RocketSecondX, Buffer.RocketSecondY + 1);
            Console.Write(" ");
            Console.SetCursorPosition(Buffer.RocketSecondX, Buffer.RocketSecondY + 2);
            Console.Write(" ");
            Console.SetCursorPosition(RocketSecondX, RocketSecondY);
            Console.Write("I");
            Console.SetCursorPosition(RocketSecondX, RocketSecondY + 1);
            Console.Write("I");
            Console.SetCursorPosition(RocketSecondX, RocketSecondY + 2);
            Console.Write("I");
        }
    }
}