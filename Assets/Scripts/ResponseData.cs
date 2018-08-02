using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ResponseData
{
    public Record[] records;
    public static ResponseData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ResponseData>(jsonString);
    }
}
[Serializable]
public class Record
{
    public int id;
    public int record;
    public string name;
    public int type;
}
