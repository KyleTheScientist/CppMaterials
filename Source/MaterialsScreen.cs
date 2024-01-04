using ComputerPlusPlus;
using ComputerPlusPlus.Tools;
using CppMaterials.Source;
using GorillaNetworking;
using Plugin = CppMaterials.Source.Plugin;

public class MaterialsScreen : IScreen
{
    public string Title => "Materials";

    public string Description => "Use [W/S] to scroll. Press [Option 1] to toggle material info.";

    int matIndex;
    int page = 0, perPage = 8;
    public string Template = "    {0} {1}\n";
    bool infoPage;

    public string GetContent()
    {
        if (infoPage)
        {
            var descriptor = Plugin.Instance.materials[matIndex].Descriptor;
            return $"{descriptor.Name}\n" +
                $"  Author: {descriptor.Author}\n" +
                $"  Desc: {descriptor.Description}";
        }
        string content = "";
        for (int i = 0; i < perPage; i++)
        {
            if (i + page * perPage >= Plugin.Instance.materials.Count)
                break;
            int _matIndex = i + page * perPage;
            var gmat = Plugin.Instance.materials[_matIndex];
            content += string.Format(Template,
                matIndex == _matIndex ? ">" : " ",
                gmat.Descriptor.Name);
        }
        // if there is another page below, add a ...
        if (Plugin.Instance.materials.Count > (page + 1) * perPage)
            content += "     ...";

        return content;
    }

    public void OnKeyPressed(GorillaKeyboardButton button)
    {
        Logging.Debug("Pressed: ", button.characterString);
        switch (button.characterString)
        {
            case "W":
                if (matIndex == 0)
                    matIndex = Plugin.Instance.materials.Count - 1;
                else
                    matIndex--;
                break;
            case "S":
                if (matIndex == Plugin.Instance.materials.Count - 1)
                    matIndex = 0;
                else
                    matIndex++;
                break;
            case "option1":
                infoPage = !infoPage;
                break;
        }

        if (matIndex < page * perPage)
            page--;
        else if (matIndex >= (page + 1) * perPage)
            page++;

        MaterialController.LocalInstance.ChangeMaterial(Plugin.Instance.materials[matIndex]);
    }

    public void Start() { }
}