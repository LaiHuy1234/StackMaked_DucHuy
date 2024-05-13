using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Direction m_direction;
    [SerializeField] private Vector3 m_targetPos;
    [SerializeField] private float Speed = 5f;
    [SerializeField] public Vector2 startPos;
    [SerializeField] public Vector2 endPos;
    public Vector2 swipeDirection;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject brickOnBridge;


    //private void Awake()
    //{
    //    instance = this;
    //}
    private void GetTouchEvent()
    // tạo event chạm vào màn hình bằng vector2
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            swipeDirection = endPos - startPos;
            ConvertSwipeDirection();
        }
        endPos = Input.mousePosition;
        //transform.position = vectorDirection * Speed * Time.deltaTime;
    }

    private void Move()
    {
        // Convert từ Vector3 => Vector2
        Vector3 vectorDirection = GetMoveDirectionBySwipeDirection(m_direction);
        // Vẽ tia RayCast
        Ray ray = new Ray(transform.position + vectorDirection * 0.6f, Vector3.down);
        RaycastHit hit;
        int countBrick = 0;
        int countUnBrick = 0;
        int countPath = 0;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(transform.position + vectorDirection, Vector3.up * 5f, Color.red);
            if (hit.collider.CompareTag(GameTag.Brick.ToString()))
            {
                Debug.Log(1);
                countBrick++;
            }
            else if (hit.collider.CompareTag(GameTag.UnBrick.ToString()))
            {
                countUnBrick++;
            }
            else if (hit.collider.CompareTag(GameTag.Finish.ToString()))
            {
                countPath++;
            }
            else if (hit.collider.CompareTag(GameTag.Destination.ToString()))
            {
                //RemoveBrick();
                Debug.Log("win");
            }
        }
        //Hàm Di chuyển nhân vật
        m_targetPos = transform.position + (countBrick + countUnBrick + countPath) * vectorDirection;
        transform.position = Vector3.MoveTowards(transform.position, m_targetPos, Speed * Time.deltaTime);
    }
    public enum Direction
    {
        None,
        Left,
        Right,
        Forward,
        Backward,
    }

    public enum GameTag
    {
        None,
        Player,
        Wall,
        Brick,
        UnBrick,
        Finish,
        Destination,
        Slow
    }
    void Update()
    {
        GetTouchEvent();
        Move();
    }

    private void ConvertSwipeDirection()
    {
        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
        {
            if (swipeDirection.x > 0)
            {
                m_direction = Direction.Right;
            }
            else
            {
                m_direction = Direction.Left;
            }
        }
        else if (Mathf.Abs(swipeDirection.x) < Mathf.Abs(swipeDirection.y))
        {
            if (swipeDirection.y > 0)
            {
                m_direction = Direction.Forward;
            }
            else
            {
                m_direction = Direction.Backward;
            }
        }
        else
        {
            m_direction = Direction.None;
        }
    }

    private Vector3 GetMoveDirectionBySwipeDirection(Direction swipeDirection)
    // chuyển đổi vector2 sang vector3
    {
        switch (swipeDirection)
        {
            case Direction.None:
                return Vector3.zero;
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            case Direction.Forward:
                return Vector3.forward;
            case Direction.Backward:
                return Vector3.back;
            default:
                return Vector3.zero;
        }
    }
}
