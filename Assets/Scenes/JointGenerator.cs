using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointGenerator : MonoBehaviour
{

    //カメラオブジェクト
    private GameObject mainCameraObj;

    //ジョイントオブジェクト
    public GameObject jointTypeAPrefabObj;
    public GameObject jointTypeBPrefabObj;

    // Start is called before the first frame update
    void Start()
    {

        //カメラオブジェクトを取得
        this.mainCameraObj = GameObject.Find("Main Camera");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ジョイントを生成する
    public void GenerateJoint() {

        //カメラの位置を取得
        Vector3 cameraPos = this.mainCameraObj.transform.position;
        Vector2 jointPosVector2 = new Vector2(cameraPos.x + 5.0f, cameraPos.y);

        //ランダムでジョイントを生成
        int jointType = Random.Range(0, 2);
        switch (jointType) {
            case 0:
                Instantiate(jointTypeAPrefabObj, jointPosVector2, Quaternion.identity);
                break;
            case 1:
                Instantiate(jointTypeBPrefabObj, jointPosVector2, Quaternion.identity);
                break;
        }

    }

}
