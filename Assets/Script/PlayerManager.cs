using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("速力")]
    [SerializeField]
    float Speed_P;

    [Header("ジャンプ力")]
    [SerializeField]
    float Jump_P;

    [SerializeField]
    float Jump_Max;

    [Header("重力")]
    [SerializeField]
    float GravityScale;

    [Header("重力の影響を受けているか")]
    [SerializeField]
    public bool Not_Gravity;

    float Horizontal;
    float Vertical;
    float Jump_keep;
    float Gravity_P;

    public bool Jumoing;

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
        if (Input.GetButtonDown("Button_B"))
        {
            Jumoing = true;
        }
        if (Input.GetButtonUp("Button_B"))
        {
            Jumoing = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Gravity();
        Jump();
    }

    void Move()
    {
        float SPVector = Speed_P * Horizontal;

        Rb.velocity = new Vector2(SPVector, transform.position.y);

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
        //ジャンプ力を保持
        Jump_keep = Jump_P;

        if(Jumoing == false)
        {

        }
        else
        {

        }
        
    }

    void Gravity()
    {

        if (Not_Gravity == false)
        {
            //現在の高さから重力加速度を差し引いた分の座標に置き換える
            float Gravity_total = transform.position.y - Gravity_P;

            transform.position = new Vector2(transform.position.x, Gravity_total);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y);
        }
    }
}
