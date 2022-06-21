using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitData
{
    public int BodyIndex {get; private set;}
    public int HatIndex {get; private set;}

    public OutfitData(
        int bodyIndex = 0,
        int hatIndex = 0)
    {
        BodyIndex = bodyIndex;
        HatIndex = hatIndex;
    }
}
