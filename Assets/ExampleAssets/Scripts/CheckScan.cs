
using System.Collections.Generic;
using UnityEngine;
using ProductProccessing;

namespace ScanChecker
{
    class ScanChecker {
        private static string status = "Box";

        public static Order VerifyBox(Assignment assignment,string scannedBox)
        {
            foreach (Order order in assignment.orders)
            {
                Debug.Log("[SCANNER] CAGE VALIDATION" + " " + order.productName.ToString() +  " " + scannedBox);
                if (order.productName == scannedBox)
                {
                    ChangeStatus(true, "Box");
                    return order;
                }
                
            }
            ChangeStatus(false, "Box");
            return null;
        }
        
        public static string Status
        {
            get { return status; }
            set
            {
                if (value == "Box" || value == "Cage" || value == "Completed")
                {
                    status = value;
                }
                else
                {
                    _ = status == "Box";
                }
            }
        }
        public static Order VerifyCage(Assignment assignment, string scannedCage)
        {
            foreach (Order order in assignment.orders)
            {
                Debug.Log("[SCANNER] CAGE VALIDATION"  + " " + order.cageNumber.ToString() + " " + order.productName + " " +scannedCage);
                if (scannedCage.Trim().Equals(order.cageNumber.Trim()))
                {
                    Debug.Log("[SCANNER] VALIDATED TRUE");
                    ChangeStatus(true, "Cage");      
                    return order;
                }
              
            }
            Debug.Log("[SCANNER] VALIDATED FALSE");
            ChangeStatus(false, "Cage");
            return null;
        }

        private static void ChangeStatus(bool success, string progress)
        {
            if(success == true)
            {
                if (progress == "Box")
                {
                    Status = "Cage";
                }
                else if (progress == "Cage")
                {
                    Status = "Completed";
                }
                else
                {
                    Status = "Box";
                }
            }
            else
            { 
                Status = progress;
            }
        }
        
    }
}