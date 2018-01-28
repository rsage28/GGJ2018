using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour {
    private Town containingTown;
    private float totalEmployeeCost;
    private float totalIncomePerTimeTick;

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
    [SerializeField, Range(0, 100)]
    private float moneyVsHappiness;
    [SerializeField]
    private MarketingPlanType marketingPlan;
    [SerializeField]
    private UpgradeType upgrade;
    [SerializeField]
    private MusicType music;
    [SerializeField]
    private AdType ad;
    [SerializeField]
    private CultMessageType cult;

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
        set { moneyVsHappiness = value; }
    }

    public MusicType Music {
        get { return music; }
        set { music = value; }
    }

    public AdType Ad {
        get { return ad; }
        set { ad = value; }
    }

    public CultMessageType Cult {
        get { return cult; }
        set { cult = value; }
    }

    public MarketingPlanType MarketingPlan {
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

    public float TotalEmployeeCost {
        get { return totalEmployeeCost; }
        set { totalEmployeeCost = value; }
    }

    public float TotalIncomePerTimeTick {
        get { return totalIncomePerTimeTick; }
        set { totalIncomePerTimeTick = value; }
    }

    public float StationUpkeep {
        get { return stationUpkeep; }
        set { stationUpkeep = value; }
    }

    // Use this for initialization
    void Start() {
        GameManager.TimeTick.AddListener(OnTimeTick);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTimeTick() {
        print("some shit here");
    }

    public void AddEmployee(Employee freshMeat) {
        Employees.Add(freshMeat);
        TotalEmployeeCost += freshMeat.CareCost + freshMeat.Wage;
    }
}
