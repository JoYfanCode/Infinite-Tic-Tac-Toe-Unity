public class MoneyData
{
    public int Money;
}

public class MoneyStorage
{
    public int Money { get; private set; }

    public void SetupMoney(int amount)
    {
        Money = amount;
    }

    public void AddMoney(int amount)
    {
        if (amount > 0)
        {
            Money += amount;
        }
    }

    public bool TrySpendMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            return true;
        }
        else
        {
            return false;
        }

    }
}