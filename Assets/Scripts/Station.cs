using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour {
    private Town containingTown;

    [SerializeField]
    private float musicEffectivePercent;
    [SerializeField]
    private float propagandaEffectivePercent;
    [SerializeField]
    private float advertisingRevenuePercent;
    [SerializeField]
    private float musicTimePercent;
    [SerializeField]
    private float adTimePercent;
    [SerializeField]
    private float cultTimePercent;
    [SerializeField]
    private float stationUpkeep;
    [SerializeField]
    private List<Employee> employees;
    [SerializeField]
    private float moneyVsHappiness;
    [SerializeField]
    private MarketingPlanType marketingPlan;
    [SerializeField]
    private UpgradeType chosenUpgradeType;
    [SerializeField]
    private MusicType chosenMusicType;
    [SerializeField]
    private AdType chosenAdType;
    [SerializeField]
    private CultMessageType chosenCultMessageType;

    public Town ContainingTown {
        get { return containingTown; }
        set { containingTown = value; }
    }

    public float MusicEffectivePercent {
        get { return musicEffectivePercent; }
        set { musicEffectivePercent = value; }
    }

    public float PropagandaEffectivePercent {
        get { return propagandaEffectivePercent; }
        set { propagandaEffectivePercent = value; }
    }

    public float AdvertisingRevenuePercent {
        get { return advertisingRevenuePercent; }
        set { advertisingRevenuePercent = value; }
    }

    public List<Employee> Employees {
        get { return employees; }
        set { employees = value; }
    }

    public float MoneyVsHappiness {
        get { return moneyVsHappiness; }
        set { moneyVsHappiness = Mathf.Clamp(value, 0, 1); }
    }

    public MusicType ChosenMusicType {
        get { return chosenMusicType; }
        set { chosenMusicType = value; }
    }

    public AdType ChosenAdType {
        get { return chosenAdType; }
        set { chosenAdType = value; }
    }

    public CultMessageType ChosenCultMessageType {
        get { return chosenCultMessageType; }
        set { chosenCultMessageType = value; }
    }

    public MarketingPlanType ChosenMarketingPlanType {
        get { return marketingPlan; }
        set { marketingPlan = value; }
    }

    public float MusicTimePercent {
        get { return musicTimePercent; }
        set { musicTimePercent = value; }
    }

    public float AdTimePercent {
        get { return adTimePercent; }
        set { adTimePercent = value; }
    }

    public float CultTimePercent {
        get { return cultTimePercent; }
        set { cultTimePercent = value; }
    }

    public float StationUpkeep {
        get { return stationUpkeep; }
        set { stationUpkeep = value; }
    }

    void Start() {
        GameManager.TimeTick.AddListener(OnTimeTick);
    }

    void OnTimeTick() {
        handleEmployees();
        addMoney();
        addListeners();
        addCultists();
    }

    private void handleEmployees() {
        List<Employee> employeesToRemove = new List<Employee>();
        foreach (Employee employee in Employees) {
            if (MoneyVsHappiness > 0.5f) {
                employee.Happiness += MoneyVsHappiness * 5;
            } else if (MoneyVsHappiness < 0.5f) {
                employee.Happiness -= (1 - MoneyVsHappiness) * 5;
            }

            if (employee.Happiness < 0) {
                employeesToRemove.Add(employee);
            }
        }
        foreach (Employee employeeToRemove in employeesToRemove) {
            Employees.Remove(employeeToRemove);
        }
    }

    private void addMoney() {
        float moneyToAdd = AdTimePercent * AdvertisingRevenuePercent * ContainingTown.Listeners*500;
        if (ContainingTown.PreferredAdType == ChosenAdType) moneyToAdd *= 1.5f;
        float moneyToSubtract = StationUpkeep;
        foreach (Employee employee in Employees) {
            moneyToSubtract += employee.Wage * MoneyVsHappiness;
            moneyToSubtract += employee.CareCost * MoneyVsHappiness;
        }
        GameManager.Instance.MoneyCount += Mathf.Ceil(moneyToAdd);
        GameManager.Instance.MoneyCount -= Mathf.Ceil(moneyToSubtract);
    }

    private void addListeners() {
        float listenersToAdd = MusicTimePercent * MusicEffectivePercent * 2;
        if (ContainingTown.PreferredMusicType == ChosenMusicType) listenersToAdd *= 1.5f;
        ContainingTown.Listeners += (int) Mathf.Ceil(listenersToAdd);
    }

    private void addCultists() {
        float cultistsToAdd = CultTimePercent * PropagandaEffectivePercent * 2;
        if (ContainingTown.PreferredCultMessageType == ChosenCultMessageType) cultistsToAdd *= 1.5f;
        ContainingTown.Cultists += (int) Mathf.Ceil(cultistsToAdd);
    }

    public void AddEmployee(Employee freshMeat) {
        Employees.Add(freshMeat);
    }
}
