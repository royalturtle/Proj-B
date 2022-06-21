using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestSceneManager : MonoBehaviour{
    [SerializeField] Toggle _toggle;
    [SerializeField] Button _button;
    
    void Start() {
        Debug.Log("AAA");
        if(_toggle) {
            _toggle.onValueChanged.AddListener((bool isOn) => {
                Debug.Log(isOn);
            });
        }   

        if(_button) {
            _button.onClick.AddListener(() => {
                SceneManager.LoadScene(5);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")){
            if(_toggle) {
                _toggle.isOn = false;
            }
        }
    }
}
