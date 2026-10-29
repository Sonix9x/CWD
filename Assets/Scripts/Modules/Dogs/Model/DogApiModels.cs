using System;

namespace CWD.Scripts.Modules.Dogs.Model
{
    [Serializable]
    public class BreedsResponse
    {
        public BreedData[] data;
    }

    [Serializable]
    public class BreedResponse
    {
        public BreedData data;
    }

    [Serializable]
    public class BreedData
    {
        public string id;
        public string type;
        public BreedAttributes attributes;
    }

    [Serializable]
    public class BreedAttributes
    {
        public string name;
        public string description;
    }
}