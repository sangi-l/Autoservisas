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
        public DateTime DateFrom { get; set; }

        [DisplayName("Data (iki)")]
        [Required]
        public DateTime DateTo { get; set; }

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

            SqlCommand cmd = new SqlCommand("GetPartDetails", con);
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
                        DateTo = Convert.ToDateTime(dr["data_iki"]),
                        TimeFrom = Convert.ToInt32(dr["laikas_nuo"]),
                        TimeTo = Convert.ToInt32(dr["laikas_iki"]),
                        MechanicID = mid
                    });
                } 
            }

            return workhours;
        }
    }
}