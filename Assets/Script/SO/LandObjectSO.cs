using UnityEngine;

[CreateAssetMenu(fileName = "LandObjectSO", menuName = "Scriptable Objects/LandObjectSO")]
public class LandObjectSO : ScriptableObject
{
    public float price;
    public float rent1;
    public float rent2;
    public float rent3;
    public float rent4;
    public float buyBackPrice1;
    public float buyBackPrice2;
    public float buyBackPrice3;
    public float buyBackPrice4;
    private float rentIncreaseAmount;
    public float upgradeCost;

    public enum LandTier{
        F2P,
        GoldFish,
        Dolphin,
        Whale
    }

    public LandTier landTier;

    private void OnEnable() {
        switch (landTier){
            case LandTier.F2P:
                rentIncreaseAmount = 50;
            break;
            case LandTier.GoldFish:
                rentIncreaseAmount = 100;
            break;
            case LandTier.Dolphin:
                rentIncreaseAmount = 150;
            break;
            case LandTier.Whale:
                rentIncreaseAmount = 200;
            break;
        }

        rent1 = price / 2;
        rent2 = rent1 + rentIncreaseAmount;
        rent3 = rent1 + rentIncreaseAmount*2;
        rent4 = rent1 + rentIncreaseAmount*3;
        upgradeCost = price / 4;

        buyBackPrice1 = rent1 - rent1/4;
        buyBackPrice2 = rent2 - rent2/4;
        buyBackPrice3 = rent3 - rent3/4;
        buyBackPrice4 = rent4 - rent4/4;
    }
}
