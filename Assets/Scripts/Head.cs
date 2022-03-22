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
        addScale = player.GetComponent<PlayerControll>().addScale;
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
            Emotion(coll.gameObject.tag);
            coll.gameObject.SetActive(false);
        }
        if (coll.gameObject.tag == "Bad")
        {
            Emotion(coll.gameObject.tag);
            coll.gameObject.SetActive(false);
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
