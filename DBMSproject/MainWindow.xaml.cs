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
using System.IO;

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
                    String req = checkbox_request();
                    if (String.IsNullOrEmpty(req))
                        req = "*";

                    button_cmd.CommandText = "Select " + req + " from " + opTableList.SelectedItem + combine_request();
                    dr = button_cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    dataZone.ItemsSource = dt.DefaultView;

                    Console.WriteLine(button_cmd.CommandText);                   
                }
                else if (opList.SelectedItem == opListVal[1])   // Delete
                {
                    button_cmd.CommandText = "Delete from " + opTableList.SelectedItem + combine_request();
                    button_cmd.ExecuteNonQuery();
                }
                else if (opList.SelectedItem == opListVal[2])   // Update
                {
                    button_cmd.CommandText = "Update " + opTableList.SelectedItem + update_attr() + combine_request();
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

        private string checkbox_request()
        {
            string req = "";
            int tableIndex = opTableList.SelectedIndex;
            Boolean doneReq = false;

            try
            {
                if (Convert.ToBoolean(current_col0.IsChecked))
                {
                    if (doneReq)
                        req += " , ";

                    req += attribute[tableIndex, 0];
                    doneReq = true;
                }
                if (Convert.ToBoolean(current_col1.IsChecked))
                {
                    if (doneReq)
                        req += " , ";

                    req += attribute[tableIndex, 1];
                    doneReq = true;
                }
                if (Convert.ToBoolean(current_col2.IsChecked))
                {
                    if (doneReq)
                        req += " , ";

                    req += attribute[tableIndex, 2];
                    doneReq = true;
                }
                if (Convert.ToBoolean(current_col3.IsChecked))
                {
                    if (doneReq)
                        req += " , ";

                    req += attribute[tableIndex, 3];
                    doneReq = true;
                }
                if (Convert.ToBoolean(current_col4.IsChecked))
                {
                    if (doneReq)
                        req += " , ";

                    req += attribute[tableIndex, 4];
                    doneReq = true;
                }
             req += aggregate_request(doneReq);
            }
            catch
            {
                ;
            }
            return req;
        }

        private string aggregate_request(Boolean doneReq)
        {
            string req = "";
            int tableIndex = opTableList.SelectedIndex;

            try
            {
                if (!String.IsNullOrEmpty(new_col0_input.Text))
                {
                    if (doneReq)
                        req += " , ";

                    req += String.Format("count(*)");
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(new_col1_input.Text))
                {
                    if (doneReq)
                        req += " , ";

                    req += String.Format("sum({0}) ", new_col1_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(new_col2_input.Text))
                {
                    if (doneReq)
                        req += " , ";

                    req += String.Format("max({0}) ", new_col2_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(new_col3_input.Text))
                {
                    if (doneReq)
                        req += " , ";

                    req += String.Format("min({0}) ", new_col3_input.Text);
                    doneReq = true;
                }
                if (!String.IsNullOrEmpty(new_col4_input.Text))
                {
                    if (doneReq)
                        req += " , ";

                    req += String.Format("avg({0}) ", new_col4_input.Text);
                    doneReq = true;
                }
            }
            catch
            {
                ;
            }
            return req;
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
                ;
            }
            return req + " ";
        }

        private string combine_request()
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
            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('2','50343','life','perry')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('3','30487','act','arron')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('4','30487','act','queen')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('5','30387','act','yu')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('6','51177','act','kelly')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('7','72544','csie','brain')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('8','72735','csie','lily')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('9','41568','econ','joan')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO FACULTY VALUES ('10','56844','twl','eason')";
            sample_cmd.ExecuteNonQuery();

            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('17',1071,'aborigine','1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('5',1071,'poor','3')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('17',1072,'aborigine','1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('5',1072,'poor','3')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('17',1081,'aborigine','1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('16',1081,'aborigine','1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('15',1081,'poor','3')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('17',1082,'aborigine','1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('16',1081,'poor','3')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('13',1081,'threegen','3')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('17',1082,'threegen','1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('16',1082,'threegen','1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO SCHOLAR VALUES ('15',1082,'threegen','3')";
            sample_cmd.ExecuteNonQuery();

            sample_cmd.CommandText = "INSERT INTO STU VALUES ('17','hua','3','F7','1')";    
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('5','larry','5','E4','2')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('16','huna','1','B1','3')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('15','freddy','6','C1','4')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('4','angella','2','E5','5')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('13','linkedin','3','H4','6')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('3','stella','1','D8','7')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('2','peter','2','E2','8')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('1','grandy','1','F3','9')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO STU VALUES ('56','kasona','4','D5','10')";
            sample_cmd.ExecuteNonQuery();

            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('1','simpoo','F7')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('2','karry','E4')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('3','sunny','B1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('4','mathu','C1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('5','peter','E5')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('6','spen','H4')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('7','huna','D8')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('8','huun','E2')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('9','lin','F3')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO PROF VALUES ('10','chen','D5')";
            sample_cmd.ExecuteNonQuery();

            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('F7','cs',1997,'csee','17')";    
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('E4','re',1928,'eng','5')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('B1','cl',1975,'lib','16')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('C1','math',1962,'cos','15')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('E5','mse',1956,'eng','4')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('H4','ba',1999,'com','13')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('D8','psy',2003,'coss','3')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('E2','ee',1930,'csee','2')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('F3','id',2005,'cod','1')";
            sample_cmd.ExecuteNonQuery();
            sample_cmd.CommandText = "INSERT INTO DEPT VALUES ('D5','econ',2001,'coss','56')";
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
