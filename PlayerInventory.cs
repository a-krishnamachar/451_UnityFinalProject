using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasKey = false;     // Whether or not the player has the key.
    public bool hasGun = false;
    public int ammo = 0;
    public int money = 0;

    static int ammoPrevLevel = 0;
    static int moneyPrevLevel = 0;
    public Transform povOrigin;
    SimpleShoot gunScript = null;
    playerMovements m_Character; // A reference to the ThirdPersonCharacter on the object

    public GameObject handgun;
    public Text textAmmo;
    public Text textMoney;

    public AudioClip getAmmoSound;
    public AudioClip noAmmoSound;
    public AudioClip getKeySound;
    public AudioClip getGunSound;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;
    public AudioClip casingDrop;
    public List<AudioClip> hitWallSound;
    AudioSource audioSource = null;


    //public static PlayerInventory instance; Zihao: I just made ammo and health static, would not make sense to make the entire inventory static

    void Start()
    {
        money = moneyPrevLevel;
        ammo = ammoPrevLevel;
        m_Character = GetComponent<playerMovements>();
        audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        //instance = this;
    }


    // Update is called once per frame
    void Update()
    {   UpdateDisplay();
        //If you want a different input, change it here
        if (hasGun){
            if (!handgun.activeSelf){
                handgun.SetActive(true);
                gunScript = GameObject.FindGameObjectWithTag(Tags.guns).GetComponent<SimpleShoot>();
                m_Character.UpdateHasGun(hasGun);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if(ammo > 0){
                    gunScript.shootOne();
                    ammo-=1;
                    m_Character.ShootOne();
                    PrimaryAttack();
                    audioSource.PlayOneShot(casingDrop,1);
                }else{
                    //play audio with no ammo
                    audioSource.PlayOneShot(noAmmoSound,1);
                }

            }
        }
        if(Input.GetKeyDown(KeyCode.F)){
            RaycastHit hit;
            if(Physics.Raycast(povOrigin.position,povOrigin.forward,out hit, 1.5f)){
                print("interacted with"+hit.transform.name+" from "+hit.distance+"m.");
                Door targetDoor = hit.transform.GetComponent<Door>();
                if(targetDoor!=null){
                    targetDoor.Interact();
                    if(targetDoor.isopen){
                        AudioSource.PlayClipAtPoint(doorOpenSound,hit.transform.position);
                    }else{
                        AudioSource.PlayClipAtPoint(doorCloseSound,hit.transform.position);
                    }
                }

                doorBlocker targetStretcher = hit.transform.GetComponent<doorBlocker>();
                if(targetStretcher!=null){
                    targetStretcher.Interact();
                    AudioSource.PlayClipAtPoint(doorCloseSound,hit.transform.position);
                }

                AmmoItem targetAmmo = hit.transform.GetComponent<AmmoItem>();
                if(targetAmmo!=null){
                    ammo+=targetAmmo.count;
                    Destroy(hit.transform.gameObject);
                    audioSource.PlayOneShot(getAmmoSound,1);
                }

                if(hit.transform.gameObject.tag=="key"){
                    hasKey=true;
                    audioSource.PlayOneShot(getKeySound,1);
                    Destroy(hit.transform.gameObject);
                }
                if(hit.transform.gameObject.tag=="GunOnGound"){
                    hasGun=true;
                    audioSource.PlayOneShot(getGunSound,1);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
    public void nextLevel(){
        moneyPrevLevel = money;
        ammoPrevLevel = ammo;
    }

	void UpdateDisplay(){
	     textAmmo.text = "Ammo: " + ammo.ToString();
	     textMoney.text = "Money: " + money.ToString();
	}

    public void AddAmmo()
    {	    int cost = 50;
	    if(money>=cost){
	    money-=cost;
            ammo += 7;
	}
    }

    void PrimaryAttack(){
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(povOrigin.position,povOrigin.forward,out hit, gunScript.attackRange);
        if(hitSomething){
            Rigidbody targetRigidbody = hit.transform.gameObject.GetComponent<Rigidbody>();
            print(hit.collider.gameObject.tag);
            if(targetRigidbody&&hit.transform.gameObject.tag==Tags.enemy){
                targetRigidbody.AddForce(povOrigin.forward*gunScript.ammoForce, ForceMode.Impulse);
                hit.transform.gameObject.GetComponent<EnemyAnimation>().gotHit(hit.collider.gameObject.tag);
            }else{
                AudioSource.PlayClipAtPoint(hitWallSound[Random.Range(0, hitWallSound.Count)],hit.transform.position);
            }
        }
    }
}
