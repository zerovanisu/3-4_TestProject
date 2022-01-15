using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField]
    PlayerManager PM;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag =="Ground")
        {
            PM.IsGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            PM.IsGround = false;
        }
    }
}
