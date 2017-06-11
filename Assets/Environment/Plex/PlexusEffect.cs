using System.Collections.Generic;
using UnityEngine;

namespace Local
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PlexusEffect : MonoBehaviour
    {

        public float maxDistance = 1.0f;

        public int maxConnections = 5;
        public int maxLineRenderers = 100;

       public new ParticleSystem particleSystem;
       public ParticleSystem.Particle[] particles;
       
       public ParticleSystem.MainModule particleSystemMainModule;

        public LineRenderer lineRendererTemplate;
       public List<LineRenderer> lineRenderers = new List<LineRenderer>();

        Transform _transform;

        void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
            particleSystemMainModule = particleSystem.main;
        }


        void LateUpdate()
        {
            int maxParticles = particleSystemMainModule.maxParticles;

            if (particles == null || particles.Length < maxParticles)
            {
                particles = new ParticleSystem.Particle[maxParticles];
            }

            int lrIndex = 0;
            int lineRendererCount = lineRenderers.Count;

            if (lineRendererCount > maxLineRenderers && maxLineRenderers >= 0)
            {
                for (int i = maxLineRenderers; i < lineRendererCount; i++)
                {
                    Destroy(lineRenderers[i].gameObject);
                }

                int removedCount = lineRendererCount - maxLineRenderers;
                lineRenderers.RemoveRange(maxLineRenderers, removedCount);

                lineRendererCount -= removedCount;
            }

            if (maxConnections > 0 && maxLineRenderers > 0)
            {
                particleSystem.GetParticles(particles);
                int particleCount = particleSystem.particleCount;

                float maxDistanceSqr = maxDistance * maxDistance;

                ParticleSystemSimulationSpace simulationSpace = particleSystemMainModule.simulationSpace;

                switch (simulationSpace)
                {
                    case ParticleSystemSimulationSpace.Local:
                        _transform = transform;
                        break;
                    case ParticleSystemSimulationSpace.World:
                        _transform = transform;
                        break;
                    case ParticleSystemSimulationSpace.Custom:
                        _transform = particleSystemMainModule.customSimulationSpace;
                        break;
                    default:
                        throw new System.NotSupportedException(string.Format("Unsupported simulation space {0}", particleSystemMainModule.simulationSpace));
                        break;

                }

                for (int i = 0; i < particleCount; i++)
                {
                    if (lrIndex == maxLineRenderers)
                    {
                        break;
                    }
                    Vector3 p1_position = particles[i].position;
                    //p1_position.z = 0;

                    int connections = 0;

                    for (int j = i + 1; j < particleCount; j++)
                    {
                        Vector3 p2_position = particles[j].position;
                        //p2_position.z = 0;
                        float distanceSqr = Vector3.SqrMagnitude(p1_position - p2_position);

                        if (distanceSqr <= maxDistanceSqr)
                        {
                            LineRenderer lr;

                            if (lrIndex == lineRendererCount)
                            {
                                lr = Instantiate(lineRendererTemplate, _transform, true);
                                //lr = Instantiate(lineRendererTemplate, _transform.position, _transform.rotation);
                                lr.transform.SetParent(_transform);
                                lr.transform.localRotation = Quaternion.identity;
                                lr.transform.localPosition = Vector3.zero;

                                lineRenderers.Add(lr);

                                lineRendererCount++;
                            }

                            lr = lineRenderers[lrIndex];

                            lr.enabled = true;
                            lr.useWorldSpace = simulationSpace == ParticleSystemSimulationSpace.World ? true : false;

                            lr.SetPosition(0, p1_position);
                            lr.SetPosition(1, p2_position);

                            lrIndex++;
                            connections++;
                            if (connections == maxConnections || lrIndex == maxLineRenderers)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            for (int i = lrIndex; i < lineRenderers.Count; i++)
            {
                lineRenderers[i].enabled = false;
            }
        }
    }
}