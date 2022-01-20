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
    public float P_Atack, Direction, Damage;

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
                //�U������((�U����(+����) + (�Z�̕␳�l * 0.75) / 2 + 10)�@-�@((�h���(+����) * �␳�l) * 0.5))�����_�����؂�グ
                Damage = Mathf.Ceil(P_Atack + (B_Attack * 0.75f) / 2 + 10) ;


                Destroy(this.gameObject);
                break;
        }
    }
}
