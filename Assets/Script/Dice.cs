using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Transform[] diceSides;
    [SerializeField] private Vector3 originTrans;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float torqueMultiplier;

    private Rigidbody diceRb;
    private bool rolling = false;
    private bool readyToCheckResult = false;

    private void Awake() 
    {
        diceRb = gameObject.GetComponent<Rigidbody>();

    }

    private void Start() 
    {
        diceRb.useGravity = false;
        // originTrans.position = gameObject.transform.position;
        DiceManager.Instance.OnRollDice += DiceManager_OnRollDice; 
    }


    private void DiceManager_OnRollDice(object sender, System.EventArgs e)
    {
        RollDice(forceMultiplier, torqueMultiplier);
    }

    public void RollDice(float forceMultiplier, float torqueMultiplier)
    {
        diceRb.useGravity = true;
        Vector3 randomTorque = new Vector3
        (
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );

        // diceRb.AddForce(this.transform.TransformDirection(Vector3.down) * forceMultiplier);
        diceRb.AddForce(Vector3.down * forceMultiplier);
        diceRb.AddTorque(randomTorque * torqueMultiplier);

        rolling = true;
        readyToCheckResult = false;
        DelayResult();
    }

    void Update()
    {
        if(!readyToCheckResult) {
            return;
        } 
        
        if(rolling && diceRb.angularVelocity.sqrMagnitude < .1f) 
        {
            rolling = false;
            GetDiceValue();    
        }
    }

    private void GetDiceValue()
    {
        if(diceSides.Length == 0) {
            Debug.LogError("Our dice doesnt have ant diceSides defined");
            return;
        }

        int topFace = 0;
        float lastYPosition = diceSides[0].position.y;

        for(int i = 1; i < diceSides.Length; i++) 
        {
            if (diceSides[i].position.y > lastYPosition)
            {
                topFace = i;
                lastYPosition = diceSides[i].position.y;
            }    
        }
        int diceResult = topFace + 1;

        // print("Dice value "+ diceResult);
        DiceManager.Instance.SetDiceValue(diceResult);
        DiceManager.Instance.Check2Dice();
        
        // reset dice position
        // print("Origin trans "+originTrans);
        transform.position = originTrans;
        diceRb.useGravity = false;
    }

    private async void DelayResult()
    {
        await Task.Delay(1000); // delay 1s
        readyToCheckResult = true;
    }

    private void OnDestroy() {
        DiceManager.Instance.OnRollDice -= DiceManager_OnRollDice; 
    }
}
