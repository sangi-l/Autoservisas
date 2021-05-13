using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Autoservisas.Models
{
    public class Registered_User
    {
        [DisplayName("ID")]
        public int RUserID { get; set; }
        [DisplayName("Vardas")]
        [Required]
        public string FName { get; set; }

        [DisplayName("Pavardė")]
        [Required]
        public string LName { get; set; }

        [DisplayName("El. paštas")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Slaptažodis")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Gimimo data")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Database"].ToString();
            con = new SqlConnection(constring);
        }
        public int GetUserId(string email, string password)
        {
            connection();
            SqlCommand cmd = new SqlCommand("GetUserId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            cmd.Parameters.AddWithValue("@elpastas", email);
            cmd.Parameters.AddWithValue("@slaptazodis", password);

            con.Open();
            sd.Fill(dt);
            con.Close();

            int userid = 0;
            foreach (DataRow dr in dt.Rows)
            {
                userid = Convert.ToInt32(dr["id_naudotojas"]);
            }
            return userid;
        }
        public bool AddUser(Registered_User model)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@vardas", model.FName);
            cmd.Parameters.AddWithValue("@pavarde", model.LName);
            cmd.Parameters.AddWithValue("@elpastas", model.Email);
            cmd.Parameters.AddWithValue("@slaptazodis", model.Password);
            cmd.Parameters.AddWithValue("@gimimo_metai", model.Birthday);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
    }
}