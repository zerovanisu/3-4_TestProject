using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{
    private GameObject Player;
    private Vector3 StartPlayerOffset;
    private Vector3 StartCameraPos;

    [SerializeField]
    private float Rate;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartPlayerOffset = Player.transform.position;
        StartCameraPos = this.transform.position;
    }

    void Update()
    {
        Vector3 v = (Player.transform.position - StartPlayerOffset) * Rate;
        this.transform.position = StartCameraPos + v;
    }
}
