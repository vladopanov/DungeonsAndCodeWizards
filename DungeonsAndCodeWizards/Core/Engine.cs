using DungeonsAndCodeWizards.Core.IO.Contracts;
using System;
using System.Linq;

namespace DungeonsAndCodeWizards.Core
{
    public class Engine
    {
        private readonly IReader reader;
        private readonly IWriter writer;

        private readonly DungeonMaster dungeonMaster;

        public Engine(IReader reader, IWriter writer)
        {
            this.reader = reader;
            this.writer = writer;
            this.dungeonMaster = new DungeonMaster();
        }

        public void Run()
        {
            string command = this.reader.ReadLine();
            while (!String.IsNullOrEmpty(command))
            {
                try
                {
                    var commandResult = this.ProcessCommand(command);
                    this.writer.WriteLine(commandResult);
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentException)
                    {
                        this.writer.WriteLine("Parameter Error: " + ex.Message);
                    }
                    if (ex is InvalidOperationException)
                    {
                        this.writer.WriteLine("Invalid Operation: " + ex.Message);
                    }
                }

                if (this.dungeonMaster.IsGameOver())
                {
                    break;
                }

                command = this.reader.ReadLine();
            }

            string stats = this.dungeonMaster.GetStats();
            this.writer.WriteLine(stats);
        }

        private string ProcessCommand(string command)
        {
            var commandArgs = command.Split(' ');
            var commandName = commandArgs[0];
            var args = commandArgs.Skip(1).ToArray();

            var output = string.Empty;
            switch (commandName)
            {
                case "JoinParty":
                    output = this.dungeonMaster.JoinParty(args);
                    break;
                case "AddItemToPool":
                    output = this.dungeonMaster.AddItemToPool(args);
                    break;
                case "PickUpItem":
                    output = this.dungeonMaster.PickUpItem(args);
                    break;
                case "UseItem":
                    output = this.dungeonMaster.UseItem(args);
                    break;
                case "UseItemOn":
                    output = this.dungeonMaster.UseItemOn(args);
                    break;
                case "GiveCharacterItem":
                    output = this.dungeonMaster.GiveCharacterItem(args);
                    break;
                case "GetStats":
                    output = this.dungeonMaster.GetStats();
                    break;
                case "Attack":
                    output = this.dungeonMaster.Attack(args);
                    break;
                case "Heal":
                    output = this.dungeonMaster.Heal(args);
                    break;
                case "EndTurn":
                    output = this.dungeonMaster.EndTurn();
                    break;
            }

            return output;
        }
    }
}
