using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SqlLabCS
{
    public class Functions
    {
        #region PrivateLits

        private SqlConnection con = null;
        private SqlCommand com = null;
        private SqlDataReader red = null;
        private string[] commands = { 
/*0*/       "SELECT * FROM [STUDENTS].[dbo].[@Text1]",
/*1*/       "INSERT INTO [STUDENTS].[dbo].[@Text1] VALUES (@Text2)",
/*2*/       "DELETE FROM [STUDENTS].[dbo].[@Text1] WHERE @Text2",
/*3*/       "DELETE FROM [STUDENTS].[dbo].[GroupStudying] WHERE ID IN (SELECT ID FROM [STUDENTS].[dbo].[GroupNames] WHERE @Text2)",
/*4*/       "DELETE FROM [STUDENTS].[dbo].[GroupLiving] WHERE ID IN (SELECT ID FROM [STUDENTS].[dbo].[GroupNames] WHERE @Text2)",
/*5*/       "UPDATE [STUDENTS].[dbo].[@Text1] SET @Text2 WHERE ID=@Text3",
/*6*/       "SELECT ID FROM [STUDENTS].[dbo].[GroupNames]"};

        #endregion

        #region Properties
        public string Connect { get; set; }
        public bool Success { get; private set; }
        public string Error { get; private set; }
        public string WhereOperate { get; set; }
        public string[] Values { get; set; }

        #endregion

        #region Functions
        public bool TryConnect (string Path)
        {
            try
            {
                con = new SqlConnection(Path);
                con.Open();
                Success = true;
                Connect = Path;
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
                Success = false;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return Success;
        }

        public void Refresh(DataGridView V, string tablename)
        {
            string call = string.Empty;
            try
            {
                con = new SqlConnection(Connect);
                com = new SqlCommand((commands[0].Replace("@Text1", tablename)), con);
                con.Open();
                red = com.ExecuteReader();

                V.Rows.Clear();
                V.Columns.Clear();

                for (int i = 0; i < red.FieldCount; i++)
                {
                    
                    switch (red.GetName(i))
                    {
                        case "ID":
                            call = "ID";
                            break;

                        case "NAME":
                            call = "Имя";
                            break;

                        case "SURNAME":
                            call = "Фамилия";
                            break;

                        case "PATRONYMIC":
                            call = "Отчество";
                            break;

                        case "NATIVECITY":
                            call = "Родной город";
                            break;

                        case "URGENCY":
                            call = "Нуждаемость в общежитии";
                            break;

                        case "LIVESIN":
                            call = "Проживает в общежитии";
                            break;

                        case "FAVSUBJECT":
                            call = "Приоритетный предмет";
                            break;

                        case "MARK":
                            call = "Оценка";
                            break;

                        case "SCORE":
                            call = "Баллы";
                            break;
                    }
                    V.Columns.Add(red.GetName(i), call);
                }

                object[] temp = new object[red.FieldCount];

                while(red.Read())
                {
                    red.GetValues(temp);
                    V.Rows.Add(temp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if(con != null)
                {
                    con.Close();
                }
                if (red != null)
                {
                    red.Close();
                }
            }
        }

        public void Add(string tablename, string[] values)
        {
            string commandchange = commands[1];
            string temp = string.Empty;
            try
            {
                bool isnotnames = true;

                switch (tablename)
                {
                    case "GroupNames":
                        isnotnames = false;
                        break;
                }

                commandchange = commandchange.Replace("@Text1", tablename);

                foreach (string v in values)
                {
                    temp += "'" + v + "'" + ",";
                }

                if (temp != string.Empty && !values.Contains(string.Empty))
                {
                    if (isnotnames)
                    {
                        con = new SqlConnection(Connect);
                        com = new SqlCommand(commands[6], con);
                        con.Open();
                        red = com.ExecuteReader();
                        bool contain = false;
                        while (red.Read())
                        {
                            if (red[0].ToString() == values[0])
                            {
                                contain = true;
                            }
                        }

                        if (contain)
                        {
                            temp = temp.Remove(temp.Length - 1);

                            commandchange = commandchange.Replace("@Text2", temp);

                            con = new SqlConnection(Connect);
                            com = new SqlCommand(commandchange, con);
                            con.Open();
                            com.ExecuteScalar();
                        }
                        else
                        {
                            MessageBox.Show("Вы пытаетесь добавить связанную запись к несуществующей основной. " +
                            "\nДля добавления записи с таким идентификатором сначала создайте главную запись \n" +
                            "в таблице Имён с тем же идентификатором. " +
                            "\nДействие завершено.", "Отсутствует основная запись",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        temp = temp.Remove(temp.Length - 1);

                        commandchange = commandchange.Replace("@Text2", temp);

                        con = new SqlConnection(Connect);
                        com = new SqlCommand(commandchange, con);
                        con.Open();
                        com.ExecuteScalar();
                    }
                }
                else
                {
                    MessageBox.Show("Несколько значений остались пустыми. " +
                        "\nДозаполните значения и повторите попытку записи. " +
                        "\nТаблица не принимает нулевых значений.", "Не все ячейки заполнены", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
                if (red != null)
                {
                    red.Close();
                }
            }
        }

        public void Delete(string tablename, string[] values)
        {
            string commandchange = commands[2];
            bool isnames = false;
            string temp = string.Empty;
            try
            {
                commandchange = commandchange.Replace("@Text1", tablename);
                string[] columns = null;

                switch (tablename)
                {
                    case "GroupNames":
                        columns = new string[] {"ID","NAME", "SURNAME", "PATRONYMIC"};
                        isnames = true;
                        break;

                    case "GroupStudying":
                        columns = new string[] { "ID", "FAVSUBJECT", "MARK", "SCORE" };
                        break;

                    case "GroupLiving":
                        columns = new string[] { "ID", "NATIVECITY", "URGENCY", "LIVESIN" };
                        break;
                }

                for (int i = 0; i < columns.Length; i++)
                {
                    if (values[i] != string.Empty)
                    {
                        temp += "(" + columns[i] + "=" + "'" + values[i] + "'" + ")" + " AND ";
                    }
                }
                if (temp != string.Empty)
                {
                    temp = temp.Remove(temp.Length - 5);

                    if (isnames)
                    {
                        con = new SqlConnection(Connect);
                        com = new SqlCommand(commands[3].Replace("@Text2", temp), con);
                        con.Open();
                        com.ExecuteScalar();
                        con.Close();

                        con = new SqlConnection(Connect);
                        com = new SqlCommand(commands[4].Replace("@Text2", temp), con);
                        con.Open();
                        com.ExecuteScalar();
                        con.Close();
                    }

                    commandchange = commandchange.Replace("@Text2", temp);

                    con = new SqlConnection(Connect);
                    com = new SqlCommand(commandchange, con);
                    con.Open();
                    com.ExecuteScalar();
                }
                else
                {
                    DialogResult rez = MessageBox.Show("Вы не указали условия удаления. " +
                        "\nПри попытке продолжения будут удалены ВСЕ данные в таблице!!! " +
                        "\nВы действительно хотите продолжить?", "Предупреждение об удалении", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Warning, 
                        MessageBoxDefaultButton.Button2);

                    if (rez == DialogResult.Yes)
                    {
                        if (isnames)
                        {
                            con = new SqlConnection(Connect);
                            com = new SqlCommand(commands[3].Replace(" WHERE @Text2", temp), con);
                            con.Open();
                            com.ExecuteScalar();
                            con.Close();

                            con = new SqlConnection(Connect);
                            com = new SqlCommand(commands[4].Replace(" WHERE @Text2", temp), con);
                            con.Open();
                            com.ExecuteScalar();
                            con.Close();
                        }

                        commandchange = commandchange.Replace(" WHERE @Text2", temp);

                        con = new SqlConnection(Connect);
                        com = new SqlCommand(commandchange, con);
                        con.Open();
                        com.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
                if (red != null)
                {
                    red.Close();
                }
            }
        }

        public void Update(string tablename, string[] values)
        {
            string commandchange = commands[5];
            string temp = string.Empty;
            try
            {
                commandchange = commandchange.Replace("@Text1", tablename);
                string[] columns = null;

                switch (tablename)
                {
                    case "GroupNames":
                        columns = new string[] { "ID", "NAME", "SURNAME", "PATRONYMIC" };
                        break;

                    case "GroupStudying":
                        columns = new string[] { "ID", "FAVSUBJECT", "MARK", "SCORE" };
                        break;

                    case "GroupLiving":
                        columns = new string[] { "ID", "NATIVECITY", "URGENCY", "LIVESIN" };
                        break;
                }

                for (int i = 1; i < columns.Length; i++)
                {
                    if (values[i] != string.Empty)
                    {
                        temp += columns[i] + "=" + "'" + values[i] + "'" + ", ";
                    }
                }
                if (temp != string.Empty && values[0] != string.Empty)
                {
                    temp = temp.Remove(temp.Length - 2);

                    commandchange = commandchange.Replace("@Text2", temp);
                    commandchange = commandchange.Replace("@Text3", values[0]);

                    con = new SqlConnection(Connect);
                    com = new SqlCommand(commandchange, con);
                    con.Open();
                    com.ExecuteScalar();
                }
                else
                {
                    if(temp == string.Empty && values[0] != string.Empty)
                    {
                        MessageBox.Show("Вы не указали ни одной ячейки для изменения." +
                            "Добавьте заменяемые значения и повторите попытку.",
                            "Что будем изменять?..",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Question);
                    }
                    if (temp != string.Empty && values[0] == string.Empty)
                    {
                        MessageBox.Show("Вы не указали идентификатор, хотя в справке было " +
                            "ЯСНО И ЧЁТКО ИЗЛОЖЕНО, ЧТО ДАННАЯ ПРОЦЕДУРА ОБЯЗАТЕЛЬНА >:(" +
                            "Добавьте идентификатор, не позорьте нацию...",
                            "Ну я же просил!..",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                    }
                    if (temp == string.Empty && values[0] == string.Empty)
                    {
                        MessageBox.Show("Это невероятно, но вы умудрились забыть не только " +
                            "про заменяемые ячейки, но ДАЖЕ про идентификатор..." +
                            "Если вы просто так жмякаете кнопку 'Изменить', то ради бога, не делайте этого.",
                            "Медицина здесь бессильна...Вызывайте санитаров.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
                if (red != null)
                {
                    red.Close();
                }
            }
        }

        #endregion
    }
}
