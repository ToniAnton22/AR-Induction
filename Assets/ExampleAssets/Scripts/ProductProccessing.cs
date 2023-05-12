using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProductProccessing
{
    public class ScanningProcess    
    {
        public bool scan;
        public Order order;
        public string status;

        public ScanningProcess(bool aScan, Order aOrder, string aStatus)
        {
            scan = aScan;
            order = aOrder;
            status = aStatus;
        }

        public string Status
        {
            get { return status; }
            set
            {
                if (value == "Box" || value== "Cage" || value== "Completed")
                {
                    status = value;
                }
            
            }
        }
    }
    public class Order
    {
        public int orderId;
        public string cageNumber;
        public string productName;
        public int numberProducts;
        public string shopDestination;
        public int productsToPut;
        public static int orderCount = 0;
        
        public Order(int aOrderId, string aCageNumber, string aProductName, int aNumberProducts, string aShopDestination, int aProductsToPut)
        {
            orderId = aOrderId;
            cageNumber = aCageNumber;
            productName = aProductName;
            numberProducts = aNumberProducts;
            shopDestination = aShopDestination;
            productsToPut = aProductsToPut;
            orderCount++;
        }

        public static List<Order> GetOrders()
        {
            List<Order> orders = new()
            {
                new Order(1, "204", "Icecream", 4, "Liverpool", 2),
                new Order(2,"205","Raphaelo",2,"Doncaster",1),
                new Order(2, "206", "Cola", 6 , "Southampton", 1)
  
            };
            return orders;
        }

    };
    public class Assignment
    {
        private int assignmentId;
        public List<Order> orders;
        public string aisle;
        public string status;

        public Assignment(List<Order> aOrders,string aAisle,string aStatus)
        {
            assignmentId = Random.Range(1, 200);
            orders = aOrders;
            aisle = aAisle;
            status = aStatus;
        }

        public static Assignment GetAssignment()
        {
            return new Assignment(Order.GetOrders(), "A23", "started");
        }
        public bool Fulfilled(bool status)
        {
            // later use for storing completed orders
            if(status == true)
            {
                return true;
            }
            return false;
            
         
        }
    };
}
