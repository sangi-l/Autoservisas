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
    public class Symptom
    {
        [DisplayName("ID")]
        public int SymptomID { get; set; }
        [DisplayName("Aprašymas")]
        [Required]
        public string Details { get; set; }

        [DisplayName("Kaina")]
        [Required]
        public double Price { get; set; }

        [DisplayName("Trukmė")]
        [Required]
        public double Duration { get; set; }

        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Database"].ToString();
            con = new SqlConnection(constring);
        }

        public bool AddSymptom(Symptom model)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewSymptom", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@aprasymas", model.Details);
            cmd.Parameters.AddWithValue("@kaina", model.Price);
            cmd.Parameters.AddWithValue("@trukme", model.Duration);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Symptom> GetSymptom()
        {
            connection();
            List<Symptom> symptomlist = new List<Symptom>();

            SqlCommand cmd = new SqlCommand("GetSymptomDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                symptomlist.Add(
                    new Symptom
                    {
                        SymptomID = Convert.ToInt32(dr["id_simptomas"]),
                        Details = Convert.ToString(dr["aprasymas"]),
                        Price = Convert.ToDouble(dr["kaina"]),
                        Duration = Convert.ToDouble(dr["trukme"])
                    });
            }
            return symptomlist;
        }

        public bool UpdateDetails(Symptom model)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateSymptomDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_simptomas", model.SymptomID);
            cmd.Parameters.AddWithValue("@aprasymas", model.Details);
            cmd.Parameters.AddWithValue("@kaina", model.Price);
            cmd.Parameters.AddWithValue("@trukme", model.Duration);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool DeleteSymptom(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteSymptom", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_simptomas", id);

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