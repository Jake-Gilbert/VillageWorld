using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TournamentSelection : MonoBehaviour
{
    public List<TournamentCandidate> tournamentPool;
    public TournamentSelection(List<TournamentCandidate> tournamentPool)
    {
        this.tournamentPool = tournamentPool;
    }

    public List<AgentVillagerEvolution> GetTournamentWinners()
    {
        List<AgentVillagerEvolution> winners = new List<AgentVillagerEvolution>();
        winners.Add(ReturnFittestMember());
        winners.Add(ReturnFittestMember());
        return winners;
    }
    

    private List<TournamentCandidate> FetchRandomCandidates(float percentageOfPopulation)
    {
        List<TournamentCandidate> randomCandidates = new List<TournamentCandidate>();
        tournamentPool.OrderBy(a => Guid.NewGuid()).ToList();
        for (int i = 0; i < (int)(tournamentPool.Count * percentageOfPopulation); i++)
        {
            randomCandidates.Add(tournamentPool[i]);
        }
        return randomCandidates;
    }


    public AgentVillagerEvolution ReturnFittestMember()
    {
        List<TournamentCandidate> tournamentCandidates = FetchRandomCandidates(0.2F);
        TournamentCandidate winningCandidate = tournamentCandidates.OrderByDescending(x => x.fitness).First();
        return winningCandidate.candidate;
    }
}
