﻿using pandapache.src.LoggingAndMonitoring;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pandapache.src.Configuration
{
    public class ServerConfiguration : IServerConfiguration
    {
        private FileSystemWatcher fileWatcher;
        private string _configurationPath;
        private static ServerConfiguration instance;
        private static readonly object lockObject = new object();


        //General configuration
        public string ServerName { get; set; } = "MyApache";
        public IPAddress ServerIP { get; set; } = System.Net.IPAddress.Any;
        public int ServerPort { get; set; } = 80;

        //Performance
        public int MaxAllowedConnections { get; set; } = 100;
        public int MaxRejectedConnections { get; set; } = 50;

        //Logging
        public string LogFolder { get; set; } = @"C:\MyApache\log\";
        public string LogFile { get; set; } = "MyApache.log";
        public int MaxLogFile { get; set; } = 10;
        public int SizeLogFile { get; set; } = 1024;
        public string LogLevel { get; set; } = "info";
        //Routing
        public string RootDirectory { get; set; } = @"C:\Apache\www\";
        public string Persistence { get; set; } = "disk";
        //Other
        // Ajoutez d'autres propriétés de configuration selon vos besoins

        public static ServerConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ServerConfiguration();
                            // Initialize configuration properties here if needed
                        }
                    }
                }
                return instance;
            }
        }

        // Private constructor to prevent instantiation
        private ServerConfiguration() 
        {
            _configurationPath = @"C:\MyApache\conf\";

            // Initialisez FileSystemWatcher pour surveiller les changements dans le fichier de configuration
            fileWatcher = new FileSystemWatcher();
            fileWatcher.Path = _configurationPath;
            fileWatcher.Filter = "*.conf";
            fileWatcher.Changed += OnConfigurationFileChanged;
            fileWatcher.EnableRaisingEvents = true;
            ReloadConfiguration();
        }

        private void OnConfigurationFileChanged(object sender, FileSystemEventArgs e)
        {
            // Traitez l'événement de changement du fichier de configuration
            // Par exemple, rechargez les paramètres de configuration depuis le fichier
            ReloadConfiguration();
        }

        private void ReloadConfiguration()
        {
            // Implémentez la logique pour recharger les paramètres de configuration depuis le fichier
            Logger.LogInfo("New configuration detected");
            string fullPath = Path.Combine(_configurationPath, "MyApache.conf");
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("The configuration file didn't exist", fullPath);
            }

            try
            {
                foreach (var line in File.ReadLines(fullPath))
                {
                    // Ignorer les lignes vides et les commentaires
                    if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
                        continue;

                    // Diviser la ligne en clé et valeur en utilisant le premier espace trouvé
                    var parts = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();

                        MapConfiguration(key, value);
                    }
                }
                 Logger.LogInfo("Configuration reloaded");
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors du chargement de la configuration du serveur.", ex);
            }


        }

        private void MapConfiguration(string key, string value)
        {
            var actionMap = new Dictionary<string, Action<string>>
            {
                ["servername"] = v => ServerName = v,
                ["serverip"] = v => {
                    if (IPAddress.TryParse(v, out var parsedIPAddress))
                        ServerIP = parsedIPAddress;
                    else
                        Logger.LogWarning("Server IP invalid");
                },
                ["serverport"] = v => TrySetIntValue(v, val => ServerPort = val, "Server port invalid"),
                ["maxallowedconnections"] = v => TrySetIntValue(v, val => MaxAllowedConnections = val, "Maximum allowed connection invalid"),
                ["maxrejectedconnections"] = v => TrySetIntValue(v, val => MaxRejectedConnections = val, "Maximum rejected connection invalid"),
                ["logfolder"] = v => LogFolder = v,
                ["logfile"] = v => LogFile = v,
                ["maxlogfile"] = v => TrySetIntValue(v, val => MaxLogFile = val, "Maximum log file invalid"),
                ["sizelogfile"] = v => TrySetIntValue(v, val => SizeLogFile = val * 1024, "Size log file invalid"),
                ["loglevel"] = v => LogLevel = v,
                ["rootdirectory"] = v => RootDirectory = v,
                ["persistence"] = v => Persistence = v
            };

            if (actionMap.TryGetValue(key.ToLower(), out var action))
            {
                action(value);
            }
            else
            {
                Logger.LogWarning($"Unknown configuration key: {key}");
            }
        }

        private void TrySetIntValue(string value, Action<int> setAction, string warningMessage)
        {
            if (int.TryParse(value, out var parsedValue))
            {
                setAction(parsedValue);
            }
            else
            {
                Logger.LogWarning(warningMessage);
            }
        }

    }

}
