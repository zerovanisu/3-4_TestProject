using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    [Header("HPゲージ")]
    [SerializeField]
    Slider HPGage;

    [Header("MPゲージ")]
    [SerializeField]
    Slider MPGage;

    float HP_Max, HP, MP_Max, MP;

    void Start()
    {

    }

    void Update()
    {
        //ステータスを取得
        HP_Max = Player.GetComponent<PlayerManager>().Player_HPMax;
        MP_Max = Player.GetComponent<PlayerManager>().Player_MPMax;
        HP = Player.GetComponent<PlayerManager>().Player_HP;
        MP = Player.GetComponent<PlayerManager>().Player_MP;

        //表示量を計算し反映させる
        HPGage.value = HP / HP_Max;
        MPGage.value = MP / MP_Max;
    }
}
