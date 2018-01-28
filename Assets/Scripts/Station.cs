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
    private List<Employee> employees;
    [SerializeField]
    private float moneyVsHappiness;
    [SerializeField]
    private MarketingPlanType marketingPlan;
    [SerializeField]
    private UpgradeType upgrade;

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
}
