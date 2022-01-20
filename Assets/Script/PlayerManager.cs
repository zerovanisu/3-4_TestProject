using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("�ő�HP")]
    [SerializeField]
    public float Player_HPMax;

    [Header("�ő�MP")]
    [SerializeField]
    public float Player_MPMax;

    [Header("���݂�HP")]
    [SerializeField]
    public float Player_HP;

    [Header("���݂�MP")]
    [SerializeField]
    public float Player_MP;

    [Header("�U����")]
    [SerializeField]
    public float Player_Attack;

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

    [Header("�e(���)")]
    [SerializeField]
    public GameObject Turara;

    [Header("�����[�h�^�C��")]
    [SerializeField]
    float Reload_Time;

    [Header("�Q�Ɨp")]
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

    private Vector2 Size;//�傫����ۑ�

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Gravity_P = GravityScale;//�d�͂��擾
        Anim = GetComponent<Animator>();

        Size = transform.localScale;//�傫����ۑ�

        //�X�e�[�^�X�𔽉f
        Player_HP = Player_HPMax;
        Player_MP = Player_MPMax;
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

        //������ς���
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
        //�W�����v�{�^���������ꂽ&��������ׂ���E�ɒB���Ă��Ȃ��Ƃ�
        if(Jumoing == true && transform.position.y < Jump_Keep)
        {
            //��ё�����
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
            //���݂̍�������d�͉����x���������������̍��W�ɒu��������
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
        //�ˌ�
        if (Input.GetButtonDown("Button_Y") && Reloading_Time <= 0)
        {
            GameObject Bullet = Instantiate(Turara);

            //�����ʒu����
            Bullet.transform.position = Shot_Point.transform.position;

            //�E�������Ă�����E�����A���������Ă����獶�����ɂ���
            if(this.transform.localScale.x >0)
            {
                Bullet.GetComponent<TuraraManager>().Direction = -1;//��Ԍ���
                Bullet.transform.rotation = Quaternion.Euler(0, 0, 270);//�摜�̌���
            }
            else if (this.transform.localScale.x < 0)
            {
                Bullet.GetComponent<TuraraManager>().Direction = 1;//��Ԍ���
                Bullet.transform.rotation = Quaternion.Euler(0, 0, 90);//�摜�̌���
            }

            //�_���[�W�v�Z
            Bullet.GetComponent<TuraraManager>().P_Atack = Player_Attack;

            Reloading_Time = Reload_Time;
        }

        //�����[�h
        if (Reloading_Time >= 0)
        {
            Reloading_Time -= Time.deltaTime;
        }
    }
}
