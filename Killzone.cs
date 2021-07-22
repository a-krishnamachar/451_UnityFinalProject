using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{
    public string nextScene;
    // Start is called before the first frame update
    void OnTriggerEnter (Collider other){
        if (other.gameObject == GameObject.FindGameObjectWithTag(Tags.player))
        {
            other.GetComponent<PlayerInventory>().nextLevel();
            SceneManager.LoadScene(nextScene);
        }

    }
}
