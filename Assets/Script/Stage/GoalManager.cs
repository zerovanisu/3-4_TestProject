using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    [Header("�J�ڂ̑��x")]
    [SerializeField]
    float Change_TimeMax;

    string Scene_Name;

    bool Scene_Change, Changeing;

    float Change_Time;

    void Start()
    {
        Changeing = false;
        Change_Time = Change_TimeMax;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //�v���C���[�ɃN���A��`����
            collision.gameObject.GetComponent<PlayerManager>().Player_Clear = true;

            Scene_Change = true;
        }

    }
}
