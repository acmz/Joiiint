using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDirector : MonoBehaviour
{
    //色の種類
    public enum JointColorStatus {
        Red = 1
        , Blue = 2
    }

    //接続されるジョイントの色
    private JointColorStatus oldJointColor;

    //ゲームプレイ判定フラグ
    private bool isFirstJoin;

    //JointGeneratorオブジェクト
    private GameObject jointGeneratorObj;

    //ジョイント生成フラグ
    private bool generateJoint;

    //カメラオブジェクト
    private GameObject mainCameraObj;

    //GameDirectorオブジェクト
    private GameObject gameDirectorObj;

    // Start is called before the first frame update
    void Start()
    {

        //JointGeneratorオブジェクトを取得
        this.jointGeneratorObj = GameObject.Find("JointGenerator");

        //メインカメラオブジェクトを取得
        this.mainCameraObj = GameObject.Find("Main Camera");

        //GameDirectorオブジェクトを取得
        this.gameDirectorObj = GameObject.Find("GameDirector");

        this.generateJoint = true;
        this.isFirstJoin = true;

    }

    // Update is called once per frame
    void Update()
    {

        //ゲームが始まっていない場合、何もしない
        if (!this.gameDirectorObj.GetComponent<GameDirector>().playingGame) {
            return;
        }

        //ジョイントを生成済みの場合は何もしない
        if (!this.generateJoint) {
            return;
        }

        //ジョイントを生成
        this.jointGeneratorObj.GetComponent<JointGenerator>().GenerateJoint();

        //ジョイント生成フラグを戻す
        this.generateJoint = false;

    }

    public void JointOnCollisionEnter(JointColorStatus inLeftJointColor, JointColorStatus inRightJointColor) {

        //接続結果判定
        if (this.isFirstJoin) {

            //1つ目は無条件で接続成功
            //次のジョイントを生成する
            this.oldJointColor = inRightJointColor;
            this.generateJoint = true;
            this.isFirstJoin = false;

            //GameDirectorにジョイントしたことを伝える
            this.gameDirectorObj.GetComponent<GameDirector>().WriteJointNum();

            return;

        }

        if(this.oldJointColor != inLeftJointColor) {

            //色が一致しないのでNG
            //ゲームオーバー処理
            this.gameDirectorObj.GetComponent<GameDirector>().GameOver();

            return;

        }

        //色が一致したのでOK
        this.oldJointColor = inRightJointColor;

        //カメラの位置をずらす
        Vector3 cameraPos = this.mainCameraObj.transform.position;

        this.mainCameraObj.transform.position = new Vector3(cameraPos.x + 4.0f, cameraPos.y, cameraPos.z);

        //GameDirectorにジョイントしたことを伝える
        this.gameDirectorObj.GetComponent<GameDirector>().WriteJointNum();

        //次のジョイントを生成する
        this.generateJoint = true;

    }
}
