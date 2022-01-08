using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("����")]
    [SerializeField]
    float Speed_P;

    [Header("�W�����v��")]
    [SerializeField]
    float Jump_P;

    [SerializeField]
    float Jump_Max;

    [Header("�d��")]
    [SerializeField]
    float GravityScale;

    [Header("�d�͂̉e�����󂯂Ă��邩")]
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
        Gravity_P = GravityScale;//�d�͂��擾
    }

    void Update()
    {
        //���͎�t
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
        //�W�����v�͂�ێ�
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
            //���݂̍�������d�͉����x���������������̍��W�ɒu��������
            float Gravity_total = transform.position.y - Gravity_P;

            transform.position = new Vector2(transform.position.x, Gravity_total);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y);
        }
    }
}
