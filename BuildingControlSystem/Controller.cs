using System;
using System.Collections.Generic;
using BuildingControlSystem.Devices;

namespace BuildingControlSystem
{
    internal class Controller
    {
        private readonly Root root;
        private readonly Dictionary<string, Action<string[]>> commands;

        public Controller(Root root)
        {
            this.root = root;

            commands = new Dictionary<string, Action<string[]>>
            {
                {"help", Help},
                {"print", Print},

                {"addgroup", AddGroup},
                {"removegroup", RemoveGroup},

                {"adddoor", AddDoor},
                {"addspeaker", AddSpeaker},
                {"addledpanel", AddLedPanel},
                {"addcardreader", AddCardReader},

                {"removedevice", RemoveDevice},
                {"movedevice", MoveDevice},

                //TODO missing modifyDevice

                {"exit", Exit}
            };
        }

        public void Run()
        {
            Console.WriteLine("Interactive mode started. Type 'help' for commands.");

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var command = parts[0].ToLowerInvariant();

                if (commands.TryGetValue(command, out var action))
                {
                    action(parts);
                }
                else
                {
                    Console.WriteLine("Unknown command. Type 'help' to see available commands.");
                }
            }
        }

        private void Help(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Incorrect command format.");
                Console.WriteLine("Usage: help");
                return;
            }

            Console.WriteLine("Commands:");
            Console.WriteLine();

            Console.WriteLine("help");
            Console.WriteLine("   - Writes out this help.");
            Console.WriteLine();

            Console.WriteLine("print");
            Console.WriteLine("   - Prints the tree structure of the current devices.");
            Console.WriteLine();

            Console.WriteLine("addGroup <name>");
            Console.WriteLine("   - Adds a group with the specified <name>.");
            Console.WriteLine("   - Group names must be unique.");
            Console.WriteLine();

            Console.WriteLine("removeGroup <name>");
            Console.WriteLine("   - Removes the group named <name> including all devices in it.");
            Console.WriteLine();

            Console.WriteLine("addDoor <group> <name> <locked> <open> <openTooLong> <openedForcibly>");
            Console.WriteLine("   - Adds a Door device to the specified group.");
            Console.WriteLine("   - <locked>, <open>, <openTooLong> and <openedForcibly> can be: true, false");
            Console.WriteLine();

            Console.WriteLine("addSpeaker <group> <name> <sound> <volume>");
            Console.WriteLine("   - Adds a Speaker device.");
            Console.WriteLine("   - <sound> can be: None, Music, Alarm.");
            Console.WriteLine();

            Console.WriteLine("addLedPanel <group> <name> <message>");
            Console.WriteLine("   - Adds a LedPanel displaying <message>.");
            Console.WriteLine();

            Console.WriteLine("addCardReader <group> <name> <cardNumber>");
            Console.WriteLine("   - Adds a CardReader device.");
            Console.WriteLine("   - <cardNumber> must be a hexadecimal string with even length (max 16).");
            Console.WriteLine();

            Console.WriteLine("removeDevice <id>");
            Console.WriteLine("   - Removes the device with the specified ID.");
            Console.WriteLine();

            Console.WriteLine("moveDevice <id> <group>");
            Console.WriteLine("   - Moves a device to another group.");
            Console.WriteLine();

            Console.WriteLine("exit");
            Console.WriteLine("   - Terminates the program.");
        }

        private void Print(string[] args)
        {
            if (!CheckArgs(args, 1, "print"))
                return;

            root.PrintTree();
        }

        private void AddGroup(string[] args)
        {
            if (!CheckArgs(args, 2, "addGroup <name>"))
                return;

            root.AddGroup(new DeviceGroup(args[1]));
        }

        private void RemoveGroup(string[] args)
        {
            if (!CheckArgs(args, 2, "removeGroup <name>"))
                return;

            root.RemoveGroup(args[1]);
        }

        private void AddDoor(string[] args)
        {
            if (!CheckArgs(args, 7, "addDoor <group> <name> <locked> <open> <openTooLong> <openedForcibly>"))
                return;

            if (!bool.TryParse(args[3], out bool locked) ||
                !bool.TryParse(args[4], out bool open) ||
                !bool.TryParse(args[5], out bool openTooLong) ||
                !bool.TryParse(args[6], out bool openedForcibly))
            {
                Console.WriteLine("Door flags must be true or false.");
                return;
            }

            var group = root.FindGroup(args[1]);
            if (group == null)
            {
                Console.WriteLine("Group not found.");
                return;
            }

            var door = new Door(args[2])
            {
                Locked = locked,
                Open = open,
                OpenForTooLong = openTooLong,
                OpenedForcibly = openedForcibly
            };

            group.AddDevice(door);
        }

        private void AddSpeaker(string[] args)
        {
            if (!CheckArgs(args, 5, "addSpeaker <group> <name> <sound> <volume>"))
                return;

            if (!Enum.TryParse(args[3], true, out Speaker.SoundType sound))
            {
                Console.WriteLine("Invalid sound type. Use: None, Music, Alarm.");
                return;
            }

            if (!float.TryParse(args[4], out float volume))
            {
                Console.WriteLine("Volume must be a number.");
                return;
            }

            var group = root.FindGroup(args[1]);
            if (group == null)
            {
                Console.WriteLine("Group not found.");
                return;
            }

            var speaker = new Speaker(args[2])
            {
                Sound = sound,
                Volume = volume
            };

            group.AddDevice(speaker);
        }

        private void AddLedPanel(string[] args)
        {
            if (!CheckArgs(args, 4, "addLedPanel <group> <name> <message>"))
                return;

            var group = root.FindGroup(args[1]);
            if (group == null)
            {
                Console.WriteLine("Group not found.");
                return;
            }

            var panel = new LedPanel(args[2], args[3]);

            group.AddDevice(panel);
        }

        private void AddCardReader(string[] args)
        {
            if (!CheckArgs(args, 4, "addCardReader <group> <name> <cardNumber>"))
                return;

            var group = root.FindGroup(args[1]);
            if (group == null)
            {
                Console.WriteLine("Group not found.");
                return;
            }

            var reader = new CardReader(args[2], args[3]);

            group.AddDevice(reader);
        }

        private void RemoveDevice(string[] args)
        {
            if (!CheckArgs(args, 2, "removeDevice <id>"))
                return;

            if (!int.TryParse(args[1], out int id))
            {
                Console.WriteLine("Device ID must be a number.");
                return;
            }

            var result = root.FindDevice(id);

            if (result == null)
            {
                Console.WriteLine("Device not found.");
                return;
            }

            result.Value.group.RemoveDevice(id);
        }

        private void MoveDevice(string[] args)
        {
            if (!CheckArgs(args, 3, "moveDevice <id> <group>"))
                return;

            if (!int.TryParse(args[1], out int id))
            {
                Console.WriteLine("Device ID must be a number.");
                return;
            }

            string targetGroup = args[2];

            var result = root.FindDevice(id);
            var group = root.FindGroup(targetGroup);

            if (result == null || group == null)
            {
                Console.WriteLine("Device or group not found.");
                return;
            }

            result.Value.group.RemoveDevice(id);
            group.AddDevice(result.Value.device);
        }

        private void Exit(string[] args)
        {
            if (!CheckArgs(args, 1, "exit"))
                return;

            Environment.Exit(0);
        }

        private static bool CheckArgs(string[] args, int expected, string usage)
        {
            if (args.Length != expected)
            {
                Console.WriteLine("Incorrect command format.");
                Console.WriteLine($"Usage: {usage}");
                return false;
            }
            return true;
        }
    }
}
