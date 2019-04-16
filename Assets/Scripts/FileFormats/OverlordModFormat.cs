namespace RolePlayOverlord.FileFormats
{
    // TODO: Move mod-related data definitions to a shared DLL once we have a proper writing tool.
    public enum ResourceTypeID
    {
        WallTexture,
        FloorTexture,
        CeilingTexture,
        SkyboxTexture,
        Audio,

        CharacterModel,
        FigureModel,

        Count
    }

    [System.Serializable]
    public class ResourceType
    {
        public int FirstResourceIndex;
        public int OnePastLastResourceIndex;

        public int Count
        {
            get { return OnePastLastResourceIndex - FirstResourceIndex; }
        }
    }

    [System.Serializable]
    public class Resource
    {
        public string File;
    }

    public class LocalData
    {
        public string[] Documents;
        public string[] Saves;
    }

    [System.Serializable]
    public class ModData
    {
        public ResourceType[] ResourceTypeEntries;
        public Resource[] Resources;
        [System.NonSerialized] public LocalData LocalData;
    }

    [System.Serializable]
    public class ModManifest
    {
        public string Name;
        public ulong RMMCode;
        public uint Version;

        public ModData Data;
    }

    public static class ModFormatInfo
    {
        // TODO: Make this a compile time constant _somehow_.
        public static readonly ulong RMMCode = RIFFCode64('r', 'm', 'm', ' ');

        public const uint VERSION = 0;

        public static ulong RIFFCode64(char a, char b, char c, char d)
        {
            return (((ulong)(a) << 0) | (ulong)(b) << 16 | (ulong)(c) << 32 | (ulong)(d) << 48);
        }
    }
}
