using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager me;
    public ParticleType[] particles;

    private void Awake()
    {
        me = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayParticle(Transform pos, string p)
    {
        foreach(ParticleType t in particles)
        {
            if(t.name.ToLower() == p.ToLower())
            {
                GameObject temp = Instantiate(t.particle, transform);
                temp.transform.position = pos.position;
            }
        }
    }
    public void PlayParticle(string p)
    {
        foreach (ParticleType t in particles)
        {
            if (t.name.ToLower() == p.ToLower())
            {
                GameObject temp = Instantiate(t.particle, transform);
                temp.transform.position = Vector3.zero;
            }
        }
    }
    public void Explode(Transform t)
    {
        foreach(ParticleType a in particles)
        {
            if(a.name.ToLower() == "explode")
            {
                GameObject temp = Instantiate(a.particle, transform);
                temp.transform.position = t.position;
                temp.GetComponent<ParticleSystem>().Play();
                var main = temp.GetComponent<ParticleSystem>().main;
                main.startColor = t.GetComponent<SpriteRenderer>().color;
            }
        }
    }
}

[System.Serializable]
public struct ParticleType
{
    public string name;
    public GameObject particle;
}