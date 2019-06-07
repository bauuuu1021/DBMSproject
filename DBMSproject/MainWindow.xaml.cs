using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;

namespace DBMSproject
{

    public partial class MainWindow : Window
    {
        SQLiteConnection conn;
        SQLiteCommand sample_cmd, list_cmd, qry_cmd, button_cmd;
        SQLiteDataReader dr;

        string[] tableListVal = new string[] { "FACULTY", "SCHOLAR", "STU", "DEPT", "PROF" };
        string[] opListVal = new string[] { "Select", "Delete", "Update", "Insert" };

        public MainWindow()
        {
            InitializeComponent();

            int i;
            for (i=0; i<tableListVal.Length; i++) 
                tableList.Items.Add(tableListVal[i]);
            for (i = 0; i < tableListVal.Length; i++)
                opTableList.Items.Add(tableListVal[i]);
            for (i = 0; i < opListVal.Length; i++)
                opList.Items.Add(opListVal[i]);

            conn = new SQLiteConnection("Data Source=DataBase.db");
            conn.Open();

        }

        private void tableList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                list_cmd = conn.CreateCommand();
                list_cmd.CommandText = "Select * From " + tableList.SelectedItem;

                dr = list_cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(dr);

                dataZone.ItemsSource = dt.DefaultView;
            }
            catch
            {
                ;
            }

        }

        private void OpSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                button_cmd = conn.CreateCommand();

                if (opList.SelectedItem == opListVal[0])        // Select
                {
                    Console.WriteLine('0');
                }
                else if (opList.SelectedItem == opListVal[1])   // Delete
                {
                    Console.WriteLine('1');
                }
                else if (opList.SelectedItem == opListVal[2])   // Update
                {
                    Console.WriteLine('2');
                }
                else if (opList.SelectedItem == opListVal[3])   // Insert
                {
                    button_cmd.CommandText = string.Format("Insert into {0} values ('{1}',{2})");
                    //Console.WriteLine("test {0} end", opTableList.Text);
                }
            }
            catch
            {
                ;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            sample_cmd = conn.CreateCommand();

            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('1','50345','life','jim')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('3','30487','act','arron')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('17',1071,'aborigine','1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('5',1071,'poor','3')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('17','hua','3','F7','1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('5','larry','5','E4','5')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('1','simpoo','F7')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('5','larry','E4')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('F7','cs',1997,'csee','17')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('E4','earth',1956,'eng','5')";
            sample_cmd.ExecuteNonQuery();

        }

        private void sendQry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                qry_cmd = conn.CreateCommand();
                qry_cmd.CommandText = queryCmd.Text;
                qry_cmd.ExecuteNonQuery();

                dr = qry_cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataZone.ItemsSource = dt.DefaultView;
            }
            catch 
            {
                ;
            }
        }
    }
}
