using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadController : MonoBehaviour
{
    public Material[] mArrayMaterial; // 임시 처리용
    public GameObject mPlayerBody;


    // Start is called before the first frame update
    void Start()
    {
        this.mPlayerBody = GameObject.Find ("PlayerBody");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        // 장애물과 충돌 시 검은색으로(임시)
        if (other.gameObject.tag == "Obstacle")
        {
            MeshRenderer[] arrayRenderer = GetComponentsInParent<MeshRenderer>();
            mArrayMaterial = new Material[2];
            for (int i = 0; i < arrayRenderer.Length; ++i)
            {
                mArrayMaterial[i] = arrayRenderer[i].material;
                mArrayMaterial[i].SetColor("_Color", Color.black);
            }
            mPlayerBody.GetComponent<PlayerController>().mIsAlive = false;
        }
    }
}
