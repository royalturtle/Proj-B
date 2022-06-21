using System.Collections;
using System.Collections.Generic;

public class LineupDefense {
    public List<BatterPositionEnum> ListData {get; private set;}
    public Dictionary<BatterPositionEnum, int> DictData {get; private set;}

    public LineupDefense(Dictionary<BatterPositionEnum, int> positionData) {
        DictData = positionData;

        ListData = new List<BatterPositionEnum>();

        for(int i = 0; i < DictData.Keys.Count; i++) {
            ListData.Add(BatterPositionEnum.CANDIDATE);
        }

        foreach(BatterPositionEnum position in DictData.Keys) {
            ListData[DictData[position]] = position;
        }
    }
}
