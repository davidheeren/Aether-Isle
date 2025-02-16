using Game;
using Inventory;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeveloperConsole
{
    public static class DeveloperCommandHelper
    {
        public static bool TryGetPlayer(DeveloperConsole console, out GameObject player)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                console.PrintConsole("Error: no player found");
                return false;
            }

            return true;
        }

        public static bool TryGetComponentOnPlayer<T>(DeveloperConsole console, out T component) where T : Component
        {
            component = null;

            if (!TryGetPlayer(console, out GameObject player))
                return false;

            if (player.TryGetComponent<T>(out component))
            {
                return true;
            }

            console.PrintConsole($"Error: Player does not have component of type: {component.GetType().Name}");
            return false;
        }
    }

    public class TeleportPlayerCommand : IDeveloperCommand
    {
        public string ID => "teleport";

        public string Description => "Teleports the player to x,y position";

        public void Invoke(float x, float y, DeveloperConsole console)
        {
            if (!DeveloperCommandHelper.TryGetPlayer(console, out GameObject player))
                return;

            player.transform.position = new Vector3(x, y, 0);
        }
    }

    public class PrintPlayerPositionCommand : IDeveloperCommand
    {
        public string ID => "print_position";

        public string Description => "Prints the player's x,y position";

        public void Invoke(DeveloperConsole console)
        {
            if (!DeveloperCommandHelper.TryGetPlayer(console, out GameObject player))
                return;

            console.PrintConsole($"Player position x: {player.transform.position.x}, y: {player.transform.position.y}");
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
            if (!DeveloperCommandHelper.TryGetComponentOnPlayer<PlayerInventoryController>(console, out PlayerInventoryController c))
                return;

            c.ClearInventory();
        }
    }

    public class GodModeCommand : IDeveloperCommand
    {
        public string ID => "god_mode";

        public string Description => "Makes player invincible";

        public void Invoke(bool on, DeveloperConsole console)
        {
            if (!DeveloperCommandHelper.TryGetComponentOnPlayer<Health>(console, out Health h))
                return;

            h.canTakeDamage = !on;
        }
    }

    public class GhostModeCommand : IDeveloperCommand
    {
        public string ID => "ghost_mode";

        public string Description => "Makes player undetectable by enemies";

        public void Invoke(bool on, DeveloperConsole console)
        {
            if (!DeveloperCommandHelper.TryGetComponentOnPlayer<Target>(console, out Target t))
                return;

            t.enabled = !on;
        }
    }

    public class NoClipCommand : IDeveloperCommand
    {
        public string ID => "no_clip";

        public string Description => "Makes player able to go through obstacles";

        public void Invoke(bool on, DeveloperConsole console)
        {
            if (!DeveloperCommandHelper.TryGetComponentOnPlayer<Collider2D>(console, out Collider2D c))
                return;

            c.isTrigger = on;
        }
    }

    public class PrintItemsCommand : IDeveloperCommand
    {
        public string ID => "print_items";

        public string Description => "Prints the IDs of all the inventory items available";

        public void Invoke(DeveloperConsole console)
        {
            if (console.itemDatabase == null)
            {
                console.PrintConsole("Error: Database is null");
                return;
            }

            foreach (ItemData item in console.itemDatabase.Items)
            {
                console.PrintConsole(item.id);
            }
        }
    }

    public class SpawnItemCommand : IDeveloperCommand
    {
        public string ID => "spawn_item";

        public string Description => "Spawns the item by the player";

        public void Invoke(string id, DeveloperConsole console)
        {
            if (console.itemDatabase == null)
            {
                console.PrintConsole("Error: Database is null");
                return;
            }

            if (console.itemPickup == null)
            {
                console.PrintConsole("Error: ItemPickup is null");
                return;
            }

            if (!console.itemDatabase.TryGetItem(id, out ItemData item))
            {
                console.PrintConsole($"Error: Item {id} does not exist");
                return;
            }

            if (!DeveloperCommandHelper.TryGetPlayer(console, out GameObject player))
                return;

            float rand = UnityEngine.Random.Range(0, Mathf.PI * 2);

            Vector2 pos = (Vector2)player.transform.position + new Vector2(Mathf.Cos(rand), Mathf.Sin(rand));

            console.itemPickup.Spawn(pos, item);
        }
    }

    public class PrintLevels : IDeveloperCommand
    {
        public string ID => "print_levels";

        public string Description => "Prints the names of all the levels";

        public void Invoke(DeveloperConsole console)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                console.PrintConsole(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
            }
        }
    }

    public class LoadLevel : IDeveloperCommand
    {
        public string ID => "load_level";

        public string Description => "Loads a level by name";

        public void Invoke(string level, DeveloperConsole console)
        {
            SceneManager.LoadScene(level);
        }
    }
}
