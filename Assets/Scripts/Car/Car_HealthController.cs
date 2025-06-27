using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_HealthController : MonoBehaviour, IDamagable
{
    private Car_Controller carController;

    public int maxHealth;
    public int currentHealth;

    private bool carBroken;

    [Header("Explosion Info")]
    [SerializeField] private int explosionDamage = 350;
    [Space]
    [SerializeField] private float explosionRaidus = 3;
    [SerializeField] private float explosionDelay = 3;
    [SerializeField] private float explosionForce = 7;
    [SerializeField] private float explosionUpwardsModifer = 2;
    [SerializeField] private Transform explosionPoint;
    [Space]
    [SerializeField] private ParticleSystem fireFx;
    [SerializeField] private ParticleSystem explosionFx;
    private void Start()
    {
        carController = GetComponent<Car_Controller>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (fireFx.gameObject.activeSelf) 
            fireFx.transform.rotation = Quaternion.identity;
    }


    public void UpdateCarHealthUI()
    {
        UI.instance.inGameUI.UpdateCarHealthUI(currentHealth, maxHealth);
    }

    private void ReduceHealth(int damage)
    {
        if (carBroken)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
            BrakeTheCar();
    }

    private void BrakeTheCar()
    {
        carBroken = true;
        carController.BrakeTheCar();

        fireFx.gameObject.SetActive(true);
        StartCoroutine(ExplosionCo(explosionDelay));
        
    }

    public void TakeDamage(int damage)
    {
        ReduceHealth(damage);
        UpdateCarHealthUI();
    }


    private IEnumerator ExplosionCo(float delay)
    {
        yield return new WaitForSeconds(delay);

        

        explosionFx.gameObject.SetActive(true);
        carController.rb.

        AddExplosionForce(explosionForce, explosionPoint.position,
        explosionRaidus, explosionUpwardsModifer, ForceMode.Impulse);

        Explode();
    } 

    private void Explode()
    {
        HashSet<GameObject> unieqEntites = new HashSet<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(explosionPoint.position, explosionRaidus);
        
        foreach (Collider hit in colliders)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();

            if (damagable != null) 
            
            { 
                GameObject rootEntry = hit.transform.root.gameObject;

                if (unieqEntites.Add(rootEntry) == false)
                    continue;
                
                damagable.TakeDamage(explosionDamage);

                

                hit.GetComponentInChildren<Rigidbody>().
                    AddExplosionForce(explosionForce,explosionPoint.position, explosionRaidus, explosionUpwardsModifer, ForceMode.VelocityChange);
        
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(explosionPoint.position, explosionRaidus);
    }
}
