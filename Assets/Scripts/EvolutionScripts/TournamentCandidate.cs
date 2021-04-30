using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentCandidate
{
    public AgentVillagerEvolution candidate;
    public int fitness;
    public TournamentCandidate(AgentVillagerEvolution agent, int fitness)
    {
        candidate = agent;
        this.fitness = fitness;
    }
   
}
