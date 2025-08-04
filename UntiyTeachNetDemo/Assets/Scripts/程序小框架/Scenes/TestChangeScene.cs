using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChangeScene : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        ScenesManager.GetInstance().LoadScene("UIManagerTestScene", () => {
            Debug.Log("切换成功");
        });
    }
    // Update is called once per frame
    void Update() {

    }
}
