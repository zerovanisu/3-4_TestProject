using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuraraManager : MonoBehaviour
{
    [Header("’e‘¬")]
    [SerializeField]
    float Speed;

    [Header("‹Z•â³’l")]
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

    //‰æ–ÊŠO‚ÉÁ‚¦‚½‚çíœ
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
                //Œø‰Ê‰¹
                //UŒ‚ˆ—((UŒ‚—Í(+‘•”õ) + (‹Z‚Ì•â³’l * 0.75) / 2 + 10)@-@((–hŒä—Í(+‘•”õ) * •â³’l) * 0.5))¬”“_–¢–Ø‚èã‚°
                Damage = Mathf.Ceil(P_Atack + (B_Attack * 0.75f) / 2 + 10) ;


                Destroy(this.gameObject);
                break;
        }
    }
}
