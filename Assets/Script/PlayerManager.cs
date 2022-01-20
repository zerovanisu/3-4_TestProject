using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField]
    public float Player_HPMax;

    [Header("最大MP")]
    [SerializeField]
    public float Player_MPMax;

    [Header("現在のHP")]
    [SerializeField]
    public float Player_HP;

    [Header("現在のMP")]
    [SerializeField]
    public float Player_MP;

    [Header("攻撃力")]
    [SerializeField]
    public float Player_Attack;

    [Header("速力")]
    [SerializeField]
    float Speed_P;

    [Header("ダッシュ速度")]
    [SerializeField]
    float Speed_D;

    [Header("ジャンプする高さ")]
    [SerializeField]
    float Jump_Max;

    [Header("ジャンプするスピード")]
    [SerializeField]
    float Jump_Speed;

    [Header("重力")]
    [SerializeField]
    float GravityScale;

    [Header("弾(つらら)")]
    [SerializeField]
    public GameObject Turara;

    [Header("リロードタイム")]
    [SerializeField]
    float Reload_Time;

    [Header("参照用")]
    [SerializeField]
    public GameObject Shot_Point;

    [System.NonSerialized]
    float Horizontal;
    float Vertical;
    float Jump_Keep;
    float Gravity_P;
    float Reloading_Time;

    [System.NonSerialized]
    public bool Not_Gravity, IsGround, Jumoing, Player_Dash;

    Animator Anim;

    Rigidbody2D Rb;

    private Vector2 Size;//大きさを保存

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Gravity_P = GravityScale;//重力を取得
        Anim = GetComponent<Animator>();

        Size = transform.localScale;//大きさを保存

        //ステータスを反映
        Player_HP = Player_HPMax;
        Player_MP = Player_MPMax;
    }

    void Update()
    {
        //入力受付
        Horizontal = Input.GetAxis("Horizontal_P");
        Vertical = Input.GetAxis("Vertical_P");

        //ダッシュ入力
        if (Input.GetButton("Button_L")) { Player_Dash = true; }
        else if (Input.GetButtonUp("Button_L")) { Player_Dash = false; }

        //接地している時
        if (IsGround == true)
        {
            //重力の影響を無くす(下に引っ張らないようにする)
            Not_Gravity = true;

            //ジャンプ入力
            if (Input.GetButtonDown("Button_B"))
            {
                //飛べる高さを計算(今の高さからJump_Keepまで飛べる)
                Jump_Keep = transform.position.y + Jump_Max;

                Jumoing = true;
            }
        }
        if (Input.GetButtonUp("Button_B"))
        {
            Jumoing = false;
            Not_Gravity = false;
        }

        Shot();
    }

    private void FixedUpdate()
    {
        Move();
        Gravity();
        Jump();
    }

    void Move()
    {
        if(Player_Dash == false)
        {
            float SPVector = Speed_P * Horizontal;

            Rb.velocity = new Vector2(SPVector, 0);
        }
        else
        {
            float SDVector = Speed_D * Horizontal;

            Rb.velocity = new Vector2(SDVector, 0);
        }

        //向きを変える
        if(Horizontal < 0)
        {
            transform.localScale = new Vector2(Size.x,Size.y);
        }
        else if(Horizontal > 0)
        {
            transform.localScale = new Vector2(-Size.x, Size.y);
        }
    }

    void Jump()
    {
        //ジャンプボタンが押された&高さが飛べる限界に達していないとき
        if(Jumoing == true && transform.position.y < Jump_Keep)
        {
            //飛び続ける
            Rb.velocity = new Vector2(Rb.velocity.x, Jump_Speed);
            Anim.SetBool("Jump",true);
        }
        else
        {
            Jumoing = false;
            Not_Gravity = false;
            Anim.SetBool("Jump", false);
        }
    }

    void Gravity()
    {

        if (Not_Gravity == false)
        {
            //現在の高さから重力加速度を差し引いた分の座標に置き換える
            float Gravity_total = transform.position.y - Gravity_P;

            Rb.velocity = new Vector2(Rb.velocity.x,Gravity_total);
        }
        else
        {
            Rb.velocity = new Vector2(Rb.velocity.x, 0);
        }
    }

    void Shot()
    {
        //射撃
        if (Input.GetButtonDown("Button_Y") && Reloading_Time <= 0)
        {
            GameObject Bullet = Instantiate(Turara);

            //生成位置を代入
            Bullet.transform.position = Shot_Point.transform.position;

            //右を向いていたら右向き、左を向いていたら左向きにする
            if(this.transform.localScale.x >0)
            {
                Bullet.GetComponent<TuraraManager>().Direction = -1;//飛ぶ向き
                Bullet.transform.rotation = Quaternion.Euler(0, 0, 270);//画像の向き
            }
            else if (this.transform.localScale.x < 0)
            {
                Bullet.GetComponent<TuraraManager>().Direction = 1;//飛ぶ向き
                Bullet.transform.rotation = Quaternion.Euler(0, 0, 90);//画像の向き
            }

            //ダメージ計算
            Bullet.GetComponent<TuraraManager>().P_Atack = Player_Attack;

            Reloading_Time = Reload_Time;
        }

        //リロード
        if (Reloading_Time >= 0)
        {
            Reloading_Time -= Time.deltaTime;
        }
    }
}
