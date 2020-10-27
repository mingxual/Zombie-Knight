using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentWallCollider : MonoBehaviour
{
    public PrefabGenerator pd;
    public Transform adjacentWallCheck;
    public Transform sideWallPrefab;
    private Transform sideWall, otherWall;

    public bool zAxis;
    
    private float maxDiff = 0.2f, maxDist = 5.0f;
    private bool validAdjacentWall = false;

    private float minDist;

    private bool finish;

    public Transform objectParent;

    void Start()
    {
        sideWall = null;
        finish = false;
        minDist = 1000.0f;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.tag != "Wall")
            return;
        float dist = Vector3.Distance(adjacentWallCheck.position, collision.collider.transform.position);
        if (dist < minDist)
        {
            minDist = dist;
            otherWall = collision.collider.transform;
        }
    }

    void Update()
    {
        if (finish)
        {
            adjacentWallCheck.position = new Vector3(1000.0f, 1000.0f, 1000.0f);
            finish = false;
        }

        Vector3 pos = adjacentWallCheck.position;
        Vector3 otherWallpos = Vector3.zero;
        if (otherWall != null)
        {
            otherWallpos = otherWall.position;
            if (!zAxis) //x
            {
                validAdjacentWall = (Mathf.Abs(otherWallpos.z - adjacentWallCheck.position.z) < maxDiff) &&
                                    (Mathf.Abs(otherWallpos.x - adjacentWallCheck.position.x) < maxDist);
            }
            else
            {
                validAdjacentWall = (Mathf.Abs(otherWallpos.z - adjacentWallCheck.position.z) < maxDist) &&
                                    (Mathf.Abs(otherWallpos.x - adjacentWallCheck.position.x) < maxDiff);
            }
            validAdjacentWall = validAdjacentWall && !pd.objCollided();
        }
        else
        {
            validAdjacentWall = false;
        }

        if (validAdjacentWall)
        {
            Vector3 sideWallPos = otherWallpos;
            Vector3 scale = sideWallPrefab.localScale;
            sideWallPos = (otherWallpos + pos) / 2.0f;
            sideWallPos.y = pd.startY;
            scale.x = computeScale(otherWallpos, pos, otherWall.localScale.x);

            if (sideWall == null)
            {
                sideWall = Instantiate(sideWallPrefab, sideWallPos, Quaternion.identity);

                sideWall.SetParent(objectParent);
                Color color = sideWall.GetComponent<MeshRenderer>().material.color;
                color.a = pd.objTransparency;
                sideWall.GetComponent<MeshRenderer>().material.color = color;

                if (zAxis) sideWall.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
            }
            else
            {
                sideWall.position = sideWallPos;
                sideWall.localScale = scale;
            }
        }
        else
        {
            if (sideWall)
            {
                Destroy(sideWall.gameObject);
                sideWall = null;
            }
        }

        if (sideWall != null)
        {
            if (Input.GetMouseButton(0))
            {
                Color color = sideWall.GetComponent<MeshRenderer>().material.color;
                color.a = 1.0f;
                sideWall.GetComponent<MeshRenderer>().material.color = color; 
                sideWall = null;
                finish = true;
            }
            else if (Input.GetMouseButton(1))
            {
                Destroy(sideWall.gameObject);
                sideWall = null;
                finish = true;
            }
        }
    }

    float computeScale(Vector3 pos1, Vector3 pos2, float scale)
    {
        if (zAxis)
        {
            return (Mathf.Abs(pos1.z - pos2.z) - scale) * 0.25f + 0.21f;
        }
        else 
        {
            return (Mathf.Abs(pos1.x - pos2.x) - scale) * 0.25f + 0.21f;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag != "Wall")
            return;
        otherWall = null;
        minDist = 1000.0f;
    }
}