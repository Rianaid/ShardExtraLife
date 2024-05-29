using ProjectM;
using Stunlock.Core;
using System;
using System.IO;
using Unity.Entities;
using UnityEngine;

namespace ShardExtraLife.Utils
{
    internal class Helper
    {
        private static World? _serverWorld;
        public static EntityManager EntityManager => Server.EntityManager;
        public static PrefabCollectionSystem PrefabCollectionSystem => Server.GetExistingSystemManaged<PrefabCollectionSystem>();
        public static World Server
        {

            get
            {
                if (_serverWorld != null) return _serverWorld;
                _serverWorld = GetWorld("Server")
                    ?? throw new System.Exception("There is no Server world (yet). Did you install a server mod on the client?");
                return _serverWorld;
            }
        }
        public static bool IsServer => Application.productName == "VRisingServer";


        private static World GetWorld(string name)
        {
            foreach (var world in World.s_AllWorlds)
            {
                if (world.Name == name)
                {
                    return world;
                }
            }
            return null;
        }
        public static void EntityCompomponentDumper(string filePath, Entity entity)
        {
            string name;
            if (!Helper.EntityManager.HasComponent<PrefabGUID>(entity))
            {
                name = $"No name!";
            }
            else
            {
                name = Helper.PrefabCollectionSystem.PrefabGuidToNameDictionary[Helper.EntityManager.GetComponentData<PrefabGUID>(entity)];
            }
            File.AppendAllText(filePath, $"----------------------{name}-----------------------" + Environment.NewLine);
            File.AppendAllText(filePath, $"----------------------{entity.ToString()}-----------------------" + Environment.NewLine);
            File.AppendAllText(filePath, "{" + Environment.NewLine);
            foreach (var componentType in Server.EntityManager.GetComponentTypes(entity))
            { File.AppendAllText(filePath, $"{componentType.ToString()}" + Environment.NewLine); }
            File.AppendAllText(filePath, "}" + Environment.NewLine);
            File.AppendAllText(filePath, $"--------------------------------------------------" + Environment.NewLine);

        }
    }
}
