using ExpressionToSelectQuery;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindDBTest
{
    public partial class Form1 : Form
    {
        AdoNetContext _adoNetContext;
        public Form1()
        {
            InitializeComponent();
            AdoNetConfiguration();
        }

        private void btnGetCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateTime = Convert.ToDateTime("1996-07-04 00:00:00.000");
                var order = new Order();
                order = GetOrder(o => o.OrderDate == dateTime);

            }
            catch (Exception ex)
            { 

                MessageBox.Show(ex.Message);
            }















            //var customer = new Customer();
            //string country = "Germany";
            //string city = "Berlin";
            //string contactName = "Maria Anders";
            //try
            //{
            //    customer = GetCustomer(c => c.Country=="Germany" && c.City==city && c.ContactName==contactName);



            //    lblComapanyName.Text = customer.CompanyName;
            //    lblAddress.Text = customer.Address;
            //    lblCity.Text = customer.City;
            //    lblContactName.Text = customer.ContactName;
            //    lblContactTitle.Text = customer.ContactTitle;
            //    lblCountry.Text = customer.Country;
            //    lblFax.Text = customer.Fax;
            //    lblPhone.Text = customer.Phone;
            //    lblPostalCode.Text = customer.PostalCode;
            //    lblRegion.Text = customer.Region;
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        private Customer GetCustomer(Expression<Func<Customer,bool>> expression)
        {
            _adoNetContext = new AdoNetContext();

            return _adoNetContext.Set<Customer>(expression);
        }
        private Order GetOrder(Expression<Func<Order, bool>> expression)
        {
            _adoNetContext = new AdoNetContext();
            return _adoNetContext.Set<Order>(expression);
        }
        private void AdoNetConfiguration()
        {
            AdoNetContext.TableSet<Customer>("Customers");
            AdoNetContext.TableSet<Order>("Orders");

            AdoNetContext.UseSql("Server=NURLAN_B;Database=Northwind;User Id=sa1;Password=2");
        }
    }
}
