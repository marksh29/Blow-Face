using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [Header("--------Options--------")]
    [SerializeField] float moveSpeed;
    [SerializeField] float boostSpeed, slowSpeed, boostTime, speedRight, emotionTime, addScale;
    [Header("--------Game--------")]
    [SerializeField] float speed; 
    [SerializeField] float limitXX;
    [SerializeField] SkinnedMeshRenderer head;   

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    void Start()
    {
        speed = moveSpeed;
    }
    void FixedUpdate()
    {
        if (Controll.Instance._state == "Game")
        {
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
            GetComponent<Rigidbody>().AddForce(Vector3.forward * speed, ForceMode.Acceleration);

            if (Input.GetMouseButtonDown(0))
            {
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            if (Input.GetMouseButton(0))
            {
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                currentSwipe.Normalize();

                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && head.transform.position.x > -limitXX) // swip left
                {
                    GetComponent<Rigidbody>().AddForce(-Vector3.right * speedRight, ForceMode.Acceleration);
                    //transform.Translate(-Vector3.right * speedRight * Time.deltaTime);
                    //head.Rotate(-Vector3.forward * rotSpeed * Time.deltaTime);
                }
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && head.transform.position.x < limitXX) // swip right
                {
                    GetComponent<Rigidbody>().AddForce(Vector3.right * speedRight, ForceMode.Acceleration);
                    //transform.Translate(Vector3.right * speedRight * Time.deltaTime);
                    //head.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
                }
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }
    }
    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Boost")
        {
            Boost();
            coll.gameObject.SetActive(false);
        }
        if (coll.gameObject.tag == "Good")
        {
            Emotion(0);            
            coll.gameObject.SetActive(false);
        }
        if (coll.gameObject.tag == "Bad")
        {
            Emotion(1);
            coll.gameObject.SetActive(false);
        }
    }

    public void Boost()
    {
        speed = boostSpeed;
        StartCoroutine(SlowSpeed());
    }
    void Emotion(int id)
    {
        StopAllCoroutines();
        head.SetBlendShapeWeight(1, 0);
        head.SetBlendShapeWeight(2, 0);        
        switch (id)
        {
            case (0):
                StartCoroutine(GoodEmotion());
                break;
            case (1):
                StartCoroutine(BadEmotion());
                break;
        }
    }

    IEnumerator SlowSpeed()
    {
        yield return new WaitForSeconds(boostTime);
        while (speed > moveSpeed)
        {
            speed -= slowSpeed;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator BadEmotion()
    {
        head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) - addScale);
        head.SetBlendShapeWeight(1, 100);
        yield return new WaitForSeconds(emotionTime);
        head.SetBlendShapeWeight(1, 0);

    }
    IEnumerator GoodEmotion()
    {
        head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) + addScale);
        head.SetBlendShapeWeight(2, 100);
        yield return new WaitForSeconds(emotionTime);
        head.SetBlendShapeWeight(2, 0);
    }
}
