using System;

namespace SerializeExtension
{
    public interface IScriptableObjectSerialize<T,TEnum>
    {
        T GetPropertyAsset(TEnum type);
        void InstantiateFromProperty(TEnum type,Action completed = null);
    }
}


