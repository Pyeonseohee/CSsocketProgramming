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
        public static void RegisterPostSQL(dynamic jsonData)
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
                        // 회원가입 완료됐다~
                    }
                    else
                    {
                        Console.Write("Insert error!");
                        // 오류났다~
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }
            }
        }

        // Login했을 때의 route(ID, PWD)
        public static void LoginPostSQL(dynamic jsonData)
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
                        Console.WriteLine($"result: {DBresult["id"]} {DBresult["name"]}, {DBresult["pw"]}");
                        string DBpwd = DBresult["pw"].ToString(); //object to string
                        if(jsonData.PWD == DBpwd) // 비밀번호가 DB와 일치하면
                        {
                            Console.WriteLine("correct password!");
                        }
                        else
                        {
                            Console.WriteLine("Incorrect password!");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Unexist!");
                        // 회원가입부터 해라~
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
