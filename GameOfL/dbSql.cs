using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfL
{
    class dbSql
    {
        static string connectionString = @"Data Source=DESKTOP-KU0OPJ6\SQLEXPRESS;Initial Catalog=Game;User Id = sa; Password = sa";
        

        public void SaveGame(int i, int j, int Cond, string Name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string cmd = $"INSERT INTO Te(i, j, LiveCell, NameGame) VALUES({i}, {j}, {Cond}, '{Name}');";
                SqlCommand command = new SqlCommand(cmd, connection);
                command.ExecuteNonQuery();              

            }     
            //Спасибо большое Вам за задание! Узнал много интересных вещей. Жалко что раньше не так сильно работал с бд
            //    поэтому на бд времени просто не хватило. Была мысль сохранять 2 таблицы одну с "Названием игры" "датой/времени" "колличеством тиков" в другую
            //    сохраняю позиции прямоугольников и жива клетка или нет, далее сделать связь между этими таблицами внешним ключом для удобной загрузки.
            //Так же не успел покрыть все тест кейсами. Еще раз спасибо Вам за задачу:) 
        }
    }
    
}

