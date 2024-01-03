using BepInEx;
using CPPMaterials.Source.Tools;
using CPPMaterials.Source.GorillaCosmetics;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilla;

namespace CPPMaterials.Source
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.11")]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        public List<GorillaMaterial> materials;
        public static Plugin Instance;

        void Awake()
        {
            Logging.Init();
            Instance = this;
        }

        void Start() => Utilla.Events.GameInitialized += OnGameInitialized;

        void LoadMaterials()
        {
            string directory = Paths.PluginPath + "/" + PluginInfo.Name;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            // Load all the asset bundles from the directory
            string[] files = Directory.GetFiles(directory);
            materials = new List<GorillaMaterial>() { new GorillaMaterial("Default") };

            for (int i = 0; i < files.Length; i++)
            {
                if (Path.GetExtension(files[i]) != ".gmatplus")
                    continue;
                try
                {
                    materials.Add(new GorillaMaterial(files[i]));
                    Logging.Info("Loaded material " + materials[i].Descriptor.Name);
                }
                catch (Exception e) { Logging.Exception(e); }
            }

            GorillaTagger.Instance.offlineVRRig.gameObject.AddComponent<MaterialController>();
        }
        void OnGameInitialized(object _, EventArgs __)
        {
            LoadMaterials();
        }

        void OnEnable() => HarmonyPatches.ApplyHarmonyPatches();

        void OnDisable() => HarmonyPatches.RemoveHarmonyPatches();

        public GorillaMaterial GetMaterial(string matID)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i].Descriptor.ID == matID)
                    return materials[i];
            }
            return null;
        }
    }
}
