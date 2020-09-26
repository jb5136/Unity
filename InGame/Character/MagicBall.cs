using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal = Vector3.zero; //Used to rotate impactparticle.
    public GameObject m_magician;
    private float m_str;
    public GameObject target;
    private bool isCheck = false;
    public float GetStr { set { m_str = value; } }
    void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            float dis = Vector3.Distance(transform.position, target.transform.position);

            if (dis <= 1.3 && !isCheck)
            {
                //transform.DetachChildren();
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
                //Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);

                //if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
                //{
                //    Destroy(hit.gameObject);
                //}


                //yield WaitForSeconds (0.05);
                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail, 3f);
                }
                Destroy(projectileParticle, 3f);
                Destroy(impactParticle, 3f);
                Destroy(gameObject);
                //projectileParticle.Stop();

                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {
                    ParticleSystem trail = trails[i];
                    if (!trail.gameObject.name.Contains("Trail"))
                        continue;

                    trail.transform.SetParent(null);
                    Destroy(trail.gameObject, 2);
                }
                target.GetComponent<MonsterHealth>().DecreaseHp(m_str, false, m_magician);
                if(target.tag == "Monster")
                    target.GetComponent<MonsterController>().PlayerAttackCheck(m_magician);
            }
        }
    }
}
