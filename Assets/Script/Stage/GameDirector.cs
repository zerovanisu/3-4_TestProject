using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance;

    [Header("�v���C���[")]
    [SerializeField]
    public GameObject Player;

    [Header("�`���[�g���A��UI")]
    [SerializeField]
    GameObject Tutorial_UI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Player.GetComponent<PlayerManager>().Frieze = true;
    }

    private void Update()
    {
        if(Tutorial_UI.activeSelf == true)
        {

        }

        if(Input.GetButtonDown("Button_A"))
        {
            Tutorial_UI.SetActive(false);

            Player.GetComponent<PlayerManager>().Frieze = false;
        }
    }

    //�_���[�W�v�Z(((�U����(+����) + (�Z�̕␳�l * 0.75) / 2 + 10)�@-�@((�h���(+����) * �␳�l) * 0.5))�����_�����؂�̂�)
    //�߂�l(�_���[�W,�U����,�U���␳�l,�h���,�h��␳�l)
    public int Damage(int DamagePoint, int Atc, float A_Skill, int Body, float B_Skill)
    {

        DamagePoint = Mathf.FloorToInt((Atc + (A_Skill * 0.75f) / 2 + 10) - ((Body * B_Skill) * 0.75f));

        Debug.Log(DamagePoint);

        return DamagePoint;
    }
}
