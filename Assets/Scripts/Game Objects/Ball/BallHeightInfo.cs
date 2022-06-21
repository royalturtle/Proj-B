using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHeightInfo {
    List<float> boundList, heightList, alphaList;
    public BallHeightInfo() {
        boundList = new List<float>();
        heightList = new List<float>();
        alphaList =  new List<float>();
    }

    public void Clear() {
        boundList.Clear();
        heightList.Clear();
        alphaList.Clear();
    }

    public float GetHeight(float relativeXpos) {
        float result = 0.1f;
        if(boundList.Count > 0) {
            int index = 0;
            relativeXpos = (relativeXpos > 1.0f) ? 1.0f : relativeXpos;
            float newRelative = relativeXpos / boundList[0];
            if(boundList != null) {
                for(int i = 1; i < boundList.Count; i++) {
                    if(boundList[i-1] <= relativeXpos) {
                        index = i;
                        newRelative = (relativeXpos - boundList[i - 1]) / (boundList[i] -  boundList[i - 1]);
                    }
                }
            }

            if(alphaList != null) {
                result = alphaList[index] * (newRelative - 1) * newRelative;
            }
        }
        return result;
    }

    public void SetThrowHeight() {
        if(boundList != null && heightList != null && alphaList != null ) {
            boundList.Clear();
            heightList.Clear();

            heightList.Add(0.1f);
            boundList.Add(1.0f);

            SetAlphaList();
        }
    }

    public void SetHeight(BaseballResultTypes result) {
        if(boundList != null && heightList != null && alphaList != null ) {
            boundList.Clear();
            heightList.Clear();

            switch(result) {
                case BaseballResultTypes.FLY_INNER:
                case BaseballResultTypes.FLY_OUTSIDE:
                case BaseballResultTypes.INFIELD_FLY:
                case BaseballResultTypes.HOMERUN:
                    heightList.Add(1.0f);

                    boundList.Add(1.0f);
                    break;
                case BaseballResultTypes.HIT1:
                case BaseballResultTypes.HIT1_LONG:
                    heightList.Add(0.75f);
                    heightList.Add(0.4f);

                    boundList.Add(0.75f);
                    boundList.Add(1.0f);
                    break;
                case BaseballResultTypes.HIT2:
                case BaseballResultTypes.HIT2_LONG:
                case BaseballResultTypes.HIT3:
                    heightList.Add(0.8f);
                    heightList.Add(0.4f);
                    heightList.Add(0.1f);

                    boundList.Add(0.8f);
                    boundList.Add(0.9f);
                    boundList.Add(1.0f);
                    break;
                case BaseballResultTypes.GROUND_BALL:
                    heightList.Add(0.1f);
                    heightList.Add(0.1f);
                    heightList.Add(0.1f);
                    heightList.Add(0.1f);

                    boundList.Add(0.25f);
                    boundList.Add(0.5f);
                    boundList.Add(0.75f);
                    boundList.Add(1.0f);
                    break;
            }
            SetAlphaList();
        }
    }

    void SetAlphaList() {
        alphaList.Clear();
        for(int i = 0; i < heightList.Count && i < boundList.Count; i++) {
            alphaList.Add(-4.0f * heightList[i]);
        }
    }
}
