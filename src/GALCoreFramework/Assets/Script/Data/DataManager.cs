using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public string DataPath;
    public string dialogPath = "GalTest.txt";
    public Dictionary<int, Dictionary<int, DialogDefine>> dialogData = null;

    public DataManager()
    {
        this.DataPath = "Data/";
    }

    public void Load()
    {
        string json = File.ReadAllText(Path.Combine(DataPath,dialogPath));
        List<DialogDefine> nodes = JsonConvert.DeserializeObject<List<DialogDefine>>(json);

        dialogData = new Dictionary<int, Dictionary<int, DialogDefine>>();

        foreach (var node in nodes)
        {
            if (!dialogData.TryGetValue(node.branchid, out var branchDict))
            {
                branchDict = dialogData[node.branchid] = new Dictionary<int, DialogDefine>();
            }
            branchDict[node.id] = node;
        }
    }

}
