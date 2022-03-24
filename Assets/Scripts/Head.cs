using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] PlayerControll player;
    [SerializeField] float emotionTime;
    [SerializeField] float addScaleSpeed, addShapeSpeed;
    [SerializeField] SkinnedMeshRenderer head;
    [SerializeField] float curFat;

    void Start()
    {
        curFat = 0;
        emotionTime = player.GetComponent<PlayerControll>().emotionTime;
        addScaleSpeed = player.GetComponent<PlayerControll>().addScaleSpeed;
        addShapeSpeed = player.GetComponent<PlayerControll>().addShapeSpeed;
        MeshChange();
    }
    void Update()
    {
        if(curFat > 0)
        {
            if (head.GetBlendShapeWeight(3) > 0)
            {
                transform.localPosition = new Vector3(0, 0.9f - (0.003f * (float)head.GetBlendShapeWeight(3)), 0);
                head.SetBlendShapeWeight(3, head.GetBlendShapeWeight(3) - addScaleSpeed);
                MeshChange();
            }
            else
            {
                if (head.GetBlendShapeWeight(0) < curFat)
                {
                    transform.localPosition = new Vector3(0, 0.9f, 0);
                    head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) + addScaleSpeed);
                    MeshChange();
                }
                else if (head.GetBlendShapeWeight(0) > curFat)
                {
                    transform.localPosition = new Vector3(0, 0.9f, 0);
                    head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) - addScaleSpeed);
                    MeshChange();
                }
            }               
        }
        else
        {
            if (head.GetBlendShapeWeight(0) > 0)
            {
                transform.localPosition = new Vector3(0, 0.9f, 0);
                head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) - addScaleSpeed);
                MeshChange();
            }
            else
            {
                if (head.GetBlendShapeWeight(3) < (curFat * -1))
                {
                    transform.localPosition = new Vector3(0, 0.9f - (0.003f * (float)head.GetBlendShapeWeight(3)), 0);
                    head.SetBlendShapeWeight(3, head.GetBlendShapeWeight(3) + addScaleSpeed);
                    MeshChange();
                }
                else if (head.GetBlendShapeWeight(3) > (curFat * -1))
                {
                    transform.localPosition = new Vector3(0, 0.9f - (0.003f * (float)head.GetBlendShapeWeight(3)), 0);
                    head.SetBlendShapeWeight(3, head.GetBlendShapeWeight(3) - addScaleSpeed);
                    MeshChange();
                }
            }            
        }        
    }
    void MeshChange()
    {
        Mesh bakeMesh = new Mesh();
        head.BakeMesh(bakeMesh);
        var collider = GetComponent<MeshCollider>();
        collider.sharedMesh = bakeMesh;
    }
    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Boost")
        {
            player.GetComponent<PlayerControll>().Boost();
            coll.gameObject.SetActive(false);
        }
        if (coll.gameObject.tag == "Good")
        {
            curFat += coll.gameObject.GetComponent<AddScale>().addScale;
            if (curFat > 100)
                curFat = 100;

            Emotion(coll.gameObject.tag);
            coll.gameObject.SetActive(false);
        }
        if (coll.gameObject.tag == "Bad")
        {
            curFat += coll.gameObject.GetComponent<AddScale>().addScale;
            if (curFat < -100)
                curFat = -100;

            Emotion(coll.gameObject.tag);
            coll.gameObject.SetActive(false);
        }
        if (coll.gameObject.tag == "Finish")
        {
            player.Win();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            player.Lose();
        }
    }

    void Emotion(string name)
    {
        StopAllCoroutines();
        head.SetBlendShapeWeight(2, 0);
        switch (name)
        {
            case ("Good"):
                StartCoroutine(GoodEmotion());
                break;
            case ("Bad"):
                StartCoroutine(BadEmotion());
                break;
        }
    }
    IEnumerator GoodEmotion()
    {
        //head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) + addScale);
        //if (head.GetBlendShapeWeight(0) < 0)
        //    head.SetBlendShapeWeight(0, 0);
        //GetComponent<CapsuleCollider>().radius = 1f - (0.4f / head.GetBlendShapeWeight(0));

        head.SetBlendShapeWeight(2, 100);
        yield return new WaitForSeconds(emotionTime);
        head.SetBlendShapeWeight(2, 0);
    }
    IEnumerator BadEmotion()
    {
        //head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) + addScale);      
        //if (head.GetBlendShapeWeight(0) > 100)
        //    head.SetBlendShapeWeight(0, 100);
        //GetComponent<CapsuleCollider>().radius = 1f - (0.4f / head.GetBlendShapeWeight(0));
        head.SetBlendShapeWeight(1, 100);
        yield return new WaitForSeconds(emotionTime);
        head.SetBlendShapeWeight(1, 0);

    }    
}
