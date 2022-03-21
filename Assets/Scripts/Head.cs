using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] Animator head;
    
    float addAngle;
    float changeAngle;

    [SerializeField] float nideScale, addScale, maxScale;

    void Start()
    {
        nideScale = transform.localScale.x;
        addAngle = Player.Instance.addAngle;
        changeAngle = Player.Instance.changeAngle;
    }
    void Update()
    {
        if(transform.localScale.x > nideScale)
        {
            transform.localScale -= new Vector3(addScale, 0, addScale); 
            float animSpeed = (((maxScale - transform.localScale.x)/2 ));
            if (transform.localScale.x < 2.3f)
                head.speed = animSpeed;
        }
        if (transform.localScale.x < nideScale)
        {
            transform.localScale += new Vector3(addScale, 0, addScale);
            float animSpeed = (((maxScale - transform.localScale.x) / 2));
            if (transform.localScale.x < 2.3f)
                head.speed = animSpeed;
        }
    }
    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Boost")
        {
            Player.Instance.Boost();
            coll.gameObject.SetActive(false);
        }
        if (coll.gameObject.tag == "Good")
        {
            if (transform.localScale.x < maxScale)
                nideScale += 0.2f;
            coll.gameObject.SetActive(false);
        }
        if (coll.gameObject.tag == "Bad")
        {
            if (transform.localScale.x > 1)
                nideScale -= 0.2f;
            coll.gameObject.SetActive(false);
        }
    }   
}
