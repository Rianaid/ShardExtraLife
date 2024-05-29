using Il2CppInterop.Runtime.Injection;
using ShardExtraLife.Databases;
using System;
using UnityEngine;

namespace ShardExtraLife.Utils
{
    internal class ModLoop : MonoBehaviour
    {
        private static ModLoop _instance;
        public static DateTime LastUpdate = DateTime.Now;

        void Update()
        {
            if (DB.ModEnabled)
            {
                TimeSpan elapsed = DateTime.Now - LastUpdate;
                if (elapsed.TotalMinutes > 5)
                {
                    LastUpdate = DateTime.Now;
                }
            }
        }

        void Start()
        {
            LastUpdate = DateTime.Now;
        }

        void LateUpdate()
        {
        }

        public static void Initialize()
        {
            if (!ClassInjector.IsTypeRegisteredInIl2Cpp<ModLoop>())
            {
                ClassInjector.RegisterTypeInIl2Cpp<ModLoop>();
            }
            _instance = Plugin.Instance.AddComponent<ModLoop>();

        }

        public static void Uninitialize()
        {
            Destroy(_instance);
            _instance = null;
        }
    }
}

