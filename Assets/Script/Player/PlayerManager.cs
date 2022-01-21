using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField]
    public int Player_HPMax;

    [Header("最大MP")]
    [SerializeField]
    public int Player_MPMax;

    [Header("現在のHP")]
    [SerializeField]
    public int Player_HP;

    [Header("現在のMP")]
    [SerializeField]
    public float Player_MP;

    [Header("攻撃力")]
    [SerializeField]
    public int Player_Attack;

    [Header("防御力")]
    [SerializeField]
    public int Player_Body;

    [Header("MP回復速度")]
    [SerializeField]
    public float MP_Heel;

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

    [Header("つららの消費MP")]
    [SerializeField]
    public int Turara_MP;

    [Header("リロードタイム")]
    [SerializeField]
    float Reload_Time;

    [Header("被弾後の硬直時間")]
    [SerializeField]
    float Damage_Time_Max;

    [Header("被弾後の無敵時間")]
    [SerializeField]
    float Muteki_Time_Max;

    [Header("無敵時の点滅速度")]
    [SerializeField]
    float Sprite_Speed;

    [Header("参照用")]
    [SerializeField]
    public GameObject Shot_Point;
    public bool Frieze;
    public bool Muteking;
    public bool Player_Clear, Player_Deth;

    [System.NonSerialized]
    float Horizontal;
    float Vertical;
    float Jump_Keep;
    float Gravity_P;
    float Reloading_Time;
    float DamagePoint;
    float Damage_Time;
    float Muteki_Time;
    SpriteRenderer Sprite_R;

    [System.NonSerialized]
    public bool Not_Gravity, IsGround, Jumoing, Player_Dash, Damageing;
    string Sprite_UD;

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
        Damage_Time = Damage_Time_Max;
        Muteki_Time = Muteki_Time_Max;

        Sprite_R = GetComponent<SpriteRenderer>();

        Player_Clear = Player_Dash = false;
    }

    void Update()
    {
        if(Frieze == false)
        {
            //入力受付
            Horizontal = Input.GetAxis("Horizontal_P");
            Vertical = Input.GetAxis("Vertical_P");
        }
        else
        {
            Rb.velocity = new Vector2(0, 0);
        }

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

        if(Player_MP > Turara_MP)
        {
            Shot();
        }

        if(Player_HP <= 0)
        {
            Player_Deth = true;
        }
    }

    private void FixedUpdate()
    {
        if(Frieze == false)
        {
            Move();
            Gravity();
            Jump();

            //MP回復
            if (Player_MP < Player_MPMax)
            {
                Player_MP += MP_Heel;
            }
            //上限をオーバーしたら最大値に抑える
            else
            {
                Player_MP = Player_MPMax;
            }
        }

        GameOver();
        Muteki();
        DamageMove();
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
        if (Input.GetButton("Button_Y") && Reloading_Time <= 0)
        {
            //MPを消費
            Player_MP -= Turara_MP;

            //つららを生成
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

            //攻撃力を渡す
            Bullet.GetComponent<TuraraManager>().P_Atack = Player_Attack;

            Reloading_Time = Reload_Time;
        }

        //リロード
        if (Reloading_Time >= 0)
        {
            Reloading_Time -= Time.deltaTime;
        }
    }

    //被弾した時の処理
    public void DamageMove()
    {
        //被弾の処理
        if(Damageing == true)
        {
            //無敵をオンにする
            Muteking = true;

            if (Damage_Time > 0)
            {
                //プレイヤーを停止
                Frieze = true;

                //被弾アニメーション
                Anim.SetBool("Damage", true);

                Damage_Time -= Time.deltaTime;
            }
            else
            {
                //フラグ解除
                Anim.SetBool("Damage", false);
                Frieze = false;
                Damageing = false;
                Damage_Time = Damage_Time_Max;//カウントを元に戻す
            }
        }
    }

    //無敵時の処理
    void Muteki()
    {
        if (Muteking == true)
        {
            Muteki_Time -= Time.deltaTime;

            //点滅処理
            if(Sprite_R.color.a >= 1)
            {
                Sprite_UD = "Down";
            }
            else if(Sprite_R.color.a <= 0)
            {
                Sprite_UD = "Up";
            }
            
            if(Sprite_UD == "Down")
            {
                float Alpha = Sprite_R.color.a - Sprite_Speed;
                Sprite_R.color = new Color(1f, 1f, 1f, Alpha);
            }
            else if(Sprite_UD == "Up")
            {
                float Alpha = Sprite_R.color.a + Sprite_Speed;
                Sprite_R.color = new Color(1f, 1f, 1f, Alpha);
            }
        }
        if (Muteki_Time <= 0)
        {
            //フラグリセット
            Muteking = false;
            Muteking = false;
            Muteki_Time = Muteki_Time_Max;
            Sprite_R.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    void GameOver()
    {
        if (Player_Clear == true)
        {
            //動きを止める
            Frieze = true;

            //勝利モーションに切り替える
            Anim.SetBool("Clear", true);
        }
        else if (Player_Deth == true)
        {
            //動きを止める
            Frieze = true;

            //敗北モーションに切り替える
            Anim.SetBool("Deth", true);
        }
        else
        {
            Anim.SetBool("Clear",false);
            Anim.SetBool("Deth", false);
        }
    }
}
