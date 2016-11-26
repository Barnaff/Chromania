using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kobapps
{
    public class ConstantsGeneratorSettings : ScriptableObject
    {

        public List<ConstantCategory> Categories;

        public bool GenerateScenes;

        public bool GenerateTags;

        public bool GenerateLayers;

        public bool GenerateBundleVersion;

        public bool GenerateBundleIdentifier;

        public bool GenerateSortingLayers;
    }

    [System.Serializable]
    public class ConstantCategory
    {
        [SerializeField]
        public string Name;

        [SerializeField]
        public List<ConstantValuePair> Constants;

        [SerializeField]
        public bool IsOpned = false;
    }

    [System.Serializable]
    public class ConstantValuePair
    {
        [SerializeField]
        public eConstantValueType ValueType;

        [SerializeField]
        public string Key;

        [SerializeField]
        public string StringValue;

        [SerializeField]
        public int IntValue;

        [SerializeField]
        public float FloatValue;

        [SerializeField]
        public Color ColorValue;

        [SerializeField]
        public Vector3 Vector3Value;

        [SerializeField]
        public Vector2 Vector2Value;
    }

    [System.Serializable]
    public enum eConstantValueType
    {
        String,
        Int,
        Float,
        Color,
        Vector3,
        Vector2,
    }
}

