namespace CppMaterials.Source.GorillaCosmetics
{
    public class CosmeticDescriptor
    {
        public string Name = "Cosmetic";
        public string Author = "Author";
        public string Description = string.Empty;
        public bool CustomColors = false;
        public bool DisablePublicLobbies = false;
        public string ID => $"{Author}.{Name}";
    }
}