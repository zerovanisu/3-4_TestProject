using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("�ő�HP")]
    [SerializeField]
    public int Player_HPMax;

    [Header("�ő�MP")]
    [SerializeField]
    public int Player_MPMax;

    [Header("���݂�HP")]
    [SerializeField]
    public int Player_HP;

    [Header("���݂�MP")]
    [SerializeField]
    public float Player_MP;

    [Header("�U����")]
    [SerializeField]
    public int Player_Attack;

    [Header("�h���")]
    [SerializeField]
    public int Player_Body;

    [Header("MP�񕜑��x")]
    [SerializeField]
    public float MP_Heel;

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

    [Header("���̏���MP")]
    [SerializeField]
    public int Turara_MP;

    [Header("�����[�h�^�C��")]
    [SerializeField]
    float Reload_Time;

    [Header("��e��̍d������")]
    [SerializeField]
    float Damage_Time_Max;

    [Header("��e��̖��G����")]
    [SerializeField]
    float Muteki_Time_Max;

    [Header("���G���̓_�ő��x")]
    [SerializeField]
    float Sprite_Speed;

    [Header("�Q�Ɨp")]
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
        Damage_Time = Damage_Time_Max;
        Muteki_Time = Muteki_Time_Max;

        Sprite_R = GetComponent<SpriteRenderer>();

        Player_Clear = Player_Dash = false;
    }

    void Update()
    {
        if(Frieze == false)
        {
            //���͎�t
            Horizontal = Input.GetAxis("Horizontal_P");
            Vertical = Input.GetAxis("Vertical_P");
        }
        else
        {
            Rb.velocity = new Vector2(0, 0);
        }

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

            //MP��
            if (Player_MP < Player_MPMax)
            {
                Player_MP += MP_Heel;
            }
            //������I�[�o�[������ő�l�ɗ}����
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
        if (Input.GetButton("Button_Y") && Reloading_Time <= 0)
        {
            //MP������
            Player_MP -= Turara_MP;

            //���𐶐�
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

            //�U���͂�n��
            Bullet.GetComponent<TuraraManager>().P_Atack = Player_Attack;

            Reloading_Time = Reload_Time;
        }

        //�����[�h
        if (Reloading_Time >= 0)
        {
            Reloading_Time -= Time.deltaTime;
        }
    }

    //��e�������̏���
    public void DamageMove()
    {
        //��e�̏���
        if(Damageing == true)
        {
            //���G���I���ɂ���
            Muteking = true;

            if (Damage_Time > 0)
            {
                //�v���C���[���~
                Frieze = true;

                //��e�A�j���[�V����
                Anim.SetBool("Damage", true);

                Damage_Time -= Time.deltaTime;
            }
            else
            {
                //�t���O����
                Anim.SetBool("Damage", false);
                Frieze = false;
                Damageing = false;
                Damage_Time = Damage_Time_Max;//�J�E���g�����ɖ߂�
            }
        }
    }

    //���G���̏���
    void Muteki()
    {
        if (Muteking == true)
        {
            Muteki_Time -= Time.deltaTime;

            //�_�ŏ���
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
            //�t���O���Z�b�g
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
            //�������~�߂�
            Frieze = true;

            //�������[�V�����ɐ؂�ւ���
            Anim.SetBool("Clear", true);
        }
        else if (Player_Deth == true)
        {
            //�������~�߂�
            Frieze = true;

            //�s�k���[�V�����ɐ؂�ւ���
            Anim.SetBool("Deth", true);
        }
        else
        {
            Anim.SetBool("Clear",false);
            Anim.SetBool("Deth", false);
        }
    }
}
