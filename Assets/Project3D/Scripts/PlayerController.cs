using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody mRigidbody;
    GameObject mCamera;

    public bool mIsAlive; // 생존 여부

    public float mForwardMoveForce; // 전방 힘
    public float mForwardMaxSpeed; // 전방 최대 속도


    public float mSideMoveForce; // 좌우 힘
    public float mSideMaxSpeed; // 좌우 최대 속도

    public bool mIsOnGround; // 점프 가능 여부
    public float mJumpForce; // 점프력

    public bool mIsAccelerated; // 가속 여부
    public float mAccelspan; // 지속 시간
    float mAccelTime = 0; // 가속 시간체크용 변수

    public bool mIsOnIce; // 얼음 판정 여부
    public Vector3 mMoveOnIce; // 얼음 진입 시 좌우 이동 벡터

    public bool mIsOnMud;

    public Material[] mArrayMaterial; // 색상 변화로 각종 처리 대체 (임시)

    void Start()
    {
        this.mRigidbody = GetComponent<Rigidbody>();
        this.mCamera = GameObject.Find("Main Camera");
        mIsAlive = true;
        mForwardMoveForce = 5000f;
        mForwardMaxSpeed = 15f;
        mSideMoveForce = 6000f;
        mSideMaxSpeed = 6f;
        mJumpForce = 1100f;
        mIsAccelerated = false;
        mAccelspan = 2f;
        mIsOnMud = false;
    }

    void Update()
    {
        // 가속 상태 이탈
        if(mIsAlive == true)
        {
            if (mIsAccelerated == true && mRigidbody.velocity.z < mForwardMaxSpeed * 2) // 가속 전진
            {
                mRigidbody.AddForce(transform.forward * mForwardMoveForce * Time.deltaTime * 2);
                // 가속 상태 이탈 관련
                mAccelTime += Time.deltaTime;
                Debug.Log(mAccelTime);
                if (mAccelTime > mAccelspan)
                {
                    mAccelTime = 0;
                    mIsAccelerated = false;
                }
            }
            else if (mRigidbody.velocity.z < mForwardMaxSpeed) // 일반 전진
            {
                mRigidbody.AddForce(transform.forward * mForwardMoveForce * Time.deltaTime);
            }

            // 좌우 이동
            int key = 0;

            // 키보드 조작
            if (Input.GetKey(KeyCode.A) && mRigidbody.velocity.x > mSideMaxSpeed * -1)
            {
                key = -1;
            }

            if (Input.GetKey(KeyCode.D) && mRigidbody.velocity.x < mSideMaxSpeed)
            {
                key = 1;
            }
            // 카메라 조작
            //if (mCamera.transform.eulerAngles.z > 10 && mCamera.transform.eulerAngles.z <= 180)
            //{
            //    key = -1;
            //}
            //if (mCamera.transform.eulerAngles.z >= 270 && mCamera.transform.eulerAngles.z < 350)
            //{
            //    key = 1;
            //}

            if (mIsOnIce == true)
            {
                mRigidbody.AddForce(mMoveOnIce);
            }
            else
            {
                mRigidbody.AddForce(transform.right * key * mSideMoveForce * Time.deltaTime);
            }

            // 점프
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (mIsOnGround == true && mIsOnMud != true)
                {
                    Jump();
                }
            }
        }
    }

    private void FixedUpdate()
    {

    }

    private void LateUpdate()
    {
        //Vector3 HoldCamRotation;
        //HoldCamRotation = new Vector3(30, 0, mCamera.transform.eulerAngles.z/2);
        //mCamera.transform.eulerAngles = HoldCamRotation;
    }

    private void OnCollisionEnter(Collision other)
    {

    }

    private void OnCollisionStay(Collision other)
    {
        // 발판 접촉(점프 가능) 여부
        if (other.gameObject.tag == "Floor" && other.impulse.normalized.y > 0)
        {
            this.mIsOnGround = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        // 바닥 이탈 시
        if (other.gameObject.tag == "Floor")
        {
            this.mIsOnGround = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 낙사 시 빨간색으로 (임시)
        if (other.gameObject.tag == "Bound")
        {
            MeshRenderer[] arrayRenderer = GetComponentsInChildren<MeshRenderer>();
            mArrayMaterial = new Material[2];
            for (int i = 0; i < arrayRenderer.Length; ++i)
            {
                mArrayMaterial[i] = arrayRenderer[i].material;
                mArrayMaterial[i].SetColor("_Color", Color.red);
            }
        }

        // 가속 바닥
        if (other.gameObject.tag == "Accel")
        {
            mIsAccelerated = true;
        }

        // 얼음 바닥
        if (other.gameObject.tag == "Ice")
        {
            mMoveOnIce = new Vector3(mRigidbody.velocity.x, 0, 0);
            this.mIsOnIce = true;
        }

        // 진흙 바닥
        if (other.gameObject.tag == "Mud")
        {
            mIsOnMud = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        // 얼음 바닥 이탈 시
        if (other.gameObject.tag == "Ice")
        {
            this.mIsOnIce = false;
        }

        // 진흙 바닥 이탈 시
        if (other.gameObject.tag == "Mud")
        {
            mIsOnMud = false;
        }
    }

    public void Jump()
    {
        mRigidbody.AddForce(transform.up * mJumpForce);
    }
}
