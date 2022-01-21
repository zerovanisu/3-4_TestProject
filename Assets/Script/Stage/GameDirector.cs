using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance;

    [Header("プレイヤー")]
    [SerializeField]
    public GameObject Player;

    [Header("遷移の速度")]
    [SerializeField]
    float Change_TimeMax;

    [Header("チュートリアルUI")]
    [SerializeField]
    GameObject Tutorial_UI;

    [Header("がめおべらUI")]
    [SerializeField]
    GameObject Gameover_UI;

    [Header("クリアUI")]
    [SerializeField]
    GameObject Clear_UI;

    string Scene_Name;
    float Change_Time;
    bool Changeing, Sound_End;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Scene_Name = SceneManager.GetActiveScene().name;

        switch (Scene_Name)
        {
            case "Stage1":
                Sound_Manager.Instance.PlayBGM(BGM.Stage_1,0.8f);
                break;
            case "Boss_Stage":
                Sound_Manager.Instance.PlayBGM(BGM.Boss,0.3f);
                break;
        }

        Tutorial_UI.SetActive(true);
        Gameover_UI.SetActive(false);
        Clear_UI.SetActive(false);

        Change_Time = Change_TimeMax;
        Changeing = Sound_End = false;
    }

    private void Update()
    {
        //チュートリアル表示時
        if(Tutorial_UI.activeSelf == true)
        {
            Player.GetComponent<PlayerManager>().Frieze = true;

            //ボタンを押されたらゲーム開始
            if (Input.GetButtonDown("Button_A"))
            {
                Sound_Manager.Instance.PlaySE(SE.Select, 1, 0);

                Tutorial_UI.SetActive(false);

                Player.GetComponent<PlayerManager>().Frieze = false;
            }
        }

        if(Sound_End == false)
        {
            if(Player.GetComponent<PlayerManager>().Player_Clear == true)
            {
                Sound_Manager.Instance.PlayBGM(BGM.Clear,0.5f);
                Sound_End = true;
            }
            else if (Player.GetComponent<PlayerManager>().Player_Deth == true)
            {
                Sound_Manager.Instance.PlayBGM(BGM.GameOver, 0.5f);
                Sound_End = true;
            }
        }

        GameOver();
        GameClear();        
    }

    void GameOver()
    {
        //ゲームオーバー画面
        if (Player.GetComponent<PlayerManager>().Player_Deth == true)
        {
            Gameover_UI.SetActive(true);

            //ボタンを押して少し経ったら遷移
            if (Input.GetButtonDown("Button_A") && Changeing == false)
            {
                Sound_Manager.Instance.PlaySE(SE.Select, 1, 0);
                Changeing = true;
            }
            if (Changeing == true)
            {
                Change_Time -= Time.deltaTime;
            }

            //現在のシーンをリロードする
            if (Change_Time <= 0)
            {
                string Now_Scene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(Now_Scene);
            }
        }
        else
        {
            Gameover_UI.SetActive(false);
        }
    }
    void GameClear()
    {
        //ゲームクリア画面
        if (Player.GetComponent<PlayerManager>().Player_Clear == true)
        {
            Clear_UI.SetActive(true);

            //ボタンを押したら少し待つ
            if (Input.GetButtonDown("Button_A") && Changeing == false)
            {
                Sound_Manager.Instance.PlaySE(SE.Select, 1, 0);
                Changeing = true;
            }
            if (Changeing == true)
            {
                Change_Time -= Time.deltaTime;
            }

            //少し待ったら遷移
            if (Change_Time <= 0)
            {

                /*//現在のシーン名を取得
                Scene_Name = SceneManager.GetActiveScene().name;

                //現在のシーンによって次に進めるシーンを変える
                switch (Scene_Name)
                {
                    case "Stage1":
                        SceneManager.LoadScene("Boss_Stage");
                        break;

                    case "Boss_Stage":
                        SceneManager.LoadScene("Clear_Scene");
                        break;
                }*/

                //タイトルに戻る
                SceneManager.LoadScene("Title_Scene");
            }
        }
        else
        {
            Clear_UI.SetActive(false);
        }
    }

    //ダメージ計算(((攻撃力(+装備) + (技の補正値 * 0.75) / 2 + 10)　-　((防御力(+装備) * 補正値) * 0.5))小数点未満切り捨て)
    //戻り値(ダメージ,攻撃力,攻撃補正値,防御力,防御補正値)
    public int Damage(int DamagePoint, int Atc, float A_Skill, int Body, float B_Skill)
    {

        DamagePoint = Mathf.FloorToInt((Atc + (A_Skill * 0.75f) / 2 + 10) - ((Body * B_Skill) * 0.75f));

        Debug.Log(DamagePoint);

        return DamagePoint;
    }
}
