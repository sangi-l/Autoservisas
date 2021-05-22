using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Autoservisas.Models
{
    public class WorkHours
    {
        [DisplayName("ID")]
        public int TimeID { get; set; }
        [DisplayName("Data (nuo)")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [DisplayName("Laikas (nuo)")]
        [Required]
        public int TimeFrom { get; set; }

        [DisplayName("Laikas (iki)")]
        [Required]
        public int TimeTo { get; set; }

        [DisplayName("MeistroID")]
        [Required]
        public int MechanicID { get; set; }


        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Database"].ToString();
            con = new SqlConnection(constring);
        }

        public List<WorkHours> GetWorkHours(int id)
        {
            List<WorkHours> workhours = new List<WorkHours>();
            connection();

            SqlCommand cmd = new SqlCommand("GetWorkHours", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                int mid = Convert.ToInt32(dr["fk_meistrasid"]);
                if(mid == id)
                {
                    workhours.Add(
                    new WorkHours
                    {
                        TimeID = Convert.ToInt32(dr["id_darbo_laikas"]),
                        DateFrom = Convert.ToDateTime(dr["data"]),
                        TimeFrom = Convert.ToInt32(dr["laikas_nuo"]),
                        TimeTo = Convert.ToInt32(dr["laikas_iki"]),
                        MechanicID = mid
                    });
                } 
            }

            return workhours;
        }

        public bool AddTime(WorkHours time)
        {
            connection();

            SqlCommand cmd = new SqlCommand("AddNewTime", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@data", time.DateFrom);
            cmd.Parameters.AddWithValue("@laikas_nuo", time.TimeFrom);
            cmd.Parameters.AddWithValue("@laikas_iki", time.TimeTo);
            cmd.Parameters.AddWithValue("@fk_meistrasid", time.MechanicID);

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