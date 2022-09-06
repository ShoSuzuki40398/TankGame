using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Copy
{
    //--------------------------------------------------------------------------------
    // �����ɓn�����I�u�W�F�N�g���f�B�[�v�R�s�[�����I�u�W�F�N�g�𐶐����ĕԂ�
    // �W�F�l���b�N���\�b�h��
    //--------------------------------------------------------------------------------
    public static T DeepCopy<T>(T target)
    {
        T result;
        BinaryFormatter b = new BinaryFormatter();
        MemoryStream mem = new MemoryStream();

        try
        {
            b.Serialize(mem, target);
            mem.Position = 0;
            result = (T)b.Deserialize(mem);
        }
        finally
        {
            mem.Close();
        }

        return result;
    }
    // �g�����\�b�h��
    public static object DeepCopy(this object target)
    {
        object result;
        BinaryFormatter b = new BinaryFormatter();
        MemoryStream mem = new MemoryStream();

        try
        {
            b.Serialize(mem, target);
            mem.Position = 0;
            result = b.Deserialize(mem);
        }
        finally
        {
            mem.Close();
        }

        return result;
    }
}
