using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class PlayerControll : MonoBehaviour
{
    [Header("--------Options--------")]
    [SerializeField] float moveSpeed;
    public float boostSpeed, slowSpeed, boostTime, speedRight, emotionTime, addScale;
    public float headForwardRoatete, headRightRotate;
    [Header("--------Game--------")]
    [SerializeField] PathFollower path;
    [SerializeField] float speed; 
    [SerializeField] GameObject head;

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    void Start()
    {
        path.speed = moveSpeed;
        //speed = moveSpeed;
    }
    void FixedUpdate()
    {
        if (Controll.Instance._state == "Game")
        {
            transform.GetChild(0).Rotate(Vector3.right * headRightRotate);

            if (Input.GetMouseButtonDown(0))
            {
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            if (Input.GetMouseButton(0))
            {
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                currentSwipe.Normalize();

                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) // swip left
                {
                    head.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    transform.GetChild(0).transform.Translate(-Vector3.right * speedRight * Time.deltaTime);
                    head.transform.Rotate(Vector3.up * headRightRotate);
                }
                else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) // swip right
                {
                    head.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    transform.GetChild(0).transform.Translate(Vector3.right * speedRight * Time.deltaTime);
                    head.transform.Rotate(Vector3.up * headRightRotate);
                }                    
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }            
        }
    }
    //public void OnTriggerEnter(Collider coll)
    //{
    //    if (coll.gameObject.tag == "Boost")
    //    {
    //        Boost();
    //        coll.gameObject.SetActive(false);
    //    }
    //    if (coll.gameObject.tag == "Good")
    //    {
    //        Emotion(0);            
    //        coll.gameObject.SetActive(false);
    //    }
    //    if (coll.gameObject.tag == "Bad")
    //    {
    //        Emotion(1);
    //        coll.gameObject.SetActive(false);
    //    }
    //}

    public void Boost()
    {
        path.speed = boostSpeed;
        //speed = 
        StartCoroutine(SlowSpeed());
    }
   

    IEnumerator SlowSpeed()
    {
        yield return new WaitForSeconds(boostTime);
        while (path.speed > moveSpeed)
        {
            path.speed -= slowSpeed;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
}
