using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float m_MoveSpeed = 1f;
    private PlayerBehaviour player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.transform.position,
            m_MoveSpeed * Time.deltaTime
        );
    }
}
