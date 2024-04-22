using System.Collections.Generic;
using System.Runtime.Serialization;

// [System.Serializable]
// public class SerializableKeyValuePair
// {
//     public int key;
//     public string value;

//     public SerializableKeyValuePair(int key, string value)
//     {
//         this.key = key;
//         this.value = value;
//     }
// }


// [System.Serializable]
// public class SerializableDictionary : Dictionary<int, string>
// {
//     // 这个列表将用于Unity编辑器中的显示
//     public List<SerializableKeyValuePair> pairs = new List<SerializableKeyValuePair>();

//     // 在序列化时，Unity会调用这个方法
//     public override void GetObjectData(SerializationInfo info, StreamingContext context)
//     {

//         base.GetObjectData(info, context);

//         // 将键值对添加到pairs列表中，以便Unity编辑器可以显示它们
//         pairs.Clear();
//         foreach (var kvp in this)
//         {
//             pairs.Add(new SerializableKeyValuePair(kvp.Key, kvp.Value));
//         }
//     }
// }


[System.Serializable]
public class SerializableUserPair
{
    public string key;
    public User value;

    public SerializableUserPair(string key, User value)
    {
        this.key = key;
        this.value = value;
    }
}

[System.Serializable]
public class PlcaeUserDict : Dictionary<string, User> 
{ 
    public List<SerializableUserPair> pairs = new List<SerializableUserPair>();

    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
        
        base.GetObjectData(info, context);
        // 将键值对添加到pairs列表中，以便Unity编辑器可以显示它们
        pairs.Clear();
        foreach (var kvp in this)
        {
            pairs.Add(new SerializableUserPair(kvp.Key, kvp.Value));
        }
    }

}