using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private GameObject mapaPrefab;
    [SerializeField]
    private Transform mapaPosition;
    private float parallaxDistance = -50;
    public float parallaxSpeed;
    private GameObject temp;

    // Start is called before the first frame update
    void Start()
    {

        temp = Instantiate(mapaPrefab, new Vector3(4.7f, 0, 50), new Quaternion(0, 180, 0,1));

    }

    // Update is called once per frame
    void Update()
    {
        if (temp.transform.position.z < parallaxDistance)
        {
            temp.transform.position = new Vector3(4.7f, 0, mapaPrefab.transform.position.z + 50);
        }
        if (mapaPrefab.transform.position.z < parallaxDistance)
        {
            mapaPrefab.transform.position = new Vector3(4.7f, 0, temp.transform.position.z + 50);
        }

        mapaPrefab.transform.Translate(Vector3.back * parallaxSpeed * Time.deltaTime);
        temp.transform.Translate(Vector3.forward * parallaxSpeed * Time.deltaTime);

    }
}
