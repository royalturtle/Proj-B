using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutsObject : MonoBehaviour
{
    private readonly byte[] offColor = new byte[4] { 128, 128, 128, 255 };
    private readonly byte[] onColor = new byte[4] { 200, 16, 0, 255 };

    [SerializeField]
    private List<Image> _outImgList;

    private bool[] checkList = null;

    private void Awake()
    {
        checkList = new bool[_outImgList.Count];
    }

    private void ClearOuts()
    {
        for (int i = 0; i < _outImgList.Count; i++)
        {
            ChangeImgColor(index: i, isAdd: false);
            checkList[i] = false;
        }
    }

    public void SetOutCount(int count)
    {
        // Check the unable small count value
        int _count = (count < 0) ? 0 : count;

        // Check the unable big count value
        _count = (_count > _outImgList.Count) ? _outImgList.Count : _count;

        if (_count == 0) ClearOuts();
        else
        {
            for(int i = 0; i < _count; i++)
            {
                if (!checkList[i]) ChangeImgColor(i);
            }
        }
    }

    private void ChangeImgColor(int index, bool isAdd=true)
    {
        // Check the unable small index value
        int _index = (index < 0) ? 0 : index;

        // Check the unable big index value
        _index = (_index > _outImgList.Count - 1) ? _outImgList.Count - 1 : _index;

        // which color to change
        byte[] changeColor = (isAdd) ? onColor : offColor;

        // change color
        _outImgList[_index].color = new Color32(changeColor[0], changeColor[1], changeColor[2], changeColor[3]);
        checkList[_index] = true;
    }
}
