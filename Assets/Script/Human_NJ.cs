using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_NJ : MonoBehaviour
{
    public float WalkSpeed = 0.1f;
    public float RotationSpeed = 100f;
    public float Hp = 400;
    public float Dmg = 5;
    public int UserID;
    public bool IsDead => Hp <= 0;
    public float JumpSpeed = 10;
    bool ground = true;
    Quaternion _TargetQuaternion;
    Vector3 _MovePosition;
    public bool isSneak;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
    }
    private void FixedUpdate()
    {
        if (IsDead) { return; }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (ground == true)
        //    {
        //        GetComponent<Rigidbody>().velocity += new Vector3(0, 1, 0);
        //        GetComponent<Rigidbody>().AddForce(Vector3.up * JumpSpeed * Time.deltaTime);
        //        //ground = false;
        //    }
        //}
    }
    // Update is called once per frame
    void Update()
    {
        if (IsDead) { return; }

        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        //{
        //    float Walk_X = Input.GetAxis("Horizontal") * WalkSpeed * Time.deltaTime;
        //    transform.Translate(Walk_X, 0, 0);
        //}
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        //{
        //    float Walk_Z = Input.GetAxis("Vertical") * WalkSpeed * Time.deltaTime;
        //    transform.Translate(0, 0, Walk_Z);
        //}
        float rotation = Input.GetAxis("Mouse X") * 300 * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        //var distance = Vector3.Distance(transform.position, _MovePosition);
        //if (distance > 0.1f)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, _MovePosition, WalkSpeed * Time.deltaTime);
        //}
        //if (transform.rotation.y != _TargetQuaternion.y)
        //{
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, _TargetQuaternion, RotationSpeed * Time.deltaTime);
        //}
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Arms"))
        {
            Damage(Dmg);
            Debug.Log(Hp);
        }
    }
    public void SetMovePosition(Vector3 movePos)
    {
        _MovePosition = movePos;
    }
    public void SetRotation(float angle)
    {
        _TargetQuaternion = Quaternion.Euler(0, angle, 0);
    }
    public void Shot()
    {
    }
    public void Damage(float dmg)
    {
        Hp -= dmg;
    }
    public IEnumerator Dead()
    {
        while (transform.eulerAngles.z < 80)
        {
            transform.Rotate(Vector3.forward * 1.5f);
            yield return null;
        }
        Destroy(gameObject);
    }
}
