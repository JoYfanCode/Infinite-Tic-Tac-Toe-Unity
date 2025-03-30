public class MoneySaveLoader : SaveLoader<MoneyData, MoneyStorage>
{
    const int DEFAULT_MONEY = 500;

    protected override MoneyData ConvertToData(MoneyStorage service)
    {
        return new MoneyData() { Money = service.Money };
    }

    protected override void SetupData(MoneyData data, MoneyStorage service)
    {
        service.SetupMoney(data.Money);
    }

    protected override void SetupDefaultData(MoneyStorage service)
    {
        service.SetupMoney(DEFAULT_MONEY);
    }
}