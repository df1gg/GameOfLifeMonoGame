namespace GameOfLifeMonoGame
{
    public static class GameSettings
    {
        public const int UpdateDelay = 5; // Update speed | 0 - fast / 99999 - slow
        public const int WindowWidth = 1820; // Screen resolution
        public const int WindowHeight = 980; // Screen resolution
        public const int CellSize = 15; // Size of one cell
        public const int GridWidth = WindowWidth / CellSize; // Grid width
        public const int GridHeight = WindowHeight / CellSize; // Grid height
    }
}
