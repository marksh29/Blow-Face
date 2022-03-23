using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] PlayerControll player;
    [SerializeField] float emotionTime, addScale;
    [SerializeField] SkinnedMeshRenderer head;

    void Start()
    {
        emotionTime = player.GetComponent<PlayerControll>().emotionTime;
        //addScale = player.GetComponent<PlayerControll>().addScale;
    }
    void Update()
    {
       
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
            addScale = coll.gameObject.GetComponent<AddScale>().addScale;
            Emotion(coll.gameObject.tag);
            coll.gameObject.SetActive(false);
        }
        if (coll.gameObject.tag == "Bad")
        {
            addScale = coll.gameObject.GetComponent<AddScale>().addScale;
            Emotion(coll.gameObject.tag);
            coll.gameObject.SetActive(false);
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
        head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) + addScale);
        if (head.GetBlendShapeWeight(0) < 0)
            head.SetBlendShapeWeight(0, 0);
        GetComponent<CapsuleCollider>().radius = 1f - (0.4f / head.GetBlendShapeWeight(0));
        head.SetBlendShapeWeight(2, 100);
        yield return new WaitForSeconds(emotionTime);
        head.SetBlendShapeWeight(2, 0);
    }
    IEnumerator BadEmotion()
    {
        head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) + addScale);
        if (head.GetBlendShapeWeight(0) > 100)
            head.SetBlendShapeWeight(0, 100);
        GetComponent<CapsuleCollider>().radius = 1f - (0.4f / head.GetBlendShapeWeight(0));
        head.SetBlendShapeWeight(1, 100);
        yield return new WaitForSeconds(emotionTime);
        head.SetBlendShapeWeight(1, 0);

    }    
}
