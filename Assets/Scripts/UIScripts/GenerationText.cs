using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerationText : MonoBehaviour
{
    public GenerationBehaviours genBehaviours;
    //GenerationBehaviours generationBehaviours;   
    private void Update()
    {
        gameObject.GetComponent<TMP_Text>().text = "Generation: " + genBehaviours.GetCurrentGeneration().ToString();
    }
}
