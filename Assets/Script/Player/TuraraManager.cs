using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuraraManager : MonoBehaviour
{
    [Header("�e��")]
    [SerializeField]
    float Speed;

    [Header("�Z�␳�l")]
    [SerializeField]
    float B_Attack;

    [System.NonSerialized]
    public float Direction, Damage;
    public int P_Atack, DamagePoint;

    Rigidbody2D Rb;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Sound_Manager.Instance.PlaySE(SE.Turara, 1f, 0f);
    }

    private void FixedUpdate()
    {
        float ShotVec = Direction * Speed;

        Rb.velocity = new Vector2(ShotVec, 0);
    }

    //��ʊO�ɏ�������폜
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Ground":
                Destroy(this.gameObject);
                break;

            case "Enemy":
                //���ʉ�

                //����̃X�e�[�^�X���擾
                int E_Body = collision.gameObject.GetComponent<EnemyManager>().Enemy_Body;

                //�Q�[���}�l�[�W���[�̎��Ń_���v���s��
                DamagePoint = GameDirector.Instance.Damage(DamagePoint, P_Atack, B_Attack, E_Body, 1);

                //�v�Z���ʂŏo���_���[�W������̗̑͂����
                collision.gameObject.GetComponent<EnemyManager>().Enemy_HP -= DamagePoint;

                Destroy(this.gameObject);
                break;
        }
    }
}
