using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController instance;

    private void Awake()
    {
        instance = this;
    }

    public int currentCoins;

    public CoinPickup coin;

    public void AddCoins(int coinToAdd)
    {
        currentCoins += coinToAdd;

        UIController.Instance.UpdateCoins();
    }

    public void DropCoin(Vector3 position, int value)
    {
        CoinPickup newCoin = Instantiate(coin, position +new Vector3(0.2f, 0.1f, 0),Quaternion.identity ); 
        newCoin.coinAmount = value;
        newCoin.gameObject.SetActive(true);
    }
}
