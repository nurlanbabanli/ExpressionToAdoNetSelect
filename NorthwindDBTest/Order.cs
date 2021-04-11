﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDBTest
{
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public int ShipVia { get; set; }
        public float Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
    }



    //SELECT TOP(1000) [OrderID]
    //  ,[CustomerID]
    //  ,[EmployeeID]
    //  ,[OrderDate]
    //  ,[RequiredDate]
    //  ,[ShippedDate]
    //  ,[ShipVia]
    //  ,[Freight]
    //  ,[ShipName]
    //  ,[ShipAddress]
    //  ,[ShipCity]
    //  ,[ShipRegion]
    //  ,[ShipPostalCode]
    //  ,[ShipCountry]
    //FROM[Northwind].[dbo].[Orders]
}
