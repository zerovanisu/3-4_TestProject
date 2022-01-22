using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField]
    public int Enemy_HPMax;

    [Header("現在のHP")]
    [SerializeField]
    public int Enemy_HP;

    [Header("攻撃力")]
    [SerializeField]
    public int Enemy_Attack;

    [Header("防御力")]
    [SerializeField]
    public int Enemy_Body;

    [Header("速力")]
    [SerializeField]
    float Speed_E;

    public int DamagePoint;
    public bool Frieze;//停止フラグ

    GameObject Player;

    Vector2 Target_Pos, MyScale;

    Renderer Renderer_E;

    void Start()
    {
        Enemy_HP = Enemy_HPMax;
        Renderer_E = GetComponent<SpriteRenderer>();
        Frieze = true;//画面内に入るまで動き始めないようにする
        Player = GameObject.Find("Player");
        MyScale = transform.localScale;
    }

    void Update()
    {
        if(Renderer_E.isVisible)
        {
            Frieze = false;
        }

        //基本この中で全ての処理を行う(停止状態を反映させるため)
        if(Frieze == false)
        {
            //ぴちゅーん
            if (Enemy_HP <= 0)
            {
                Sound_Manager.Instance.PlaySE(SE.Deth, 0.2f, 0);
                Destroy(this.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        //止まっていないときプレイヤーを追う
        if (Frieze == false)
        {
            Target_Pos = new Vector2 (Player.transform.position.x,transform.position.y);

            float MoveVec = Speed_E * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, Target_Pos, MoveVec);

            if(transform.position.x < Target_Pos.x)
            {
                transform.localScale = new Vector2(-MyScale.x, MyScale.y);
            }
            else
            {
                transform.localScale = new Vector2(MyScale.x,MyScale.y);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject Player = collision.gameObject;

            //プレイヤーが無敵でないならダメージを与える
            if(Player.GetComponent<PlayerManager>().Muteking == false)
            {
                Sound_Manager.Instance.PlaySE(SE.Damage_P,1,0);

                //相手のステータスを取得
                int P_Body = Player.GetComponent<PlayerManager>().Player_Body;

                //ゲームマネージャーの式でダメ計を行う
                DamagePoint = GameDirector.Instance.Damage(DamagePoint, Enemy_Attack, 1, P_Body, 1);

                //計算結果で出たダメージ分相手の体力を削る
                Player.GetComponent<PlayerManager>().Player_HP -= DamagePoint;

                //プレイヤーの被弾処理
                Player.GetComponent<PlayerManager>().Damageing = true;
            }
        }
    }
}
