using Game;
using Inventory;
using System;
using UnityEngine;

namespace DeveloperConsole
{
    public class TeleportPlayerCommand : IDeveloperCommand
    {
        public string ID => "teleport";

        public string Description => "Teleports the player to x,y position";

        public void Invoke(float x, float y, DeveloperConsole console)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p == null)
            {
                console.PrintConsole("Error: no player found");
                return;
            }

            p.transform.position = new Vector3(x, y, 0);
        }
    }

    public class PrintPlayerPositionCommand : IDeveloperCommand
    {
        public string ID => "print_position";

        public string Description => "Prints the player's x,y position";

        public void Invoke(DeveloperConsole console)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p == null)
            {
                console.PrintConsole("Error: no player found");
                return;
            }

            console.PrintConsole($"Player position: {p.transform.position.x}, {p.transform.position.y}");
        }
    }

    public class RestartCommand : IDeveloperCommand
    {
        public string ID => "restart";

        public string Description => "Restarts the level";

        public void Invoke()
        {
            SceneSwitcher.Instance.Restart();
        }
    }

    public class ClearInventoryCommand : IDeveloperCommand
    {
        public string ID => "clear_inventory";

        public string Description => "Clears player's inventory (Still in testing)";

        public void Invoke(DeveloperConsole console)
        {
            PlayerInventoryController c = GameObject.FindFirstObjectByType<PlayerInventoryController>();

            if (c == null)
            {
                console.PrintConsole("Error: no player found");
                return;
            }

            c.ClearInventory();
        }
    }
}
