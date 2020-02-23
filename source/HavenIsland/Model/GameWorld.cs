namespace DeenGames.HavenIsland.Model
{
    // Represents a single save-game world. Maybe it should be called GameWorld.
    public class GameWorld
    {
        public static GameWorld Instance = new GameWorld();

        public int PlayerEnergy { get; set; }
        public int PlayerMaxEnergy { get; set; }

        private GameWorld()
        {
            this.PlayerEnergy = 100;
            this.PlayerMaxEnergy = 100;
        }
    }
}