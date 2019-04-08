using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointController : MonoBehaviour
{

    //***回転関係***
    //1度に回転させる角度
    private const float ROLL_ANGLE = 180.0f;
    private const float ROLL_ANGLE_HALF = 90.0f;

    //回転速度
    private const float ROLL_SPEED = 20.0f;

    //回転速度保持用
    private float xRolling;
    private float zRolling;

    //回転角度合計保持用
    private float xRollSum;
    private float zRollSum;

    //回転方向保持
    private float rollDirection;

    //***移動関係***
    //移動スピード
    private const float MOVE_SPEED = 50.0f;

    //***テクスチャ関係***
    //スプライトレンダラー（自分自身）
    private SpriteRenderer jointSpriteRenderer;

    //表と裏のスプライト
    public Sprite frontSprite;
    public Sprite backSprite;

    //スプライト書き換え判定フラグ
    private bool isChangeSprite;

    //***ステータス***
    //方向
    private enum JointDirectionStatus {
        Left = 1
        , Right = 2
    }
    private JointDirectionStatus jointDirection;

    //裏表
    private enum JoinFrontBackStatus {
        Front = 1
        , Back = 2
    }
    private JoinFrontBackStatus jointFrontBack;

    //色
    public JointDirector.JointColorStatus jointLeftColor;
    public JointDirector.JointColorStatus jointRightColor;

    //GameDirectorオブジェクト
    private GameObject gameDirectorObj;

    //カメラオブジェクト
    private GameObject mainCameraObj;

    //カメラからの距離（ジョイント失敗判定用）
    private const float outDistance = 13.0f;

    // Start is called before the first frame update
    void Start()
    {

        this.xRolling = 0f;
        this.zRolling = 0f;

        this.xRollSum = 0f;
        this.zRollSum = 0f;

        this.rollDirection = 0f;

        this.jointSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        this.isChangeSprite = false;

        //GameDirectorオブジェクトを取得
        this.gameDirectorObj = GameObject.Find("GameDirector");

        //メインカメラオブジェクトを取得
        this.mainCameraObj = GameObject.Find("Main Camera");

        //裏表の設定
        int jointStatus = Random.Range(0, 2);
        if (jointStatus == 0) {
            this.jointFrontBack = JoinFrontBackStatus.Front;
            this.jointSpriteRenderer.sprite = this.frontSprite;
        }
        else {
            this.jointFrontBack = JoinFrontBackStatus.Back;
            //色を反転
            switch (this.jointLeftColor) {
                case JointDirector.JointColorStatus.Blue:
                    this.jointLeftColor = JointDirector.JointColorStatus.Red;
                    break;

                case JointDirector.JointColorStatus.Red:
                    this.jointLeftColor = JointDirector.JointColorStatus.Blue;
                    break;
            }
            switch (this.jointRightColor) {
                case JointDirector.JointColorStatus.Blue:
                    this.jointRightColor = JointDirector.JointColorStatus.Red;
                    break;

                case JointDirector.JointColorStatus.Red:
                    this.jointRightColor = JointDirector.JointColorStatus.Blue;
                    break;
            }
            this.jointSpriteRenderer.sprite = this.backSprite;
        }

        //向きの設定
        jointStatus = Random.Range(0, 2);
        if (jointStatus == 0) {
            this.jointDirection = JointDirectionStatus.Left;
        }
        else {
            this.jointDirection = JointDirectionStatus.Right;
            transform.Rotate(0, 0, ROLL_ANGLE);
        }


    }

    // Update is called once per frame
    void Update()
    {

        //ゲーム終了時は何もしない
        if(!this.gameDirectorObj.GetComponent<GameDirector>().playingGame) {
            return;
        }

        //接続済みの場合は何もしない
        if(GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Static) {
            return;
        }

        //カメラから離れすぎたらゲームオーバー
        if(transform.position.x > this.mainCameraObj.transform.position.x + outDistance) {

            //GameDirectorにゲームオーバーを伝える
            this.gameDirectorObj.GetComponent<GameDirector>().GameOver();
            Destroy(this);
            return;

        }

        //左右回転角度設定
        if(Input.GetKey(KeyCode.LeftArrow)) {

            this.zRolling = ROLL_SPEED;
            this.rollDirection = 1f;

        }

        if (Input.GetKey(KeyCode.RightArrow)) {

            this.zRolling = ROLL_SPEED;
            this.rollDirection = -1f;

        }

        //上下回転
        if (Input.GetKey(KeyCode.UpArrow)) {

            this.xRolling = ROLL_SPEED;
            this.rollDirection = 1f;

        }

        if (Input.GetKey(KeyCode.DownArrow)) {

            this.xRolling = ROLL_SPEED;
            this.rollDirection = -1f;

        }

        //回転
        transform.Rotate(this.xRolling * this.rollDirection, 0, this.zRolling * this.rollDirection);

        //回転角度合計加算
        this.zRollSum += this.zRolling;
        this.xRollSum += this.xRolling;

        //z軸の回転が半分になったら、左右の属性を書き換える。
        if (this.zRollSum >= ROLL_ANGLE_HALF && !this.isChangeSprite) {

            switch (this.jointDirection) {
                case JointDirectionStatus.Left:
                    this.jointDirection = JointDirectionStatus.Right;
                    break;

                case JointDirectionStatus.Right:
                    this.jointDirection = JointDirectionStatus.Left;
                    break;
            }

            this.isChangeSprite = true;

        }

        //x軸の回転が半分になったら、裏表の属性・スプライト・色の属性を書き換える
        if (this.xRollSum >= ROLL_ANGLE_HALF && !this.isChangeSprite) {

            switch (this.jointFrontBack) {
                case JoinFrontBackStatus.Front:
                    this.jointFrontBack = JoinFrontBackStatus.Back;
                    this.jointSpriteRenderer.sprite = this.backSprite;
                    break;

                case JoinFrontBackStatus.Back:
                    this.jointFrontBack = JoinFrontBackStatus.Front;
                    this.jointSpriteRenderer.sprite = this.frontSprite;
                    break;
            }

            switch (this.jointLeftColor) {
                case JointDirector.JointColorStatus.Blue:
                    this.jointLeftColor = JointDirector.JointColorStatus.Red;
                    break;

                case JointDirector.JointColorStatus.Red:
                    this.jointLeftColor = JointDirector.JointColorStatus.Blue;
                    break;
            }

            switch (this.jointRightColor) {
                case JointDirector.JointColorStatus.Blue:
                    this.jointRightColor = JointDirector.JointColorStatus.Red;
                    break;

                case JointDirector.JointColorStatus.Red:
                    this.jointRightColor = JointDirector.JointColorStatus.Blue;
                    break;
            }

            this.isChangeSprite = true;

        }

        //回転角度合計が ROLL_ANGLE 以上になったら、回転をやめる。
        if (this.zRollSum >= ROLL_ANGLE || this.xRollSum >= ROLL_ANGLE) {

            this.zRolling = 0f;
            this.zRollSum = 0f;
            this.xRolling = 0f;
            this.xRollSum = 0f;
            this.rollDirection = 0f;
            this.isChangeSprite = false;

        }

        //ジョイント
        if (Input.GetKey(KeyCode.Space)) {

            //左へ移動
            float moveDirection = 0f;
            moveDirection = -1f;

            GetComponent<Rigidbody2D>().velocity = transform.right.normalized * MOVE_SPEED * moveDirection;

        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {

        //既にStatic化してたら、何もしない。
        if(GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Static) {
            return;
        }

        //自身をStatic化
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        //音を鳴らす
        AudioSource jointSE = gameObject.GetComponent<AudioSource>();
        jointSE.Play();

        //JointDirectorに接続したことを伝える
        GameObject jointDirectorObj = GameObject.Find("JointDirector");
        jointDirectorObj.GetComponent<JointDirector>().JointOnCollisionEnter(this.jointLeftColor, this.jointRightColor);

    }

}
