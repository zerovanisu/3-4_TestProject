using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    [Header("HP�Q�[�W")]
    [SerializeField]
    Slider HPGage;

    [Header("MP�Q�[�W")]
    [SerializeField]
    Slider MPGage;

    float HP_Max, HP, MP_Max, MP;

    void Start()
    {

    }

    void Update()
    {
        //�X�e�[�^�X���擾
        HP_Max = Player.GetComponent<PlayerManager>().Player_HPMax;
        MP_Max = Player.GetComponent<PlayerManager>().Player_MPMax;
        HP = Player.GetComponent<PlayerManager>().Player_HP;
        MP = Player.GetComponent<PlayerManager>().Player_MP;

        //�\���ʂ��v�Z�����f������
        HPGage.value = HP / HP_Max;
        MPGage.value = MP / MP_Max;
    }
}
