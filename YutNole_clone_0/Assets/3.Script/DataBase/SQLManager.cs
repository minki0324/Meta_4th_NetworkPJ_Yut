using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using LitJson;


public class user_info
{
    public string User_name { get; private set; }
    public string User_Password { get; private set; }
    public string User_ID { get; private set; }

    public user_info(string id, string password ,string name)
    {
        User_ID = id;
        User_Password = password;
        User_name = name;
    }
}

public class server_info
{
    /*
     string serverInfo =
            $"server = {itemdata[0]["IP"]};" + $"" +
            $" Database = {itemdata[0]["TableName"]};" +
            $" Uid = {itemdata[0]["ID"]};" +
            $" Pwd = {itemdata[0]["PW"]};" +
            $" Port = {itemdata[0]["PORT"]};" +
            $" CharSet=utf8;";
    */
    public string IP { get; private set; }
    public string TableName { get; private set; }
    public string ID { get; private set; }
    public string PW { get; private set; }
    public string PORT { get; private set; }

    public server_info(string ip, string tableName, string id, string pw, string port)
    {
        IP = ip;
        TableName = tableName;
        ID = id;
        PW = pw;
        PORT = port;
    }
}


public class SQLManager : MonoBehaviour
{
    public user_info info;

    public MySqlConnection connection;
    public MySqlDataReader reader;

    public string DB_path = string.Empty;
    public static SQLManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        DB_path = Application.dataPath + "/Database";
        string serverinfo = Serverset(DB_path);
        try
        {
            if(serverinfo.Equals(string.Empty))
            {
                Debug.Log("SQL Server Json Error!");
                return;
            }

            connection = new MySqlConnection(serverinfo);
            connection.Open();
            Debug.Log("SQL Server Open Compelate!");

        }catch(Exception e)
        {
            Debug.Log(e.Message) ;
        }
    }

    private void Default_Data(string path)
    {

        List<server_info> userInfo = new List<server_info>();
        userInfo.Add(new server_info("13.124.124.144", "programming", "root", "1234", "3306"));

        JsonData data = JsonMapper.ToJson(userInfo);
        File.WriteAllText(path + "/config.json", data.ToString());
    }

    private string Serverset(string path)
    {
        if (!File.Exists(path)) // 경로가 있나요?
        {
            Directory.CreateDirectory(path);
        }

        if (!File.Exists(path + "/config.json"))  // 파일이 있나요?
        {
            Default_Data(path);
        }
        
        string Jsonstring = File.ReadAllText(path + "/config.json");

        JsonData itemdata = JsonMapper.ToObject(Jsonstring);
        string serverInfo =
            $"server = {itemdata[0]["IP"]};" + $"" +
            $" Database = {itemdata[0]["TableName"]};" +
            $" Uid = {itemdata[0]["ID"]};" +
            $" Pwd = {itemdata[0]["PW"]};" +
            $" Port = {itemdata[0]["PORT"]};" +
            $" CharSet=utf8;";

        return serverInfo;
            
    }

    private bool connection_check(MySqlConnection con)
    {
        //현재 MySQLConnection open 이 아니라면?
        if(con.State != System.Data.ConnectionState.Open)
        {
            con.Open();
            if(con.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
        }
       
            return true;
       
    }
   public bool Join(string id , string password , string nickname)
    {
        try
        {
            //1.connection open 상황인지 확인 -> 메소드화
            if (!connection_check(connection))
            {
                return false;
            }
           
            if (IsIdOrNicknameDuplicate(id, nickname))
            {
                // 중복된 아이디 또는 닉네임이 있으면 가입 실패
                return false;
            }

            string SQL_command =
                string.Format(@"INSERT INTO user_info VALUE('{0}','{1}','{2}');", id, password ,nickname);

            MySqlCommand cmd = new MySqlCommand(SQL_command, connection);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Debug.Log("회원가입 성공!");
                return true;
            }
            else
            {
                Debug.Log("회원가입 실패.");
                return false;
            }
        }
        
        catch (Exception e)
        {
            if (!reader.IsClosed) reader.Close();
            Debug.Log(e.Message);
            return false;
        }
    }
    private bool IsIdOrNicknameDuplicate(string id, string nickname)
    {
        // 아이디 또는 닉네임이 이미 존재하는지 확인하는 쿼리
        string duplicateCheckCommand =
            string.Format(@"SELECT COUNT(*) FROM user_info WHERE User_ID = '{0}' OR User_Name = '{1}';", id, nickname);

        MySqlCommand checkCmd = new MySqlCommand(duplicateCheckCommand, connection);
        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

        // count가 0이 아니면 중복된 아이디 또는 닉네임이 존재함
        if(count > 0)
        {
            Debug.Log("중복발생");
            
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool Login(string id, string password)
    {
        //직접적으로 DB에서 데이터를 가지고 오는 메소드
        //조회되는 데이터가 없다면 bool값을 false;
        //조회가 되는 데이터가 있다면 true값 던지기전에
        //위에서 선언한 info 에다가 담은 다음에 던짐.

        //데이터 가지고오기 

        /*
         1.connection open 상황인지 확인 -> 메소드화

        
        2. Reader 상태가 읽고 있는 상황인지 확인 (한 쿼리문당 리더 한개씩)
        3. 데이터를 다 읽었으면 Reader의 상태 확인 후 Close
         */

        try
        {
            //1.connection open 상황인지 확인 -> 메소드화
            if (!connection_check(connection))
            {
                return false;
            }

            string SQL_command =
                string.Format(@"SELECT User_ID,User_Password,User_Name FROM user_info
                              WHERE User_ID='{0}' AND User_Password = '{1}' ;", id, password);

            MySqlCommand cmd = new MySqlCommand(SQL_command, connection);
            reader = cmd.ExecuteReader();

            //Reader 읽은 데이터가 1개 이상 존재해?
            if (reader.HasRows)
            {
                //읽은 데이터를 하나씩 나열함
                while (reader.Read())
                {
                    /*
                     삼항연산자
                     */
                    string ID = (reader.IsDBNull(0)) ? string.Empty : (string)reader["User_ID"].ToString();
                    string pass = (reader.IsDBNull(1)) ? string.Empty : (string)reader["User_Password"].ToString();
                    string nick = (reader.IsDBNull(2)) ? string.Empty : (string)reader["User_Name"].ToString();
                    if(!ID.Equals(string.Empty) || !pass.Equals(string.Empty))
                    {
                        //정상적으로 Data를 불러온 상황
                        info = new user_info(ID, pass ,nick);
                        if (!reader.IsClosed) reader.Close();
                        return true;
                    }
                    else//로그인실패
                    {
                        break;
                    }
                }//while
            }//if
                if (!reader.IsClosed) reader.Close();
            return false;


        }catch(Exception e)
        {
            if (!reader.IsClosed) reader.Close();
            Debug.Log(e.Message);
            return false;
        }
    }
}
