using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("����")]
    [SerializeField]
    float Speed_P;

    [Header("�_�b�V�����x")]
    [SerializeField]
    float Speed_D;

    [Header("�W�����v���鍂��")]
    [SerializeField]
    float Jump_Max;

    [Header("�W�����v����X�s�[�h")]
    [SerializeField]
    float Jump_Speed;

    [Header("�d��")]
    [SerializeField]
    float GravityScale;

    [Header("�d�͂̉e�����󂯂Ă��邩")]
    [SerializeField]
    public bool Not_Gravity;

    [Header("�ڒn���Ă��邩")]
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
        Gravity_P = GravityScale;//�d�͂��擾
    }

    void Update()
    {
        //���͎�t
        Horizontal = Input.GetAxis("Horizontal_P");
        Vertical = Input.GetAxis("Vertical_P");

        //�_�b�V������
        if (Input.GetButton("Button_L")) { Player_Dash = true; }
        else if (Input.GetButtonUp("Button_L")) { Player_Dash = false; }

        //�ڒn���Ă��鎞
        if (IsGround == true)
        {
            //�d�͂̉e���𖳂���(���Ɉ�������Ȃ��悤�ɂ���)
            Not_Gravity = true;

            //�W�����v����
            if (Input.GetButtonDown("Button_B"))
            {
                //��ׂ鍂�����v�Z(���̍�������Jump_Keep�܂Ŕ�ׂ�)
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

        //������ς���
        if(Horizontal < 0)
        {
            //�E����
        }
        else if(Horizontal > 0)
        {
            //������
        }
    }

    void Jump()
    {
        //�W�����v�{�^���������ꂽ&��������ׂ���E�ɒB���Ă��Ȃ��Ƃ�
        if(Jumoing == true && transform.position.y < Jump_Keep)
        {
            //��ё�����
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
            //���݂̍�������d�͉����x���������������̍��W�ɒu��������
            float Gravity_total = transform.position.y - Gravity_P;

            Rb.velocity = new Vector2(Rb.velocity.x,Gravity_total);
        }
        else
        {
            Rb.velocity = new Vector2(Rb.velocity.x, 0);
        }
    }
}
