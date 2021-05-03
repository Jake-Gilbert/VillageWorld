using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentCandidate
{
    public AgentVillagerEvolution candidate;
    public float fitness;
    public TournamentCandidate(AgentVillagerEvolution agent, float fitness)
    {
        candidate = agent;
        this.fitness = fitness;
    }
   
}
