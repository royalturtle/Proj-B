// https://forum.unity.com/threads/textmeshpro-precull-dorebuilds-performance.762968/#post-5083490
// https://stackoverflow.com/questions/58735616/how-calculate-autosize-for-group-buttons-in-unity3d
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
 
public class TextAutoSizeController : MonoBehaviour {
    [SerializeField] TMP_Text[] CompareObjects, ChangeObjects;
 
    /*
    void Awake() {
        if (TextObjects == null || TextObjects.Length == 0)
            return;
 
        float preferSize = 1000.0f;
        
        for(int i = 0; i < TextObjects.Length; i++) {
            TextObjects[i].enableAutoSizing = true;
            TextObjects[i].ForceMeshUpdate();
            if(TextObjects[i].fontSize < preferSize) {
                preferSize = TextObjects[i].fontSize;
            }
        }
 
        for (int i = 0; i < TextObjects.Length; i++) {
            TextObjects[i].enableAutoSizing = false;
            TextObjects[i].fontSize = preferSize;
        }
    }
    */

    public void SetOptimumSize() {
        float optimumPointSize = GetOptimumPointSize();
        for (int i = 0; i < ChangeObjects.Length; i++) {
            ChangeObjects[i].fontSize = optimumPointSize;
        }
    }

    float GetOptimumPointSize() {
        float result = 10.0f;
        if (CompareObjects != null && CompareObjects.Length > 0) {
            // Iterate over each of the text objects in the array to find a good test candidate
            // There are different ways to figure out the best candidate
            // Preferred width works fine for single line text objects
            int candidateIndex = 0;
            float maxPreferredWidth = 0;
 
            for (int i = 0; i < CompareObjects.Length; i++)
            {
                float preferredWidth = CompareObjects[i].preferredWidth;
                if (preferredWidth > maxPreferredWidth)
                {
                    maxPreferredWidth = preferredWidth;
                    candidateIndex = i;
                }
            }
 
            // Force an update of the candidate text object so we can retrieve its optimum point size.
            CompareObjects[candidateIndex].enableAutoSizing = true;
            CompareObjects[candidateIndex].ForceMeshUpdate();
            result = CompareObjects[candidateIndex].fontSize;
    
            // Disable auto size on our test candidate
            CompareObjects[candidateIndex].enableAutoSizing = false;
            
        }
        return result;
    }
}