using UnityEngine;
using System.Collections;

public class OstrichMenu : MonoBehaviour
{
    private GameObject deCamera, deWorld, animHead;

    public float cameraStartY, cameraStayY, cameraMoveDownY, cameraSpeed, screenShakeAmount, screenShakeTime, animheadSpeed, timer, maxTime;

    private bool ducking = false, timerBool;
    private Animator anim;
    private Player player;

	void Start ()
	{
	    //GameObject.FindGameObjectWithTag("AnimHead");
        //GetComponentInChildren<Transform>().position = new Vector3(GetComponentInChildren<Transform>().position.x, GetComponentInChildren<Transform>().position.y, -500);

	    animHead = GameObject.FindGameObjectWithTag("AnimHead");
	    anim = GetComponent<Animator>();
	    deWorld = GetComponentInParent<Transform>().gameObject;
	    deCamera = GameObject.FindGameObjectWithTag("MainCamera");
        deCamera.transform.position = new Vector3(10, cameraStartY, -10);
        player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	void Update () 
    {

	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        anim.SetTrigger("Space");
	    }

	    if (ducking)
	    {
	        animHead.transform.Translate(new Vector3(0, animheadSpeed*Time.deltaTime, 0));
            iTween.MoveTo(deCamera, new Vector3(10, cameraMoveDownY, -10), cameraSpeed);
	    }
	    else
	    {
            iTween.MoveTo(deCamera, new Vector3(10, cameraStayY, -10), cameraSpeed);
	    }

        if (timerBool)
        {
            timer += Time.deltaTime;
            if (timer > maxTime)
            {
                GetComponent<OstrichMenu>().enabled = false;
            }
        }
    }

    public void DuckStart()
    {
        animHead.GetComponent<SpriteRenderer>().enabled = true;
        ducking = true;
    }


    public void StartGamePls()
    {
        iTween.MoveTo(animHead, player.gameObject.transform.position, 0.1f);
        iTween.CameraFadeAdd();
        //iTween.ShakePosition(deCamera, new Vector3(screenShakeAmount, screenShakeAmount), screenShakeTime);
        player.SpawnPlayer();
        timerBool = true;
    }
}
