namespace GameOfLifeMonoGame
{
    public static class GameSettings
    {
        public const int UpdateDelay = 3; // Update speed | 0 - fast / 99999 - slow
        public const int WindowWidth = 1820; // Screen resolution
        public const int WindowHeight = 980; // Screen resolution
        public const int CellSize = 28; // Size of one cell
        public const int GridWidth = WindowWidth / CellSize; // Grid width
        public const int GridHeight = WindowHeight / CellSize; // Grid height
        public static readonly int[] BirthRules = { 3 }; // Birth rule
        public static readonly int[] SurvivalRules = { 2, 3 }; // Survival rule
    }
}
