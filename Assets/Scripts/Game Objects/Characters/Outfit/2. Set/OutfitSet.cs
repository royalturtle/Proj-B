using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitSet
{
    public const int DefenseCount = 9, RunnerCount = 4;

    public List<OutfitData> DefenseList {get; private set;}
    public List<OutfitData> RunnerList {get; private set;}

    public OutfitSet()
    {
        DefenseList = new List<OutfitData>();
        for(int i = 0; i < DefenseCount; i++)
        {
            DefenseList.Add(new OutfitData());
        }

        RunnerList = new List<OutfitData>();
        for(int i = 0; i < RunnerCount; i++)
        {
            RunnerList.Add(null);
        }
    }

    public void SetBases(BaseMultipleStatus baseStatus)
    {
        Debug.Log("Base Test");
        for(int i = 1; i < RunnerCount; i++)
        {
            BaseSingleStatus player = baseStatus.GetSingleStatus(i);
            RunnerList[i] = player != null ? new OutfitData() : null;
            Debug.Log(player != null);
        }
    }
}
