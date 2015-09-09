using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using MySql.Data.MySqlClient; 
using TShockAPI;
using TShockAPI.DB;
using Terraria;
using TerrariaApi.Server;

namespace PedEconomy
{

    [ApiVersion(1, 21)]
    public class PedEconomy : TerrariaPlugin
    {

        public struct rank{
            public string groupName;
            public int points;
            public string displayName;
        }

        public int RankListLength = File.ReadLines(@"ranklist.txt").Count();
        public int SubRankListLength = File.ReadLines(@"subranklist.txt").Count();
        public string PointName = "PedPoints";
        public string PointNameSingular = "PedPoint";
        public SQLiteConnection db;
        public rank[] ranks;
        public rank[] subRanks;

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }
        public override string Author
        {
            get { return "Annonymus"; }
        }
        public override string Name
        {
            get { return "PedEconomy"; }
        }
        public override string Description
        {
            get { return "Replacement for SEconomy on pedguin's server cause SEconomy is shit."; }
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("PedEconomy.user", points, "points"));
            Commands.ChatCommands.Add(new Command("PedEconomy.admin", award, "award"));
            Commands.ChatCommands.Add(new Command("PedEconomy.unregistered", login, "login"));
            Commands.ChatCommands.Add(new Command("PedEconomy.user", levelup, "levelup"));
            Commands.ChatCommands.Add(new Command("PedEconomy.admin", awardAll, "awardall"));
            Commands.ChatCommands.Add(new Command("PedEconomy.admin", start, "createdbresetaccounts"));
            Commands.ChatCommands.Add(new Command("PedEconomy.admin", closedb, "closedb"));
            Commands.ChatCommands.Add(new Command("PedEconomy.user", password, "password"));
            Commands.ChatCommands.Add(new Command("PedEconomy.admin", reload, "reloadeconomy"));
            db = new SQLiteConnection("Data Source=PedEconomy.sqlite;Version=3;");

            
            StreamReader reader = File.OpenText(@"rankList.txt");

