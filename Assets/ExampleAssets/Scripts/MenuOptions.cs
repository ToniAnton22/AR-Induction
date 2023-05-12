using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProductProccessing;
public class MenuOptions : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private Button assignmentOne;
    [SerializeField]
    private Button assignmentTwo;
    [SerializeField]
    private Button assignmentThree;

    // Start is called before the first frame update
    void Start()
    {
        assignmentOne.onClick.AddListener(StartAssignmentOne);
        assignmentTwo.onClick.AddListener(StartAssignmentTwo);
        assignmentThree.onClick.AddListener(StartAssignmentThree);
    }

    public void StartAssignmentOne()
    {
        List<Order> orders = new()
        {
            new Order(1, "305", "Icecream", 6, "Southampton", 2),
            new Order(2, "306", "Maximus", 2, "London", 2),
            new Order(3, "312", "Left", 4, "Great Apenstone", 2)
        };

        Assignment assign1 = new(orders, "A23", "onGoing");
        Debug.Log("ASSIGNMENT STARTED" + assign1.aisle);
        AndroidScanner.AndroidScanner.StartAssignment(assign1);


    }
    public void StartAssignmentTwo()
    {
        List<Order> orders = new()
        {
            new Order(1, "305", "Pepsi", 6, "Southampton", 2),
            new Order(2, "306", "Maximus", 2, "London", 2),
            new Order(3, "312", "Left", 4, "Great Apenstone", 2)
        };

        Assignment assign2 = new Assignment(orders, "A23", "onGoing");

        AndroidScanner.AndroidScanner.StartAssignment(assign2);
    }
    public void StartAssignmentThree()
    {
        List<Order> orders = new()
        {
            new Order(1, "305", "Pepsi", 6, "Southampton", 2),
            new Order(2, "306", "Maximus", 2, "London", 2),
            new Order(3, "312", "Left", 4, "Great Apenstone", 2)
        };

        Assignment assign3 = new Assignment(orders, "A23", "onGoing");
        AndroidScanner.AndroidScanner.StartAssignment(assign3);
    }

    // Update is called once per frame
  
}
