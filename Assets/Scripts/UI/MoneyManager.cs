using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance {  get; private set; }

    public int currentMoney = 0;
    public Text moneyText;

    [System.Serializable]
    public class MoneyChangedEvent : UnityEvent<int> { }
    public MoneyChangedEvent onMoneyChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateMoneyUI();
        onMoneyChanged.Invoke(currentMoney);
    }

    //MoneyManager.Instance.SpendMoney(가격) 
    public bool SpendMoney(int amount)
    {
        if(currentMoney >= amount)
        {
            currentMoney -= amount;
            onMoneyChanged.Invoke(currentMoney);
            UpdateMoneyUI();

            SFXManager.Instance.Play("money");
            
            Debug.Log("구매 성공");

            return true;
        }
        else
        {
            Debug.Log("돈 부족");
            return false;
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        onMoneyChanged.Invoke(currentMoney);
        UpdateMoneyUI();

        SFXManager.Instance.Play("money");
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = $"{currentMoney}";
    }


}
