using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotSpeed;

    protected Rigidbody playerRB;

    void Start ()
    {
        playerRB = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1) == true)
            playerRB.velocity = moveSpeed * -transform.right;
        else
            playerRB.velocity = new Vector3(0, 0, 0);


        Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        Vector3 mouseOnScreen = (Vector3)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        Quaternion New = Quaternion.Euler(new Vector3(0f, -angle, 0f));
        transform.rotation = Quaternion.Lerp(transform.rotation, New, Time.time * rotSpeed);

    }


    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
