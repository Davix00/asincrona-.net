using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//espacio de nombre para el manejo de sqlserver
using System.Data.SqlClient;
using System.Collections;

//espacio de nombre para el manejo de hilos
using System.Threading;
using System.Runtime.InteropServices;

namespace peticiones_asincronas
{
    public partial class Form1 : Form
    {
        //consultas select per.rowguid, per.FirstName,per.LastName,per.PersonType, adr.City, adr.StateProvinceID, adr.rowguid  from Person.Person as per,
        //Person.Address as adr;
        //select* from Person.Person;

        //varriables
        string connectionString = "Server=DESKTOP-3SAM15G;Database=AdventureWorks2012;Integrated Security=True;"; //cadena de conexion
        public delegate DataTable delegado(string query); //creacion del delegado


        //funciones 
        public Form1()
        {
            InitializeComponent();
        }

        public DataTable ejecutaQuery(string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    // Ejecutar la consulta SELECT
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return dataTable;
        }


        private void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;

            // Crear y ejecutar un nuevo hilo
            Thread hilo = new Thread(() =>
            {
                delegado ejecuta = ejecutaQuery; // Asigna el método al delegado
                 
                if (button1.InvokeRequired)
                {
                    button1.Invoke(new MethodInvoker(delegate
                    {
                        dataGridView1.DataSource = ejecuta("select poh.PurchaseOrderID as OrderID, poh.VendorID, pod.PurchaseOrderDetailID as OrderDetailID, pod.ProductID, pod.UnitPrice, pod.StockedQty, sm.Name as NombreEnvio, sm.ShipBase from Purchasing.PurchaseOrderHeader as poh, Purchasing.PurchaseOrderDetail as pod, Purchasing.ShipMethod as sm where poh.VendorID = 1662 and pod.PurchaseOrderID = poh.PurchaseOrderID and sm.ShipMethodID = poh.ShipMethodID;\r\n"); // Llama al delegado con la query
                        button1.Enabled = true;
                    }));
                }
            });

            hilo.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;

            // Crear y ejecutar un nuevo hilo
            Thread hilo2 = new Thread(() =>
            {
                delegado ejecuta = ejecutaQuery; // Asigna el método al delegado
                    
                if (button1.InvokeRequired)
                {
                    button1.Invoke(new MethodInvoker(delegate
                    {
                        dataGridView2.DataSource = ejecuta("select poh.VendorID, v.BusinessEntityID as VendorIdV, poh.PurchaseOrderID, pod.PurchaseOrderID,poh.SubTotal, pod.UnitPrice, pod.OrderQty,pod.ProductID, p.Name, p.Color, v.Name, v.AccountNumber from Purchasing.PurchaseOrderHeader as poh, Purchasing.PurchaseOrderDetail as pod, Production.Product as p, Purchasing.Vendor as v where poh.VendorID between 1568 and 1572 and pod.PurchaseOrderID = poh.PurchaseOrderID and v.BusinessEntityID = poh.VendorID and pod.ProductID = p.ProductID;\r\n"); // Llama al delegado con la query
                        button1.Enabled = true;
                    }));
                }
            });

            hilo2.Start();
        }
    }
}
