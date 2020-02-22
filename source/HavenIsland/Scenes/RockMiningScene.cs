using DeenGames.HavenIsland.Map.Entities;
using DeenGames.HavenIsland.Map.UI;
using Puffin.Core;
using Puffin.Core.Ecs;
using Puffin.Core.Ecs.Components;
using System;
using System.IO;

namespace DeenGames.HavenIsland.Scenes
{
    public class RockMiningScene : Scene
    {
        private const int GRID_WIDTH = 3;
        private const int GRID_HEIGHT = 3;
        private const int TILE_WIDTH = 150;
        private const int TILE_HEIGHT = 150;
        private const int FONT_SIZE = 72;
        private const int GRID_TILES_X_OFFSET = 300;
        private const int GRID_TILES_Y_OFFSET = 100;

        private int integrityLeft;
        private Entity label;
        
        override public void Ready()
        {
            base.Ready();
            var random = new Random();

            this.BackgroundColour = 0x397b44;
            this.Add(new EnergyBar());

            // Model concerns
            this.integrityLeft = 20 + random.Next(11); // 20-30

            this.label = new Entity(true).Label($"Integrity left: {integrityLeft}");
            this.label.Get<TextLabelComponent>().FontSize = FONT_SIZE;
            this.Add(this.label);
            this.label.Move(GRID_TILES_X_OFFSET + 30, GRID_TILES_Y_OFFSET - FONT_SIZE - 16);
            int index = 0;

            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    index++;

                    var gridTile = new RockTile(index)
                        .Move(
                            GRID_TILES_X_OFFSET + (x * (TILE_WIDTH + 10)),
                            GRID_TILES_Y_OFFSET + (y * (TILE_HEIGHT + 10)));

                    gridTile
                        .Mouse(() => {
                            this.OnTileSelected(gridTile as RockTile);
                        }, TILE_WIDTH, TILE_HEIGHT)
                        .Keyboard((action) => {
                            // Super hack; please, no more than 9 grid tiles.
                            // Compare action ("Pressed5") to index (5)
                            var actionName = action.ToString();
                            if (actionName.StartsWith("Pressed") && actionName.Contains((gridTile as RockTile).Index.ToString()))
                            {
                                this.OnTileSelected(gridTile as RockTile);
                            }
                        });

                    this.Add(gridTile);
                }
            }
        }

        private void OnTileSelected(RockTile gridTile)
        {
            this.integrityLeft -= gridTile.Integrity;
            Player.LatestInstance.SubtractEnergy(gridTile.Integrity);
            this.label.Get<TextLabelComponent>().Text = $"Integrity left: {integrityLeft}";
            this.Remove(gridTile);

            if (this.integrityLeft <= 0)
            {
                // Communicate: success or failure, and which rock to smash
                HavenIslandGame.LatestInstance.ShowScene(new MapScene());
            }
        }

        class RockTile : Entity
        {
            private static Random random = new Random();
            public int Index { get; private set; }
            public int Integrity { get; private set; }

            public RockTile(int index) : base()
            {
                this.Index = index;
                this.Integrity = 3 + random.Next(5); // 3-7
                this.Sprite(Path.Join("Content", "Images", "Sprites", "Rock-Texture.png"))
                    .Label($"{this.Integrity}", 40, 0);
                this.Get<TextLabelComponent>().FontSize = FONT_SIZE * 2;
            }
        }
    }
}