            try {
                string line;
                string word;
                ranks = new rank[RankListLength];
                subRanks = new rank[SubRankListLength];

                for (int i = 0;i < RankListLength;i++) {
                    line = reader.ReadLine();
                    int oldj = 0;
                    for (int k = 0;k < 3;k++) {
                        int j;
                        if (k < 2) {
                            for (j = oldj;line.ElementAt(j) != ' ';j++) {

                            }
                        } else {
                            for (j = oldj;j < line.Length;j++) {

                            }
                        }
                        word = line.Substring(oldj, j - oldj);
                        oldj += j - oldj + 1;
                        switch (k) {
                            case 0:
                                ranks[i].groupName = word;
                                break;
                            case 1:
                                ranks[i].points = Int32.Parse(word);
                                break;
                            case 2:
                                ranks[i].displayName = word;
                                break;
                        }
                    }
                }

                reader = File.OpenText(@"subrankList.txt");
                for (int i = 0;i < SubRankListLength;i++) {
                    line = reader.ReadLine();
                    int oldj = 0;
                    for (int k = 0;k < 3;k++) {
                        int j;
                        if (k < 2) {
                            for (j = oldj;line.ElementAt(j) != ' ';j++) {

                            }
                        } else {
                            for (j = oldj;j < line.Length;j++) {

                            }
                        }
                        word = line.Substring(oldj, j - oldj);
                        oldj += j - oldj + 1;
                        switch (k) {
                            case 0:
                                subRanks[i].groupName = word;
                                break;
                            case 1:
                                subRanks[i].points = Int32.Parse(word);
                                break;
                            case 2:
                                subRanks[i].displayName = word;
                                break;
                        }
                    }
                }
                reader.Close();
            } catch {
                reader.Close();
                
            }
            
        }

        private void reload(CommandArgs args) {

            StreamReader reader = File.OpenText(@"rankList.txt");
            try {
                string line;
                string word;
                ranks = new rank[RankListLength];
                subRanks = new rank[SubRankListLength];

                for (int i = 0;i < RankListLength;i++) {
                    line = reader.ReadLine();
                    int oldj = 0;
                    for (int k = 0;k < 3;k++) {
                        int j;
                        if (k < 2) {
                            for (j = oldj;line.ElementAt(j) != ' ';j++) {

                            }
                        } else {
                            for (j = oldj;j < line.Length;j++) {

                            }
                        }
                        word = line.Substring(oldj, j - oldj);
                        oldj += j - oldj + 1;
                        switch (k) {
                            case 0:
                                ranks[i].groupName = word;
                                break;
                            case 1:
                                ranks[i].points = Int32.Parse(word);
                                break;
                            case 2:
                                ranks[i].displayName = word;
                                break;
                        }
                    }
                }

                reader = File.OpenText(@"subrankList.txt");
                for (int i = 0;i < SubRankListLength;i++) {
                    line = reader.ReadLine();
                    int oldj = 0;
                    for (int k = 0;k < 3;k++) {
                        int j;
                        if (k < 2) {
                            for (j = oldj;line.ElementAt(j) != ' ';j++) {

                            }
                        } else {
                            for (j = oldj;j < line.Length;j++) {

                            }
                        }
                        word = line.Substring(oldj, j - oldj);
                        oldj += j - oldj + 1;
                        switch (k) {
                            case 0:
                                subRanks[i].groupName = word;
                                break;
                            case 1:
                                subRanks[i].points = Int32.Parse(word);
                                break;
                            case 2:
                                subRanks[i].displayName = word;
                                break;
                        }
                    }
                }
                reader.Close();
                args.Player.SendMessage("rank lists reloaded", 255, 128, 0);
            } catch {
                reader.Close();
                args.Player.SendErrorMessage("unknown exception in /reloadeconomy");
            }

        }
        
        private void closedb(CommandArgs args){
            db.Close();
        }

        private void start(CommandArgs args){
            SQLiteConnection.CreateFile("PedEconomy.sqlite");
            db.Open();
            SQLiteCommand command = new SQLiteCommand("CREATE TABLE Economy(Username VARCHAR(21),Points INT NOT NULL DEFAULT 0,Password VARCHAR(21), PRIMARY KEY(Username));", db);
            command.ExecuteNonQuery();
            db.Close();
            GC.Collect();
        }

        private void Start() {
            //db = new SqliteConnection("uri=file://" + TShock.SavePath + "PedEconomy.sqlite,Version=3");
            //db.Query("UPDATE Economy SET Points=" + points + " WHERE Username=" + player);

            //SQLiteConnection.CreateFile("PedEconomy.sqlite");
            db = new SQLiteConnection("Data Source=PedEconomy.sqlite;Version=3;");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        public PedEconomy(Main game) : base(game)
        {
            Order = 1;
        }

        private void points(CommandArgs args) {
            TSPlayer player = args.Player;

            bool remote;

            string name;

            if (args.Parameters.Count == 0)
            {
                name = args.Player.User.Name;
                remote = false;
            }
            else if (args.Parameters.Count == 1)
            {
                
                name = args.Parameters[0];
                remote = true;
            }
            else
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /points [name]");
                return;
            }
            try { 
                db.Open();

                SQLiteCommand command = new SQLiteCommand("SELECT * FROM Economy WHERE Username='" + name + "'", db);

                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();
                int points = (int) reader["Points"];

                db.Close();
                GC.Collect();
                if (remote == false)
                {
                    player.SendMessage("You currently have " + points + " " + PointName, 255, 128, 0);
                }
                else
                {
                    player.SendMessage(name + " currently has " + points + " " + PointName, 255, 128, 0);
                }
            }catch {
                db.Close();
                GC.Collect();
                args.Player.SendErrorMessage("Unknown exception in /points");
            }

        }

        public void award(CommandArgs args){

            if (args.Parameters.Count != 2)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /award player points");
            }else{
                System.Collections.Generic.List<TSPlayer> foundplr = TShock.Utils.FindPlayer(args.Parameters[0]);
                if (foundplr.Count == 0)
                {
                    args.Player.SendErrorMessage("Invalid player!");
                }else if (foundplr.Count > 1){
                    args.Player.SendErrorMessage("More than one (" + args.Parameters.Count + ") player matched!");
                }else{

                    TSPlayer player = foundplr[0];
                    string name = player.User.Name;

                    try
                    {

                        db.Open();
                        SQLiteCommand command = new SQLiteCommand("SELECT * FROM Economy WHERE Username='" + name + "'", db);
                        SQLiteDataReader reader = command.ExecuteReader();
                        reader.Read();
                        int pointsCurr = (int)reader["Points"];
                        int points;
                        string sPoints = args.Parameters[1];
                        bool abort = false;
                        for (int i = 0; (i < sPoints.Length) && (abort == false); i++)
                        {
                            if (sPoints.ElementAt(i) == '.')
                            {
                                sPoints = sPoints.Substring(0, sPoints.Length - i);
                                abort = true;
                            }
                        }
                        if (!Int32.TryParse(sPoints, out points))
                        {
                            args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /award player points(numerical)");
                        }
                        else
                        {
                            if (points > 1) {
                                player.SendMessage("You were awarded " + points + " " + PointName, 255, 128, 0);
                                args.Player.SendMessage(name + " was awarded " + points + " " + PointName, 255, 128, 0);
                            } else if(points > 0) {
                                player.SendMessage("You were awarded " + points + " " + PointNameSingular, 255, 128, 0);
                                args.Player.SendMessage(name + " was awarded " + points + " " + PointNameSingular, 255, 128, 0);
                            } else if (points > -2) {
                                player.SendMessage(points * (-1) + " " + PointNameSingular + " was taken from you", 255, 128, 0);
                                args.Player.SendMessage(points * (-1) + " " + PointNameSingular + " was taken from " + name, 255, 128, 0);
                            } else {
                                player.SendMessage(points * (-1) + " " + PointName + " were taken from you", 255, 128, 0);
                                args.Player.SendMessage(points * (-1) + " " + PointName + " were taken from you" + name, 255, 128, 0);
                            }
                            points = points + pointsCurr;
                            SQLiteCommand command2 = new SQLiteCommand("UPDATE Economy SET Points=" + points + " WHERE Username='" + name + "'", db);
                            command2.ExecuteNonQuery();
                            
                        }
                        command.Dispose();
                        db.Close();
                        GC.Collect();
                    }catch {
                        db.Close();
                        GC.Collect();
                        args.Player.SendErrorMessage("Unknown exception in /award");
                    }
                }
            }
        }

        public void awardAll(CommandArgs args){

            if (args.Parameters.Count != 1)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /awardall points");
            }
            else
            {
                int points;
                if (!Int32.TryParse(args.Parameters[0], out points))
                {
                    args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /awardall points(numerical)");
                }
                else
                {
                    try
                    {

                        db.Open();
                        int i = 0;
                        while (TShock.Players[i] != null)
                        {
                            if (TShock.Players[i].IsLoggedIn){
                                SQLiteCommand command = new SQLiteCommand("SELECT * FROM Economy WHERE Username='" + TShock.Players[i].User.Name + "'", db);
                                SQLiteDataReader reader = command.ExecuteReader();
                                reader.Read();
                                int currpoints = (int)reader["Points"];

                                command = new SQLiteCommand("UPDATE Economy SET Points=" + (points + currpoints) + " WHERE Username='" + TShock.Players[i].User.Name + "'", db);
                                command.ExecuteNonQuery();
                            }
                            i++;
                        }
                        TSPlayer.All.SendMessage("You got " + points + " " + PointName + " for being awesome!", 255, 128, 0);
                        db.Close();
                        GC.Collect();
                    }catch{
                        db.Close();
                        GC.Collect();
                        args.Player.SendErrorMessage("Unknown exception in /awardall");
                    }
                }
            }
        }

        private void levelup(CommandArgs args)
        {
            try {

                if (args.Parameters.Count != 0) {
                    args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /levelup");
                } else {
                    string group = args.Player.User.Group;

                    int i;

                    for (i = 0;i < RankListLength && group != ranks[i].groupName;i++) {

                    }
                    if (i >= RankListLength) {
                        for (i = 0;i < SubRankListLength && group != subRanks[i].groupName;i++) {

                        }
                        if (i >= SubRankListLength) {
                            args.Player.SendErrorMessage("You are not eligible to rank up (You must to be a normal registered user without moderating, administrative or celebrity privileges)!");
                        } else {
                            db.Open();
                            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Economy WHERE Username='" + args.Player.User.Name + "'", db);
                            SQLiteDataReader reader = command.ExecuteReader();

                            reader.Read();
                            int points = (int)reader["Points"];
                            string password = (string)reader["Password"];

                            if (points >= subRanks[i + 1].points) {
                                command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - subRanks[i + 1].points) + " WHERE Username='" + args.Player.User.Name + "'", db);
                                command.ExecuteNonQuery();

                                Commands.HandleCommand(TSPlayer.Server, "/user group \"" + args.Player.User.Name + "\" " + subRanks[i + 1].groupName);
                                string username = args.Player.User.Name;
                                Commands.HandleCommand(args.Player, "/logout");
                                Commands.HandleCommand(args.Player, "/login \"" + username + "\" " + password);
                                TSPlayer.All.SendMessage(args.Player.Name + " has ranked up to " + subRanks[i + 1].displayName, 255, 128, 0);
                                Commands.HandleCommand(TSPlayer.Server, "/firework \"" + args.Player.User.Name + "\" blue");
                            } else {
                                args.Player.SendErrorMessage("You don't have enough points to rank up, you need " + subRanks[i + 1].points + " " + PointName + " to become " + subRanks[i + 1].displayName);
                            }
                        }
                    } else {
                        db.Open();
                        SQLiteCommand command = new SQLiteCommand("SELECT * FROM Economy WHERE Username='" + args.Player.User.Name + "'", db);
                        SQLiteDataReader reader = command.ExecuteReader();

                        reader.Read();
                        int points = (int)reader["Points"];
                        string password = (string)reader["Password"];

                        if (points >= ranks[i + 1].points) {
                            command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - ranks[i + 1].points) + " WHERE Username='" + args.Player.User.Name + "'", db);
                            command.ExecuteNonQuery();

                            Commands.HandleCommand(TSPlayer.Server, "/user group \"" + args.Player.User.Name + "\" " + ranks[i + 1].groupName);
                            string username = args.Player.User.Name;
                            Commands.HandleCommand(args.Player, "/logout");
                            Commands.HandleCommand(args.Player, "/login \"" + username + "\" " + password);
                            TSPlayer.All.SendMessage(args.Player.Name + " has ranked up to " + ranks[i + 1].displayName, 255, 128, 0);
                            Commands.HandleCommand(TSPlayer.Server, "/firework \"" + args.Player.User.Name + "\" blue");
                        } else {
                            args.Player.SendErrorMessage("You don't have enough points to rank up, you need " + ranks[i + 1].points + " " + PointName + " to become " + ranks[i + 1].displayName);
                        }
                        db.Close();
                        GC.Collect();
                    }
                }
            } catch {
                db.Close();
                GC.Collect();
                args.Player.SendErrorMessage("Unknown exception in /levelup");
            }
        }

        /*private string advancePrestige(string prestige, int points, string name){
            switch (prestige)
            {

                case "Copper":
                    if (points >= 100)
                    {
                        prestige = "Iron";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
                case "Iron":
                    if (points >= 100)
                    {
                        prestige = "Silver";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
                case "Silver":
                    if (points >= 100)
                    {
                        prestige = "Gold";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
                case "Gold":
                    if (points >= 100)
                    {
                        prestige = "Demonite";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
                case "Demonite":
                    if (points >= 100)
                    {
                        prestige = "Hellstone";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
                case "Hellstone":
                    if (points >= 100)
                    {
                        prestige = "Cobalt";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
                case "Cobalt":
                    if (points >= 100)
                    {
                        prestige = "Mythril";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
                case "Mythril":
                    if (points >= 100)
                    {
                        prestige = "Adamantite";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
                case "Adamantite":
                    if (points >= 100)
                    {
                        prestige = "Chlorophyte";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
                case "Chlorophyte":
                    if (points >= 100)
                    {
                        prestige = "Luminite";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
                case "Luminite":
                    if (points >= 100)
                    {
                        prestige = "NEWMAIN";
                        SQLiteCommand command = new SQLiteCommand("UPDATE Economy SET Points=" + (points - 100) + " WHERE Username='" + name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    break;
            }
            return prestige;
        }

        */

        private void login(CommandArgs args)
        {

            string username = args.Player.User.Name;
            TSPlayer player = args.Player;

            try{
                db.Open();
                SQLiteCommand command = new SQLiteCommand("SELECT EXISTS(SELECT 1 FROM Economy WHERE Username='" + username + "')", db);
                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();
                bool UserExists = reader.Get<bool>(0);

                if (!UserExists)
                {
                    command = new SQLiteCommand("INSERT INTO Economy VALUES('" + username + "', '0', '" + args.Parameters[args.Parameters.Count-1] + "');", db);
                    command.ExecuteNonQuery();
                    player.SendMessage("Your Account has been initialized to 0 " + PointName + ", win in various gamemodes to get more and rank up", 255, 128, 0);
                }

                command = new SQLiteCommand("UPDATE Economy SET Password='" + args.Parameters[args.Parameters.Count - 1] + "' WHERE Username='" + username + "'", db);
                command.ExecuteNonQuery();

                db.Close();
                GC.Collect();
            }catch{
                db.Close();
                GC.Collect();
                args.Player.SendErrorMessage("Unknown exception in /login");
            }
        }

        private void password(CommandArgs args)
        {
            if (args.Parameters.Count == 2)
            {
                TSPlayer player = args.Player;
                try
                {
                    db.Open();

                    SQLiteCommand command = new SQLiteCommand("SELECT EXISTS(SELECT 1 FROM Economy WHERE Username='" + args.Player.User.Name + "')", db);
                    SQLiteDataReader reader = command.ExecuteReader();
                    reader.Read();
                    string password = (string)reader["Password"];

                    if (args.Parameters[0] == password)
                    {
                        command = new SQLiteCommand("UPDATE Economy SET Password='" + args.Parameters[1] + "' WHERE Username='" + args.Player.User.Name + "'", db);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        args.Player.SendErrorMessage("Your old password (" + args.Parameters[0] + ") doesn't match the current password");
                    }

                    db.Close();
                    GC.Collect();
                }
                catch
                {
                    db.Close();
                    GC.Collect();
                    args.Player.SendErrorMessage("Unknown exception in /password");
                }
            }
        }
    }
}

