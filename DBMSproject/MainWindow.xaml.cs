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
        string[,] attribute = new string[,] { { "id", "tel", "office", "name", " " }
                                            , { "holder", "sem", "name", "principle", " " }
                                            , { "id", "name", "grade", "dept", "tutor" }
                                            , { "id", "name", "since", "college", "leader" }
                                            , { "id", "name", "dept", " ", " " } };

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
                    button_cmd.CommandText = "Delete from " + tableList.SelectedItem + combineRequest();
                    button_cmd.ExecuteNonQuery();
                }
                else if (opList.SelectedItem == opListVal[2])   // Update
                {
                    button_cmd.CommandText = "Update " + tableList.SelectedItem + update_attr() + combineRequest();
                    button_cmd.ExecuteNonQuery();
                }
                else if (opList.SelectedItem == opListVal[3])   // Insert
                {
                    String req = String.Format("{0} , {1} , {2}", current_col0_input.Text, current_col1_input.Text, current_col2_input.Text);

                    if (!String.IsNullOrEmpty(current_col3_input.Text))
                    {
                        req += " , " + current_col3_input.Text;
                    }
                    if (!String.IsNullOrEmpty(current_col4_input.Text))
                    {
                        req += " , " + current_col4_input.Text;
                    }

                    button_cmd.CommandText = String.Format("Insert into {0} values ({1})", tableList.SelectedItem, req);
                    button_cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                ;
            }
        }

        private string update_attr()
        {
            string req = " set ";
            int tableIndex = opTableList.SelectedIndex;
            Boolean doneReq = false;

            try
            {
                if (!String.IsNullOrEmpty(new_col0_input.Text))
                {
                    if (doneReq)
                        req += " , ";

                    req += String.Format("{0} = {1} ", attribute[tableIndex, 0], new_col0_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(new_col1_input.Text))
                {
                    if (doneReq)
                        req += " , ";

                    req += String.Format("{0} = {1} ", attribute[tableIndex, 1], new_col1_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(new_col2_input.Text))
                {
                    if (doneReq)
                        req += " , ";

                    req += String.Format("{0} = {1} ", attribute[tableIndex, 2], new_col2_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(new_col3_input.Text))
                {
                    if (doneReq)
                        req += " , ";

                    req += String.Format("{0} = {1} ", attribute[tableIndex, 3], new_col3_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(new_col4_input.Text))
                {
                    if (doneReq)
                        req += " , ";

                    req += String.Format("{0} = {1} ", attribute[tableIndex, 4], new_col4_input.Text);
                    doneReq = true;
                }
            }
            catch
            {

            }
            return req + " ";
        }
        private string combineRequest()
        {
            String req = "";
            int tableIndex = opTableList.SelectedIndex;
            Boolean doneReq = false;

            try
            {
                if (!String.IsNullOrEmpty(current_col0_input.Text))
                {
                    if (doneReq)
                        req += "and ";
                    else
                        req += " where ";

                    req += String.Format("{0} in ({1}) ", attribute[tableIndex, 0], current_col0_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(current_col1_input.Text))
                {
                    if (doneReq)
                        req += "and ";
                    else
                        req += " where ";

                    req += String.Format("{0} in ({1}) ", attribute[tableIndex, 1], current_col1_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(current_col2_input.Text))
                {
                    if (doneReq)
                        req += "and ";
                    else
                        req += " where ";

                    req += String.Format("{0} in ({1}) ", attribute[tableIndex, 2], current_col2_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(current_col3_input.Text))
                {
                    if (doneReq)
                        req += "and ";
                    else
                        req += " where ";

                    req += String.Format("{0} in ({1}) ", attribute[tableIndex, 3], current_col3_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(current_col4_input.Text))
                {
                    if (doneReq)
                        req += "and ";
                    else
                        req += " where ";

                    req += String.Format("{0} in ({1}) ", attribute[tableIndex, 4], current_col4_input.Text);
                    doneReq = true;
                }
            }
            catch
            {
                ;
            }
            return req;
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
