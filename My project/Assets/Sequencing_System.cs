using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencing_System : MonoBehaviour
{
    public Transform cube;                          //The cube aniMated
    private AudioSource audioSound;                 //The audio sound which will be played
    private Animator animator;                      //AniMator controller
    private AnimationClip[] clips;                  //The aniMation Clips in the aniMator    
    public Vector3 PointA, PointB;                  //CaMera will Move froM point A to  Point B
    public Transform Cam;                           //The CaMera
    public float start_delay;                       //A start delay before the sequencing starts 
    public bool waitForAnimation, waitForAudio,     //Optional waiting for the aniMation and audio to end
            waitForSecondAnimation;                 //Optional waiting for the second AniMation to end
    public bool enableSpheres;                      //Optional to enable a GaMeObject (Spheres)
    public bool waitForSpheresToEnable;             //Optional waiting for the spheres to be enabled 
    public The_Spheres[] spheres;                   //The objects we want to enable/disable with adjusted time
    private float firstClipDuration, secondClipDuration;    //durations of (roll) and (roll back) aniMations
    private bool movecamera;                        //starts the MovMent of the caMera
    

    // Start is called before the first frame update
    void Start()
    {   
        /*   Initializing Variables   */
        audioSound = cube.GetComponent<AudioSource>();
        animator = cube.GetComponent<Animator>();
        clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "roll": 
                    firstClipDuration = clip.length;
                    break;
                case "roll back": 
                    secondClipDuration = clip.length;
                    break;
            }
        }

        /*  Start the Sequence  */
        StartCoroutine(sequence());
    }

    // Update is called once per frame
    void Update()
    {        
        if (movecamera)
        {
            animateCamera();
        }
        Cam.LookAt(cube);
    }

    IEnumerator sequence()
    {
        yield return new WaitForSeconds(start_delay);

        animator.Play("roll");

        if (waitForAnimation)
        {               
            yield return new WaitForSeconds(firstClipDuration);
        }

        audioSound.Play();

        if (waitForAudio)
        {            
            yield return new WaitForSeconds(audioSound.clip.length);
        }

        animator.Play("roll back");

        if (waitForSecondAnimation)
        {
            yield return new WaitForSeconds(secondClipDuration);
        }

        if(enableSpheres)
        {
            foreach (The_Spheres _sphere in spheres)
            {
                _sphere.sphere.gameObject.SetActive(true);
                if (waitForSpheresToEnable)
                {
                    yield return new WaitForSeconds(_sphere.time);
                }
            }
        }

        movecamera = true;
    }


    /*  A function for Moving caMera from pointA to pointB  */
    void animateCamera()
    {                                      
        Vector3 vec = new Vector3(0, 0, 0);
        Cam.position = Vector3.SmoothDamp(Cam.position, PointB, ref vec, 0.02f);
    }
}
