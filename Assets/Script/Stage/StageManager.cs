using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("遷移の速度")]
    [SerializeField]
    float Scene_Speed;

    [Header("プレイヤー")]
    [SerializeField]
    GameObject Player;

    string Scene_Name;

    bool Scene_Change, Changeing;

    void Start()
    {
        
    }

    void Update()
    {
        //ゴール後ボタンが押されたら遷移開始
        if(Input.GetButtonDown("Button_A") && Scene_Change == true)
        {
            Changeing = true;
        }

        //遷移処理
        if(Changeing == true)
        {
            //現在のシーンによって次に進めるシーンを変える
            switch (Scene_Name)
            {
                case "Stage1":
                    SceneManager.LoadScene("Boss_Stage");
                    break;

                case "Boss_Stage":
                    SceneManager.LoadScene("Clear_Scene");
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーにクリアを伝える
            Player.GetComponent<PlayerManager>().Player_Clear = true;


            //現在のシーン名を取得
            Scene_Name = SceneManager.GetActiveScene().name;

            Scene_Change = true;
        }

    }
}
