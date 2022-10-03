using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingManager : MonoBehaviour
{
    public GameObject instructions;
    // Start is called before the first frame update
    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    public void ShowInstructions() { 
        instructions.SetActive(true);
    }

    public void HideInstructions() { 
        instructions.SetActive(false);
    }
}
