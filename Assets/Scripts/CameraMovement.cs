using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera2D;
    [SerializeField]
    private Camera m_Camera3D;

    private Vector3 m_Camera2DGameOfLifePos;
    private Vector3 m_Camera3DGameOfLifePos;

    private bool m_In3DMode;


    void Start ()
    {
        m_In3DMode = false;
        m_Camera2D.enabled = true;
        m_Camera3D.enabled = false;
	}

    public void SetRightCamera(bool camera3DOn)
    {
        if (camera3DOn)
        {
            m_Camera3D.enabled = true;
            m_Camera2D.enabled = false;
            m_In3DMode = true;
        }
        else
        {
            m_Camera2D.enabled = true;
            m_Camera3D.enabled = false;
            m_In3DMode = false;
        }
    }


    void FixedUpdate ()
    {
        if (m_In3DMode)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (50f * Time.fixedDeltaTime), transform.position.z);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (50f * Time.fixedDeltaTime), transform.position.z);
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y + 1f * Time.fixedDeltaTime, transform.rotation.z, transform.rotation.w);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y - 1f * Time.fixedDeltaTime, transform.rotation.z, transform.rotation.w);
            }
        }
    }
}
