using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{

    //接続したジョイントの数
    private GameObject jointNumUI;
    private int jointNum;
    private const string JOINT_NUM_MSG = " Joiiiiint!";

    //残り時間
    private GameObject timeLeftUI;
    private const float LIMIT_TIME = 180.0f;
    private const string TIME_LEFT_MSG = "残り ";
    private const string TIME_LEFT_FORMAT = "F2";
    private float timeLeft = 0f;

    //進行状況メッセージ
    private GameObject gameMsgUI;
    private const string MSG_WAIT = "Ready...";
    private const string MSG_START = "GO!!!";
    private const string MSG_SUCCESS = "SUCCESS!!!";
    private const string MSG_GAMEOVER = "GAME OVER...";

    //タイトル画面へ戻る案内メッセージ
    private GameObject returnMsgUI;

    //ゲームプレイコントロールフラグ
    public bool playingGame = false;

    // Start is called before the first frame update
    void Start()
    {

        //接続したジョイントの数のUI取得、初期化
        this.jointNumUI = GameObject.Find("JointNum");
        this.jointNum = 0;
        this.jointNumUI.GetComponent<Text>().text = this.jointNum + JOINT_NUM_MSG;

        //残り時間のUI取得、初期化
        this.timeLeftUI = GameObject.Find("TimeLeft");
        this.timeLeft = LIMIT_TIME;
        this.timeLeftUI.GetComponent<Text>().text = TIME_LEFT_MSG + this.timeLeft.ToString(TIME_LEFT_FORMAT);

        //進行状況メッセージのUI取得
        this.gameMsgUI = GameObject.Find("GameMsg");

        //タイトル画面へ戻るメッセージのUI取得、非表示
        this.returnMsgUI = GameObject.Find("ReturnMsg");
        this.returnMsgUI.GetComponent<Text>().enabled = false;

        //ゲームスタート演出
        StartCoroutine("GameStart");

    }

    // Update is called once per frame
    void Update()
    {

        //ゲームが終わっていたら、エスケープキーでタイトル画面に戻る
        if (this.gameMsgUI.GetComponent<Text>().enabled) {

            string viewMsg = this.gameMsgUI.GetComponent<Text>().text;
            if (viewMsg.Equals(MSG_SUCCESS) || viewMsg.Equals(MSG_GAMEOVER)) {

                if (Input.GetKey(KeyCode.Escape)) {

                    //タイトルシーンへ移動
                    SceneManager.LoadScene("TitleScene");

                }

            }

        }

        //ゲームが始まっていない場合、何もしない
        if (!this.playingGame) {
            return;
        }

        //残り時間減算
        this.TimeLeftMinus();

    }

    public void WriteJointNum() {

        //ジョイント数を加算し画面に表示する
        this.jointNum++;
        this.jointNumUI.GetComponent<Text>().text = this.jointNum + JOINT_NUM_MSG;

    }

    //残り時間減算
    private void TimeLeftMinus() {

        //残り時間が0の場合、ゲーム終了
        if (this.timeLeft == 0f) {

            this.playingGame = false;

            //メッセージ表示
            this.gameMsgUI.GetComponent<Text>().text = MSG_SUCCESS;
            this.gameMsgUI.GetComponent<Text>().enabled = true;
            this.returnMsgUI.GetComponent<Text>().enabled = true;
            return;

        }

        //残り時間減算
        this.timeLeft -= Time.deltaTime;

        //残り時間が0以下になったら、0固定にする
        if (this.timeLeft <= 0f) {

            this.timeLeft = 0f;

        }

        //表示更新
        this.timeLeftUI.GetComponent<Text>().text = TIME_LEFT_MSG + this.timeLeft.ToString(TIME_LEFT_FORMAT);

    }

    private IEnumerator GameStart() {

        //ゲームWait
        this.gameMsgUI.GetComponent<Text>().text = MSG_WAIT;

        yield return new WaitForSeconds(1.5f);
        
        //ゲーム開始
        this.gameMsgUI.GetComponent<Text>().text = MSG_START;

        yield return new WaitForSeconds(1.0f);

        //メッセージ非表示
        this.gameMsgUI.GetComponent<Text>().enabled = false;

        //ゲーム開始
        this.playingGame = true;

    }

    public void GameOver() {

        //ゲーム終了
        this.playingGame = false;

        //メッセージ表示
        this.gameMsgUI.GetComponent<Text>().text = MSG_GAMEOVER;
        this.gameMsgUI.GetComponent<Text>().enabled = true;
        this.returnMsgUI.GetComponent<Text>().enabled = true;

    }

}
