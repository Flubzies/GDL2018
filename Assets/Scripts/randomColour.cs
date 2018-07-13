using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomColour : MonoBehaviour
{
    [SerializeField] Color minColour;
    [SerializeField] Color maxColour;
    [SerializeField] bool skinnedMesh;
    [SerializeField] bool applyToAllMaterials;

    //[SerializeField] protected Color[] colourBank;
    //protected int num;

    void Start()
    {
        //num = Random.Range(0, colourBank.Length);
        //MeshRenderer m = GetComponent<MeshRenderer>();
        //m.material.color = colourBank[num];

        Vector3 minCol;
        Vector3 maxCol;

        Color.RGBToHSV(minColour, out minCol.x, out minCol.y, out minCol.z);
        Color.RGBToHSV(maxColour, out maxCol.x, out maxCol.y, out maxCol.z);
        Color c = Random.ColorHSV(minCol.x, maxCol.x, minCol.y, maxCol.y, minCol.z, maxCol.z, minColour.a, maxColour.a);

        if (skinnedMesh)
        {
            if (applyToAllMaterials)
                foreach (Material item in GetComponent<SkinnedMeshRenderer>().materials) item.color = c;
            else GetComponent<SkinnedMeshRenderer>().material.color = c;
        }
        else
        {
            if (applyToAllMaterials)
                foreach (Material item in GetComponent<MeshRenderer>().materials) item.color = c;
            else GetComponent<MeshRenderer>().material.color = c;

        }
    }

}
