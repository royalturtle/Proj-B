using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectConditionListView : MonoBehaviour {
    [field:SerializeField] public List<SelectConditionItem> itemList {get; private set;}
    public int Count { get { return itemList == null ? 0 : itemList.Count; } }

    public void SetData(int index, LocalizationTypes key, int value) {
        if(Utils.IsValidIndex(itemList, index)) {
            itemList[index].SetLabel(key:key, value:value);
        }
    }
}
