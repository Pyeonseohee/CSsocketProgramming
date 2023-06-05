using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server1
{
    internal class SQLClass
    {
        // Register했을 때의 route(ID, NAME, PWD)
        public static string RegisterPostSQL(dynamic jsonData)
        {
            // RDS 서버에 접속
            string StringToConnection = "Server=nowmsm-db.cirkkpu5fv9s.us-east-1.rds.amazonaws.com;Database=nowMSM;Uid=admin;Pwd=00000000;";
            using (MySqlConnection conn = new MySqlConnection(StringToConnection))
            {
                Console.Write("success connection!");
                try
                {
                    conn.Open();
                    string InsertQuery = $"insert into user(id, name, pw) values('{jsonData.ID}', '{jsonData.NAME}', '{jsonData.PWD}')";
                    Console.Write("SQL insert start!");

                    // command connection
                    MySqlCommand cmd = new MySqlCommand(InsertQuery, conn);

                    // 만약에 내가처리한 Mysql에 정상적으로 들어갔다면 메세지를 보여주라는 뜻
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.Write("Insert success!");
                        return "success";
                        // 회원가입 완료됐다~
                    }
                    else
                    {
                        Console.Write("Insert error!");
                        return "error";
                        // DB 오류났다~
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                    return "";
                }
            }
        }

        // Login했을 때의 route(ID, PWD)
        public static string LoginPostSQL(dynamic jsonData)
        {
            // RDS 서버에 접속
            string StringToConnection = "Server=nowmsm-db.cirkkpu5fv9s.us-east-1.rds.amazonaws.com;Database=nowMSM;Uid=admin;Pwd=00000000;";
            using (MySqlConnection conn = new MySqlConnection(StringToConnection))
            {
                Console.Write("success connection!");
                try
                {
                    conn.Open();
                    string searchQuery = $"select * from user where id='{jsonData.ID}'";

                    // command connection
                    MySqlCommand cmd = new MySqlCommand(searchQuery, conn);
                    MySqlDataReader DBresult = cmd.ExecuteReader();
                    if(DBresult.Read())
                    {
                        string DBpwd = DBresult["pw"].ToString(); //object to string
                        if(jsonData.PWD == DBpwd) // 비밀번호가 DB와 일치하면
                        {
                            Console.WriteLine("correct password!");
                            return DBresult["user_id"].ToString() ;
                        }
                        else
                        { // 비밀번호 불일치
                            Console.WriteLine("Incorrect password!");
                            return "incorrect";
                        }

                    }
                    else
                    {
                        Console.WriteLine("Unexist!");
                        return "unexist";
                        // 회원가입부터 해라~
                    }
                    conn.Close();

                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                    return "";
                }
            }
        }

        public static void CalenderGetSQL(dynamic jsonData)
        {
            // RDS 서버에 접속
            string StringToConnection = "Server=nowmsm-db.cirkkpu5fv9s.us-east-1.rds.amazonaws.com;Database=nowMSM;Uid=admin;Pwd=00000000;";
            using (MySqlConnection conn = new MySqlConnection(StringToConnection))
            {
                Console.Write("success connection!");
                try
                {
                    conn.Open();
                    Console.WriteLine(jsonData);
                    string searchQuery = $"select emtion, date from log where user_id='{jsonData.USER_ID}'";

                    // command connection
                    MySqlCommand cmd = new MySqlCommand(searchQuery, conn);
                    MySqlDataReader DBresult = cmd.ExecuteReader();
                    while(DBresult.Read())
                    {
                        Console.WriteLine($"result: {DBresult["emtion"]} {DBresult["date"]}");

                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }
            }

        }
    }
}
