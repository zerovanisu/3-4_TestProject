using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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

    [Header("重力の影響を受けているか")]
    [SerializeField]
    public bool Not_Gravity;

    [Header("接地しているか")]
    [SerializeField]
    public bool IsGround;

    float Horizontal;
    float Vertical;
    float Jump_Keep;
    float Gravity_P;

    public bool Jumoing;
    public bool Player_Dash;

    Rigidbody2D Rb;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Gravity_P = GravityScale;//重力を取得
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
    }

    private void FixedUpdate()
    {
        Move();
        Gravity();
        Jump();
        Debug.Log(Rb.velocity);
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
            //右向き
        }
        else if(Horizontal > 0)
        {
            //左向き
        }
    }

    void Jump()
    {
        //ジャンプボタンが押された&高さが飛べる限界に達していないとき
        if(Jumoing == true && transform.position.y < Jump_Keep)
        {
            //飛び続ける
            Rb.velocity = new Vector2(Rb.velocity.x, Jump_Speed);
        }
        else
        {
            Jumoing = false;
            Not_Gravity = false;
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
}
