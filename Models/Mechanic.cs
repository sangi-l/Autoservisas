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
    public class Mechanic
    {
        [DisplayName("ID")]
        public int MechanicID { get; set; }
        [DisplayName("Nuotrauka")]
        [Required]
        public string Picture { get; set; }

        [DisplayName("Kategorija")]
        [Required]
        public string Category { get; set; }

        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Database"].ToString();
            con = new SqlConnection(constring);
        }
        public bool IsMechanic(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("IsMechanic", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            cmd.Parameters.AddWithValue("@id_naudotojas", id);

            con.Open();
            sd.Fill(dt);
            con.Close();

            int userid = 0;
            foreach (DataRow dr in dt.Rows)
            {
                userid = Convert.ToInt32(dr["id_naudotojas"]);
            }

            if (userid >= 1)
                return true;
            else
                return false;
        }
    }
}