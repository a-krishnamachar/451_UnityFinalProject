using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{


    public GameObject s1;
    public GameObject s2;
    public GameObject s3;
    public GameObject s4;
    
    public Scene scene;
    

    // Start is called before the first frame update
    void Start()
    {
        s1.SetActive(true);
	s2.SetActive(true);
	//s3.SetActive(false);
	s4.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadS1(){

        var parameters = new LoadSceneParameters(LoadSceneMode.Single);

        scene = SceneManager.LoadScene("S1", parameters);
	
    }


    public void loadS2(){
	var parameters = new LoadSceneParameters(LoadSceneMode.Single);
      
	scene = SceneManager.LoadScene("S2", parameters);
	
    }

    public void loadS3(){
	var parameters = new LoadSceneParameters(LoadSceneMode.Single);
      
	scene = SceneManager.LoadScene("S3", parameters);
	
    }

    public void loadS4(){
	var parameters = new LoadSceneParameters(LoadSceneMode.Single);
     
	scene = SceneManager.LoadScene("S4", parameters);
	
    }
}
