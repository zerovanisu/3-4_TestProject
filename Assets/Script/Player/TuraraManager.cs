using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuraraManager : MonoBehaviour
{
    [Header("弾速")]
    [SerializeField]
    float Speed;

    [Header("技補正値")]
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

    //画面外に消えたら削除
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
                //効果音

                //相手のステータスを取得
                int E_Body = collision.gameObject.GetComponent<EnemyManager>().Enemy_Body;

                //ゲームマネージャーの式でダメ計を行う
                DamagePoint = GameDirector.Instance.Damage(DamagePoint, P_Atack, B_Attack, E_Body, 1);

                //計算結果で出たダメージ分相手の体力を削る
                collision.gameObject.GetComponent<EnemyManager>().Enemy_HP -= DamagePoint;

                Destroy(this.gameObject);
                break;
        }
    }
}
