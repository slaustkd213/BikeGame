using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("IsInputLeft", true);
        }

        else
        {
            anim.SetBool("IsInputLeft", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("IsInputRight", true);
        }

        else
        {
            anim.SetBool("IsInputRight", false);
        }
    }
}
