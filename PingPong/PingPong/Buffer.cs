namespace PingPong
{
    public class Buffer(Program program)
    {
        public int RocketFirstX { get; set; } = program.RocketFirstX;
        public int RocketFirstY { get; set; } = program.RocketFirstY;
        public int RocketSecondX { get; set; } = program.RocketSecondX;
        public int RocketSecondY { get; set; } = program.RocketSecondY;
        public int BallX { get; set; } = program.BallX;
        public int BallY { get; set; } = program.BallY;
    }
}
