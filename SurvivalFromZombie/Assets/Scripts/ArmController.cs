using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmController : MonoBehaviour
{
    [SerializeField] GameObject barrelPrefab;
    [SerializeField] GameObject barrelPreview;
    [SerializeField] GameObject barrelOnHand;
    
    [SerializeField] float setDelay;
    [SerializeField] float setDistance;

    [SerializeField] Text textNumOfBarrel;

    int numOfBarrel = 1;

    Vector3 previewOriginpos;
    bool isBuildable;
    bool isReady = true;

    Material previewMat;
    Transform mainCamera;
    Animator anim;



    RaycastHit hit;

    private void Start()
    {
        previewOriginpos = barrelPreview.transform.localPosition;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
        previewMat = barrelPreview.GetComponent<MeshRenderer>().material;
        anim = GetComponent<Animator>();

        textNumOfBarrel.text = numOfBarrel.ToString();
    }

    private void Update()
    {
        if (WeaponManager.isArm)
        {
            if (isReady && numOfBarrel > 0) 
            {
                RayCast();
            }

            if (Input.GetButtonDown("Fire1") && isReady && isBuildable && numOfBarrel > 0)
            {
                isReady = false;
                isBuildable = false;
                StartCoroutine(SetReady());

                Instantiate(barrelPrefab, barrelPreview.transform.position, barrelPreview.transform.rotation);
                barrelPreview.transform.localPosition = previewOriginpos;

                anim.SetTrigger("Reload");
                
                numOfBarrel--;
                textNumOfBarrel.text = numOfBarrel.ToString();

                if (numOfBarrel == 0) DoNotRenderBarrelOnHand();
            }
        }
    }

    void RayCast()
    {
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, setDistance))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                barrelPreview.transform.position = hit.point;

                if (hit.transform.gameObject.tag == "Ground")
                {
                    previewMat.color = Color.green;
                    isBuildable = true;
                }
                else
                {
                    previewMat.color = Color.red;
                    isBuildable = false;
                }
            }
        }
        else
        {
            previewMat.color = Color.red;
            isBuildable = false;
        }
    }

    IEnumerator SetReady()
    {
        yield return new WaitForSeconds(setDelay);

        isReady = true;
    }

    public bool CanChange()
    {
        if (!isReady) return false;

        return true;
    }

    void DoNotRenderBarrelOnHand()
    {
        barrelOnHand.SetActive(false);
    }
}
