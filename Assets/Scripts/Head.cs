using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] PlayerControll player;
    [SerializeField] float emotionTime;
    [SerializeField] float addScaleSpeed, addShapeSpeed, glassDestroy;
    [SerializeField] SkinnedMeshRenderer head; 
    [SerializeField] float curFat, force;
    [SerializeField] Material[] materials;

    [SerializeField] Color32 _color;
    void Start()
    {
        materials = GetComponent<SkinnedMeshRenderer>().materials;
        _color = new Color32(255, 255, 255, 255);

        curFat = 0;
        emotionTime = player.GetComponent<PlayerControll>().emotionTime;
        addScaleSpeed = player.GetComponent<PlayerControll>().addScaleSpeed;
        addShapeSpeed = player.GetComponent<PlayerControll>().addShapeSpeed;
        glassDestroy = player.GetComponent<PlayerControll>().glassDestroy;
        MeshChange();
    }
    void Update()
    {
        if (curFat > 0 && Controll.Instance._state == "Game")
        {
            if (head.GetBlendShapeWeight(3) > 0)
            {
                transform.localPosition = new Vector3(0, 0.9f - (0.003f * (float)head.GetBlendShapeWeight(3)), 0);
                head.SetBlendShapeWeight(3, head.GetBlendShapeWeight(3) - addShapeSpeed);
                MeshChange();
            }
            else
            {
                if (head.GetBlendShapeWeight(0) < curFat)
                {                    
                    head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) + addShapeSpeed);
                    MeshChange();
                }
                else if (head.GetBlendShapeWeight(0) > curFat)
                {
                    head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) - addShapeSpeed);
                    MeshChange();
                }
                transform.localPosition = new Vector3(0, 0.9f + (0.007f * (float)head.GetBlendShapeWeight(0)), 0);               
            }               
        }
        else
        {
            if (head.GetBlendShapeWeight(0) > 0)
            {
                transform.localPosition = new Vector3(0, 0.9f + (0.007f * (float)head.GetBlendShapeWeight(0)), 0);
                head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) - addShapeSpeed);
                MeshChange();
            }
            else
            {
                if (head.GetBlendShapeWeight(3) < -curFat)
                {                  
                    head.SetBlendShapeWeight(3, head.GetBlendShapeWeight(3) + addShapeSpeed);
                    MeshChange();
                }
                else if (head.GetBlendShapeWeight(3) > -curFat)
                {
                    head.SetBlendShapeWeight(3, head.GetBlendShapeWeight(3) - addShapeSpeed);
                    MeshChange();
                }                
                transform.localPosition = new Vector3(0, 0.9f - (0.003f * (float)head.GetBlendShapeWeight(3)), 0);
                             
            }            
        }
        _color = new Color32(((byte)(2.5f * (float)head.GetBlendShapeWeight(0))), ((byte)(2.5f * (float)head.GetBlendShapeWeight(3))), ((byte)(2.5f * (float)head.GetBlendShapeWeight(3))), 255);
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
        if (Controll.Instance._state == "Game")
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

                //Emotion(coll.gameObject.tag);
                coll.gameObject.SetActive(false);
            }
            if (coll.gameObject.tag == "Bad")
            {
                curFat += coll.gameObject.GetComponent<AddScale>().addScale;
                if (curFat < -100)
                    curFat = -100;

                //Emotion(coll.gameObject.tag);
                coll.gameObject.SetActive(false);
            }
            if (coll.gameObject.tag == "Finish")
            {
                player.Win();
            }
            if (coll.gameObject.tag == "WallExit")
            {
                coll.gameObject.GetComponent<Wall>().DropWall();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(Controll.Instance._state == "Game")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Lose(collision.gameObject);
            }
            if (collision.gameObject.tag == "Glass")
            {
                if(curFat >= glassDestroy)
                    collision.gameObject.GetComponent<Glass>().EffectOn();
                else
                    Lose(collision.gameObject);
            }
        }       
    }
    void Lose(GameObject obj)
    {
        player.Lose();
        Vector3 vect = transform.position - obj.transform.position;
        gameObject.layer = 0;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(new Vector3(vect.x, 1, vect.z) * force, ForceMode.Impulse);
        GetComponent<Head>().enabled = false;
    }

    //void Emotion(string name)
    //{
    //    StopAllCoroutines();
    //    head.SetBlendShapeWeight(2, 0);
    //    switch (name)
    //    {
    //        case ("Good"):
    //            StartCoroutine(GoodEmotion());
    //            break;
    //        case ("Bad"):
    //            StartCoroutine(BadEmotion());
    //            break;
    //    }
    //}
    //IEnumerator GoodEmotion()
    //{
    //    //head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) + addScale);
    //    //if (head.GetBlendShapeWeight(0) < 0)
    //    //    head.SetBlendShapeWeight(0, 0);
    //    //GetComponent<CapsuleCollider>().radius = 1f - (0.4f / head.GetBlendShapeWeight(0));

    //    head.SetBlendShapeWeight(2, 100);
    //    yield return new WaitForSeconds(emotionTime);
    //    head.SetBlendShapeWeight(2, 0);
    //}
    //IEnumerator BadEmotion()
    //{
    //    //head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) + addScale);      
    //    //if (head.GetBlendShapeWeight(0) > 100)
    //    //    head.SetBlendShapeWeight(0, 100);
    //    //GetComponent<CapsuleCollider>().radius = 1f - (0.4f / head.GetBlendShapeWeight(0));
    //    head.SetBlendShapeWeight(1, 100);
    //    yield return new WaitForSeconds(emotionTime);
    //    head.SetBlendShapeWeight(1, 0);

    //}    
}
