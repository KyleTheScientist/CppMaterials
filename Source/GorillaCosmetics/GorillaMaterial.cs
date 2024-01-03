using ComputerPlusPlus.Tools;
using System;
using UnityEngine;

namespace CPPMaterials.Source.GorillaCosmetics
{
    public class GorillaMaterial
    {
        public string FileName { get; }
        public AssetBundle AssetBundle { get; }
        public CosmeticDescriptor Descriptor { get; }

        Material material;

        public GorillaMaterial(string path)
        {
            if (path != "Default")
            {
                try
                {
                    // load
                    FileName = path;
                    var bundleAndJson = PackageUtils.AssetBundleAndJSONFromPackage(FileName);
                    AssetBundle = bundleAndJson.bundle;
                    PackageJSON json = bundleAndJson.json;

                    // get material object and stuff
                    GameObject materialObject = AssetBundle.LoadAsset<GameObject>("_Material");
                    material = materialObject.GetComponent<Renderer>().material;

                    // Make Descriptor
                    Descriptor = PackageUtils.ConvertJsonToDescriptor(json);
                    if (material.shader.name == "Standard")
                    {
                        Logging.Warning("Material " + Descriptor.Name + " is using the Standard shader. This shader is not supported by Gorilla Tag anymore. Please use the Universal Render pipeline.");
                        material.shader = Shader.Find("GorillaTag/UberShader");
                        throw new Exception("Invalid Shader");
                    }
                }
                catch (Exception e)
                {
                    Logging.Debug(e);
                    throw new Exception($"Loading material at {path} failed.");
                }
            }
            else
            {
                Descriptor = new CosmeticDescriptor();
                Descriptor.Name = "Default";
                Descriptor.CustomColors = true;
                material = GorillaTagger.Instance.offlineVRRig.materialsToChangeTo[0];
            }
        }

        public Material GetMaterial()
        {
            return UnityEngine.Object.Instantiate(material);
        }
    }
}