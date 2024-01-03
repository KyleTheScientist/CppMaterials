using ComputerPlusPlus;
using CppMaterials.Source;
using GorillaNetworking;
using Plugin = CppMaterials.Source.Plugin;

public class MaterialsScreen : IScreen
{
    public string Title => "Materials";

    public string Description => "";
    int matIndex;

    public string GetContent()
    {
        return $"\n\n    Material: {Plugin.Instance.materials[matIndex].Descriptor.Name}";
    }

    public void OnKeyPressed(GorillaKeyboardButton button)
    {
        if (button.characterString == "enter")
        {
            if (matIndex == Plugin.Instance.materials.Count - 1)
                matIndex = 0;
            else
                matIndex++;
        }
        MaterialController.LocalInstance.ChangeMaterial(Plugin.Instance.materials[matIndex]);
    }

    public void Start()
    {
    }
}