using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{

    public DiceSideChecker Up, Down, Left, Right, Front, Back;
    public List<AudioClip> diceSoundCollision;
    BoxCollider diceCollider;
    public Transform groundCenter;
    public Vector3 dicePosition;
    public Quaternion diceRotation;
    Rigidbody diceRigid;
    [Range(0, 15)]
    public float draggedForce;
    public int diceValue = 0;
    public bool isRolled = false;
    SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        diceCollider = GetComponent<BoxCollider>();
        diceRigid = GetComponent<Rigidbody>();
        diceRigid.isKinematic = true;
        dicePosition = this.transform.position;
        diceRotation = this.transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        diceValue = GetDiceValue();
    }
    //Start rolling the dice from the force apply on the dice
    public IEnumerator RollDiceOnForce(float draggedForce, Vector2 mouseDown, Vector2 mouseUp)
    {
        ResetPosition();
        diceRigid.isKinematic = false;
        //Start throw up the dice with the force with direction to center of ground
        Vector3 directionPush =
        new Vector3(mouseUp.x - mouseDown.x, 0, Mathf.Abs(mouseUp.y - mouseDown.y)).normalized;
        diceRigid.AddForce(directionPush * draggedForce * 100, ForceMode.Impulse);
        //random throw torque
        diceRigid.AddTorque(directionPush.x < 0 ? -Vector3.Cross(directionPush, Vector3.up) : Vector3.Cross(directionPush, Vector3.up) * Random.Range(15, 30) * 1000, ForceMode.Impulse);
        //random throw torque offset
        diceRigid.AddTorque(directionPush * Random.Range(3, 7) * 1000, ForceMode.Impulse);
        yield return new WaitForSeconds(1);
        while (isDiceStatic() == false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        isRolled = true;
    }
    public void ResetPosition()
    {
        this.transform.position = dicePosition;
        this.transform.rotation = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 1);
        diceRigid.velocity = Vector3.zero;
        diceRigid.angularVelocity = Vector3.zero;
        diceRigid.isKinematic = true;

    }
    //Check if the dice is stay in the ground
    public bool isDiceStatic()
    {
        return diceRigid.velocity == Vector3.zero;
    }
    public int GetDiceValue()
    {
        if (isDiceStatic())
        {
            if (Up.isTriggered)
            {
                return Up.value;
            }
            else if (Down.isTriggered)
            {
                return Down.value;
            }
            else if (Left.isTriggered)
            {
                return Left.value;
            }
            else if (Right.isTriggered)
            {
                return Right.value;
            }
            else if (Front.isTriggered)
            {
                return Front.value;
            }
            else if (Back.isTriggered)
            {
                return Back.value;
            }
        }
        return -1;
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Play the sound effect when the dice collides with anything
        soundManager.PlaySFX(diceSoundCollision[Random.Range(0, diceSoundCollision.Count)]);
    }
}