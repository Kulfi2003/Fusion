using UnityEngine;

public class ApplyForceToParticles : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;
    private Vector3[] originalVelocities;
    private bool velocityAdded = false;
    private Vector3 addedVelocity;

    public float forceAmount = 50;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        originalVelocities = new Vector3[particles.Length];
    }

    // Add velocity to all existing particles
    public void ApplyForce(Vector3 velocity)
    {
        velocity *= forceAmount;

        int numParticles = particleSystem.GetParticles(particles);

        if (!velocityAdded)
        {
            for (int i = 0; i < numParticles; i++)
            {
                originalVelocities[i] = particles[i].velocity;
            }
            velocityAdded = true;
        }

        addedVelocity += velocity;

        for (int i = 0; i < numParticles; i++)
        {
            particles[i].velocity += velocity;
        }

        particleSystem.SetParticles(particles, numParticles);
    }

    // Remove the added velocity from all particles
    public void StopForce()
    {
        if (!velocityAdded) return;

        int numParticles = particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticles; i++)
        {
            particles[i].velocity = originalVelocities[i];
        }

        velocityAdded = false;
        addedVelocity = Vector3.zero;
        particleSystem.SetParticles(particles, numParticles);
    }
}
